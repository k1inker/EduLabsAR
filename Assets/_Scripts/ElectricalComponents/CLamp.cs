

using UnityEngine;

namespace EduLab
{
    public class CLamp : CElectricalComponent
    {
        [SerializeField] private Light pointLight;

        private const int offLightIntensity = 0;
        private const int onLightIntensity = 5;
        public override void Init()
        {
            base.Init();
            
            this.pointLight.intensity = offLightIntensity;
        }

        public override void Powered()
        {
            this.pointLight.intensity = onLightIntensity;
        }

        public override void Unpowered()
        {
            this.pointLight.intensity = offLightIntensity;
        }

        protected override void ConnectToElecticalComponent(CConnector connectedConnector)
        {

        }

        protected override void DisconnectElectricalComponent()
        {

        }
    }
}