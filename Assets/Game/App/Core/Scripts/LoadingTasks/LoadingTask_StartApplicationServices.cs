using System;
using Services;

namespace Game.App
{
    public sealed class LoadingTask_StartApplicationServices : ILoadingTask
    {
        private readonly IAppStartListener[] services;

        [ServiceInject]
        public LoadingTask_StartApplicationServices(IAppStartListener[] services)
        {
            this.services = services;
        }

        public void Do(Action<LoadingResult> callback)
        {
            for (int i = 0, count = services.Length; i < count; i++)
            {
                var service = services[i];
                service.Start();
            }

            callback?.Invoke(LoadingResult.Success());
        }
    }
}