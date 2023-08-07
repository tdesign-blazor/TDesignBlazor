namespace TDesign;

/// <summary>
/// 评论用于对页面内容的反馈、评价、讨论等，如对文章的评价，对话题的讨论。
/// </summary>
[CssClass("t-comment")]
public class TComment : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// 设置头像的 uri 地址。
    /// </summary>
    [ParameterApiDoc("头像的 uri 地址")]
    [Parameter] public string? Avatar { get; set; }
    /// <summary>
    /// 设置作者的名称。
    /// </summary>
    [ParameterApiDoc("作者的名称")]
    [Parameter] public string? Author { get; set; }
    /// <summary>
    /// 设置显示的日期时间字符串。
    /// </summary>
    [ParameterApiDoc("显示的日期时间字符串")]
    [Parameter] public string? DateTime { get; set; }
    /// <summary>
    /// 设置评论的操作内容。每一个操作需要用 li 标记进行渲染。
    /// </summary>
    [ParameterApiDoc("评论的操作内容。每一个操作需要用 li 标记进行渲染")]
    [Parameter] public RenderFragment? OperationContent { get; set; }
    /// <summary>
    /// 设置当前评论的回复内容。
    /// </summary>
    [ParameterApiDoc("当前评论的回复内容")]
    [Parameter] public RenderFragment? ReplyContent { get; set; }
    /// <summary>
    /// 设置评论内容的引用内容。
    /// </summary>
    [ParameterApiDoc("评论内容的引用内容")]
    [Parameter] public RenderFragment? QuateContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(0, "div", content =>
        {
            BuildAvatar(builder, 0);

            BuildCommentContent(content, 1);



        }, new { @class = "t-comment__inner" });

        builder.CreateElement(2, "div", ReplyContent!, new { @class = "t-comment__reply" }, ReplyContent is not null);
    }

    /// <summary>
    /// 构建头像。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    private void BuildAvatar(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", avatar =>
        {
            builder.CreateElement(0, "img", attributes: new
            {
                src = Avatar,
                @class = "t-comment__avatar-image"
            });
        }, new { @class = "t-comment__avatar" }, !string.IsNullOrEmpty(Avatar));
    }
    /// <summary>
    /// 构建评论内容
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    private void BuildCommentContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateElement(0, "div", author =>
            {
                author.CreateElement(0, "span", Author, new { @class = "t-comment__name" }, !string.IsNullOrEmpty(Author));
                author.CreateElement(1, "span", DateTime, new { @class = "t-comment__time" }, !string.IsNullOrEmpty(DateTime));
            }, new { @class = "t-comment__author" });

            content.CreateElement(1, "div", ChildContent!, new { @class = "t-comment__detail" }, ChildContent is not null);
            content.CreateElement(2, "div", QuateContent!, new { @class = "t-comment__quote" }, QuateContent is not null);
            content.CreateElement(3, "ul", OperationContent!, new { @class = "t-comment__actions" }, OperationContent is not null);
        }, new { @class = "t-comment__content" });
    }
}
