using AngleSharp.Dom;

namespace TDesign.Test.Components.Data;
public class CardTest : TestBase<TCard>
{
    [Fact(DisplayName ="Card - 默认渲染")]
    public void Test_Card_Render()
    {
        var component = GetComponent(m => m.AddChildContent("卡片内容"));
            component.Should().HaveTag("div")
            .And.HaveClass("t-card")
            .And.HaveClass("t-card--bordered")
            ;

        component.Find(".t-card__body").Html().Should().Be("卡片内容");
    }

    [Fact(DisplayName ="Card - 无边框")]
    public void Test_Card_Borderless()
    {
        GetComponent(m => m.Add(p => p.Borderless, true)).Should().NotHaveClass("t-card--bordered");
    }

    [Fact(DisplayName = "Card - 悬浮效果")]
    public void Test_Card_Hover()
    {
        GetComponent(m => m.Add(p => p.HoverShadow, true)).Should().NotHaveClass("t-card--shadow-horver");
    }

    [Fact(DisplayName ="Card - 头部标题")]
    public void Test_Card_HeadContent()
    {
        var component = GetComponent(m => m.Add(p => p.HeaderTitleContent, HtmlHelper.CreateContent("Header")));

        component.Find(".t-card__header>.t-card__header-wrapper>div>.t-card__title").Html().Should().Be("Header");
    }

    [Fact(DisplayName ="Card - 头部副标题")]
    public void Test_Card_HeadContent_SubTitle()
    {
        var component = GetComponent(m => m.Add(p => p.HeaderTitleContent, HtmlHelper.CreateContent("Header"))
                                            .Add(p=>p.HeaderSubTitleContent,HtmlHelper.CreateContent("SubTitle")));

        component.Find(".t-card__header>.t-card__header-wrapper>div>.t-card__subtitle").Html().Should().Be("SubTitle");
    }

    [Fact(DisplayName ="Card - 头部没有主标题，但是有副标题，则不显示")]
    public void Test_Card_HeaderContent_Without_HeadTitleContent_And_Set_SubTitleContent_Then_SubTitle_Dose_Not_Display()
    {
        var component = GetComponent(m => m.Add(p => p.HeaderSubTitleContent, HtmlHelper.CreateContent("SubTitle")));

        Throws<ElementNotFoundException>(() => component.Find(".t-card__header>.t-card__header-wrapper>div>.t-card__subtitle"));
    }

    [Fact(DisplayName = "Card - 头部描述")]
    public void Test_Card_HeadContent_Description()
    {
        var component = GetComponent(m => m.Add(p => p.HeaderTitleContent, HtmlHelper.CreateContent("Header"))
                                            .Add(p => p.HeaderDescriptionContent, HtmlHelper.CreateContent("Description")));

        component.Find(".t-card__header>.t-card__header-wrapper>div>.t-card__description").Html().Should().Be("Description");
    }

    [Fact(DisplayName = "Card - 头部没有主标题，但是有副标题，则不显示")]
    public void Test_Card_HeaderContent_Without_HeadTitleContent_And_Set_DescriptionContent_Then_Description_Dose_Not_Display()
    {
        var component = GetComponent(m => m.Add(p => p.HeaderDescriptionContent, HtmlHelper.CreateContent("Description")));

        Throws<ElementNotFoundException>(() => component.Find(".t-card__header>.t-card__header-wrapper>div>.t-card__description"));
    }

    [Fact(DisplayName ="Card - 头部操作内容")]
    public void Test_Card_HeaderAction()
    {
        var component = GetComponent(m => m.Add(p => p.HeaderActionContent, HtmlHelper.CreateContent("Action")));
        component.Find(".t-card__header>.t-card__actions").Html().Should().Be("Action");
    }

    [Fact(DisplayName ="Card - 头部分割线")]
    public void Test_Card_HeaderDivider()
    {
        var component = GetComponent(m => m.Add(p => p.HeaderTitleContent, HtmlHelper.CreateContent("Header"))
                                            .Add(p=>p.HeaderDivider,true));
        component.Find(".t-card__header").Should().HaveClass("t-card__title--bordered");
    }

    [Fact(DisplayName ="Card - 头部设置了分割线，但没有标题，期望无分割线")]
    public void Test_Card_HeaderDivier_Without_Set_HeaderTitleContent_Then_Header_Dose_Not_Display()
    {
        var component = GetComponent(m => m.Add(p => p.HeaderDivider, true));
        Throws<ElementNotFoundException>(()=> component.Find(".t-card__header"));
    }

    [Fact(DisplayName ="Card - 底部内容")]
    public void Test_Card_FooterContent()
    {
        var component = GetComponent(m => m.Add(p => p.FooterContent, HtmlHelper.CreateContent("Footer")));

        component.Find(".t-card__footer>.t-card__footer-wrapper").Html().Should().Be("Footer");
    }
}
