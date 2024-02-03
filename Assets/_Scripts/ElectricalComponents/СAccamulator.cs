using System;
using UnityEngine;

namespace EduLab
{
    public class СAccamulator : CElectricalComponent
    {
        public Action<bool> OnShortingCircuit;

        public override void Powered()
        {
            Debug.Log("Connected chain");
        }

        public override void Unpowered()
        {
            Debug.Log("Disconnect Chain");
        }

        protected override void ConnectToElecticalComponent(CConnector connectedConnector)
        {
            if (connector1.connectedConnector != null && connector1.connectedConnector != null)
            {
                this.OnShortingCircuit.Invoke(true);
            }
        }
        protected override void DisconnectElectricalComponent()
        {
            this.OnShortingCircuit.Invoke(false);
        }
    }
}