using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign;
public record struct DialogResult
{
    public bool Cancelled { get; internal set; }
    public object? Data { get; internal set; }
}

public interface IDialogInstance
{
    DialogResult Result { get; }
    Task Dismis();
}

internal class DialogInstance : IDialogInstance
{
    public RenderFragment? DialogContent { get; private set; }

    public IDialogInstance Create(Type dialogTemplate)
    {
        var instance = new DialogInstance();

        instance.DialogContent = builder =>
        {
            builder.OpenComponent(0,dialogTemplate);
            builder.CloseComponent();
        };

        return instance;
    }

    public DialogResult Result { get; }

    public Task Dismis()
    {
        throw new NotImplementedException();
    }
}
