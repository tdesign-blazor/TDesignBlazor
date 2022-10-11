namespace TDesignBlazor.Test.Components
{
    public class SliderTest:TestBase<Slider>
    {
        [Fact(DisplayName ="Slider - 默认渲染值是12的滑块")]
        public void Test_Render_With_Value_Is_12()
        {
            var markup = @"
<div class=""t-slider__container"" aria-valuetext=""12"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""100"" aria-orientation=""horizontal"" class=""t-slider"">
        <div class=""t-slider__rail"">
            <div class=""t-slider__track"" style=""width:12%;left:0%;""></div>
            <div tabindex=""0"" show-tooltip=""true"" class=""t-slider__button-wrapper"" style=""left:12%;"">
                <div class=""t-slider__button""></div>
            </div>
        </div>
    </div>
</div>
";
            GetComponent(m => m.Add(p => p.Value, 12)).MarkupMatches(markup);
        }

        [Fact(DisplayName ="Slider - 默认渲染值是 30,70 的滑块")]
        public void Test_Render_With_Value_Is_30_70()
        {
            var markup = @"
<div class=""t-slider__container"" aria-valuetext=""30-70"">
    <div role=""slider"" aria-valuemin=""0"" aria-valuemax=""100"" aria-orientation=""horizontal"" class=""t-slider"">
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
            GetComponent(m => m.Add(p => p.Value, (30,70))).MarkupMatches(markup);
        }
    }
}
