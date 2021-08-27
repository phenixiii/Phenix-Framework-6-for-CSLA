{*******************************************************************************
 *
 * Unit Name : WorkflowTables.sql
 * Purpose   : 工作流表集
 * Author    : wangming
 * Maintainer:
 * History   :
 *
 * CreateDate: 2013-12-08
 *******************************************************************************}

CREATE TABLE PH_Workflow (                   --工作流
  WF_ID NUMERIC(15) NOT NULL,
  WF_Namespace VARCHAR(255) NOT NULL,        --命名空间
  WF_TypeName VARCHAR(255) NOT NULL,         --类型名称
  WF_Caption VARCHAR(4000) NULL,             --标签
  WF_XamlCode LONG /*TEXT*/ NULL,            --xaml代码
  WF_Create_UserNumber VARCHAR(10) NOT NULL, --构建工号
  WF_Create_Time DATE NOT NULL,              --构建时间
  WF_Change_UserNumber VARCHAR(10) NULL,     --更新工号
  WF_Change_Time DATE NULL,                  --更新时间
  WF_Disable_UserNumber VARCHAR(10) NULL,    --禁用工号
  WF_Disable_Time DATE NULL,                 --禁用时间
  PRIMARY KEY(WF_ID),
  UNIQUE(WF_Namespace, WF_TypeName)
)
/
CREATE TABLE PH_Workflow_Instance (      --工作流实例
  WI_ID VARCHAR(36) NOT NULL,            --工作流实例句柄(GUID)
  WI_WF_Namespace VARCHAR(255) NOT NULL, --命名空间
  WI_WF_TypeName VARCHAR(255) NOT NULL,  --类型名称
  WI_Content LONG /*TEXT*/ NULL,         --内容
  WI_Time DATE NOT NULL,                 --时间
  PRIMARY KEY(WI_ID)
)
/
CREATE TABLE PH_Workflow_TaskContext (   --工作流任务上下文
  WC_WI_ID VARCHAR(36) NOT NULL,         --工作流实例句柄(GUID)
  WC_Worker_UserNumber VARCHAR(10) NULL, --作业工号
  WC_Content LONG /*TEXT*/ NULL,         --内容
  WC_Time DATE NOT NULL,                 --时间
  PRIMARY KEY(WC_WI_ID)
)
/
CREATE TABLE PH_Workflow_Task (                 --工作流任务
  WT_WI_ID VARCHAR(36) NOT NULL,                --工作流实例句柄(GUID)
  WT_BookmarkName VARCHAR(255) NOT NULL,        --书签名称
  WT_Plugin_AssemblyName VARCHAR(255) NOT NULL, --插件程序集名
  WT_Worker_RL_Name VARCHAR(100) NULL,          --作业角色
  WT_Caption VARCHAR(4000) NULL,                --标签
  WT_Message VARCHAR(4000) NULL,                --消息
  WT_Urgent NUMERIC(1) default 0 NOT NULL,      --急件
  WT_State NUMERIC(5) default 0 NOT NULL,       --任务状态(Phenix.Core.Workflow.TaskState)
  WT_Dispatch_Time DATE NOT NULL,               --发送时间
  WT_Receive_Time DATE NULL,                    --接收时间
  WT_Hold_Time DATE NULL,                       --挂起时间
  WT_Abortive_Time DATE NULL,                   --中断时间
  WT_Complete_Time DATE NULL,                   --完结时间
  PRIMARY KEY(WT_WI_ID, WT_BookmarkName)
)
/