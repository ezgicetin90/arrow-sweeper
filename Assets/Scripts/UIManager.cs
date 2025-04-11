using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject inGameUI;
    
    public GameObject levelCompletePanel;
    public GameObject levelFailPanel;

    private void Start()
    {
        ShowMainMenu(); // Show the main menu at game start
    }

    public void OnPlayButtonPressed()
    {
        mainMenuUI.SetActive(false);
        inGameUI.SetActive(true);
        GameManager.Instance.StartLevel(); // We’ll add this function soon
    }

    public void ShowMainMenu()
    {
        inGameUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
    
    public void ShowLevelComplete()
    {
        inGameUI.SetActive(false);
        levelCompletePanel.SetActive(true);
    }

    public void OnNextLevelButtonPressed()
    {
        levelCompletePanel.SetActive(false);
        inGameUI.SetActive(true);

        LevelManager.Instance.LoadNextLevel();
    }
    
    public void ShowLevelFail()
    {
        inGameUI.SetActive(false);
        levelFailPanel.SetActive(true);
    }

    public void OnRetryButtonPressed()
    {
        levelFailPanel.SetActive(false);
        inGameUI.SetActive(true);

        GameManager.Instance.StartLevel();
    }
    
}