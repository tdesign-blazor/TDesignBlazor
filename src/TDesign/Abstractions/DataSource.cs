using OneOf.Types;

namespace TDesign;
/// <summary>
/// 表示数据源对象。支持静态和动态的数据检索。
/// </summary>
/// <typeparam name="T">要加载的数据类型。</typeparam>
public class DataSource<T> : OneOfBase<IEnumerable<T>, Func<int, int, Task<(IEnumerable<T> data, int count)>>>
{
    /// <summary>
    /// 表示无任何数据的数据源。
    /// </summary>
    public readonly static DataSource<T> Empty = Parse(Enumerable.Empty<T>());

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSource{T}"/> class.
    /// </summary>
    /// <param name="input">数据源类型。</param>
    protected DataSource(OneOf<IEnumerable<T>, Func<int, int, Task<(IEnumerable<T> data, int count)>>> input) : base(input)
    {
    }


    /// <summary>
    /// 对数据源进行检索。
    /// </summary>
    /// <param name="take">要获取的数据量。</param>
    /// <param name="skip">要跳过的数据量。</param>
    public Task<(IEnumerable<T>, int)> Query(int take, int skip)
        => Match(a =>
        {
            var data = a.Take(take).Skip(skip);
            var count = !a.Any() ? 1 : a.Count();
            return Task.FromResult<(IEnumerable<T> result, int count)>(new(data, count));
        }, b => b.Invoke(take, skip));

    /// <summary>
    /// 转换 List 为 <see cref="DataSource{T}"/> 的数据源。
    /// </summary>
    /// <param name="value">要转换的列表。</param>
    public static implicit operator DataSource<T>(List<T> value) =>Parse(value);
    /// <summary>
    /// 转换一个委托为 <see cref="DataSource{T}"/> 的数据源。
    /// </summary>
    /// <param name="value">一个可以用于查询的委托。</param>
    public static implicit operator DataSource<T>(Func<int, int, Task<(IEnumerable<T> data, int count)>> value) => Parse(value);

    /// <summary>
    /// 转换 <see cref="IEnumerable{T}"/> 为 <see cref="DataSource{T}"/> 的数据源。
    /// </summary>
    /// <param name="value">可迭代的结果。</param>
    public static DataSource<T> Parse(IEnumerable<T> value) => new(OneOf<IEnumerable<T>, Func<int , int, Task<(IEnumerable<T> data, int count)>>>.FromT0(value));


    /// <summary>
    /// 转换具备2个参数和相同返回值的方法为 <see cref="DataSource{T}"/> 的数据源。
    /// </summary>
    /// <param name="value">具备有2个参数和一个 <see cref="Task{TResult}"/> 任务的返回值的回调方法。</param>
    public static DataSource<T> Parse(Func<int, int, Task<(IEnumerable<T> data, int count)>> value) => new(OneOf<IEnumerable<T>, Func<int, int, Task<(IEnumerable<T> data, int count)>>>.FromT1(value));
}
