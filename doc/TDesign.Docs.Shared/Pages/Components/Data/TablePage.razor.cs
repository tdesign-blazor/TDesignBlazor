using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TDesign.Docs.Shared.Pages.Components.Data;
partial class TablePage
{
    [Inject] IJSRuntime JS { get; set; }
    public class User
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }

        public static List<User> GetData(int max=5)
        {
            var names = "赵钱孙李周吴郑王冯陈";
            var number = "一二三四五六七八九十";




            var list=new List<User>();
            for ( int i = 0; i < max; i++ )
            {
                var random = new Random().Next(0, max);
                var index = random.ToString().Length switch
                {
                    1 => random,
                    2 => random % 10,
                    3 => random % 100,
                    4 => random % 1000,
                    _ => 0
                };

                var name = names[index];
                var num = number[index];


                var age = new Random().Next(10, 60);

                list.Add(new() { Id = i + 1, Name = $"{name}{num}", Age = age, Gender = age % 2 == 0 });
            }
            return list;
        }
    }

    public static async Task<(IEnumerable<User> data,int count)> Mock(int take,int skip)
    {
        await Task.Delay(1000);
        var result= User.GetData(50);
        var count = result.Count;
        var data = result.Take(take).Skip(skip);
        return (data,count);
    }

    async Task RowSelected(TTableRowItemEventArgs<User> e) 
        => await JS.InvokeVoidAsync("alert", e.Item is null?"你什么都没选":$"你选择的是：{e.Item?.Name}");
}
