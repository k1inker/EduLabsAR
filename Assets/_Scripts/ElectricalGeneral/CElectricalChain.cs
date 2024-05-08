using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EduLab
{
    public class CElectricalChain : MonoBehaviour
    {
        public static CElectricalChain Instance { get; private set; }
        
        [SerializeField] private CElectricalComponent[] electricalComponents;
        [SerializeField] private СAccamulator accamulator;

        private float timeToShowGameFinish = 3f;

        public Action OnFinishGame;
        
        //===================//
        // UNITY METHODS
        //===================//
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

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
                this.CheckPoweredElectricalComponents();
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

        private void CheckPoweredElectricalComponents()
        {
            if (this.electricalComponents.All(component => component.IsPowered))
            {
                this.OnFinishGame?.Invoke();
                StartCoroutine(this.StartShowGameFinish());
            }
        }
        private IEnumerator StartShowGameFinish()
        {
            yield return new WaitForSeconds(this.timeToShowGameFinish);
            CGameMenu.Instance.ShowGameFinishMenu();
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