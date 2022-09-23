using Microsoft.AspNetCore.Components.Forms;

namespace TDesign;
internal class FormFieldCssClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        var invalid = editContext.GetValidationMessages(fieldIdentifier).Any();

        //是否修改过，这里可能会做一个配置。
        if (editContext.IsModified(fieldIdentifier))
        {
            if (invalid)
            {
                return Status.Error.GetCssClass();
            }
            return Status.Success.GetCssClass();
        }

        if (invalid)
        {
            return Status.Error.GetCssClass();
        }
        return Status.Default.GetCssClass();
    }
}
