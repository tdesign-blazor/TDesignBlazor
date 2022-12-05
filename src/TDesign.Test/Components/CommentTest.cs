using ComponentBuilder;

namespace TDesign.Test.Components;
public class CommentTest : TestBase<TComment>
{
    [Fact(DisplayName = "Comment - 基本渲染")]
    public void Test_Render()
    {
        GetComponent(m => m.Add(p => p.Author, "作者")
                        .Add(p => p.Avatar, "avatar.jpg")
                        .Add(p => p.DateTime, "12:30")
                        .AddChildContent("内容")
        ).MarkupMatches(builder =>
        {
            builder.CreateElement(0, "div", content =>
            {
                content.CreateElement(0, "div", inner =>
                {
                    inner.CreateElement(0, "div", avatar =>
                    {
                        avatar.CreateElement(0, "img", attributes: new
                        {
                            src = "avatar.jpg",
                            @class = "t-comment__avatar-image"
                        });
                    }, new { @class = "t-comment__avatar" });

                    inner.CreateElement(1, "div", comment =>
                    {
                        comment.CreateElement(0, "div", author =>
                        {
                            author.CreateElement(1, "span", "作者", new { @class = "t-comment__name" });
                            author.CreateElement(2, "span", "12:30", new { @class = "t-comment__time" });

                        }, new { @class = "t-comment__author" });

                        comment.CreateElement(1, "div", "内容", new { @class = "t-comment__detail" });

                    }, new { @class = "t-comment__content" });

                }, new { @class = "t-comment__inner" });
            }, new { @class = "t-comment" });
        });
    }


    [Fact(DisplayName = "Comment - 带操作的评论")]
    public void Test_Comment_With_Operation()
    {
        GetComponent(m => m.Add(p => p.Author, "作者")
                        .Add(p => p.Avatar, "avatar.jpg")
                        .Add(p => p.DateTime, "12:30")
                        .AddChildContent("内容")
                        .Add(p => p.OperationContent, builder =>
                        {
                            builder.CreateElement(0, "li", "回复");
                        })
        ).MarkupMatches(builder =>
        {
            builder.CreateElement(0, "div", content =>
            {
                content.CreateElement(0, "div", inner =>
                {
                    inner.CreateElement(0, "div", avatar =>
                    {
                        avatar.CreateElement(0, "img", attributes: new
                        {
                            src = "avatar.jpg",
                            @class = "t-comment__avatar-image"
                        });
                    }, new { @class = "t-comment__avatar" });

                    inner.CreateElement(1, "div", comment =>
                    {
                        comment.CreateElement(0, "div", author =>
                        {
                            author.CreateElement(1, "span", "作者", new { @class = "t-comment__name" });
                            author.CreateElement(2, "span", "12:30", new { @class = "t-comment__time" });

                        }, new { @class = "t-comment__author" });

                        comment.CreateElement(1, "div", "内容", new { @class = "t-comment__detail" });

                        comment.CreateElement(2, "ul", operation =>
                        {
                            operation.CreateElement(0, "li", "回复");
                        }, new { @class = "t-comment__actions" });
                    }, new { @class = "t-comment__content" });

                }, new { @class = "t-comment__inner" });
            }, new { @class = "t-comment" });
        });
    }


    [Fact(DisplayName = "Comment - 带引用的评论")]
    public void Test_Comment_With_Quate()
    {
        GetComponent(m => m.Add(p => p.Author, "作者")
                        .Add(p => p.Avatar, "avatar.jpg")
                        .Add(p => p.DateTime, "12:30")
                        .AddChildContent("内容")
                        .Add(p => p.OperationContent, builder =>
                        {
                            builder.CreateElement(0, "li", "回复");
                        })
                        .Add(p => p.QuateContent, builder =>
                        {
                            builder.CreateComponent<TComment>(0, "引用内容", new
                            {
                                Author = "引用作者",
                                DateTime = "09:00"
                            });
                        })
        ).MarkupMatches(builder =>
        {
            builder.CreateElement(0, "div", content =>
            {
                content.CreateElement(0, "div", inner =>
                {
                    inner.CreateElement(0, "div", avatar =>
                    {
                        avatar.CreateElement(0, "img", attributes: new
                        {
                            src = "avatar.jpg",
                            @class = "t-comment__avatar-image"
                        });
                    }, new { @class = "t-comment__avatar" });

                    inner.CreateElement(1, "div", comment =>
                    {
                        comment.CreateElement(0, "div", author =>
                        {
                            author.CreateElement(1, "span", "作者", new { @class = "t-comment__name" });
                            author.CreateElement(2, "span", "12:30", new { @class = "t-comment__time" });

                        }, new { @class = "t-comment__author" });

                        comment.CreateElement(1, "div", "内容", new { @class = "t-comment__detail" });

                        comment.CreateElement(2, "div", quate =>
                        {
                            quate.CreateComponent<TComment>(0, "引用内容", new
                            {
                                Author = "引用作者",
                                DateTime = "09:00"
                            });
                        }, new { @class = "t-comment__quote" });

                        comment.CreateElement(2, "ul", operation =>
                        {
                            operation.CreateElement(0, "li", "回复");
                        }, new { @class = "t-comment__actions" });
                    }, new { @class = "t-comment__content" });

                }, new { @class = "t-comment__inner" });
            }, new { @class = "t-comment" });
        });
    }


    [Fact(DisplayName = "Comment - 带回复的评论")]
    public void Test_Comment_With_Reply()
    {
        GetComponent(m => m.Add(p => p.Author, "作者")
                        .Add(p => p.Avatar, "avatar.jpg")
                        .Add(p => p.DateTime, "12:30")
                        .AddChildContent("内容")
                        .Add(p => p.OperationContent, builder =>
                        {
                            builder.CreateElement(0, "li", "回复");
                        })
                        .Add(p => p.ReplyContent, builder =>
                        {
                            builder.CreateComponent<TComment>(0, "回复内容", new
                            {
                                Author = "回复作者",
                                DateTime = "09:00"
                            });
                        })
        ).MarkupMatches(builder =>
        {
            builder.CreateElement(0, "div", content =>
            {
                content.CreateElement(0, "div", inner =>
                {
                    inner.CreateElement(0, "div", avatar =>
                    {
                        avatar.CreateElement(0, "img", attributes: new
                        {
                            src = "avatar.jpg",
                            @class = "t-comment__avatar-image"
                        });
                    }, new { @class = "t-comment__avatar" });

                    inner.CreateElement(1, "div", comment =>
                    {
                        comment.CreateElement(0, "div", author =>
                        {
                            author.CreateElement(1, "span", "作者", new { @class = "t-comment__name" });
                            author.CreateElement(2, "span", "12:30", new { @class = "t-comment__time" });

                        }, new { @class = "t-comment__author" });

                        comment.CreateElement(1, "div", "内容", new { @class = "t-comment__detail" });

                        comment.CreateElement(2, "ul", operation =>
                        {
                            operation.CreateElement(0, "li", "回复");
                        }, new { @class = "t-comment__actions" });
                    }, new { @class = "t-comment__content" });

                }, new { @class = "t-comment__inner" });

                content.CreateElement(1, "div", reply =>
                {
                    reply.CreateComponent<TComment>(0, "回复内容", new
                    {
                        Author = "回复作者",
                        DateTime = "09:00"
                    });
                }, new { @class = "t-comment__reply" });
            }, new { @class = "t-comment" });
        });
    }
}
