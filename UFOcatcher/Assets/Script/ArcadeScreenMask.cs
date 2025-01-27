using UnityEngine;
using System.Collections;

public class ArcadeScreenMask : MonoBehaviour
{
    public float duration = 1.0f;  // Duration of the animation
    private float _progress = 0.0f;

    private MeshRenderer mr;
    private Renderer r;

    public Material DefaultMat;
    public Material EndScreenMat;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        r = GetComponent<Renderer>();
    }

    public void ChangeEndMat()
    {
        mr.material = EndScreenMat;
    }
    
    public void ChangeDefaultMat()
    {
        mr.material = DefaultMat;
    }


    Coroutine animcour = null;
    public void PlayReflectionAnimation(bool doReset = true)
    {
        if (!doReset && animcour != null)
        {
            StopCoroutine(animcour);
            animcour = null;
        }

        if(animcour == null)
            animcour = StartCoroutine(AnimateProgress(doReset));
    }
    // Coroutine to animate progress from current value to target value
    public IEnumerator AnimateProgress(bool doReset = true)
    {
        float timeElapsed = 0f;

        float _alpha = 0.5f;

        r.sharedMaterial.SetFloat("_AdditiveLayerAlpha", _alpha);

        while (timeElapsed < duration)
        {
            _progress = Mathf.Lerp(1.0f, -1.0f, timeElapsed / duration);  // Smoothly interpolate between start and end values
            r.sharedMaterial.SetFloat("_Progress", _progress);  // Update the material's progress
            timeElapsed += Time.unscaledDeltaTime;  // Accumulate the time elapsed
            yield return null;  // Wait for the next frame
        }

        // Ensure the final progress value is exactly the target value
        _progress = doReset ? 1.0f : -1.0f;
        r.sharedMaterial.SetFloat("_Progress", _progress);

        timeElapsed = 0f;

        if (!doReset)
        {
            while (timeElapsed < duration)
            {
                _alpha = Mathf.Lerp(0.5f, 0.01f, timeElapsed / duration);  // Smoothly interpolate between start and end values
                r.sharedMaterial.SetFloat("_AdditiveLayerAlpha", _alpha);  // Update the material's progress
                timeElapsed += Time.unscaledDeltaTime * 2.0f;  // Accumulate the time elapsed
                yield return null;  // Wait for the next frame
            }
        }

        animcour = null;
    }
}
