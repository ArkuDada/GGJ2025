using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UFOController : MonoBehaviour
{
	public Vector2 _inputVec = Vector2.zero;
	public float _speed = 5.0f;
	public Vector2 _screenBoundsMin;
	public Vector2 _screenBoundsMax;

	private bool _beamActive = false;
	public GameObject _beam;

	void Start()
	{
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

		if (_beamActive && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 999, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
		{
			if (hit.transform.TryGetComponent<BaseObject>(out var objectHit) && objectHit.State == BaseObject.ObjectState.Grounded)
			{
				objectHit.BubbleLift();
			}
		}
	}

	public void ActivateSkill(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			_beamActive = true;
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

			baseObject.DestroyObject();
		}
	}
}