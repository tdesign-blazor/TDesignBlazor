using Microsoft.AspNetCore.Components;
using OneOf;
using System.Reflection.Emit;

namespace TDesignBlazor.Test.Components
{
    public class SliderTest : TestBase<Slider>
    {
        [Fact(DisplayName = "Slider - 默认渲染值是12的滑块")]
        public void Test_Render_With_Value_Is_12()
        {
            var markup = @"
<div class=""t-slider__container"" aria-valuetext=""12"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""100"" aria-orientation=""horizontal"" class=""t-slider"">
        <div class=""t-slider__rail"">
            <div class=""t-slider__track"" style=""width:12%;left:0%""></div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:12%"">
                <div class=""t-slider__button""></div>
            </div>
        </div>
    </div>
</div>
";
            GetComponent(m => m.Add(p => p.Value, 12)).MarkupMatches(markup);
        }

        [Fact(DisplayName = "Slider - 默认渲染值是 30,70 的滑块")]
        public void Test_Render_With_Value_Is_30_70()
        {
            var markup = @"
<div class=""t-slider__container"" aria-valuetext=""30-70"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""100"" aria-orientation=""horizontal"" class=""t-slider"">
        <div class=""t-slider__rail"">
            <div class=""t-slider__track"" style=""width:40%;left:30%""></div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:30%"">
                <div class=""t-slider__button""></div>
            </div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:70%"">
                <div class=""t-slider__button""></div>
            </div>
        </div>
    </div>
</div>
";
            GetComponent(m => m.Add(p => p.Value, (30, 70))).MarkupMatches(markup);
        }


        [Fact(DisplayName = "Slider - 垂直显示")]
        public void Test_Vertical()
        {
            var markup1 = @"
<div class=""t-slider__container is-vertical"" aria-valuetext=""12"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""100"" aria-orientation=""vertical"" class=""t-slider is-vertical t-slider--vertical"">
        <div class=""t-slider__rail"">
            <div class=""t-slider__track"" style=""width:12%;left:0%;""></div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:12%;"">
                <div class=""t-slider__button""></div>
            </div>
        </div>
    </div>
</div>
";
            GetComponent(m => m.Add(p => p.Value, 12).Add(p => p.Vertical, true)).MarkupMatches(markup1);


            var markup2 = @"
<div class=""t-slider__container is-vertical"" aria-valuetext=""30-70"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""100"" aria-orientation=""vertical"" class=""t-slider is-vertical t-slider--vertical"">
        <div class=""t-slider__rail"">
            <div class=""t-slider__track"" style=""width:40%;left:30%;""></div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:30%;"">
                <div class=""t-slider__button""></div>
            </div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:70%;"">
                <div class=""t-slider__button""></div>
            </div>
        </div>
    </div>
</div>
";
            GetComponent(m => m.Add(p => p.Value, (30, 70)).Add(p => p.Vertical, true)).MarkupMatches(markup2);
        }

        [Fact(DisplayName = "Slider - 禁用状态")]
        public void Test_Disabled()
        {
            var markup1 = @"
<div class=""t-slider__container"" aria-valuetext=""12"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""100"" aria-orientation=""horizontal"" class=""t-slider t-is-disabled"">
        <div class=""t-slider__rail t-is-disabled"">
            <div class=""t-slider__track"" style=""width:12%;left:0%;""></div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" disabled=""disabled"" style=""left:12%;"">
                <div class=""t-slider__button""></div>
            </div>
        </div>
    </div>
</div>
";
            GetComponent(m => m.Add(p => p.Value, 12).Add(p => p.Disabled, true)).MarkupMatches(markup1);


            var markup2 = @"
<div class=""t-slider__container"" aria-valuetext=""30-70"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""100"" aria-orientation=""horizontal"" class=""t-slider  t-is-disabled"">
        <div class=""t-slider__rail t-is-disabled"">
            <div class=""t-slider__track"" style=""width:40%;left:30%;""></div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" disabled=""disabled"" style=""left:30%;"">
                <div class=""t-slider__button""></div>
            </div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" disabled=""disabled"" style=""left:70%;"">
                <div class=""t-slider__button""></div>
            </div>
        </div>
    </div>
</div>
";
            GetComponent(m => m.Add(p => p.Value, (30, 70)).Add(p => p.Disabled, true)).MarkupMatches(markup2);
        }

        [Fact(DisplayName = "Slider - 带刻度 1 个 Value 值")]
        public void Test_Marks_With_Single_Value()
        {
            var markup1 = @"
<div class=""t-slider__container"" aria-valuetext=""12"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""100"" aria-orientation=""horizontal"" class=""t-slider"">
        <div class=""t-slider__rail"">
            <div class=""t-slider__track"" style=""width:12%;left:0%;""></div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:12%;"">
                <div class=""t-slider__button""></div>
            </div>
            <div>
                <div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:0%;""></div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:20%;""></div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:40%;""></div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:60%;""></div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:80%;""></div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:100%;""></div>
                </div>
                <div class=""t-slider__mark"">
                    <div class=""t-slider__mark-text"" style=""left:0%;"">0°C</div>
                    <div class=""t-slider__mark-text"" style=""left:20%;"">20°C</div>
                    <div class=""t-slider__mark-text"" style=""left:40%;"">40°C</div>
                    <div class=""t-slider__mark-text"" style=""left:60%;"">60°C</div>
                    <div class=""t-slider__mark-text"" style=""left:80%;"">80°C</div>
                    <div class=""t-slider__mark-text"" style=""left:100%;"">100°C</div>
                </div>
            </div>
        </div>
    </div>
</div>
";
            GetComponent(m => m.Add(p => p.Value, 12).Add(p => p.Marks, new Dictionary<double, OneOf<string?, RenderFragment?, MarkupString?>>
            {
                [0D] = "0°C",
                [20D] = "20°C",
                [40D] = "40°C",
                [60D] = "60°C",
                [80D] = "80°C",
                [100D] = "100°C"
            })).MarkupMatches(markup1);


        }

        [Fact(DisplayName = "Slider - 带刻度的 2 个 Value 值")]
        public void Test_Marks_With_Two_Value()
        {

            var markup2 = @"
<div class=""t-slider__container"" aria-valuetext=""30-70"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""100"" aria-orientation=""horizontal"" class=""t-slider "">
        <div class=""t-slider__rail"">
            <div class=""t-slider__track"" style=""width:40%;left:30%;""></div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:30%;"">
                <div class=""t-slider__button""></div>
            </div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:70%;"">
                <div class=""t-slider__button""></div>
            </div>
            <div>
                <div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:0%;""></div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:20%;""></div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:40%;""></div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:60%;""></div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:80%;""></div>
                    <div class=""t-slider__stop t-slider__mark-stop"" style=""left:100%;""></div>
                </div>
                <div class=""t-slider__mark"">
                    <div class=""t-slider__mark-text"" style=""left:0%;"">0°C</div>
                    <div class=""t-slider__mark-text"" style=""left:20%;"">20°C</div>
                    <div class=""t-slider__mark-text"" style=""left:40%;"">40°C</div>
                    <div class=""t-slider__mark-text"" style=""left:60%;"">60°C</div>
                    <div class=""t-slider__mark-text"" style=""left:80%;"">80°C</div>
                    <div class=""t-slider__mark-text"" style=""left:100%;"">100°C</div>
                </div>
            </div>
        </div>
    </div>
</div>
";
            GetComponent(m => m.Add(p => p.Value, (30, 70)).Add(p => p.Marks, new Dictionary<double, OneOf<string?, RenderFragment?, MarkupString?>>
            {
                [0D] = "0°C",
                [20D] = "20°C",
                [40D] = "40°C",
                [60D] = "60°C",
                [80D] = "80°C",
                [100D] = "100°C"
            })).MarkupMatches(markup2);
        }
    }
}
