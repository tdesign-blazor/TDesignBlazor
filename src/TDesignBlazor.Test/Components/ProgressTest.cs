using ComponentBuilder;

namespace TDesignBlazor.Test.Components;
public class ProgressTest : TestBase<Progress>
{
    [Fact(DisplayName = "进度条 - 渲染 50% 进度的进度条和样式")]
    public void Test_Progress_Default()
    {
        GetComponent(m => m.Add(p => p.Value, 50)).MarkupMatches(@"
<div class=""t-progress"">
    <div class=""t-progress--thin t-progress--status--undefined"">
        <div class=""t-progress__bar"">
            <div class=""t-progress__inner"" style=""width:50%""></div>
        </div>
        <div class=""t-progress__info""></div>
    </div>
</div>
");
    }

    [Fact(DisplayName = "进度条 - ShowLabel 参数")]
    public void Test_Progress_ShowLabel_Parameter()
    {
        GetComponent(m => m.Add(p => p.Value, 50).Add(p => p.ShowLabel, true)).MarkupMatches(@"
<div class=""t-progress"">
    <div class=""t-progress--thin t-progress--status--undefined"">
        <div class=""t-progress__bar"">
            <div class=""t-progress__inner"" style=""width:50%""></div>
        </div>
        <div class=""t-progress__info"">50%</div>
    </div>
</div>
");
    }

    [Fact(DisplayName = "进度条 - Plump 类型时，Value < 10 的样式")]
    public void Test_Progress_Plump_UnderTen_Label()
    {
        GetComponent(m => m.Add(p => p.Value, 5).Add(p => p.ShowLabel, true).Add(p => p.Theme, ProgressTheme.Plump)).MarkupMatches(@"
<div class=""t-progress"">
    <div class=""t-progress__bar t-progress--plump t-progress--status--undefined t-progress--under-ten"">
        <div class=""t-progress__inner"" style=""width:5%""></div>
        <div class=""t-progress__info"">5%</div>
    </div>
</div>
");
    }

    [Fact(DisplayName = "进度条 - Plump 类型时，Value >= 10 的样式")]
    public void Test_Progress_Plump_OverTen_Label()
    {
        GetComponent(m => m.Add(p => p.Value, 15).Add(p => p.ShowLabel, true).Add(p => p.Theme, ProgressTheme.Plump)).MarkupMatches(@"
<div class=""t-progress"">
    <div class=""t-progress__bar t-progress--plump t-progress--status--undefined t-progress--over-ten"">
        <div class=""t-progress__inner"" style=""width:15%"">
            <div class=""t-progress__info"">15%</div>
        </div>
    </div>
</div>
");
    }

    [Theory(DisplayName ="进度条 - Status 参数")]
    [InlineData(new object[] { Status.Warning })]
    [InlineData(new object[] { Status.Success })]
    [InlineData(new object[] { Status.Error })]
    [InlineData(new object[] { Status.Default })]
    public void Test_Progress_Status_Parameter(Status status)
    {
        GetComponent(m => m.Add(p => p.Value, 15).Add(p => p.ShowLabel, true).Add(p => p.Status, status)).MarkupMatches($@"
<div class=""t-progress"">
    <div class=""t-progress--thin t-progress--status--{status.ToString().ToLower()}"">
        <div class=""t-progress__bar"">
            <div class=""t-progress__inner"" style=""width:15%""></div>
        </div>
        <div class=""t-progress__info"">15%</div>
    </div>
</div>
");
    }

    [Fact(DisplayName ="进度条 - Circle 类型")]
    public void Test_Progress_Circle()
    {
        GetComponent(m => m.Add(p => p.Value, 30).Add(p => p.ShowLabel, true).Add(p => p.Theme, ProgressTheme.Circle).Add(p=>p.Size,Size.Small))
            .MarkupMatches($@"
<div class=""t-progress"">
    <div class=""t-progress--circle t-progress--status--undefined"" style=""width:72px;height:72px;font-size:14px;"">
        <div class=""t-progress__info"">30%</div>
        <svg width=""72"" height=""72"" viewBox=""0 0 72 72"">
            <circle cx=""36"" cy=""36"" r=""34"" stroke-width=""4"" fill=""none"" class=""t-progress__circle-outer""></circle>
            <circle cx=""36"" cy=""36"" r=""34"" stroke-width=""4"" fill=""none"" stroke-linecap=""round"" transform=""matrix(0,-1,1,0,0,72)"" stroke-dasharray=""64.08849013323179 214.62830044410595"" class=""t-progress__circle-inner""></circle>
        </svg>
    </div>
</div>
");
    }
}
