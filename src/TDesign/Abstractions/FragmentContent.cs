namespace TDesign.Abstractions;
public sealed class FragmentContent : OneOfBase<string?,MarkupString?, RenderFragment?>
{
    internal FragmentContent(OneOf<string?,MarkupString?, RenderFragment?> input) : base(input)
    {
    }

    public string? Text => (string?)Value;
    public RenderFragment? Fragment => (RenderFragment?)Value;
    public MarkupString? Markup => (MarkupString?)Value;
     

    public static implicit operator FragmentContent(string? text) => new(text);
    public static implicit operator FragmentContent(RenderFragment? fragment) => new(fragment);
    public static implicit operator FragmentContent(MarkupString? markup) => new(markup);
}
