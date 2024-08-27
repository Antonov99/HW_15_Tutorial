using System;
using Services;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.App
{
    public sealed class LoadingTask_InstallApplicationServices : ILoadingTask
    {
        public void Do(Action<LoadingResult> callback)
        {
            var serviceInstaller = Object.FindObjectOfType<ServiceInstaller>();
            serviceInstaller.Install();
            callback.Invoke(LoadingResult.Success());
        }
    }
}