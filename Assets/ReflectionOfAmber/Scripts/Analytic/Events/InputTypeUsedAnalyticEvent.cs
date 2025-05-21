using System.Collections.Generic;

namespace ReflectionOfAmber.Scripts.Analytic.Events
{
    public class InputTypeUsedAnalyticEvent : IAnalyticEvent
    {
        public string EventName => "InputTypeUsed";
        public Dictionary<string, object> Data { get; }

        public InputTypeUsedAnalyticEvent(InputTypeUsed inputTypeUsed)
        {
            Data = new Dictionary<string, object>
            {
                {"InputType", inputTypeUsed.ToString()},
            };
        }
    }

    public enum InputTypeUsed
    {
        MOUSE_CLICK,
        ARROW_UI_CLICK,
        KEBOARD_CLICK,
    }
}