## TDesignBlazor
一套基于 TDesign 的 Blazor 企业级组件库

>[TDesign](https://tdesign.tencent.com/) 是腾讯各业务团队在服务业务过程中沉淀的一套企业级设计体系。
>TDesign 具有统一的 价值观，一致的设计语言和视觉风格，帮助用户形成连续、统一的体验认知。在此基础上，TDesign 提供了开箱即用的 UI 组件库、设计指南 和相关 设计资产，以优雅高效的方式将设计和研发从重复劳动中解放出来，同时方便大家在 TDesign 的基础上扩展，更好的的贴近业务需求。

## ✨ 特性

- 🌈 提炼自企业级中后台产品的交互语言和视觉风格
- 📦 开箱即用的高质量 Blazor 组件，可在多种托管方式共享
- 💕 支持基于 WebAssembly 的客户端和基于 SignalR 的服务端 UI 事件交互
- 🎨 支持渐进式 Web 应用（PWA）
- 🛡 使用 C# 构建，多范式静态语言带来高效的开发体验
- ⚙️ 基于 .NET 6，可直接引用丰富的 .NET 类库
- 🎁 可与已有的 ASP.NET Core MVC、Razor Pages 项目无缝集成
- 💴 基于 MIT 开源协议

## 🌈 在线示例
[https://achievedowner.github.io/TDesignBlazor/](https://achievedowner.github.io/TDesignBlazor/)

## 🖥 支持环境

- 兼容 .NET 6。
- Blazor WebAssembly .NET 6 正式版。
- 支持服务端双向绑定。
- 支持 WebAssembly 静态文件部署。
- 主流 4 款现代浏览器，以及 Internet Explorer 11+（限 [Blazor Server](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1&WT.mc_id=DT-MVP-5003987)）。
- 可直接运行在 [.NET MAUI](https://dotnet.microsoft.com/zh-cn/apps/maui?WT.mc_id=DT-MVP-5003987)、[WPF](https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/wpf?view=aspnetcore-6.0&WT.mc_id=DT-MVP-5003987)、[Windows Forms](https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/windows-forms?view=aspnetcore-6.0) 等 Blazor 混合客户端环境中。
- 可直接运行在 [Electron](http://electron.atom.io/) 等基于 Web 标准的环境上。

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br> Edge / IE | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/safari/safari_48x48.png" alt="Safari" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Safari | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/opera/opera_48x48.png" alt="Opera" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Opera | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/electron/electron_48x48.png" alt="Electron" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Electron |
| :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
|                                                                                          Edge 16 / IE 11†                                                                                           |                                                                                                 522                                                                                                  |                                                                                                57                                                                                                |                                                                                                11                                                                                                |                                                                                              44                                                                                              |                                                                                               Chromium 57                                                                                                |

> 由于 [WebAssembly](https://webassembly.org) 的限制，Blazor WebAssembly 不支持 IE 浏览器，但 Blazor Server 支持 IE 11†。 详见[官网说明](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1&WT.mc_id=DT-MVP-5003987)。
> 从 .NET 5 开始，Blazor 不再官方支持 IE 11。详见 [Blazor: Updated browser support](https://docs.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/5.0/blazor-browser-support-updated)。社区项目 [Blazor.Polyfill](https://github.com/Daddoon/Blazor.Polyfill) 提供了非官方支持。


## 📦 安装使用
- 从 Nuget 直接安装
```bash
> Install-Package TDesignBlazor
```

- 在项目中注册
```cs
builder.Service.AddTDesignBlazor();
```

- 在 `wwwroot/index.html`(WebAssembly) 或 `Pages/_Host.cshtml`(Server) 中引入静态文件:
```html
<link rel=""stylesheet"" href=""_content/TDesignBlazor/reset.css"" /> <!--重置 HTML 默认样式-->
<link rel=""stylesheet"" href=""_content/TDesignBlazor/tdesign.min.css"" /> <!--tdesign 核心样式-->
<link rel=""stylesheet"" href=""_content/TDesignBlazor/tdesign-icons.css"" /><!--图标样式-->
```
- 在 `_Imports.razor` 中加入命名空间
```cs
@using TDesignBlazor
@using TDesignBlazor.Components
```
## 🔗 链接

- [ISSUE 链接](https://github.com/AchievedOwner/TDesignBlazor/issues)
- [Blazor 官方文档](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/?WT.mc_id=DT-MVP-5003987)
- [MS Learn 平台 Blazor 教程](https://docs.microsoft.com/zh-cn/learn/modules/build-blazor-webassembly-visual-studio-code/?WT.mc_id=DT-MVP-5003987)

## 🗺 里程碑

查看[这个 issue](https://github.com/AchievedOwner/TDesignBlazor/milestone/) 来了解我们的开发计划