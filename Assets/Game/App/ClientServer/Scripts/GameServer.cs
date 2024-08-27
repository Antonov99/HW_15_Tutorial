using System.Text;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Game.App
{
    public sealed class GameServer
    {
        private readonly string url;
        private readonly int port;

        public GameServer(string url, int port)
        {
            this.url = url;
            this.port = port;
        }

        public UnityWebRequest Get(string route)
        {
            var url = CombineUrl(route);
            return UnityWebRequest.Get(url);
        }

        public UnityWebRequest Post(string route, object body)
        {
            var bodyJson = JsonConvert.SerializeObject(body);
            return Post(route, bodyJson);
        }

        public UnityWebRequest Post(string route, string bodyJson)
        {
            var url = CombineUrl(route);
            var request = UnityWebRequest.Post(url, "POST");
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyJson));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        public UnityWebRequest Put(string route, string data)
        {
            var url = CombineUrl(route);
            var request = UnityWebRequest.Put(url, data);
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        private string CombineUrl(string route)
        {
            return $"{url}:{port}/{route}";
        }
    }
}