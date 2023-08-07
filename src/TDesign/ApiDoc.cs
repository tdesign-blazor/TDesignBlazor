using System.Reflection;
using System.Text;

namespace TDesign;

/// <summary>
/// 应用于参数，用于自动生成 API 文档。
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
internal class ParameterApiDocAttribute : Attribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="comment">描述。</param>
    public ParameterApiDocAttribute(string? comment)
    {
        Comment = comment;
    }
    /// <summary>
    /// 参数的数据类型。
    /// </summary>
    public string? Type { get; set; }
    /// <summary>
    /// 注释。
    /// </summary>
    public string? Comment { get; }
    /// <summary>
    /// 必填项。
    /// </summary>
    public bool Required { get; set; }
    /// <summary>
    /// 默认值。
    /// </summary>
    public object? Value { get; set; }
}

/// <summary>
/// 方法可被 api 文档识别。
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
internal class MethodApiDocAttribute : Attribute
{
    public MethodApiDocAttribute(string comment)
    {
        Comment = comment;
    }

    public string Comment { get; }

    public string? ReturnType { get; set; }
    public string? Parameters { get; set; }
}

/// <summary>
/// For document only.
/// </summary>
public class ApiDoc
{
    /// <summary>
    /// 获取指定组件的 api 文档。
    /// </summary>
    /// <param name="componentType">组件。</param>
    /// <returns></returns>
    public static List<(string name, string type, bool requried, object? defaultValue, string? comment)> GetParameterApiDoc(Type componentType)
    {
        var list = new List<(string name, string type, bool requried, object? defaultValue, string? comment)>();

        foreach ( var parameter in componentType.GetProperties().Where(m => m.IsDefined(typeof(ParameterApiDocAttribute), false)).OrderBy(m => m.Name) )
        {
            var name = parameter.Name;
            var attr = parameter.GetCustomAttribute<ParameterApiDocAttribute>()!;

            var type = attr.Type ?? parameter.PropertyType.Name;
            var required = parameter.GetCustomAttribute<EditorRequiredAttribute>() is not null || attr.Required;
            var value = attr.Value;
            if ( attr.Value is null )
            {
                if ( type == typeof(Boolean).Name )
                {
                    value = value is null ? "false" : (bool?)value;
                }
            }

            if ( Nullable.GetUnderlyingType(parameter.PropertyType) is not null )
            {
                type = $"Nullable<{Nullable.GetUnderlyingType(parameter.PropertyType).Name}>";
            }
            var comment = attr.Comment;
            list.Add((name, type, required, value, comment));
        }

        return list;
    }

    public static List<(string name,string? comment, string returnType, string parameters)> GetMethodApiDoc(Type componentType)
    {
        List<(string name, string? comment, string returnType, string parameters)> list = new();

        foreach(var method in componentType.GetMethods(BindingFlags.Public|BindingFlags.Instance).Where(m=>m.IsDefined(typeof(MethodApiDocAttribute),false)).OrderBy(m=>m.Name))
        {
            var attr= method.GetCustomAttribute<MethodApiDocAttribute>();

            var name = method.Name;
            var returnType= attr.ReturnType?? method.ReturnType.Name;
            var parameterList = new List<string>();
            foreach ( var item in method.GetParameters() )
            {
                parameterList.Add($"{item.ParameterType.Name} {item.Name}");
                if ( item.HasDefaultValue )
                {
                    parameterList.Add($" = {item.DefaultValue}");
                }
            }

            var parameterString = string.Join(", ", parameterList);

            list.Add((name, attr.Comment, returnType, parameterString));
        }
        return list;
    }
}
