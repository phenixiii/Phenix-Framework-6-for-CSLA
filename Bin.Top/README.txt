0，本目录名为bin.Top
1，本目录下的Phenix框架及配套的第三方程序集，运行环境为NET Framework 4.5
2，CSLA版本为改写过的4.3.14.0，改写仅限于为BusinessBase添加了支持Newtonsoft.Json的MemberSerialization.OptOut序列化模式的Newtonsoft.Json.JsonIgnore标签，请配套使用Phenix.Adding工具自动生成的业务类代码
3，Phenix.Windows.dll封装的DevExpress版本为13.2.X，如需支持其他版本，请自行改写Phenix.Extensions\Phenix.Windows目录下的Phenix.Windows.DevExpress.v13.2.X.Top.csproj并重新编译即可
4，本目录含所见即所得的代码生成Addin工具Phenix.Addin.dll、Phenix.VSPackage.vsix，可通过本目录下的Phenix.Addin.Install.exe注册到Microsoft Visual Studio 2010/2012/2013/2017
5，本目录下的Phenix框架，与Bin目录下框架的区别在于增加了对WebAPI、WebSocket的支持，其余功能完全一致
6，WebAPI的客户端框架，源码见Phenix.Extensions\Phenix.Web.Client目录下的Phenix.Web.Client.Top.csproj、Phenix.Extensions\Phenix.Web.Client.Ajax目录下的phenix.js（示例见Phenix.Test\Phenix.Test.Ajax），可以参考改写为其他平台的客户端框架