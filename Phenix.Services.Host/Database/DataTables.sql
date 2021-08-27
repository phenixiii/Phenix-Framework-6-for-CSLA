{*******************************************************************************
 *
 * Unit Name : DataTables.sql
 * Purpose   : 数据表集
 * Author    : wangming
 * Maintainer:
 * History   :
 *
 * CreateDate: 2013-12-12
 *******************************************************************************}

 CREATE TABLE PH_BusinessCode (                  --业务码
  BC_Name VARCHAR(4000) NOT NULL,                --名称(BusinessCodeName_BusinessCodeCriteriaPropertyName=BusinessCodeCriteriaProperty值)
  BC_Caption VARCHAR(100) NULL,			         --标签
  BC_FormatString VARCHAR(4000) NOT NULL,        --格式字符串
  BC_FillOnSaving NUMERIC(1) default 0 NOT NULL, --是否在提交时填充值
  PRIMARY KEY(BC_Name)
)
/
CREATE TABLE PH_Serial (         --流水号
  SR_Key VARCHAR(4000) NOT NULL, --键
  SR_Value NUMERIC(15) NOT NULL, --值
  SR_Time DATE NOT NULL,         --时间
  PRIMARY KEY(SR_Key)
)
/