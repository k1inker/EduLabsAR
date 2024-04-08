using System.Collections;
using Photon.Pun;
using UnityEngine;

public class СARSpawnerNetwork : MonoBehaviourPunCallbacks
{
    [SerializeField] private CNickNameController nickNamePlayerPrefab;

    [SerializeField] private Transform cameraTransport;

    private void Start()
    {
        StartCoroutine(this.Connect());
    }

    private IEnumerator Connect()
    {
        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }
        GameObject arPlayer = PhotonNetwork.Instantiate(nickNamePlayerPrefab.gameObject.name, Vector3.zero, Quaternion.identity);
        
        arPlayer.transform.SetParent(cameraTransport, false);
        
        this.nickNamePlayerPrefab.SetNickName(PhotonNetwork.NickName);
    }
}
