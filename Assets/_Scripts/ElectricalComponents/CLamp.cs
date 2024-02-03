

using UnityEngine;

namespace EduLab
{
    public class CLamp : CElectricalComponent
    {
        public override void Powered()
        {
            Debug.Log($"Connected {gameObject.name}");
        }

        public override void Unpowered()
        {
            Debug.Log($"Disconnected {gameObject.name}");
        }

        protected override void ConnectToElecticalComponent(CConnector connectedConnector)
        {

        }

        protected override void DisconnectElectricalComponent()
        {

        }
    }
}