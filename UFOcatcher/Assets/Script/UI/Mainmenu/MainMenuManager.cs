using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private string firstGameSceneName = "SampleScene";

    [SerializeField]
    private string tutorialSceneName = "TutorialScene";

    [SerializeField]
    private string creditsSceneName = "Credits";

    [SerializeField]
    private string menuSceneName = "MainMenu";

    private string targetSceneName;

    private void Start()
    {

    }

    public void OnClickStart()
    {
        // Set target scene to the tutorial scene
        targetSceneName = firstGameSceneName;

        // Call start game
        if (GameUtility.FadingUIExists()) 
        {
            FadingUI.Instance.StartFadeIn();
            FadingUI.Instance.OnStopFading.AddListener(LoadTargetScene);
        }

    }

    public void ClickToGoToCredit()
    {
        // Set target scene to the credits scene
        targetSceneName = creditsSceneName;

        // Call start game
        FadingUI.Instance.StartFadeIn();
        FadingUI.Instance.OnStopFading.AddListener(LoadTargetScene);
    }

    public void ClickToMainMenu()
    {
        // Set target scene to the main menu scene
        targetSceneName = menuSceneName;

        // Call start game
        FadingUI.Instance.StartFadeIn();
        FadingUI.Instance.OnStopFading.AddListener(LoadTargetScene);
    }

    private void OnPartDestroyed(string sceneName)
    {
        // Set target scene based on which event is triggered
        targetSceneName = sceneName;

        FadingUI.Instance.StartFadeIn();
        FadingUI.Instance.OnStopFading.AddListener(LoadTargetScene);
    }

    private void LoadTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }

    public void ClickToExit()
    {
        Application.Quit();
    }

}
