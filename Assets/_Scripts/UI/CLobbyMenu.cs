using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CLobbyMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject lobbyMenu;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private TMP_InputField createInput;
    [SerializeField] private TMP_InputField joinInput;
    private void Awake()
    {
        this.createRoomButton.onClick.AddListener(this.CreateRoom);
        this.joinRoomButton.onClick.AddListener(this.JoinRoom);
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
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }
}
