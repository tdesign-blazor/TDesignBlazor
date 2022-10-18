using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TDesign
{
    public class PopupOptions
    {
        [JsonConverter(typeof(EnumDescriptionConverter<PopupPlacement>))]
        [JsonPropertyName("placement")]
        public PopupPlacement Placement { get; set; } = PopupPlacement.Auto;

        [JsonIgnore]
        public Action<State> OnFirstUpdate { get; set; }

        [JSInvokable("CallOnFirstUpdate")]
        public void CallOnFirstUpdate(State state) => OnFirstUpdate?.Invoke(state);
    }

    public class State
    {
        [JsonConverter(typeof(EnumDescriptionConverter<PopupPlacement>))]
        [JsonPropertyName("placement")]
        public PopupPlacement Placement { get; set; }
    }

    public enum PopupPlacement
    {
        [Description("auto")] Auto,
        [Description("auto-start")] AutoStart,
        [Description("auto-end")] AutoEnd,
        [Description("top")] Top,
        [Description("top-start")] TopStart,
        [Description("top-end")] TopEnd,
        [Description("bottom")] Bottom,
        [Description("bottom-start")] BottomStart,
        [Description("bottom-end")] BottomEnd,
        [Description("right")] Right,
        [Description("right-start")] RightStart,
        [Description("right-end")] RightEnd,
        [Description("left")] Left,
        [Description("left-start")] LeftStart,
        [Description("left-end")] LeftEnd
    }
}
