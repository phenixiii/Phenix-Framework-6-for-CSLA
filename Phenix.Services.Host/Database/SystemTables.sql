{*******************************************************************************
 *
 * Unit Name : SystemTables.sql
 * Purpose   : 系统表集
 * Author    : wangming
 * Maintainer:
 * History   :
 *
 * CreateDate: 2004-09-02
 *******************************************************************************}
               create user Phenix identified by Phenix
               grant DBA to Phenix

create tablespace sgwy datafile 'E:\app\sgwy.ora' size 1000m
alter user sgwy default tablespace sgwy

               create database PhenixDB
               sp_addlogin Phenix, Phenix, PhenixDB
               use PhenixDB
               sp_adduser Phenix, Phenix, db_owner


create sequence SEQ_PH
/
CREATE TABLE PH_SystemInfo (                          --系统信息
  SI_Name VARCHAR(30) NOT NULL,                       --名称
  SI_Version VARCHAR(30) NOT NULL,                    --版本号
  SI_Enterprise VARCHAR(255) NOT NULL,                --企业名（界面、报表等处显示的企业标签）
  SI_AssemblyInfoChangedTime DATE NULL,               --程序集资料更新时间
  SI_TableFilterInfoChangedTime DATE NULL,            --表过滤器资料更新时间
  SI_RoleInfoChangedTime DATE NULL,                   --角色资料更新时间
  SI_SectionInfoChangedTime DATE NULL,                --切片资料更新时间
  SI_DepartmentInfoChangedTime DATE NULL,             --部门资料更新时间
  SI_PositionInfoChangedTime DATE NULL,               --岗位资料更新时间
  SI_TableInfoChangedTime DATE NULL,                  --表结构更新时间
  SI_WorkflowInfoChangedTime DATE NULL,               --工作流资料更新时间
  SI_EmptyRolesIsDeny NUMERIC(1) default 0 NOT NULL,  --(用户/功能)未配置角色代表授权规则为禁用
  SI_EasyAuthorization NUMERIC(1) default 1 NOT NULL, --宽松的授权
  PRIMARY KEY(SI_Name)
)
/
CREATE TABLE PH_HostInfo (                         --服务容器信息
  HI_Address VARCHAR(39) NOT NULL,                 --IP地址
  HI_Name VARCHAR(255) NOT NULL,         　        --主机名
  HI_Active NUMERIC(1) default 0 NOT NULL,         --活动中
  HI_ActiveTime DATE NOT NULL,                     --活动时间
  HI_LinkCount NUMERIC(15) default 0 NOT NULL,     --链接数
  PRIMARY KEY(HI_Address, HI_Name)
)
/
CREATE INDEX I_PH_HostInfo on PH_HostInfo (
   HI_ActiveTime DESC,
   HI_LinkCount ASC
)
/
CREATE TABLE PH_SequenceMarker (                   --序号标识
  SM_ID NUMERIC(3) default 0 NOT NULL,             --标识ID
  SM_HostAddress VARCHAR(39) NOT NULL,             --IP地址
  SM_HostName VARCHAR(255) NOT NULL,         　    --主机名
  SM_ActiveTime DATE NOT NULL,                     --活动时间
  PRIMARY KEY(SM_ID),
  UNIQUE(SM_HostAddress)
)
/
insert into PH_SystemInfo
(SI_Name, SI_Version, SI_Enterprise)
values
('Phenix .NET Framework','1.0.0.0','请填写企业名称')
/
