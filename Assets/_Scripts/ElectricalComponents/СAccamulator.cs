using System;

namespace EduLab
{
    public class СAccamulator : CElectricalComponent
    {
        public Action<bool> OnShortingCircuit;
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