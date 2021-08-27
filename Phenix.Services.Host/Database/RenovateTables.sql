{*******************************************************************************
 *
 * Unit Name : RenovateTables.sql
 * Purpose   : ��̬ˢ�±�
 * Author    : wangming
 * Maintainer:
 * History   :
 *
 * CreateDate: 2001-12-19
 *******************************************************************************}

CREATE TABLE PH_RenovateLog (        --��̬ˢ����־
  RL_TableName VARCHAR(30) NOT NULL, --������
  RL_ROWID VARCHAR(18) NOT NULL,     --���¼��ROWID
  RL_Time DATE NOT NULL,             --����ʱ��
  RL_Action NUMERIC(5) NOT NULL      --ִ�ж���(Phenix.Core.Mapping.ExecuteAction)
)
/
CREATE INDEX I_PH_RL_Time on PH_RenovateLog(RL_Time)
/

--Ǩ��Packer
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
