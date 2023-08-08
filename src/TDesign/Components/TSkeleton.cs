using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 骨架屏。当网络较慢时，在页面真实数据加载之前，给用户展示出页面的大致结构。
/// </summary>
[ParentComponent]
[CssClass("t-skeleton")]
public class TSkeleton : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// 设置是否显示骨架屏。
    /// </summary>
    [ParameterApiDoc("是否显示骨架屏")]
    [Parameter][EditorRequired] public bool Loading { get; set; }
    /// <summary>
    /// 当 <see cref="Loading"/> 是 <c>false</c> 时显示的内容。
    /// </summary>
    [ParameterApiDoc("当 Loading 是 false 时显示的内容。")]
    [Parameter] public override RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 当 <see cref="Loading"/> 是 <c>true</c> 时显示的内容。
    /// </summary>
    [ParameterApiDoc("当 Loading 是 true 时显示的内容。")]
    [Parameter] public RenderFragment? LoadingContent { get; set; }

    /// <summary>
    /// 设置动画效果。
    /// </summary>
    [ParameterApiDoc("动画效果。")]
    [Parameter] public SkeletonAnimation? Animation { get; set; }


    /// <summary>
    /// 设置骨架屏的模式。可以快速设置骨架屏显示的模式。
    /// </summary>
    [ParameterApiDoc("骨架屏的模式。可以快速设置骨架屏显示的模式。", Value =$"{nameof(SkeletonTheme.Paragraph)}")]
    [Parameter] public SkeletonTheme? Theme { get; set; } = SkeletonTheme.Paragraph;

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (Theme is not null && LoadingContent is null)
        {
            LoadingContent = GetThemeContent();
        }
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        if (Loading)
        {
            builder.AddContent(sequence, LoadingContent);
        }
        else
        {
            base.AddContent(builder, sequence);
        }
    }

    /// <summary>
    /// 获取主题对应的骨架屏。
    /// </summary>
    RenderFragment GetThemeContent()
    => Theme switch
    {
        SkeletonTheme.Text => builder => builder.CreateComponent<TSkeletonRow>(0, col => col.CreateComponent<TSkeletonColumn>(0)),
        SkeletonTheme.Paragraph => builder =>
        {
            builder.CreateComponent<TSkeletonRow>(0, col => col.CreateComponent<TSkeletonColumn>(0));
            builder.CreateComponent<TSkeletonRow>(1, col => col.CreateComponent<TSkeletonColumn>(0));
            builder.CreateComponent<TSkeletonRow>(2, col => col.CreateComponent<TSkeletonColumn>(0));
        }
        ,
        SkeletonTheme.Avatar => builder => builder.CreateComponent<TSkeletonRow>(0, col => col.CreateComponent<TSkeletonColumn>(0, attributes: new { Type = SkeletonColumnType.Circle, style = "height:56px;width:56px" })),
        SkeletonTheme.AvatarText => builder => builder.CreateComponent<TSkeletonRow>(0, col =>
        {
            col.CreateComponent<TSkeletonColumn>(0, attributes: new { Type = SkeletonColumnType.Circle });
            col.CreateComponent<TSkeletonColumn>(1, attributes: new { style = "height:32px" });
        }),
        SkeletonTheme.Tab => builder =>
        {
            builder.CreateComponent<TSkeletonRow>(0, col => col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "height:30px" }));
            builder.CreateComponent<TSkeletonRow>(0, col => col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "height:200px" }));
        }
        ,
        SkeletonTheme.Article => builder =>
        {
            builder.CreateComponent<TSkeletonRow>(0, col => col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "width:100%;height:30px" }));
            builder.CreateComponent<TSkeletonRow>(0, col => col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "width:100%;height:200px" }));

            builder.CreateComponent<TSkeletonRow>(0, col =>
            {
                col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "height:30px" });
                col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "height:30px" });
                col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "height:30px" });
            });

            builder.CreateComponent<TSkeletonRow>(0, col =>
            {
                col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "height:30px" });
                col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "height:30px" });
            });

            builder.CreateComponent<TSkeletonRow>(0, col =>
            {
                col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "height:30px" });
                col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "height:30px" });
            });

            builder.CreateComponent<TSkeletonRow>(0, col =>
            {
                col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "height:30px" });
                col.CreateComponent<TSkeletonColumn>(0, attributes: new { style = "height:30px" });
            });
        }
        ,
        _ => builder => { }
    };
}
/// <summary>
/// 表示骨架屏中的行。
/// </summary>
[ChildComponent(typeof(TSkeleton))]
[ParentComponent]
[CssClass("t-skeleton__row")]
public class TSkeletonRow : TDesignComponentBase, IHasChildContent
{
    /// <inheritdoc/>
    [ParameterApiDoc("需要 TSkeletonColumn 组件的内容")]
    [Parameter] public RenderFragment? ChildContent { get; set; }

}

/// <summary>
/// 表示骨架屏中的行。
/// </summary>
[ChildComponent(typeof(TSkeleton))]
[ChildComponent(typeof(TSkeletonRow))]
[CssClass("t-skeleton__col")]
public class TSkeletonColumn : TDesignComponentBase
{

    /// <summary>
    /// 获取 <see cref="TSkeleton"/> 组件。
    /// </summary>
    [CascadingParameter] public TSkeleton? CascadingSkeleton { get; set; }

    /// <summary>
    /// 设置列的类型。
    /// </summary>
    [ParameterApiDoc("列的类型", Value =$"{nameof(SkeletonColumnType.Text)}")]
    [Parameter][CssClass("t-skeleton--type-")] public SkeletonColumnType Type { get; set; } = SkeletonColumnType.Text;

    /// <inheritdoc/>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append($"t-skeleton--animation-{CascadingSkeleton?.Animation?.GetCssClass()}", CascadingSkeleton?.Animation is not null);
    }
}
/// <summary>
/// 骨架屏列的类型。
/// </summary>
public enum SkeletonColumnType
{
    /// <summary>
    /// 长方形。
    /// </summary>
    [CssClass("rect")] Rectangle,
    /// <summary>
    /// 圆形。
    /// </summary>
    Circle,
    /// <summary>
    /// 文本。
    /// </summary>
    Text
}
/// <summary>
/// 骨架屏的动画效果。
/// </summary>
public enum SkeletonAnimation
{
    /// <summary>
    /// 渐变效果。
    /// </summary>
    Gradient,
    /// <summary>
    /// 闪烁效果。
    /// </summary>
    Flashed
}
/// <summary>
/// 骨架屏主题模式。
/// </summary>
public enum SkeletonTheme
{
    /// <summary>
    /// 文本。
    /// </summary>
    Text,
    /// <summary>
    /// 段落。
    /// </summary>
    Paragraph,
    /// <summary>
    /// 头像。
    /// </summary>
    Avatar,
    /// <summary>
    /// 头像文本。
    /// </summary>
    AvatarText,
    /// <summary>
    /// 选项卡。
    /// </summary>
    Tab,
    /// <summary>
    /// 长篇文章。
    /// </summary>
    Article
}