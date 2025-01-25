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

    public GameObject _beam;

    public GameObject _HortBar;
    public GameObject _VertBar;

    
    private int _score = 0;

    [SerializeField] private TextMeshProUGUI _text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _beam.SetActive(false);
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

        _VertBar.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        _HortBar.transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }
    
    public void ActivateSkill(InputAction.CallbackContext context)
    {
        Debug.Log("ActivateSkill");
        if (context.performed)
        {
            _beam.SetActive(true);
        }
        else if (context.canceled)
        {
            _beam.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent<PriceObject>(out var price))
        {
            switch(price.State)
            {
                case PriceObject.ObjectState.Up:
                    _score++;
                    break;
                case PriceObject.ObjectState.Down:
                    _score--;
                    break;
            }

            _text.text = $"Score: {_score}";
            Destroy(other.gameObject);
        }
    }
}