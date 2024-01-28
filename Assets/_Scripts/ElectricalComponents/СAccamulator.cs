using System.Collections.Generic;
using UnityEngine;
public class СAccamulator : CElectricalComponent
{
    protected override void ConnectToElecticalComponent(CConnector connectedConnector)
    {
        if(connector1.connectedConnector != null && connector2.connectedConnector != null)
        {
            var connectionList = connector1.connectedConnector.electricalComponent.CheckConnectedElectricalChain(this, this, new List<CElectricalComponent>());
            foreach(var connection in connectionList)
            {
                Debug.Log(connection);
            }
        }
    }

    protected override void DisconnectElectricalComponent(CConnector connectedConnector)
    {
        
    }
}
