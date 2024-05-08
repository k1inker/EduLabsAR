

using UnityEngine;

namespace EduLab
{
    public class CLamp : CElectricalComponent
    {
        [SerializeField] private Light pointLight;

        private const int offLightIntensity = 0;
        private const int onLightIntensity = 1;
        public override void Init()
        {
            base.Init();
            
            this.pointLight.intensity = offLightIntensity;
        }

        public override void Powered()
        {
            base.Powered();
            this.pointLight.intensity = onLightIntensity;
        }

        public override void Unpowered()
        {
            base.Unpowered();
            this.pointLight.intensity = offLightIntensity;
        }
    }
}