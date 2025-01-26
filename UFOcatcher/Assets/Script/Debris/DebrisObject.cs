using UnityEngine;

enum DebrisState
{
    Prepare,
    Fall
}

[RequireComponent(typeof(LineRenderer))]
public class DebrisObject : MonoBehaviour
{
    // Speed at which the object falls
    [SerializeField] float fallSpeed = 5f;

    [SerializeField] int decreaseScore = 10;

    [SerializeField] GameObject crosshair;

    [SerializeField] DebrisState currentDebrisState;

    // LineRenderer for the laser indicator
    private LineRenderer lineRenderer;

    // Layer mask to define what the laser can hit (e.g., ground)
    public LayerMask groundLayer;

    // Max distance for the laser if no object is hit
    public float maxLaserLength = 100f;

    // Laser size reduction rate
    [SerializeField] float laserShrinkRate = 0.05f;

    // Minimum line width before triggering the fall state
    [SerializeField] float spawnLineWidth = 0.01f;

    // Minimum line width before triggering the fall state
    [SerializeField] float minLineWidth = 0.01f;

    // Color change speed
    [SerializeField] float colorChangeSpeed = 1f;

    private Color startColor = Color.yellow;
    private Color endColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        // Get the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();

        // Configure the LineRenderer's default properties
        lineRenderer.positionCount = 2; // A line with 2 points: start and end
        lineRenderer.startWidth = 0.2f; // Initial width of the laser
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = startColor;

        currentDebrisState = DebrisState.Prepare;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentDebrisState)
        {
            case DebrisState.Prepare:
                UpdateLaser();
                UpdateToSpawnDebris();
                ChangeLaserColor();
                break;

            case DebrisState.Fall:
                if (crosshair != null)
                {
                    crosshair.SetActive(false);
                }
                transform.position += Vector3.down * fallSpeed * Time.deltaTime;
                break;
        }

        ShrinkLaser();
    }

    void UpdateLaser()
    {
        // Set the start position of the laser to the DebrisObject's position
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

    void UpdateToSpawnDebris()
    {
        if (lineRenderer.startWidth <= spawnLineWidth)
        {
            currentDebrisState = DebrisState.Fall;
        }
    }

    void ShrinkLaser()
    {
        if (lineRenderer.startWidth > minLineWidth)
        {
            // Gradually shrink the line width
            float newWidth = Mathf.Max(lineRenderer.startWidth - laserShrinkRate * Time.deltaTime, minLineWidth);
            lineRenderer.startWidth = newWidth;
            lineRenderer.endWidth = newWidth;
        }
        else
        {
            // When the laser width reaches the minimum, switch to the Fall state
            lineRenderer.enabled = false;
            currentDebrisState = DebrisState.Fall;
        }
    }

    void ChangeLaserColor()
    {
        float t = Mathf.PingPong(Time.time * colorChangeSpeed, 1f);
        Color currentColor = Color.Lerp(startColor, endColor, t);
        lineRenderer.startColor = currentColor;
        lineRenderer.endColor = currentColor;
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
