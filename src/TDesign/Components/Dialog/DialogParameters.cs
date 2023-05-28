using System.Collections;

namespace TDesign;

/// <summary>
/// 表示对话框的参数。
/// </summary>
public class DialogParameters : IEnumerable<KeyValuePair<string, object?>>
{
    Dictionary<string, object?> _parametersStore = new();

    /// <summary>
    /// 获取或设置指定参数名称的值。
    /// </summary>
    /// <param name="name">参数名称。</param>
    /// <returns>指定名称关联的值，如果名称不存在，则返回数据类型的默认值或 <c>null</c>。</returns>
    public object? this[string name]
    {
        get => Get(name);
        set => Set(name, value);
    }

    /// <summary>
    /// 设置指定参数名称和关联值，如果参数名称重复，则覆盖已有值。
    /// </summary>
    /// <param name="name">参数的名称。</param>
    /// <param name="value">关联的值。</param>
    /// <returns>关联名称和值的 <see cref="DialogParameters"/> 实例。</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> 是 null 或空白字符串。</exception>
    public DialogParameters Set(string name,object? value)
    {
        if ( string.IsNullOrWhiteSpace(name) )
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }
        if ( _parametersStore.ContainsKey(name) )
        {
            _parametersStore[name] = value;
        }
        else
        {
            _parametersStore.Add(name, value);
        }
        return this;
    }

    /// <summary>
    /// 获取指定参数名称的值。
    /// </summary>
    /// <param name="name">参数的名称。</param>
    /// <returns>指定名称关联的值，如果名称不存在，则返回数据类型的默认值或 <c>null</c>。</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> 是 null 或空白字符串。</exception>
    public object? Get(string name)
    {
        if ( string.IsNullOrWhiteSpace(name) )
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }

        if ( _parametersStore.TryGetValue(name, out var value) )
        {
            return value;
        }
        return default;
    }

    /// <summary>
    /// 获取参数的迭代器。
    /// </summary>
    /// <returns>可迭代的键值对。</returns>
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => _parametersStore.GetEnumerator();

    /// <summary>
    /// 获取迭代器。
    /// </summary>
    /// <returns>一个迭代器。</returns>
    IEnumerator IEnumerable.GetEnumerator() => _parametersStore.GetEnumerator();

    internal void SetDialogTemplate<TTemplate>()
    {
        var type=typeof(TTemplate);
        Set("DialogTemplate", type);
    }

    internal Type GetDialogTemplate() => Get("DialogTemplate") as Type ?? throw new InvalidCastException("DialogTemplate casting error");
}
