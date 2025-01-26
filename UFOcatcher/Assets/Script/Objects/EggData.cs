using UnityEngine;

public class EggData : MonoBehaviour
{
    [SerializeField] private GameObject chickPrefab;
    public float hatchChance = 0.33f;
    public void Hatch()
    {
        if(Random.Range(0.0f, 1.0f) < hatchChance)
        {
            Instantiate(chickPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
