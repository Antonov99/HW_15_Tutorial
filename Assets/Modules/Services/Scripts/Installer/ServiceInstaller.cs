using UnityEngine;
using UnityEngine.Serialization;

namespace Services
{
    public sealed class ServiceInstaller : MonoBehaviour
    {
        [SerializeField]
        private bool installOnAwake;

        [SerializeField]
        private bool resolveDependencies;   

        [Space, SerializeField]
        private MonoBehaviour[] monoServices;

        [Space, SerializeField, FormerlySerializedAs("serviceLoaders")]
        private ServicePackBase[] servicePacks;

        private void Awake()
        {
            if (installOnAwake)
            {
                Install();
            }
        }

        public void Install()
        {
            InstallServicesFromBehaviours();
            InstallServicesFromPacks();

            if (resolveDependencies)
            {
                ServiceInjector.ResolveDependencies();
            }
        }

        private void InstallServicesFromBehaviours()
        {
            ServiceLocator.AddServices(monoServices);
        }

        private void InstallServicesFromPacks()
        {
            for (int i = 0, count = servicePacks.Length; i < count; i++)
            {
                var pack = servicePacks[i];
                var services = pack.ProvideServices();
                ServiceLocator.AddServices(services);
            }
        }
    }
}