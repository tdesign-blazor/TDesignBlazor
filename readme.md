![TDesign](./asset/tdesign-blazor-banner.png)

![TDesign](https://user-images.githubusercontent.com/88708072/147124305-fbb74f9f-65b2-44f9-9f1c-e812ce63a547.gif)
**基于腾讯 [TDesign](https://tdesign.tencent.com/) 的 Blazor 企业级组件库**


[![CodeQL](https://github.com/AchievedOwner/TDesignBlazor/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/AchievedOwner/TDesign/actions/workflows/codeql-analysis.yml)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/b63260c20d2546f296045d746833e7e4)](https://www.codacy.com/gh/AchievedOwner/TDesignBlazor/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=AchievedOwner/TDesignBlazor&amp;utm_campaign=Badge_Grade)
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/b63260c20d2546f296045d746833e7e4)](https://www.codacy.com/gh/AchievedOwner/TDesignBlazor/dashboard?utm_source=github.com&utm_medium=referral&utm_content=AchievedOwner/TDesignBlazor&utm_campaign=Badge_Coverage)
![Version](https://img.shields.io/github/v/release/AchievedOwner/TDesignBlazor)

## ✨ 特性

- 🌈 提炼自 TDesign 的企业级中后台产品的交互语言和视觉风格
- 📦 开箱即用的高质量 Blazor 组件，可在多种托管方式共享
- 💕 支持基于 WebAssembly 的客户端和基于 SignalR 的服务端 UI 事件交互
- 🎨 支持渐进式 Web 应用（PWA）
- 🛡 使用 C# 构建，多范式静态语言带来高效的开发体验
- ⚙️ 基于 .NET 6/7，可直接引用丰富的 .NET 类库
- 🎁 可与已有的 ASP.NET Core MVC、Razor Pages 项目无缝集成
- 💴 基于 MIT 开源协议

## 🌈 Demo 演示
* 在线演示
    * [https://achievedowner.github.io/TDesignBlazor](https://achievedowner.github.io/TDesignBlazor/) Nightly - master 合并后更新
    * [http://tblazor.com](http://tblazor.com) 稳定版 - 版本发布后更新
* 源码运行
    * `TDesign.Docs.ServerSide` 是服务端
    * `TDesign.Docs.WebAssembly` 是客户端
    * `TDesign.Docs.Shared` 是两端共享组件和页面

## 🖥 支持环境

- ![.NET 6](https://img.shields.io/badge/.NET-v6.0-green) ![.NET 6](https://img.shields.io/badge/.NET-v7.0-green)
- 支持 WebAssembly 静态文件部署
- 主流 4 款现代浏览器，以及 Internet Explorer 11+（限 [Blazor Server](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1&WT.mc_id=DT-MVP-5003987)）
- 可直接运行在 [.NET MAUI](https://dotnet.microsoft.com/zh-cn/apps/maui?WT.mc_id=DT-MVP-5003987)、[WPF](https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/wpf?view=aspnetcore-6.0&WT.mc_id=DT-MVP-5003987)、[Windows Forms](https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/windows-forms?view=aspnetcore-6.0) 等 Blazor 混合客户端环境中
- 可直接运行在 [Electron](http://electron.atom.io/) 等基于 Web 标准的环境上

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br> Edge / IE | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/safari/safari_48x48.png" alt="Safari" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Safari | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/opera/opera_48x48.png" alt="Opera" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Opera | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/electron/electron_48x48.png" alt="Electron" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Electron |
| :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
|                                                                                          Edge 16 / IE 11†                                                                                           |                                                                                                 522                                                                                                  |                                                                                                57                                                                                                |                                                                                                11                                                                                                |                                                                                              44                                                                                              |                                                                                               Chromium 57                                                                                                |

> 由于 [WebAssembly](https://webassembly.org) 的限制，Blazor WebAssembly 不支持 IE 浏览器，但 Blazor Server 支持 IE 11†。 详见[官网说明](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1&WT.mc_id=DT-MVP-5003987)。
> 从 .NET 5 开始，Blazor 不再官方支持 IE 11。详见 [Blazor: Updated browser support](https://docs.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/5.0/blazor-browser-support-updated)。社区项目 [Blazor.Polyfill](https://github.com/Daddoon/Blazor.Polyfill) 提供了非官方支持。


## 📦 安装使用
- 从 Nuget 直接安装
    ```bash
    > Install-Package TDesign
    ```

- 在项目中注册服务
    ```cs
    builder.Service.AddTDesign();
    ```

- 在 `wwwroot/index.html`(WebAssembly) 或 `Pages/_Host.cshtml`(Server) 中引入静态文件:
    ```html
    <link rel=""stylesheet"" href="_content/TDesign/tdesign-blazor.css" />
    <script src="_content/TDesign/tdesign-blazor.js"></script>
    ```
- 在 `_Imports.razor` 中加入命名空间
    ```cs
    @using TDesign
    ```
- 在 `App.razor` 增加 `TDesignContainer` 组件，用于动态组件的渲染
    ```html
    <Router AppAssembly="@typeof(App).Assembly">
        <Found Context="routeData">
            <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
            <FocusOnNavigate RouteData="@routeData" Selector="h1" />
        </Found>
        <NotFound>
            <PageTitle>Not found</PageTitle>
            <LayoutView Layout="@typeof(MainLayout)">
                <p role="alert">Sorry, there's nothing at this address.</p>
            </LayoutView>
        </NotFound>
    </Router>

    <!--增加组件-->
    <TDesignContainer/>
    ```

## :pencil: 参与贡献
* 如果你有意向参与贡献，请先阅读[贡献指南](./Contributing.md)
* 有任何问题，欢迎通过 [Github issues](https://github.com/AchievedOwner/TDesignBlazor/issues) 反馈
* 提供目前进度的实时[看板](https://github.com/users/AchievedOwner/projects/4)

**我们的贡献者**
非常感谢每一个项目贡献者的辛勤付出

<div>
    <a href="https://github.com/AchievedOwner/TDesignBlazor/graphs/contributors">
    <img src="https://contrib.rocks/image?repo=AchievedOwner/TDesignBlazor" />
    </a>
</div>

## :house: 社区支持
如果您在此过程中遇到任何问题，请通过以下渠道寻求帮助：
* 作者邮箱：[william-zzh@outlook.com]
* 作者微信：playermaker（请注明来源）
* 官方微信：TDesignBlazor 官方群
* 交流论坛：[https://github.com/AchievedOwner/TDesignBlazor/discussions](https://github.com/AchievedOwner/TDesignBlazor/discussions)

> 我们鼓励有经验的用户帮助新手，并会授予勋章。

## :triangular_flag_on_post: 行为准则
本项目采用了贡献者契约定义的行为准则，以澄清我们社区的预期行为。请仔细阅读[行为准则](./CodeOfConduct.md)。

## :newspaper: 许可证（License）
[![TDesign](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/AchievedOwner/TDesignBlazor/blob/master/LICENSE.md)

## 🔗 相关链接
- [Blazor 官方文档](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/?WT.mc_id=DT-MVP-5003987)
- [MS Learn 平台 Blazor 教程](https://docs.microsoft.com/zh-cn/learn/modules/build-blazor-webassembly-visual-studio-code/?WT.mc_id=DT-MVP-5003987)
- [ComponentBuilder 自动化组件框架](https://github.com/AchievedOwner/ComponentBuilder)
