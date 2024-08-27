using System;
using System.Collections.Generic;

namespace GameNodes
{
    public sealed class GameContext : GameNode
    {
        private void Awake()
        {
            Install();
        }

        private void OnEnable()
        {
            Send<GameInit>();
            Send<GameStart>();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Send<GamePause>();
            }
            else
            {
                Send<GameResume>();
            }
        }

        private void OnDisable()
        {
            Send<GameFinish>();
        }

        // public async void Start()
        // {
        //     await this.InstallAsync();
        //     Debug.Log("Installed Async!");
        //     await this.CallAsync<GameInit>();
        //     Debug.Log("Inited Game!");
        //     await this.CallAsync<GameStart>();
        //     Debug.Log("Started Game!");
        // }

        protected override IEnumerable<object> LoadServices()
        {
            yield return new InputSystem();
        }
    }
}