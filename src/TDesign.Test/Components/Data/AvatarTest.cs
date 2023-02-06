using ComponentBuilder;

namespace TDesign.Test.Components.Data;
public class AvatarTest : TestBase<TAvatar>
{
    [Fact(DisplayName = "Avatar - 渲染 img 元素和默认样式")]
    public void Test_Render_Avatar()
    {
        GetComponent().Should().HaveTag("div").And.HaveClass("t-avatar");
    }

    [Fact(DisplayName = "Avatar - Icon 参数")]
    public void Test_Icon_Parameter()
    {
        GetComponent(p => p.Add(m => m.Icon, IconName.Add))
            .Should().HaveClass("t-avatar__icon")
            .And.HaveChildMarkup("<i class=\"t-icon t-icon-add\" />")
            ;
    }

    [Fact(DisplayName = "Avatar - Url 参数")]
    public void Test_Url_Parameter()
    {
        GetComponent(m => m.Add(p => p.Url, "img.baidu.com"))
            .Should().HaveChildMarkup("<img src=\"img.baidu.com\"/>");
    }

    [Fact(DisplayName = "Avatar - ChildContent 参数")]
    public void Test_ChildContent_Parameter()
    {
        GetComponent(m => m.AddChildContent("text"))
            .Should().HaveChildMarkup("<span style=\"transform:scale(1)\">text</span>");
    }

    [Fact(DisplayName = "Avatar - Size 参数")]
    public void Test_Size_Parameter()
    {
        GetComponent(m => m.Add(p => p.Size, Size.Small)).Should().HaveClass("t-size-s");
        GetComponent(m => m.Add(p => p.Size, Size.Medium)).Should().HaveClass("t-size-m");
        GetComponent(m => m.Add(p => p.Size, Size.Large)).Should().HaveClass("t-size-l");
    }

    [Fact(DisplayName = "AvatarGroup - 渲染头像组")]
    public void Test_Render_AvatarGroup_With_Avatars()
    {
        TestContext.RenderComponent<TAvatarGroup>(m => m.AddChildContent(p => p.CreateComponent<TAvatar>(0)))
            .Should().HaveClass("t-avatar-group").And.HaveClass("t-avatar--offset-left")
            .And.HaveChildMarkup("<div class=\"t-avatar t-avatar--circle\"></div>")
            ;
    }

    [Fact(DisplayName = "AvatarGroup - 渲染头像组统一大小")]
    public void Test_Render_AvatarGroup_Size_Parapeter_With_Avatars()
    {
        var group = TestContext.RenderComponent<TAvatarGroup>(m => m.AddChildContent(p => p.CreateComponent<TAvatar>(0)).Add(p => p.Size, Size.Large));


        group.Should().HaveClass("t-avatar-group");

        group.FindComponent<TAvatar>().Should().HaveClass("t-size-l");
    }

    [Fact(DisplayName = "AvatarGroup - Left 参数")]
    public void Test_AvatarGroup_Left_Parameter()
    {
        TestContext.RenderComponent<TAvatarGroup>(m => m.Add(p => p.Left, true))
            .Should().HaveClass("t-avatar--offset-right");
    }
}
