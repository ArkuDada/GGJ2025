using UnityEngine;

public class EggData : MonoBehaviour
{
    [SerializeField] private GameObject chickPrefab;
    
    public void Hatch()
    {
        Instantiate(chickPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
