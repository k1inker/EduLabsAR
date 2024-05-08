using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CMainMenuManager : MonoBehaviourPunCallbacks
{
    [Header("Reference button")]
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button exitButton;

    [Header("Reference menu")]
    [SerializeField] private GameObject mainMenu;
    
    //===================//
    // UNITY METHODS
    //===================//
    private void Awake()
    {
        this.singlePlayerButton.onClick.AddListener(this.OnSingleplayerClick);
        this.multiplayerButton.onClick.AddListener(this.OnMultiplayerClick);
        this.exitButton.onClick.AddListener(this.OnExitClick);
    }
    private void OnDestroy()
    {
        this.singlePlayerButton.onClick.RemoveListener(this.OnSingleplayerClick);
        this.multiplayerButton.onClick.RemoveListener(this.OnMultiplayerClick);
        this.exitButton.onClick.RemoveListener(this.OnExitClick);
    }
    
    //===================//
    // PRIVATE METHODS
    //===================//
    
    private void OnSingleplayerClick()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom("");
        SceneManager.LoadScene("Game");
    }
    private void OnMultiplayerClick()
    {
        PhotonNetwork.OfflineMode = false;
        SceneManager.LoadScene("ConnectToServer");
    }
    private void OnExitClick()
    {
        Application.Quit();
    }
    
    //===================//
    // PUBLIC METHODS
    //===================//
}
