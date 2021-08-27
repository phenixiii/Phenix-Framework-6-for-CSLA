{*******************************************************************************
 *
 * Unit Name : RenovateProcedures.sql
 * Purpose   : 动态刷新存储过程集
 * Author    : wangming
 * Maintainer:
 * History   :
 *
 * CreateDate: 2012-08-04
 *******************************************************************************}

--Oracle
CREATE PROCEDURE PH_Record_Has_Inserted(i_TableName VARCHAR, i_ROWID VARCHAR) as
begin
  insert into PH_RenovateLog
    (RL_TableName, RL_ROWID, RL_Time, RL_Action)
    values
     (i_TableName, i_ROWID, sysdate, 1);
end;
/
CREATE PROCEDURE PH_Record_Has_Updated(i_TableName VARCHAR, i_ROWID VARCHAR) as
begin
  insert into PH_RenovateLog
    (RL_TableName, RL_ROWID, RL_Time, RL_Action)
    values
     (i_TableName, i_ROWID, sysdate, 2);
end;
/
CREATE PROCEDURE PH_Record_Has_Deleted(i_TableName VARCHAR, i_ROWID VARCHAR) as
begin
  insert into PH_RenovateLog
    (RL_TableName, RL_ROWID, RL_Time, RL_Action)
    values
     (i_TableName, i_ROWID, sysdate, 3);
end;
/

--MSSql

/