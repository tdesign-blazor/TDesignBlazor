using AngleSharp.Dom;
using System.Net.Mime;
using TDesign.Abstractions;

namespace TDesign.Test.Components.Data;
public class TableTest:TestBase<TTable<TableTest.TestData>>
{
    [Fact(DisplayName = "Table - 空数据表格")]
    public void Test_EmptyTable()
    {
        var table = RenderComponent(m => m.Add(p => p.ChildContent, b =>
        {
            b.CreateComponent<TTableFieldColumn<TestData,string>>(0, attributes: new { Field = nameof(TestData.Id) });
        }).Add(p=>p.Data, DataSource<TestData>.Empty));

        table.Find("tr.t-table__empty-row").Should().NotBeNull();
        table.Find("tr.t-table__empty-row>td>div.t-table__empty");
    }

    [Fact(DisplayName = "Table - 自定义空数据表格")]
    public void Test_EmptyTable_Customize_EmptyContent()
    {
        var table = RenderComponent(m => m.Add(p => p.ChildContent,(b =>
        {
            b.CreateComponent<TTableFieldColumn<TestData,string>>(0, attributes: new { Field = nameof(TestData.Id) });
        }))
        .Add(p => p.Data,DataSource<TestData>.Empty)
        .Add(p=>p.EmptyContent,builder=>builder.AddContent(0,"个性化空数据"))
        );

        table.Find("tr.t-table__empty-row").Should().NotBeNull();
        table.Find("tr.t-table__empty-row>td>div.t-table__empty").Html().Should().Be("个性化空数据");
    }

    //[Fact(DisplayName ="Table - 呈现4条数据")]
    //public void Test_Table_With_Data()
    //{
    //    var table = RenderComponent(m => m.Add(p => p.ChildContent, b =>
    //    {
    //        b.CreateComponent<TTableFieldColumn<TestData,int>>(0, attributes: new { Field =p=>p.Id) });
    //        b.CreateComponent<TTableFieldColumn<TestData>>(0, attributes: new { Field = nameof(TestData.Name) });
    //        b.CreateComponent<TTableFieldColumn<TestData>>(0, attributes: new { Field = nameof(TestData.Birthday) });
    //        b.CreateComponent<TTableFieldColumn<TestData>>(0, attributes: new { Field = nameof(TestData.Gender) });
    //    })))
    //    .Add(p => p.Data, DataSource<TestData>.Parse(TestData.GetData())));
    //    table.Should().NotBeNull();
    //    table.Find(".t-table__body>tr").ChildElementCount.Should().Be(4);
    //}

    //[Fact(DisplayName ="Table - 自定义表底模板")]
    //public void Test_Table_FooterContent()
    //{
    //    var table = RenderComponent(m => m.Add(p => p.ChildContent, new RenderFragment<TestData>(value => new RenderFragment(b =>
    //    {
    //        b.CreateComponent<TTableFieldColumn<TestData>>(0, attributes: new { Field = nameof(TestData.Id) });
    //    })))
    //    .Add(p => p.Data, DataSource<TestData>.Parse(TestData.GetData()))
    //    .Add(p => p.FooterContent, builder => builder.AddContent(0, "表底数据"))
    //    );

    //    table.Find("tfoot.t-table__footer").Should().NotBeNull();
    //    table.Find("tfoot>tr.t-table__row--full").Should().NotBeNull();
    //}

    //[Fact(DisplayName = "Table - 自定义列的表底模板")]
    //public void Test_TableColumn_FooterContent()
    //{
    //    var table = RenderComponent(m => m.Add(p => p.ChildContent, new RenderFragment<TestData>(value => new RenderFragment(b =>
    //    {
    //        b.CreateComponent<TTableFieldColumn<TestData>>(0, attributes: new {
    //            Field = nameof(TestData.Id),
    //            FooterContent =HtmlHelper.CreateContent(b=>b.AddContent(0,"列1")) });
    //    })))
    //    .Add(p => p.Data, DataSource<TestData>.Parse(TestData.GetData()))
    //    );

    //    table.Find("tfoot.t-table__footer").Should().NotBeNull();
    //    table.Find("tfoot>tr.t-tdesign__custom-footer-tr").Should().NotBeNull();
    //}

    public class TestData
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Gender { get; set; }
        public DateTime? Birthday { get; set; }

        public static IEnumerable<TestData> GetData()
        {
            yield return new TestData { Id = 1, Name = "张三", Gender = true, Birthday = new DateTime(1990, 5, 6) };
            yield return new TestData { Id = 2, Name = "李四", Gender = false, Birthday = new DateTime(1975, 1, 12) };
            yield return new TestData { Id = 3, Name = "王五", Gender = true, Birthday = new DateTime(1996, 9, 26) };
            yield return new TestData { Id = 4, Name = "赵六", Gender = false, Birthday = new DateTime(1999, 4, 10) };
            yield return new TestData { Id = 5, Name = "钱七", Gender = true, Birthday = new DateTime(1955, 8, 15) };
        }
    }

}

