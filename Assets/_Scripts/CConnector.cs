using System;
using UnityEngine;

public class CConnector : MonoBehaviour
{
    public CElectricalComponent electricalComponent {  get; private set; }
    public CConnector connectedConnector { get; private set; }

    public Action<CConnector> OnConnect;
    public Action<CConnector> OnDisconnect;

    public void Init(CElectricalComponent electricalComponent)
    {
        this.electricalComponent = electricalComponent;
    }
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
        this.connectedConnector = connectionConnector;
        connectionConnector.connectedConnector = this;

        this.OnConnect?.Invoke(connectionConnector);
    }
    
    public void Disconect()
    {
        if(this.connectedConnector != null)
        {
            this.connectedConnector.ForcedDisconect();
        }

        this.OnDisconnect?.Invoke(this.connectedConnector);
        this.connectedConnector = null;
    }
    
    public void ForcedDisconect()
    {
        this.OnDisconnect?.Invoke(this.connectedConnector);
        this.connectedConnector = null;
    }
}
