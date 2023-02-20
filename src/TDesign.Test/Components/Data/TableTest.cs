using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign.Test.Components.Data
{
    public class TableTest:TestBase<TTable<TableTestData>>
    {
        [Fact(DisplayName = "Table - 空数据表格")]
        public void Test_EmptyTable()
        {
            var table = GetComponent(m => m.Add(p => p.ChildContent, new RenderFragment<TableTestData>(value => new RenderFragment(b =>
            {
                b.CreateComponent<FieldColumn<TableTestData>>(0, attributes: new { Value = 1 });
            }))));
            table.Find("tr.t-table__empty-row");
            table.Find("tr.t-table__empty-row>td>div.t-table__empty");
        }

        [Fact(DisplayName = "Table - 自定义空数据表格")]
        public void Test_EmptyTable_Customize_EmptyContent()
        {
            var table = GetComponent(m => m.Add(p => p.ChildContent, new RenderFragment<TableTestData>(value => new RenderFragment(b =>
            {
                b.CreateComponent<FieldColumn<TableTestData>>(0, attributes: new { Value = 1 });
            }))).Add(p=>p.EmptyContent,builder=>builder.AddContent(0,"个性化空数据")));
            table.Find("tr.t-table__empty-row");
            table.Find("tr.t-table__empty-row>td>div.t-table__empty").Html().Should().Be("个性化空数据");
        }
    }


    public class TableTestData
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Gender { get; set; }
        public DateTime? Birthday { get; set; }

        public static IEnumerable<TableTestData> GetData()
        {
            yield return new TableTestData { Id = 1, Name = "张三", Gender = true, Birthday = new DateTime(1990, 5, 6) };
            yield return new TableTestData { Id = 2, Name = "李四", Gender = false, Birthday = new DateTime(1975, 1, 12) };
            yield return new TableTestData { Id = 3, Name = "王五", Gender = true, Birthday = new DateTime(1996, 9, 26) };
            yield return new TableTestData { Id = 4, Name = "赵六", Gender = false, Birthday = new DateTime(1999, 4, 10) };

        }
    }
}
