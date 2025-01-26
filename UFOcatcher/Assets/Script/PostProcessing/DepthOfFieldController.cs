using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthOfFieldController : MonoBehaviour
{
    Ray raycast;
    RaycastHit hit;
    bool isHit;
    float hitDistance;

    [Range(1, 10)]
    public float focusSpeed = 1.0f;
    public float maxFocusDistance = 5.0f;

    public VolumeProfile volumeProfile;
    DepthOfField depthOfField;
    public GameObject UFOPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeProfile.TryGet(out depthOfField);
    }

    // Update is called once per frame
    void Update()
    {
        if(depthOfField.active)
            SetFocus();
    }

    void SetFocus()
    {
        depthOfField.focusDistance.value = Vector3.Distance(transform.position, 0.5f * (UFOPos.transform.position + new Vector3(UFOPos.transform.position.x, 0.0f, UFOPos.transform.position.z)));
    }

    public void ToggleDepthOfField(bool active)
    {
        depthOfField.active = active;
    }
}
