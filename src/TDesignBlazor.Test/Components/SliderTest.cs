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
        <div class=""t-slider__rail"" style="""">
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
        <div class=""t-slider__rail"" style="""">
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
            <div class=""t-slider__rail"" style=""height:100%"">
                <div class=""t-slider__track"" style=""height:12%;bottom:0%;""></div>
                <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""bottom:12%;"">
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
            <div class=""t-slider__rail"" style=""height:100%"">
                <div class=""t-slider__track"" style=""height:40%;bottom:30%;""></div>
                <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""bottom:30%;"">
                    <div class=""t-slider__button""></div>
                </div>
                <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""bottom:70%;"">
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
        <div class=""t-slider__rail t-is-disabled"" style="""">
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
        <div class=""t-slider__rail t-is-disabled"" style="""">
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
        <div class=""t-slider__rail"" style="""">
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
            GetComponent(m => m.Add(p => p.Value, 12).Add(p => p.Marks, new ()
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
        <div class=""t-slider__rail"" style="""">
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
            GetComponent(m => m.Add(p => p.Value, (30, 70)).Add(p => p.Marks, new ()
            {
                [0D] = "0°C",
                [20D] = "20°C",
                [40D] = "40°C",
                [60D] = "60°C",
                [80D] = "80°C",
                [100D] = "100°C"
            })).MarkupMatches(markup2);
        }


        [Fact(DisplayName = "Slider - 单个值 Min 参数")]
        public void Given_Single_Value_When_Set_Min()
        {
            var markup = @"
<div class=""t-slider__container"" aria-valuetext=""50"">
    <div role=""slider"" aria-valuemin=""30"" aria-valuemax=""100"" aria-orientation=""horizontal"" class=""t-slider"">
        <div class=""t-slider__rail"" style="""">
            <div class=""t-slider__track"" style=""width:50%;left:0%""></div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:50%"">
                <div class=""t-slider__button""></div>
            </div>
        </div>
    </div>
</div>
";
            GetComponent(m => m.Add(p => p.Value, 50).Add(p=>p.Min,30)).MarkupMatches(markup);
        }
        [Fact(DisplayName = "Slider - 单个值 Max 参数")]
        public void Given_Single_Value_When_Set_Max()
        {
            var markup = @"
<div class=""t-slider__container"" aria-valuetext=""50"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""70"" aria-orientation=""horizontal"" class=""t-slider"">
        <div class=""t-slider__rail"" style="""">
            <div class=""t-slider__track"" style=""width:50%;left:0%""></div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:50%"">
                <div class=""t-slider__button""></div>
            </div>
        </div>
    </div>
</div>
";
            GetComponent(m => m.Add(p => p.Value, 50).Add(p => p.Max, 70)).MarkupMatches(markup);
        }

        [Fact(DisplayName = "Slider - 单个值 Min 参数")]
        public void Given_Multiple_Value_When_Set_Min()
        {
            var markup = @"
<div class=""t-slider__container"" aria-valuetext=""30-70"">
    <div role=""slider"" aria-valuemin=""20"" aria-valuemax=""100"" aria-orientation=""horizontal"" class=""t-slider"">
        <div class=""t-slider__rail"" style="""">
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
            GetComponent(m => m.Add(p => p.Value, (30, 70)).Add(p => p.Min, 20)).MarkupMatches(markup);
        }

        [Fact(DisplayName = "Slider - 单个值 Max 参数")]
        public void Given_Multiple_Value_When_Set_Max()
        {
            var markup = @"
<div class=""t-slider__container"" aria-valuetext=""30-70"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""80"" aria-orientation=""horizontal"" class=""t-slider"">
        <div class=""t-slider__rail"" style="""">
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
            GetComponent(m => m.Add(p => p.Value, (30, 70)).Add(p => p.Max, 80)).MarkupMatches(markup);
        }


        [Fact(DisplayName = "Slider - 单个 Value 值大于 100 抛出 InvalidOperationException")]
        public void When_Value_More_Than_100_Then_Throw_InvalidOperationException()
        {
            Throws<InvalidOperationException>(() => GetComponent(m => m.Add(p => p.Value, 200)));
        }

        [Fact(DisplayName = "Slider - 单个 Value 值小于 0 抛出 InvalidOperationException")]
        public void When_Value_Less_Than_0_Then_Throw_InvalidOperationException()
        {
            Throws<InvalidOperationException>(() => GetComponent(m => m.Add(p => p.Value, -50)));
        }

        [Fact(DisplayName = "Slider - 2个 Value 值，最大值大于 100 抛出 InvalidOperationException")]
        public void Given_2_Value_When_Max_Value_More_Than_100_Then_Throw_InvalidOperationException()
        {
            Throws<InvalidOperationException>(() => GetComponent(m => m.Add(p => p.Value, (50, 200))));
        }

        [Fact(DisplayName = "Slider - 2个 Value 值，最小值小于 0 抛出 InvalidOperationException")]
        public void Given_2_Value_When_Min_Value_Less_Than_0_Then_Throw_InvalidOperationException()
        {
            Throws<InvalidOperationException>(() => GetComponent(m => m.Add(p => p.Value, (-50, 50))));
        }

        [Fact(DisplayName = "Slider - 2个 Value 值，最小值大于最大值，抛出 InvalidOperationException")]
        public void Given_2_Value_When_Min_Value_More_Than_Max_Value_Then_Throw_InvalidOperationException()
        {
            Throws<InvalidOperationException>(() => GetComponent(m => m.Add(p => p.Value, (80, 50))));
        }

        [Fact(DisplayName ="Slider - 单个值，Value 小于 Min 抛出 InvalidOperationException")]
        public void Given_Single_Value_When_Value_Less_Than_Min_Then_Throw_InvalidOperationException()
        {
            Throws<InvalidOperationException>(() => GetComponent(m => m.Add(p => p.Value, 10).Add(p => p.Min, 30)));
        }
        [Fact(DisplayName = "Slider - 单个值，Value 大于 Max 抛出 InvalidOperationException")]
        public void Given_Single_Value_When_Value_More_Than_Max_Then_Throw_InvalidOperationException()
        {
            Throws<InvalidOperationException>(() => GetComponent(m => m.Add(p => p.Value, 60).Add(p => p.Max, 30)));
        }
        [Fact(DisplayName = "Slider - 多个值，Value 的 MinValue 小于 Min 抛出 InvalidOperationException")]
        public void Given_Multiple_Value_When_MinValue_Less_Than_Min_Then_Throw_InvalidOperationException()
        {
            Throws<InvalidOperationException>(() => GetComponent(m => m.Add(p => p.Value, (10,80)).Add(p => p.Min, 30)));
        }
        [Fact(DisplayName = "Slider - 多个值，Value 的 MaxValue 大于 Max 抛出 InvalidOperationException")]
        public void Given_Multiple_Value_When_MaxValue_More_Than_Max_Then_Throw_InvalidOperationException()
        {
            Throws<InvalidOperationException>(() => GetComponent(m => m.Add(p => p.Value, (10, 80)).Add(p => p.Max, 30)));
        }
        [Fact(DisplayName = "Slider - Min 大于 Max 抛出 InvalidOperationException")]
        public void When_Max_Less_Than_Min_Then_Throw()
        {
            Throws<InvalidOperationException>(() => GetComponent(m => m.Add(p => p.Max, 30).Add(p=>p.Min,50)));
        }
    }
}
