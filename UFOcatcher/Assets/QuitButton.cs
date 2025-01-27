using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Quit()
    {
#if UNITY_EDITOR
        // Update is called once per frame
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        // Quit the application
        Application.Quit();
    }
}