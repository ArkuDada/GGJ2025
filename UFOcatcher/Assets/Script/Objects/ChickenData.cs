using System;
using UnityEngine;

public class ChickenData : MonoBehaviour
{
    [SerializeField]
    private GameObject eggPrefab;

    [SerializeField]
    private float layEggInterval = 5.0f;

    private float timer = 0.0f;

    [SerializeField]
    private float egglayForce = 1000.0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= layEggInterval)
        {
            timer = 0.0f;
            if(!ObjectSpawner.Instance.IsReachedMaxObjectCount) LayEgg();
        }
    }

    public void LayEgg()
    {
        var egg = Instantiate(eggPrefab, transform.position, Quaternion.identity);

        var rb = egg.GetComponent<Rigidbody>();

// Generate a random direction vector
        Vector3 randomDirection = new Vector3(
            UnityEngine.Random.Range(-1.0f, 1.0f),
            UnityEngine.Random.Range(0.3f, 0.7f),
            UnityEngine.Random.Range(-1.0f, 1.0f)
        ).normalized;

// Apply the force to the egg's Rigidbody
        rb.AddForce(randomDirection * egglayForce);
    }
}