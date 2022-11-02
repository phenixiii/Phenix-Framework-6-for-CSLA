{*******************************************************************************
 *
 * Unit Name : SecurityTables.sql
 * Purpose   : 安全表集
 * Author    : wangming
 * Maintainer:
 * History   :
 *
 * CreateDate: 2004-09-02
 *******************************************************************************}

CREATE TABLE PH_Department (          --部门
  DP_ID NUMERIC(15) NOT NULL,
  DP_DP_ID NUMERIC(15) NULL,          --上级部门
  DP_Name VARCHAR(100) NOT NULL,      --名称
  DP_Code VARCHAR(10) NOT NULL,       --代码
  DP_PT_ID NUMERIC(15) NULL,          --岗位树
  DP_In_Headquarters NUMERIC(1) NULL, --是否属于总部
  PRIMARY KEY(DP_ID),
  UNIQUE(DP_DP_ID, DP_Code),
  UNIQUE(DP_DP_ID, DP_Name)
)
/
CREATE TABLE PH_Position (       --岗位
  PT_ID NUMERIC(15) NOT NULL,
  PT_PT_ID NUMERIC(15) NULL,     --上级岗位
  PT_Name VARCHAR(100) NOT NULL, --名称
  PT_Code VARCHAR(10) NOT NULL,  --代码
  PRIMARY KEY(PT_ID),
  UNIQUE(PT_PT_ID, PT_Code),
  UNIQUE(PT_PT_ID, PT_Name)
)
/
CREATE TABLE PH_User (                                --用户
  US_ID NUMERIC(15) NOT NULL,
  US_UserNumber VARCHAR(10) NOT NULL,                 --登录工号
  US_Password VARCHAR(100) NOT NULL,                  --登录口令
  US_PasswordChangedTime DATE NULL,                   --登录口令更新时间
  US_Name VARCHAR(100) NOT NULL,                      --姓名
  US_Login DATE NULL,                                 --登录时间
  US_Logout DATE NULL,                                --登出时间
  US_LoginFailure DATE NULL,                          --登录失败时间
  US_LoginFailureCount NUMERIC(2) default 0 NOT NULL, --登录失败次数
  US_LoginAddress VARCHAR(39) NULL,                   --登录IP地址
  US_LastOperationTime DATE NULL,                     --最近操作时间
  US_DP_ID NUMERIC(15) NULL,		    	          --所属部门
  US_PT_ID NUMERIC(15) NULL,	     	              --担任岗位
  US_Locked NUMERIC(1) default 0 NOT NULL,            --锁定
  PRIMARY KEY(US_ID),
  UNIQUE(US_UserNumber)
)
/
CREATE TABLE PH_UserLog (             --用户日志
  US_ID NUMERIC(15) NOT NULL,
  US_UserNumber VARCHAR(10) NOT NULL, --登录工号
  US_Login DATE NOT NULL,             --登录时间
  US_Logout DATE NULL,                --登出时间
  US_LoginAddress VARCHAR(39) NULL    --登录IP地址
)
/
CREATE TABLE PH_Section (        --切片
  ST_ID NUMERIC(15) NOT NULL,
  ST_Name VARCHAR(100) NOT NULL, --名称
  ST_Caption VARCHAR(100) NULL,  --标签
  PRIMARY KEY(ST_ID),
  UNIQUE(ST_Name)
)
/
CREATE TABLE PH_User_Section (   --用户-切片
  US_ID NUMERIC(15) NOT NULL,
  US_US_ID NUMERIC(15) NOT NULL, --所属用户
  US_ST_ID NUMERIC(15) NOT NULL, --所属切片
  US_Inputer VARCHAR(10) NULL,   --记录人(=UserPrincipal.User.Identity.UserNumber)
  US_InputTime DATE NULL,        --记录日期
  PRIMARY KEY(US_ID),
  UNIQUE(US_US_ID, US_ST_ID)
)
/
CREATE TABLE PH_TableFilter (	                      --表过滤器
  TF_ID NUMERIC(15) NOT NULL,
  TF_Name VARCHAR(30) NOT NULL,                       --表名
  TF_Caption VARCHAR(100) NULL,			              --标签(在配置PH_Section_TableFilter时方便选择表过滤器)
  TF_Compare_ColumnName VARCHAR(30) NOT NULL,         --用于比较的字段的字段名(在配置PH_Section_TableFilter时提供ST_AllowRead_ColumnValue的填充值)
  TF_Friendly_ColumnName VARCHAR(30) NOT NULL,        --用于友好显示的字段的字段名(在配置PH_Section_TableFilter时提供ST_AllowRead_ColumnValue的友好显示值)
  TF_NoneSectionIsDeny NUMERIC(1) default 0 NOT NULL, --无切片时认为是被拒绝
  PRIMARY KEY(TF_ID),
  UNIQUE(TF_Name, TF_Compare_ColumnName)
)
/
CREATE TABLE PH_Section_TableFilter (          --切片-表过滤器
  ST_ID NUMERIC(15) NOT NULL,
  ST_ST_ID NUMERIC(15) NOT NULL,               --所属切片
  ST_TF_ID NUMERIC(15) NOT NULL,               --所属表过滤器
  ST_Friendly_ColumnValue VARCHAR(2000) NULL,  --用于友好显示的字段的字段值
  ST_AllowRead_ColumnValue VARCHAR(2000) NULL, --允许fetch的记录里用于比较的字段的字段值
  ST_AllowEdit NUMERIC(1) default 1 NOT NULL,  --是否允许edit
  PRIMARY KEY(ST_ID),
  UNIQUE(ST_ST_ID, ST_TF_ID, ST_AllowRead_ColumnValue)
)
/
CREATE TABLE PH_Role (           --角色
  RL_ID NUMERIC(15) NOT NULL,
  RL_Name VARCHAR(100) NOT NULL, --名称
  RL_Caption VARCHAR(100) NULL,  --标签
  PRIMARY KEY(RL_ID),
  UNIQUE(RL_Name)
)
/
CREATE TABLE PH_User_Role (      --用户-角色
  UR_ID NUMERIC(15) NOT NULL,
  UR_US_ID NUMERIC(15) NOT NULL, --所属用户
  UR_RL_ID NUMERIC(15) NOT NULL, --所属角色
  UR_Inputer VARCHAR(10) NULL,   --记录人(=UserPrincipal.User.Identity.UserNumber)
  UR_InputTime DATE NULL,        --记录日期
  PRIMARY KEY(UR_ID),
  UNIQUE(UR_US_ID, UR_RL_ID)
)
/
CREATE TABLE PH_User_Grant_Role ( --用户可授权角色
  GR_ID NUMERIC(15) NOT NULL,
  GR_US_ID NUMERIC(15) NOT NULL,  --所属用户
  GR_RL_ID NUMERIC(15) NOT NULL,  --所属角色
  GR_Inputer VARCHAR(10) NULL,    --记录人(=UserPrincipal.User.Identity.UserNumber)
  GR_InputTime DATE NULL,         --记录日期
  PRIMARY KEY(GR_ID),
  UNIQUE(GR_US_ID, GR_RL_ID)
)
/
CREATE TABLE PH_Assembly (                    --程序集
  AS_ID NUMERIC(15) NOT NULL,
  AS_Name VARCHAR(255) NOT NULL,              --名称
  AS_Caption VARCHAR(100) NULL,               --标签
  --AS_Enabled NUMERIC(1) default 1 NOT NULL,   --是否激活
  PRIMARY KEY(AS_ID),
  UNIQUE(AS_Name)
)
/
CREATE TABLE PH_AssemblyClass (                                --程序集类
  AC_ID NUMERIC(15) NOT NULL,
  AC_AS_ID NUMERIC(15) NOT NULL,                               --所属程序集
  AC_Name VARCHAR(255) NOT NULL,                               --名称
  AC_Caption VARCHAR(100) NULL,                                --标签
  --AC_CaptionConfigured NUMERIC(1) default 0 NOT NULL,          --标签已被配置
  --AC_PermanentExecuteAction NUMERIC(5) default 0 NOT NULL,     --指示当处于哪种执行动作时本字段需要记录新旧值(Phenix.Core.Mapping.ExecuteAction)
  --AC_PermanentExecuteConfigured NUMERIC(1) default 0 NOT NULL, --持久化执行变更方式已被配置
  AC_Type NUMERIC(5) default 0 NOT NULL,                       --类型(Phenix.Core.Dictionary.AssemblyClassType)
  AC_Authorised NUMERIC(1) default 0 NOT NULL,                 --可被授权, 默认Phenix.Core.Dictionary.AssemblyClassType.Form/ApiController才可被授权(=1)，其余都是不受权限控制(=0)
  PRIMARY KEY(AC_ID),
  UNIQUE(AC_AS_ID, AC_Name)
)
/
CREATE TABLE PH_AssemblyClass_Group ( --程序集类聚合
  AG_ID NUMERIC(15) NOT NULL,
  AG_AC_ID NUMERIC(15) NOT NULL,      --所属程序集类
  AG_Name VARCHAR(30) NOT NULL,       --聚合名
  PRIMARY KEY(AG_ID),
  UNIQUE(AG_AC_ID, AG_Name)
)
/
CREATE TABLE PH_AssemblyClass_Role (            --程序集类-角色(受PH_SystemInfo.SI_EmptyRolesIsDeny约束)
  AR_ID NUMERIC(15) NOT NULL,
  AR_AC_ID NUMERIC(15) NOT NULL,                --所属程序集类
  AR_RL_ID NUMERIC(15) NOT NULL,                --所属角色
  AR_AllowCreate NUMERIC(1) default 1 NOT NULL, --是否允许create，针对AC_Type = Business(CanCreate)、Form(New/Load)
  --AR_AllowGet NUMERIC(1) default 1 NOT NULL,    --是否允许fetch，针对AC_Type = Business(CanFatch)、Businesses(CanFatch)、Command(CanExecute)
  AR_AllowEdit NUMERIC(1) default 1 NOT NULL,   --是否允许edit，针对AC_Type = Business(CanEdit)、Businesses(CanEdit)、Command(CanExecute)
  AR_AllowDelete NUMERIC(1) default 1 NOT NULL, --是否允许delete，针对AC_Type = Business(CanDelete)
  PRIMARY KEY(AR_ID),
  UNIQUE(AR_AC_ID, AR_RL_ID)
)
/
CREATE TABLE PH_AssemblyClass_Department ( --程序集类-部门(如未指定所属部门则不限制)
  AD_ID NUMERIC(15) NOT NULL,
  AD_AC_ID NUMERIC(15) NOT NULL,           --所属程序集类
  AD_DP_ID NUMERIC(15) NOT NULL,           --所属部门
  PRIMARY KEY(AD_ID),
  UNIQUE(AD_AC_ID, AD_DP_ID)
)
/
CREATE TABLE PH_AssemblyClassProperty (                        --程序集类属性
  AP_ID NUMERIC(15) NOT NULL,
  AP_AC_ID NUMERIC(15) NOT NULL,                               --所属程序集类
  AP_Name VARCHAR(255) NOT NULL,                               --名称
  AP_Caption VARCHAR(100) NULL,                                --标签
  --AP_CaptionConfigured NUMERIC(1) default 0 NOT NULL,          --标签已被配置
  --AP_TableName VARCHAR(30) NULL,                               --映射对应的表名
  --AP_ColumnName VARCHAR(30) NULL,                              --映射对应的表列名
  --AP_Alias VARCHAR(30) NULL,                                   --映射对应的别名
  --AP_PermanentExecuteModify NUMERIC(5) default 7 NOT NULL,     --指示当处于哪种执行变更时本字段需要记录新旧值(Phenix.Core.Mapping.ExecuteModify)
  --AP_PermanentExecuteConfigured NUMERIC(1) default 0 NOT NULL, --持久化执行变更方式已被配置
  AP_Configurable NUMERIC(1) default 0 NOT NULL,               --是否可配置的
  AP_ConfigValue VARCHAR(2000) NULL,                           --配置的值
  --AP_IndexNumber NUMERIC(5) default -1 NOT NULL,               --索引号
  --AP_Required NUMERIC(1) NULL,                                 --是否必输(null: 默认、0:允许为空、1:不允许为空)
  --AP_Visible NUMERIC(1) default 1 NOT NULL,                    --是否可见
  PRIMARY KEY(AP_ID),
  UNIQUE(AP_AC_ID, AP_Name)
)
/
CREATE TABLE PH_AssemblyClassProperty_Value (    --程序集类属性配置值
  AV_ID NUMERIC(15) NOT NULL,
  AV_AP_ID NUMERIC(15) NOT NULL,                 --所属程序集类属性
  AV_ConfigKey VARCHAR(2000) NOT NULL,           --配置键
  AV_Configurable NUMERIC(1) default 1 NOT NULL, --是否可配置的
  AV_ConfigValue VARCHAR(2000) NULL,             --配置值
  PRIMARY KEY(AV_ID),
  UNIQUE(AV_AP_ID, AV_ConfigKey)
)
/
CREATE TABLE PH_AssemblyClassProperty_Role (    --程序集类属性-角色(受PH_SystemInfo.SI_EmptyRolesIsDeny约束)
  AR_ID NUMERIC(15) NOT NULL,
  AR_AP_ID NUMERIC(15) NOT NULL,                --所属程序集类属性
  AR_RL_ID NUMERIC(15) NOT NULL,                --所属角色
  --AR_AllowRead NUMERIC(1) default 1 NOT NULL,   --是否允许get，针对AC_Type = Business
  AR_AllowWrite NUMERIC(1) default 1 NOT NULL,  --是否允许set，针对AC_Type = Business
  PRIMARY KEY(AR_ID),
  UNIQUE(AR_AP_ID, AR_RL_ID)
)
/
CREATE TABLE PH_AssemblyClassMethod (                 --程序集类方法
  AM_ID NUMERIC(15) NOT NULL,
  AM_AC_ID NUMERIC(15) NOT NULL,                      --所属程序集类
  AM_Name VARCHAR(255) NOT NULL,                      --名称
  AM_Caption VARCHAR(100) NULL,                       --标签
  --AM_CaptionConfigured NUMERIC(1) default 0 NOT NULL, --标签已被配置
  --AM_Tag VARCHAR(4000) NULL,                          --标记
  --AM_AllowVisible NUMERIC(1) NULL,                    --是否允许显示即使没权限(null: 默认、0:允许显示、1:不允许显示)，针对AC_Type = Form上的功能按钮
  AM_AM_ID NUMERIC(15) NULL,                          --所属程序集类方法(针对AC_Type = Form上的树状菜单，为便于查看可在自己的权限配置界面上手工调整到与实际菜单一致)
  PRIMARY KEY(AM_ID),
  UNIQUE(AM_AC_ID, AM_Name)
)
/
CREATE TABLE PH_AssemblyClassMethod_Role (         --程序集类方法-角色(受PH_SystemInfo.SI_EmptyRolesIsDeny约束)
  AR_ID NUMERIC(15) NOT NULL,
  AR_AM_ID NUMERIC(15) NOT NULL,                   --所属程序集类方法
  AR_RL_ID NUMERIC(15) NOT NULL,                   --所属角色
  PRIMARY KEY(AR_ID),
  UNIQUE(AR_AM_ID, AR_RL_ID)
)
/
CREATE TABLE PH_AssemblyClassMethod_Departm ( --程序集类方法-部门(如未指定所属部门则不限制)
  AD_ID NUMERIC(15) NOT NULL,
  AD_AM_ID NUMERIC(15) NOT NULL,              --所属程序集类方法
  AD_DP_ID NUMERIC(15) NOT NULL,              --所属部门
  PRIMARY KEY(AD_ID),
  UNIQUE(AD_AM_ID, AD_DP_ID)
)
/
CREATE TABLE PH_ExecuteLog (              --执行日志
  EL_ID NUMERIC(15) NOT NULL,
  EL_Time DATE NOT NULL,                  --时间
  EL_UserNumber VARCHAR(10) NOT NULL,     --登录工号
  EL_BusinessName VARCHAR(255) NOT NULL,  --业务类名
  EL_Message VARCHAR(4000) NULL,          --消息
  EL_ExceptionName VARCHAR(255) NULL,     --错误名
  EL_ExceptionMessage VARCHAR(4000) NULL, --错误消息
  PRIMARY KEY(EL_ID)
)
/
CREATE TABLE PH_ExecuteActionLog (          --执行动作日志
  EA_ID NUMERIC(15) NOT NULL,
  EA_Time DATE NOT NULL,                    --时间
  EA_UserNumber VARCHAR(10) NOT NULL,       --登录工号
  EA_BusinessName VARCHAR(255) NOT NULL,    --业务类名
  EA_BusinessPrimaryKey VARCHAR(4000) NULL, --业务主键值
  EA_Action NUMERIC(5) NOT NULL,            --执行动作(Phenix.Core.Mapping.ExecuteAction)
  EA_Log LONG /*TEXT*/ NULL,                --日志
  PRIMARY KEY(EA_ID)
)
/
CREATE INDEX I_PH_EA_Time on PH_ExecuteActionLog (EA_Time)
/
CREATE TABLE PH_ProcessLock (                    --过程锁
  PL_Name VARCHAR(255) NOT NULL,                 --名称
  PL_Caption VARCHAR(1000) NULL,    	         --标签
  PL_AllowExecute NUMERIC(1) default 1 NOT NULL, --是否允许Execute
  PL_Time DATE NOT NULL,                         --时间
  PL_ExpiryTime DATE NULL,                       --限期(为空时代表不自动失效)
  PL_UserNumber VARCHAR(10) NOT NULL,            --登录工号
  PL_Remark VARCHAR(4000) NULL,                  --备注
  PRIMARY KEY(PL_Name)
)
/
insert into PH_Role
(RL_ID, RL_Name, RL_Caption)
values
(0,'Admin','Admin')
/
insert into PH_User
(US_ID, US_Name, US_UserNumber, US_Password)
values
(0,'Administrator','ADMIN','ADMIN')
/
insert into PH_User_Role
(UR_ID, UR_US_ID, UR_RL_ID)
values
(0,0,0)
/
              
--迁移Packer
insert into PH_Role
(RL_ID, RL_Name, RL_Caption)
select RL_ID, RL_Name, RL_Chinese_Name
from Role
where RL_ID not in (select RL_ID from PH_Role)
  and RL_Name not in (select RL_Name from PH_Role)
/
insert into PH_User
(US_ID, US_UserNumber, US_Password, US_Name)
select EP_ID, EP_UserID, EP_Password, EP_Name
from Employee
where EP_ID not in (select US_ID from PH_User)
  and EP_UserID not in (select US_UserNumber from PH_User)
/
create sequence PH_User_Role_S
/
insert into PH_User_Role
(UR_ID, UR_US_ID, UR_RL_ID)
select PH_User_Role_S.nextval, EPR_EP_ID, EPR_RL_ID
from Employee_Role, dual
where EPR_EP_ID || EPR_RL_ID not in (select UR_US_ID || UR_RL_ID from PH_User_Role)
/

--维护
select * FROM PH_ASSEMBLYCLASS_ROLE 
WHERE AR_AC_ID IN (SELECT PH_ASSEMBLYCLASS.AC_ID 
FROM PH_ASSEMBLYCLASS 
WHERE AC_AS_ID IN (SELECT AS_ID 
FROM PH_ASSEMBLY 
WHERE AS_NAME LIKE '%Business.%')); 

select * FROM PH_AssemblyClassProperty_Role 
WHERE AR_AP_ID IN (SELECT PH_AssemblyClassProperty.AP_ID 
FROM PH_AssemblyClassProperty, PH_AssemblyClass, PH_Assembly
WHERE AP_AC_ID = AC_ID and  AC_AS_ID = AS_ID and AS_NAME LIKE '%Business.%'); 