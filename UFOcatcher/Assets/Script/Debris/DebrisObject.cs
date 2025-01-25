using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DebrisObject : MonoBehaviour
{
    // Speed at which the object falls
    [SerializeField] float fallSpeed = 5f;

    [SerializeField] int decreaseScore = 10;

    [SerializeField] GameObject crosshair;

    // LineRenderer for the laser indicator
    private LineRenderer lineRenderer;

    // Layer mask to define what the laser can hit (e.g., ground)
    public LayerMask groundLayer;

    // Max distance for the laser if no object is hit
    public float maxLaserLength = 100f;

    // Start is called before the first frame update
    void Start()
    {
        // Get the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();

        // Configure the LineRenderer's default properties
        //lineRenderer.startColor = Color.red; // Laser color
        //lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2; // A line with 2 points: start and end
    }

    // Update is called once per frame
    void Update()
    {
        // Move the AcidRain object downward
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Update the laser position
        UpdateLaser();
    }

    void UpdateLaser()
    {
        // Set the start position of the laser to the AcidRain's position
        Vector3 startPosition = transform.position;

        // Cast a ray downward to detect where the laser should end
        RaycastHit hit;
        if (Physics.Raycast(startPosition, Vector3.down, out hit, maxLaserLength, groundLayer))
        {
            // If the ray hits something, stop the laser at the hit point
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, hit.point);

            if (crosshair != null)
            {
                crosshair.transform.position = hit.point + Vector3.up * 0.01f; // Slight offset to avoid z-fighting
                crosshair.SetActive(true);
            }
        }
        else
        {
            // If nothing is hit, extend the laser to the maximum length
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, startPosition + Vector3.down * maxLaserLength);

            if (crosshair != null)
            {
                crosshair.SetActive(false);
            }
        }
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
