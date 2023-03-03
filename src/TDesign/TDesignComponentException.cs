using System.ComponentModel;
using System.Runtime.Serialization;

namespace TDesign;

/// <summary>
/// 引发 TDesign 组件的异常。
/// </summary>
public class TDesignComponentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TDesignComponentException"/> class.
    /// </summary>
    /// <param name="component">组件对象。</param>
    /// <param name="message">异常信息。</param>
    public TDesignComponentException(object component, string? message) : base(GetMessage(component,message))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TDesignComponentException"/> class.
    /// </summary>
    /// <param name="component">组件对象。</param>
    /// <param name="message">异常信息。</param>
    /// <param name="innerException">内部异常。</param>
    public TDesignComponentException(object component, string? message, Exception? innerException) : base(GetMessage(component, message), innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TDesignComponentException"/> class.
    /// </summary>
    /// <param name="info">The info.</param>
    /// <param name="context">The context.</param>
    protected TDesignComponentException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    static string? GetMessage(object component, string? message)
        => $"\"{component.GetType().Name}\"组件在使用过程中出现异常：\r{message}";
}
