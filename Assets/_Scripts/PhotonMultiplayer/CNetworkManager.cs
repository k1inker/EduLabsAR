using Photon.Pun;
using UnityEngine;

public class CNetworkManager : MonoBehaviourPunCallbacks
{
    private CNetworkManager instance;

    public CNetworkManager Instance
    {
        get { return this.instance; }
    }
    /////////////////////
    /// UNITY METHODS
    /////////////////////
    private void Awake()
    {
        this.instance = this;
    }
    
    /////////////////////
    /// PUBLIC METHODS
    /////////////////////
}
