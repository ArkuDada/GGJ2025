using UnityEngine;
using System.Collections;

public class ArcadeScreenMask : MonoBehaviour
{
    private float _progress = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Coroutine to animate progress from current value to target value
    public IEnumerator AnimateProgress()
    {
        float duration = 1.0f;  // Duration of the animation
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            _progress = Mathf.Lerp(1.0f, -1.0f, timeElapsed / duration);  // Smoothly interpolate between start and end values
            GetComponent<Renderer>().sharedMaterial.SetFloat("_Progress", _progress);  // Update the material's progress
            timeElapsed += Time.deltaTime;  // Accumulate the time elapsed
            yield return null;  // Wait for the next frame
        }

        // Ensure the final progress value is exactly the target value
        _progress = 1.0f;
        GetComponent<Renderer>().sharedMaterial.SetFloat("_Progress", _progress);
    }
}
