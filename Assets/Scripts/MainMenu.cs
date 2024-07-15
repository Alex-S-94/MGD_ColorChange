using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void LoadLevelOverview()
    {
        SceneManager.LoadScene("LevelOverview");
    }

    public void ContinueGame()
    {
        int lastLevel = PlayerPrefs.GetInt("LastLevel", 1);
        SceneManager.LoadScene("Level_" + lastLevel);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        // Editor-spezifisches Verhalten: Stoppe den Play-Modus
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_ANDROID
        // Beende die App auf Android
        AndroidJavaObject activity = new AndroidJavaObject("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = activity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("finish");
        #else
        // Standardmäßiges Beenden für andere Plattformen
        Application.Quit();
#endif
    }
}

