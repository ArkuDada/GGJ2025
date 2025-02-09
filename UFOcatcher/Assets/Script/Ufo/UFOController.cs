using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UFOController : MonoBehaviour
{
	public Vector2 _inputVec = Vector2.zero;
	public float _speed = 5.0f;

	public float _screenBounds = 5.0f;

	private Vector2 _screenBoundsMin;
	private Vector2 _screenBoundsMax;

	private bool _beamActive = false;

	public float BeamRadius = 1f;
	public GameObject _beam;
	public GameObject _ufoMesh;

	public Arcade _arcade;

	public SpriteRenderer alienSpriteRenderer;
	public Sprite neutralSprite;
	public Sprite sadSprite;
	public Sprite happySprite;

	public GameObject confetti;

	private const float HURT_VISUALS_DURATION = 0.5f;
	private const float HAPPY_VISUALS_DURATION = 2f;

	private float timeUntilResetToNeutral = 0;

	[SerializeField]
	private AudioSource _source;

	[SerializeField]
	private Image energyBar;

	[SerializeField]
	private float currentEnergy = 1.0f; // Initial energy set to 100%

	[SerializeField]
	private float energyDepletionRate = 0.1f; // Energy depletion per second when beam is active

	[SerializeField]
	private float energyRegenerationRate = 0.05f; // Energy regeneration per second when beam is inactive

	[SerializeField]
	private float energyThreshold = 0.1f; // Minimum energy required to activate the beam

	[SerializeField]
	private float cooldownDuration = 2.0f; // Cooldown duration after energy is depleted

	private bool isCooldown = false;
	private float cooldownTimer = 0.0f;

	[SerializeField]
	GameManager gameManager;

	private const float SHAKE_MAGNITUDE_DEBRIS = 5;
	private const float SHAKE_MAGNITUDE_BEAM = 1;

	private const float DEBRIS_SHAKE_DURATION = 2;
	private float remainingDebrisShakeDuration = 0;

	void Start()
	{
		if (_arcade == null)
			_arcade = GameObject.Find("Arcade").GetComponent<Arcade>();

		if (gameManager == null)
			gameManager = GameManager.Instance;

		if (confetti == null)
			confetti = GameObject.FindGameObjectWithTag("FloorPlane").transform.Find("Confetti").gameObject;

		_screenBoundsMin = new Vector2(-_screenBounds, -_screenBounds);
		_screenBoundsMax = new Vector2(_screenBounds, _screenBounds);
		UpdateEnergyBar();
	}

	void Update()
	{
		float y = Input.GetAxisRaw("Vertical");
		float x = Input.GetAxisRaw("Horizontal");
		_inputVec = new Vector2(x, y);
		_arcade.SetVisualJoystickInputs(_inputVec);
		_arcade.SetVisualButtonInputs(_beamActive);

		transform.position += new Vector3(_inputVec.x, 0, _inputVec.y) * Time.deltaTime * _speed;
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, _screenBoundsMin.x, _screenBoundsMax.x),
			transform.position.y,
			Mathf.Clamp(transform.position.z, _screenBoundsMin.y, _screenBoundsMax.y));

		// Handle energy depletion, regeneration, and cooldown
		HandleEnergy();

		// Rotation, shaking
		UpdateRotation(_inputVec);

		if (_beamActive && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 999,
			   Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
		{
			if (hit.transform.TryGetComponent<BaseObject>(out var objectHit) &&
			   objectHit.State == BaseObject.ObjectState.Grounded)
			{
				objectHit.BubbleLift();
			}
			else
			{
				var sphereHit = Physics.SphereCastAll(hit.point, BeamRadius, Vector3.down, 999);
				hit = sphereHit.OrderBy(o => Vector3.Distance(hit.point, o.transform.position)).First();
				if (hit.transform.TryGetComponent<BaseObject>(out objectHit) &&
				   objectHit.State == BaseObject.ObjectState.Grounded)
				{
					objectHit.BubbleLift();
				}
			}
		}

		if (timeUntilResetToNeutral > 0)
		{
			timeUntilResetToNeutral -= Time.deltaTime;
		}
		else if (alienSpriteRenderer.sprite != neutralSprite)
		{
			alienSpriteRenderer.sprite = neutralSprite;
		}

		if (IsGoAway)
		{
			transform.position += Vector3.up * GoAwaySpeed * Time.unscaledDeltaTime;

			if (transform.position.y > 10.0f)
			{
				transform.position = new Vector3(0, -100, 0);
			}
		}

		remainingDebrisShakeDuration = Math.Max(0, remainingDebrisShakeDuration - Time.deltaTime);
	}

	public void TriggerDebrisShake()
	{
		remainingDebrisShakeDuration = DEBRIS_SHAKE_DURATION;
	}

	public bool IsGoAway = false;
	public float GoAwaySpeed = 5.0f;

	public void ActivateSkill(InputAction.CallbackContext context)
	{
		if (context.performed && currentEnergy > energyThreshold && !isCooldown)
		{
			_beamActive = true;
			_source.Play();
		}
		else if (context.canceled)
		{
			_beamActive = false;
			_source.Stop();
		}
		_beam.SetActive(_beamActive);
	}

	private void HandleEnergy()
	{
		if (isCooldown)
		{
			cooldownTimer -= Time.deltaTime;
			if (cooldownTimer <= 0.0f)
			{
				isCooldown = false;
				currentEnergy = 1.0f;

			}
		}
		else if (_beamActive)
		{
			currentEnergy -= energyDepletionRate * Time.deltaTime;
			currentEnergy = Mathf.Clamp01(currentEnergy);

			// Trigger cooldown if energy is depleted
			if (currentEnergy <= 0f)
			{
				_beamActive = false;
				_beam.SetActive(false);
				_source.Stop();
				isCooldown = true;
				cooldownTimer = cooldownDuration;
			}
		}
		else
		{
			currentEnergy += energyRegenerationRate * Time.deltaTime;
			currentEnergy = Mathf.Clamp01(currentEnergy);
		}

		UpdateEnergyBar();
	}

	private void UpdateEnergyBar()
	{
		if (energyBar != null)
		{
			if (isCooldown)
			{
				energyBar.color = Color.gray;
				energyBar.fillAmount = Mathf.Clamp01(1 - (cooldownTimer / cooldownDuration));
			}
			else
			{
				energyBar.color = Color.yellow;
				energyBar.fillAmount = currentEnergy;
			}

		}
	}

	void UpdateRotation(Vector2 _input)
	{
		Vector3 joystickWantAngle_Euler = _arcade.joystick.localEulerAngles;
		joystickWantAngle_Euler *= -1;

		// Generate random offsets for each axis (rotation in degrees)
		float xx = UnityEngine.Random.Range(-1, 1);
		float yy = UnityEngine.Random.Range(-1, 1);
		float zz = UnityEngine.Random.Range(-1, 1);

		float cumulativeShakeMangitude = (SHAKE_MAGNITUDE_BEAM * (_beamActive ? 1 : 0)) + (SHAKE_MAGNITUDE_DEBRIS * (remainingDebrisShakeDuration > 0 ? 1 : 0));
		_ufoMesh.transform.localRotation = Quaternion.Euler(joystickWantAngle_Euler + new Vector3(xx, yy, zz) * cumulativeShakeMangitude);
	}


	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.TryGetComponent<BaseObject>(out var baseObject))
		{
			gameManager.QuestManager.CollectedObject(baseObject);
		}
	}

	// Trigger hurt visuals
	public void TriggerSad()
	{
		// Happy overrides sad
		if (timeUntilResetToNeutral > 0)
			return;

		alienSpriteRenderer.sprite = sadSprite;
		timeUntilResetToNeutral = HURT_VISUALS_DURATION;
	}

	// Trigger happy visuals
	public void TriggerHappy()
	{
		alienSpriteRenderer.sprite = happySprite;
		timeUntilResetToNeutral = HAPPY_VISUALS_DURATION;
	}

	// Trigger celebration (confetti) visuals (e.g.: A quest a completed).
	public void TriggerCelebration()
	{
		TriggerHappy();
		GameObject newConfetti = Instantiate(confetti);
		newConfetti.transform.parent = transform;
		newConfetti.transform.localPosition = Vector3.zero;
		newConfetti.SetActive(true);
		Destroy(newConfetti, 5);
	}
}