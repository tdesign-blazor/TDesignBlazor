namespace TDesign;

public static class ValueConverter
{
    /// <summary>
    /// 将指定的值转换为指定的数据类型。
    /// </summary>
    /// <typeparam name="TTarget">目标类型。</typeparam>
    /// <param name="value">当前值。</param>
    /// <param name="convension">自定义转换方法。</param>
    /// <returns>转换成功则返回 <c>true</c>；否则返回 <c>false</c>。</returns>
    public static TTarget? To<TTarget>(this object value, Func<object, TTarget?>? convension = default)
    {
        if (value is null)
        {
            return default;
        }

        convension ??= DefaultConvension;
        return convension(value);


        static TTarget? DefaultConvension(object? value)
        {
            var targetType = typeof(TTarget);

            var nullValueType = Nullable.GetUnderlyingType(targetType);
            if (nullValueType is not null)
            {
                return (TTarget?)Convert.ChangeType(value, nullValueType);
            }

            return (TTarget?)Convert.ChangeType(value, targetType);
        }
    }
}
