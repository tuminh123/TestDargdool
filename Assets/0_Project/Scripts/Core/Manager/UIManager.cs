using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject settingMenuPanel;
    [SerializeField] private GameObject gameInstructionsPanel;

    //get
    public GameObject MenuCanvas => menuCanvas;
    public GameObject PlayCanvas => playerCanvas;
    public GameObject GameOverPanel => gameOverPanel;
    public GameObject SettingPanel => settingPanel;
    public GameObject SettingMenuPanel => settingMenuPanel;
    public GameObject GameInstructionsPanel => gameInstructionsPanel;

    private void Start()
    {
        settingMenuPanel.SetActive(false);
        gameInstructionsPanel.SetActive(false);
    }

    public void SetUI(GameState currentState)
    {
        menuCanvas?.SetActive(currentState == GameState.MENU);
        playerCanvas?.SetActive(currentState == GameState.PLAY);
        gameOverPanel?.SetActive(currentState == GameState.LOSE);
        settingPanel?.SetActive(currentState == GameState.PAUSE);
    }
}
