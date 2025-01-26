using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShaker : MonoBehaviour
{
    static CameraShaker instance;
    public static CameraShaker Instance => instance;

    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private Camera cam;

    // Vector3
    private void Awake()
    {
        instance = this;
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;

        DoScreenShake();
    }


    void Update()
    {
        duration -= Time.unscaledDeltaTime;
        
        if(duration > 0)
        {
            var rand = Random.insideUnitCircle;
            rand.y = Math.Abs(rand.y);
            Vector3 rand3 = new Vector3(rand.x, rand.y, 0) * shakeMagnitude;

            cam.transform.position = originalPosition + rand3;
            
        }
        else
        {
            duration = 0;
            cam.transform.position = originalPosition;
        }
        
    }

    public float shakeDuration = 0f;
    public float shakeMagnitude = 0.7f;
    
    public float duration = 0.3f;
    
    public void DoScreenShake()
    {
        duration = shakeDuration;
    }
}