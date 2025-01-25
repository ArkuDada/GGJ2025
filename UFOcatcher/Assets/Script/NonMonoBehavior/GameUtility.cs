using UnityEngine;

public static class GameUtility
{
    public static bool FadingUIExists()
    {
        return FadingUI.Instance != null;
    }

    public static bool GameManagerExists()
    {
        return GameManager.Instance != null;
    }

    // Other utility methods
}