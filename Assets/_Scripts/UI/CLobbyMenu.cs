using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CLobbyMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject lobbyMenu;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private TMP_InputField createInput;
    [SerializeField] private TMP_InputField joinInput;

    [Header("NickName Menu")] 
    [SerializeField] private GameObject nickNameMenu;
    [SerializeField] private TMP_InputField nickNameInputField;
    [SerializeField] private Button setNickButton;
    private void Awake()
    {
        this.createRoomButton.onClick.AddListener(this.CreateRoom);
        this.joinRoomButton.onClick.AddListener(this.JoinRoom);
        this.setNickButton.onClick.AddListener(this.SetNewNick);
    }

    private void OnDestroy()
    {
        this.createRoomButton.onClick.RemoveListener(this.CreateRoom);
        this.joinRoomButton.onClick.RemoveListener(this.JoinRoom);
    }

    private void CreateRoom()
    {
        PhotonNetwork.CreateRoom(this.createInput.text);
    }

    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom(this.joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(3);
        this.nickNameInputField.text = PhotonNetwork.NickName;
    }

    private void SetNewNick()
    {
        PhotonNetwork.NickName = this.nickNameInputField.text;
        this.nickNameMenu.SetActive(false);
        this.lobbyMenu.SetActive(true);
    }
}
