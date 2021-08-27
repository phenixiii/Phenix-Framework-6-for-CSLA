0，本目录名为bin
1，本目录下的Phenix框架及配套的第三方程序集，运行环境为NET Framework 4.0
2，CSLA版本为改写过的4.3.14.0，改写仅限于为BusinessBase添加了支持Newtonsoft.Json的MemberSerialization.OptOut序列化模式的Newtonsoft.Json.JsonIgnore标签，请配套使用Phenix.Adding工具自动生成的业务类代码
3，Phenix.Windows.dll封装的DevExpress版本为13.2.X，如需支持其他版本，请自行改写Phenix.Extensions\Phenix.Windows目录下的Phenix.Windows.DevExpress.v13.2.X.csproj并重新编译即可
4，本目录下的Phenix框架，缺少Bin.Top目录下框架的WebAPI支持，其余功能完全一致