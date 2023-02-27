namespace TDesign.Test.Components.Navigation;

public class PaginationTest : TestBase<TPagination>
{
    [Fact(DisplayName = "Pagination - 完整的 HTML 渲染")]
    public void Test_Render()
    {
        var component = RenderComponent(m => m.Bind(p => p.PageIndex, 1, value => { })
                                            .Bind(p => p.PageSize, 10, value => { })
                                            .Bind(p => p.Total, 100, value => { })
                                            );

        component.Find(".t-pagination").Should().NotBeNull();

        component.Find(".t-pagination__total").Should().NotBeNull();



        /*
<div class="t-pagination t-size-m">
    <div class="t-pagination__total">共 36 项数据</div>
    <div class="t-select__wrap t-pagination__select">
        <div class="t-select-input t-select">
            <div class="t-input__wrap t-input--auto-width">
                <div class="t-input t-is-readonly t-input--prefix t-input--suffix">
                    <div class="t-input__prefix"></div><input readonly="readonly" autocomplete="" placeholder="请选择" type="text" unselectable="on" class="t-input__inner" style="width: 46.3438px;">
                    <span class="t-input__input-pre">5 条/页</span>
                    <span class="t-input__suffix t-input__suffix-icon">
                        <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" class="t-fake-arrow t-select__right-icon">
                            <path d="M3.75 5.7998L7.99274 10.0425L12.2361 5.79921" stroke="black" stroke-opacity="0.9" stroke-width="1.3"></path>
                        </svg>
                    </span>
                </div>
            </div>
        </div>
    </div>
    <div class="t-pagination__btn t-pagination__btn-prev">
        <svg fill="none" viewBox="0 0 16 16" width="1em" height="1em" class="t-icon t-icon-chevron-left">
            <path fill="currentColor" d="M9.54 3.54l.92.92L6.92 8l3.54 3.54-.92.92L5.08 8l4.46-4.46z" fill-opacity="0.9"></path>
        </svg>
    </div>
    <ul class="t-pagination__pager">
        <li class="t-pagination__number">1</li>
        <li class="t-pagination__number">2</li>
        <li class="t-pagination__number">3</li>
        <li class="t-pagination__number">4</li>
        <li class="t-pagination__number">5</li>
        <li class="t-pagination__number">6</li>
        <li class="t-pagination__number">7</li>
        <li class="t-pagination__number">8</li>
    </ul>
    <div class="t-pagination__btn t-pagination__btn-next">
        <svg fill="none" viewBox="0 0 16 16" width="1em" height="1em" class="t-icon t-icon-chevron-right">
            <path fill="currentColor" d="M6.46 12.46l-.92-.92L9.08 8 5.54 4.46l.92-.92L10.92 8l-4.46 4.46z" fill-opacity="0.9"></path>
        </svg>
    </div>
</div>
        */
    }
}
