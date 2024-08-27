using Newtonsoft.Json;
using Services;

namespace Game.App
{
    public abstract class GameMediator<TData, TGameService> : IGameMediator
    {
        protected virtual string DataKey
        {
            get { return typeof(TData).Name; }
        }

        [ServiceInject]
        private GameFacade gameFacade;

        void IGameMediator.SetupData(GameRepository repository)
        {
            var service = gameFacade.GetService<TGameService>();

            if (repository.TryGetData(DataKey, out var json))
            {
                var data = JsonConvert.DeserializeObject<TData>(json);
                SetupFromData(service, data);
            }
            else
            {
                SetupByDefault(service);
            }
        }

        void IGameMediator.SaveData(GameRepository repository)
        {
            var service = gameFacade.GetService<TGameService>();
            var data = ConvertToData(service);
            var json = JsonConvert.SerializeObject(data);
            repository.SetData(DataKey, json);
        }

        protected abstract void SetupFromData(TGameService service, TData data);

        protected abstract void SetupByDefault(TGameService service);

        protected abstract TData ConvertToData(TGameService service);
    }
}