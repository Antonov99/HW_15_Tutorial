using System;
using Services;

namespace Game.App
{
    public sealed class LoadingTask_InitApplicationServices : ILoadingTask
    {
        private readonly IAppInitListener[] services;

        [ServiceInject]
        public LoadingTask_InitApplicationServices(IAppInitListener[] services)
        {
            this.services = services;
        }

        public void Do(Action<LoadingResult> callback)
        {
            for (int i = 0, count = services.Length; i < count; i++)
            {
                var service = services[i];
                service.Init();
            }

            callback?.Invoke(LoadingResult.Success());
        }
    }
}