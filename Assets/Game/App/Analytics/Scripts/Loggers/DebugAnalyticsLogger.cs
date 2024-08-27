using System.Text;
using UnityEngine;

namespace Game.App
{
    public sealed class DebugAnalyticsLogger : IAnalyticsLogger
    {
        private readonly string colorHtml;

        private readonly StringBuilder stringBuilder;
        
        public DebugAnalyticsLogger(Color color)
        {
            colorHtml = ColorUtility.ToHtmlStringRGBA(color);
            stringBuilder = new StringBuilder();
        }

        public void LogEvent(string eventName, params AnalyticsParameter[] parameters)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                eventName = AnalyticsConst.UNDEFINED;
            }
            
            stringBuilder.Clear();
            stringBuilder
                .Append("Log Event: ")
                .Append($"<color=#{colorHtml}>")
                .Append($"{eventName}");

            if (parameters is {Length: > 0})
            {
                stringBuilder.Append(", parameters: ");
                foreach (var parameter in parameters)
                {
                    stringBuilder.Append($"(key: {parameter.name}, value: {parameter.value})");
                }
            }

            stringBuilder.Append("</color>");
            
            Debug.Log(stringBuilder.ToString());
        }
    }
}