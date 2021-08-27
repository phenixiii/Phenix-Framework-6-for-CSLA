{*******************************************************************************
 *
 * Unit Name : RuleTables.sql
 * Purpose   : 规则表集
 * Author    : wangming
 * Maintainer:
 * History   :
 *
 * CreateDate: 2010-11-09
 *******************************************************************************}

 CREATE TABLE PH_PromptCode (                 --提示码
  PC_Name VARCHAR(61) NOT NULL,               --名称
  PC_Key VARCHAR(30) NOT NULL,                --键
  PC_Caption VARCHAR(4000) NOT NULL,          --标签
  PC_Value VARCHAR(30) NOT NULL,              --值
  PC_ReadLevel NUMERIC(5) default 0 NOT NULL, --级别(Phenix.Core.Rule.ReadLevel)
  PC_Addtime DATE NOT NULL,                   --添加时间
  PC_UserNumber VARCHAR(10) NULL,             --登录工号
  PC_DP_ID NUMERIC(15) NULL,                  --部门
  PC_PT_ID NUMERIC(15) NULL,                  --岗位
  PRIMARY KEY(PC_Name, PC_Key)
)
/
CREATE TABLE PH_PromptCode_Action ( --提示码活动
  PC_Name VARCHAR(61) NOT NULL,     --名称
  PC_ActionTime DATE NOT NULL,      --活动时间
  PRIMARY KEY(PC_Name)
)
/
CREATE TABLE PH_CriteriaExpression (          --条件表达式
  CE_Name VARCHAR(255) NOT NULL,              --名称
  CE_Key VARCHAR(30) NOT NULL,                --键
  CE_Caption VARCHAR(4000) NULL,              --标签
  CE_Tree LONG /*TEXT*/ NULL,                 --表达式树
  CE_ReadLevel NUMERIC(5) default 0 NOT NULL, --级别(Phenix.Core.Rule.ReadLevel)
  CE_Addtime DATE NOT NULL,                   --添加时间
  CE_UserNumber VARCHAR(10) NULL,             --登录工号
  CE_DP_ID NUMERIC(15) NULL,                  --部门
  CE_PT_ID NUMERIC(15) NULL,                  --岗位
  PRIMARY KEY(CE_Name, CE_Key)
)
/
CREATE TABLE PH_CriteriaExpression_Action ( --条件表达式活动
  CE_Name VARCHAR(255) NOT NULL,            --名称
  CE_ActionTime DATE NOT NULL,              --活动时间
  PRIMARY KEY(CE_Name)
)
/

