本程序集架构在Phenix.NET框架上，在客户端和服务端都需部署
请编译后被业务系统主程序引用（需同时引用Phenix.TeamHub.Prober程序集）
第三方系统也可参考工程中的代码，编写自己的功能实现，以挂接到TeamHub平台上

——功能清单——
Worker：
	实现Phenix.TeamHub.Prober.IWorker功能，其Register()函数应被业务系统主程序代码调用以完成注册
SubmitLogCommand：
	提供向TeamHub平台的PT_ExecuteLog表添加日志的功能，Worker的SubmitLog()函数里有被调用执行

——补充说明——
以下表结构需事先生成（也可运行Phenix.TeamHub.Prober.Station.x86/x64.exe程序来自动生成）：
CREATE TABLE PT_ExecuteLog (                 --执行日志
  EL_ID NUMERIC(15) NOT NULL,
  EL_Time DATE NOT NULL,                     --时间
  EL_UserID VARCHAR(100) NULL,               --用户ID
  EL_UserNumber VARCHAR(100) NULL,           --用户工号
  EL_Message VARCHAR(4000) NULL,             --消息
  EL_AssemblyName VARCHAR(255) NULL,         --程序集名
  EL_NamespaceName VARCHAR(255) NULL,        --命名空间名
  EL_ClassName VARCHAR(255) NULL,            --类名
  EL_MethodName VARCHAR(255) NULL,           --方法名
  EL_ExceptionName VARCHAR(255) NULL,        --错误名
  EL_ExceptionMessage VARCHAR(4000) NULL,    --错误消息
  EL_ExceptionStackTrace LONG /*TEXT*/ NULL, --错误调用堆栈
  PRIMARY KEY(EL_ID)
)