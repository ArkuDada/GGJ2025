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

    public GameObject _bubblePrefab;
    
    private int _score = 0;

    [SerializeField] private TextMeshProUGUI _text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        _inputVec = new Vector2(x, y);

        transform.position += new Vector3(_inputVec.x, 0, _inputVec.y) * Time.deltaTime * _speed;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, _screenBoundsMin.x, _screenBoundsMax.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, _screenBoundsMin.y, _screenBoundsMax.y));

    }
    
    public void ActivateSkill(InputAction.CallbackContext context)
    {
        Debug.Log("ActivateSkill");
        if(context.performed)
        {
            var bubble = Instantiate(_bubblePrefab, transform.position + (Vector3.down * _bubbleSpawnOffset), Quaternion.identity).GetComponent<CaptureBubble>();
        }
    }

    [SerializeField]
    private float _bubbleSpeed = 1.0f;
    private float _bubbleSpawnOffset = 1.0f;
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent<BaseObject>(out var price))
        {
            switch(price.State)
            {
                case BaseObject.ObjectState.Up:
                    _score++;
                    break;
                case BaseObject.ObjectState.Down:
                    _score--;
                    break;
            }

            _text.text = $"Score: {_score}";
            Destroy(other.gameObject);
        }
    }
}