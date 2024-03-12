using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CLobbyMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private TMP_InputField createInput;
    [SerializeField] private TMP_InputField joinInput;
    private void Awake()
    {
        this.createRoomButton.onClick.AddListener(CreateRoom);
        this.joinRoomButton.onClick.AddListener(JoinRoom);
    }

    private void OnDestroy()
    {
        this.createRoomButton.onClick.RemoveListener(CreateRoom);
        this.joinRoomButton.onClick.RemoveListener(JoinRoom);
    }

    private void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(3);
    }
}
