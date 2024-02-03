using System;
using UnityEngine;
namespace EduLab
{
    public class CConnector : MonoBehaviour
    {
        public CElectricalComponent electricalComponent { get; private set; }
        public CConnector connectedConnector { get; private set; }

        public Action<CConnector> OnConnect;
        public Action OnDisconnect;

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
        public void ForcedDisconect()
        {
            this.OnDisconnect?.Invoke();
            this.connectedConnector = null;
        }
        public void ForcedConnect(CConnector connectionConnector)
        {
            this.connectedConnector = connectionConnector;
            this.OnConnect?.Invoke(connectionConnector);
        }

        private void Connect(CConnector connectionConnector)
        {
            this.connectedConnector = connectionConnector;
            connectionConnector.ForcedConnect(this);

            this.OnConnect?.Invoke(connectionConnector);
        }

        public void Disconect()
        {
            this.OnDisconnect?.Invoke();

            if (this.connectedConnector != null)
            {
                this.connectedConnector.ForcedDisconect();
            }

            this.connectedConnector = null;
        }
    }
}