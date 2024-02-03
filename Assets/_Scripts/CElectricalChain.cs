using System.Collections.Generic;
using UnityEngine;

namespace EduLab
{
    public class CElectricalChain : MonoBehaviour
    {
        [SerializeField] private CElectricalComponent[] electricalComponents;

        [SerializeField] private СAccamulator accamulator;

        private List<CElectricalComponent> connectedComponentsWithChain = new List<CElectricalComponent>();
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
            }
        }
        private void Refresh()
        {
            List<CElectricalComponent> connectedComponents = CheckCicruit(this.accamulator.GetConnectedElectricalComponents(), null);
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
        private List<CElectricalComponent> CheckCicruit(List<CElectricalComponent> components, CElectricalComponent prevComponent)
        {
            if(components.Count < 2)
            {
                this.connectedComponentsWithChain.Clear();
                return this.connectedComponentsWithChain;
            }

            foreach(var component in components)
            {
                if(component.Equals(this.accamulator))
                {
                    return this.connectedComponentsWithChain;
                }

                if(component.Equals(prevComponent))
                {
                    continue;
                }

                this.connectedComponentsWithChain.Add(component);
                return CheckCicruit(component.GetConnectedElectricalComponents(), component);
            }

            return this.connectedComponentsWithChain;
        }
    }
}