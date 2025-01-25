using UnityEngine;

public class AcidRain : MonoBehaviour
{
    // Speed at which the object falls
    [SerializeField]
    private float fallSpeed = 5f;

    [SerializeField]
    private int decreaseScore = 10;

    // Update is called once per frame
    private void Update()
    {
        // Move the object downward along the Y-axis
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<UFOController>(out var ufoController))
        {
            if (GameUtility.GameManagerExists())
            {
                GameManager.Instance.ScoreManager.DecrementScore(decreaseScore);
            }
            Destroy(this.gameObject);
        } 
        else if (other.gameObject.CompareTag("FloorPlane")) 
        {
            Destroy(this.gameObject);
        }
    }
}
