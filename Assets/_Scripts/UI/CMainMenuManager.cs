using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        this.singleplayerButton.onClick.AddListener(this.OnSingleplayerClick);
        this.multiplayerButton.onClick.AddListener(this.OnMultiplayerClick);
        this.backToMenuButton.onClick.AddListener(this.OnBackToMenuClick);
        this.exitButton.onClick.AddListener(this.OnExitClick);
    }
    private void OnDestroy()
    {
        this.singleplayerButton.onClick.RemoveListener(this.OnSingleplayerClick);
        this.multiplayerButton.onClick.RemoveListener(this.OnMultiplayerClick);
        this.backToMenuButton.onClick.RemoveListener(this.OnBackToMenuClick);
        this.exitButton.onClick.RemoveListener(this.OnExitClick);
    }
    private void OnSingleplayerClick()
    {
        this.mainMenu.SetActive(false);
        this.levelSelectMenu.SetActive(true);
    }
    private void OnMultiplayerClick()
    {
        SceneManager.LoadScene(1);
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
