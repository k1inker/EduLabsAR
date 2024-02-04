using UnityEngine;
using UnityEngine.UI;

public class CMainMenuManager : MonoBehaviour
{
    [Header("Reference button")]
    [SerializeField] private Button singleplayerButton;
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button[] startLevelButtons;

    [Header("Reference menu")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelectMenu;
    private void Awake()
    {
        singleplayerButton.onClick.AddListener(OnSingleplayerClick);
        multiplayerButton.onClick.AddListener(OnMultiplayerClick);
        backToMenuButton.onClick.AddListener(OnBackToMenuClick);
        exitButton.onClick.AddListener(OnExitClick);
    }
    private void OnDestroy()
    {
        singleplayerButton.onClick.RemoveListener(OnSingleplayerClick);
        multiplayerButton.onClick.RemoveListener(OnMultiplayerClick);
        backToMenuButton.onClick.RemoveListener(OnBackToMenuClick);
        exitButton.onClick.RemoveListener(OnExitClick);
    }
    private void OnSingleplayerClick()
    {
        this.mainMenu.SetActive(false);
        this.levelSelectMenu.SetActive(true);
    }
    private void OnMultiplayerClick()
    {

    }
    private void OnExitClick()
    {
        Application.Quit();
    }
    private void OnBackToMenuClick()
    {
        this.levelSelectMenu.SetActive(false);
        this.mainMenu.SetActive(true);
    }
}
