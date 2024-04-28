using System.Collections.Generic;
using UnityEngine;

namespace EduLab
{
    public class CElectricalChain : MonoBehaviour
    {
        [SerializeField] private CElectricalComponent[] electricalComponents;

        [SerializeField] private СAccamulator accamulator;
        //===================//
        // UNITY METHODS
        //===================//
        private void Start()
        {
            foreach(var component in this.electricalComponents)
            {
                component.Init();
            }
            this.accamulator.Init();
            this.accamulator.OnShortingCircuit += RefreshStatusCicruit;
        }
        private void OnDestroy()
        {
            foreach (var component in this.electricalComponents)
            {
                component.OnConnect -= Refresh;
                component.OnDisconnect -= Refresh;
            }
            this.accamulator.OnDisconnect -= Refresh;
        }
        //===================//
        // PRIVATE METHODS
        //===================//
        // for oprimisation subscribe methods
        private void RefreshStatusCicruit(bool isShorting)
        {
            if(isShorting)
            {
                foreach (var component in this.electricalComponents)
                {
                    component.OnConnect += Refresh;
                    component.OnDisconnect += Refresh;
                }
                this.Refresh();
                return;
            }

            foreach (var component in this.electricalComponents)
            {
                component.OnConnect -= Refresh;
                component.OnDisconnect -= Refresh;
                component.Unpowered();
            }
        }
        private void Refresh()
        {
            List<CElectricalComponent> connectedComponentsWithChain = new List<CElectricalComponent>();
            List<CElectricalComponent> connectedComponents = CheckCircuit(
                this.accamulator.GetConnectedElectricalComponents(), 
                null, 
                connectedComponentsWithChain);
            if (connectedComponents.Count > 0)
            {
                foreach(var electricalComponent in this.electricalComponents)
                {
                    if(connectedComponents.Contains(electricalComponent))
                    {
                        electricalComponent.Powered();
                        continue;
                    }
                    electricalComponent.Unpowered();
                }
            }
            else
            {
                foreach (var electricalComponent in this.electricalComponents)
                {
                    electricalComponent.Unpowered();
                }
            }
        }
        private List<CElectricalComponent> CheckCircuit(
            List<CElectricalComponent> components, 
            CElectricalComponent prevComponent,
            List<CElectricalComponent> visited)
        {
            if(components.Count < 2)
            {
                return new List<CElectricalComponent>();
            }

            foreach(var component in components)
            {
                if(component.Equals(this.accamulator))
                {
                    visited.Add(component);
                    return visited;
                }

                if(component.Equals(prevComponent) || visited.Contains(component))
                {
                    continue;
                }

                visited.Add(component);
                var result = this.CheckCircuit(component.GetConnectedElectricalComponents(), component, visited);
                if (result.Count > 0)
                {
                    return result; // Якщо замкнення знайдено
                }
                visited.Remove(component);
            }
            
            return new List<CElectricalComponent>();
        }
    }
}