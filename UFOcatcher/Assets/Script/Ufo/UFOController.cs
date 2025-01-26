using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UFOController : MonoBehaviour
{
	public Vector2 _inputVec = Vector2.zero;
	public float _speed = 5.0f;

	public float _screenBounds = 5.0f;

	private Vector2 _screenBoundsMin;
	private Vector2 _screenBoundsMax;

	private bool _beamActive = false;
	public GameObject _beam;

	public SpriteRenderer alienSpriteRenderer;
	public Sprite neutralSprite;
	public Sprite sadSprite;
	public Sprite happySprite;

	public GameObject confetti;

	private const float HURT_VISUALS_DURATION = 1f;
	private const float HAPPY_VISUALS_DURATION = 3f;

	private float timeUntilResetToNeutral = 0;

	void Start()
	{
		_screenBoundsMin = new Vector2(-_screenBounds, -_screenBounds);
		_screenBoundsMax = new Vector2(_screenBounds, _screenBounds);
	}

	void Update()
	{
		float y = Input.GetAxis("Vertical");
		float x = Input.GetAxis("Horizontal");
		_inputVec = new Vector2(x, y);

		transform.position += new Vector3(_inputVec.x, 0, _inputVec.y) * Time.deltaTime * _speed;
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, _screenBoundsMin.x, _screenBoundsMax.x),
			transform.position.y,
			Mathf.Clamp(transform.position.z, _screenBoundsMin.y, _screenBoundsMax.y));

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
				var sphereHit = Physics.SphereCastAll(hit.point, 1f, Vector3.down, 999);
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
	}

	public void ActivateSkill(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			_beamActive = true;
			SoundManager.instance.PlaySFX("Shoot Sound");
		}
		else if (context.canceled)
		{
			_beamActive = false;
		}
		_beam.SetActive(_beamActive);
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.TryGetComponent<BaseObject>(out var baseObject))
		{
			GameManager.Instance.QuestManager.CollectedObject(baseObject);

			baseObject.DespawnObject();
		}
	}

	// Trigger hurt visuals
	public void TriggerSad()
	{
		alienSpriteRenderer.sprite = sadSprite;
		timeUntilResetToNeutral = HURT_VISUALS_DURATION;
	}

	// Trigger happy visuals
	public void TriggerHappy()
	{
		alienSpriteRenderer.sprite = sadSprite;
		timeUntilResetToNeutral = HAPPY_VISUALS_DURATION;
	}

	// Trigger celebration (confetti) visuals (e.g.: A quest a completed).
	public void TriggerCelebration()
	{
		TriggerHappy();
		GameObject newConfetti = Instantiate(confetti);
		newConfetti.transform.parent = transform;
		newConfetti.transform.localPosition = Vector3.zero;
		Destroy(newConfetti, 5);
	}
}