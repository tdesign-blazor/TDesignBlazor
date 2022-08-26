namespace TDesignBlazor;

public class Theme : Enumeration
{
    protected Theme(string value) : base(value)
    {
    }

    public static readonly Theme Primary = nameof(Primary);
    public static readonly Theme Danger = nameof(Danger);
    public static readonly Theme Warning = nameof(Warning);
    public static readonly Theme Success = nameof(Success);

    public static implicit operator Theme(string name) => new(name.ToLower());
}

public class MessageTheme : Theme
{
    protected MessageTheme(string value) : base(value)
    {
    }

    public static readonly Theme Question = nameof(Question);
}
