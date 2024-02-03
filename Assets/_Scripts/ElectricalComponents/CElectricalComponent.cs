﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace EduLab
{
    public abstract class CElectricalComponent : MonoBehaviour
    {
        [SerializeField] protected CConnector connector1;
        [SerializeField] protected CConnector connector2;

        public Action OnConnect;
        public Action OnDisconnect;
        public void Init()
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
        public List<CElectricalComponent> GetConnectedElectricalComponents()
        {
            List<CElectricalComponent> connectedComponents = new List<CElectricalComponent>();
            if (connector1.connectedConnector != null)
            {
                connectedComponents.Add(connector1.connectedConnector.electricalComponent);
            }
            if(connector2.connectedConnector != null)
            {
                connectedComponents.Add(connector2.connectedConnector.electricalComponent);
            }
            return connectedComponents;
        }
        protected virtual void ConnectToElecticalComponent(CConnector connectedConnector)
        {
            this.OnConnect?.Invoke();
        }
        protected virtual void DisconnectElectricalComponent()
        {
            this.OnDisconnect?.Invoke();
        }
        public abstract void Powered();
        public abstract void Unpowered();
    }
}