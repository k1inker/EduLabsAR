using System.Collections.Generic;
using UnityEngine;

public abstract class CElectricalComponent : MonoBehaviour
{
    [SerializeField] protected CConnector connector1;
    [SerializeField] protected CConnector connector2;

    private Dictionary<CConnector, CElectricalComponent> connectedComponentsDictionary = new Dictionary<CConnector, CElectricalComponent>();
    public void Awake()
    {
        this.connector1.Init(this);
        this.connector2.Init(this);
    }
    public void Start()
    {
        this.connector1.OnConnect += ConnectToElecticalComponent;
        this.connector2.OnConnect += ConnectToElecticalComponent;
        this.connector1.OnDisconnect += DisconnectElectricalComponent;
        this.connector2.OnDisconnect += DisconnectElectricalComponent;
    }
    private void OnDisable()
    {
        this.connector1.OnConnect -= ConnectToElecticalComponent;
        this.connector2.OnConnect -= ConnectToElecticalComponent;
        this.connector1.OnDisconnect -= DisconnectElectricalComponent;
        this.connector2.OnDisconnect -= DisconnectElectricalComponent;
    }
    public bool IsOurComponentConnector(CConnector chekenConnector)
    {
        if (connector1.Equals(chekenConnector) || connector2.Equals(chekenConnector))
        {
            return true;
        }
        return false;
    }
    protected abstract void ConnectToElecticalComponent(CConnector connectedConnector);
    protected abstract void DisconnectElectricalComponent(CConnector connectedConnector);
    public List<CElectricalComponent> CheckConnectedElectricalChain(CElectricalComponent signalingComponent, СAccamulator accamulator, List<CElectricalComponent> connectionList)
    {
        if ((connector1.electricalComponent.Equals(signalingComponent)
                && connector2.electricalComponent == null)
            || (connector1.electricalComponent == null
                && connector2.electricalComponent.Equals(signalingComponent)))
        {
            return null;
        }

        connectionList.Add(this);

        if(connector1.electricalComponent.Equals(accamulator) || connector2.electricalComponent.Equals(accamulator))
        {
            return connectionList;
        }
        
        if(!connector1.electricalComponent.Equals(signalingComponent))
        {
            return connector1.electricalComponent.CheckConnectedElectricalChain(signalingComponent, accamulator, connectionList);
        }
        else
        {
            return connector2.electricalComponent.CheckConnectedElectricalChain(this, accamulator, connectionList);
        }
    }
}
