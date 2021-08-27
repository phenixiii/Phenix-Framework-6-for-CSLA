{*******************************************************************************
 *
 * Unit Name : RenovateTables.sql
 * Purpose   : 动态刷新表集
 * Author    : wangming
 * Maintainer:
 * History   :
 *
 * CreateDate: 2001-12-19
 *******************************************************************************}

CREATE TABLE PH_RenovateLog (        --动态刷新日志
  RL_TableName VARCHAR(30) NOT NULL, --表名称
  RL_ROWID VARCHAR(18) NOT NULL,     --表记录的ROWID
  RL_Time DATE NOT NULL,             --更新时间
  RL_Action NUMERIC(5) NOT NULL      --执行动作(Phenix.Core.Mapping.ExecuteAction)
)
/
CREATE INDEX I_PH_RL_Time on PH_RenovateLog(RL_Time)
/

--迁移Packer
create or replace trigger DataUpdate_LOG_T
after insert
on DataUpdate_LOG
for each row
begin
  insert into PH_RenovateLog
    (RL_TableName, RL_ROWID, RL_Time)
  values
    (:new.DUL_DT_Name, :new.DUL_ROWID, :new.DUL_Time);
end DataUpdate_LOG_T;
/
