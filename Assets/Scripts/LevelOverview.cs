using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelOverview : MonoBehaviour
{
    public List<Button> levelButtons;

    void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("LastLevel", 0);

        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i <= unlockedLevel)
            {
                int levelIndex = i + 1; 
                levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
                SetButtonState(levelButtons[i], true);
            }
            else
            {
                SetButtonState(levelButtons[i], false);
            }
        }
    }

    void SetButtonState(Button button, bool isUnlocked)
    {
        CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = isUnlocked;
            canvasGroup.blocksRaycasts = isUnlocked;
        }
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene("Level_" + levelIndex);
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ClearSafeState()
    {
        PlayerPrefs.SetInt("LastLevel", 0);
        SceneManager.LoadScene("LevelOverview");
    }
}
