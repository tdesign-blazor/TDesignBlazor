﻿using ComponentBuilder.Interceptors;
using TDesign.Specifications;

namespace TDesign.Interceptors;
/// <summary>
/// 定义对参数进行规范的拦截器。
/// </summary>
internal class SpecificationInterceptor : ComponentInterceptorBase
{
    public override void InterceptOnParameterSet(IBlazorComponent component)
    {
        if ( component is IHasHeaderFragment headerFragment && component is IHasHeaderText headerText && headerText.HeaderText.IsNotNullOrWhiteSpace() )
        {
            headerFragment.HeaderContent ??= builder => builder.AddContent(0, headerText.HeaderText);
        }

        if ( component is IHasFooterFragment footerFragment && component is IHasFooterText footerText && footerText.FooterText.IsNotNullOrWhiteSpace() )
        {
            footerFragment.FooterContent ??= builder => builder.AddContent(0, footerText.FooterText);
        }

        if ( component is IHasTitleFragment titleFragment && component is IHasTitleText titleText && titleText.TitleText.IsNotNullOrWhiteSpace() )
        {
            titleFragment.TitleContent ??= builder => builder.AddContent(0, titleText.TitleText);
        }
    }
}
