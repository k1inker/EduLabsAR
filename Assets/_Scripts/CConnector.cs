using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CConnector : MonoBehaviour
{
    [SerializeField] private CElectricalComponent electricalComponent;

    public CConnector connectedConnector { get; private set; }

    public Action<CConnector> OnConnect;
    public Action<CConnector> OnDisconnect;
    public bool TryConnect(CConnector connectionConnector)
    {
        if (connectionConnector != null && !this.electricalComponent.IsOurComponentConnector(connectionConnector))
        {
            if (connectionConnector.connectedConnector == null)
            {
                this.Connect(connectionConnector);
            }
            else
            {
                connectionConnector.Disconect();
                this.Connect(connectionConnector);
            }
            return true;
        }
        return false;
    }

    public void Connect(CConnector connectionConnector)
    {
        this.OnConnect?.Invoke(connectionConnector);
        this.connectedConnector = connectionConnector;
        connectionConnector.connectedConnector = this;
    }
    
    public void Disconect()
    {
        if(this.connectedConnector != null)
        {
            this.connectedConnector.ForcedDisconect();
        }

        this.OnDisconnect?.Invoke(this);
        this.connectedConnector = null;
    }
    
    public void ForcedDisconect()
    {
        this.OnDisconnect?.Invoke(this);
        this.connectedConnector = null;
    }
}
