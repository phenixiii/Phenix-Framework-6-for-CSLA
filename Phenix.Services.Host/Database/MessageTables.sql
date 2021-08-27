{*******************************************************************************
 *
 * Unit Name : MessageTables.sql
 * Purpose   : 消息表集
 * Author    : wangming
 * Maintainer:
 * History   :
 *
 * CreateDate: 2018-08-23
 *******************************************************************************}

 CREATE TABLE PH_Message (                    --消息
  MS_ID NUMERIC(15) NOT NULL,
  MS_Send_UserNumber VARCHAR(10) NOT NULL,    --发送工号
  MS_Receive_UserNumber VARCHAR(10) NOT NULL, --接收工号
  MS_CreatedTime DATE NOT NULL,               --创建时间
  MS_SendedTime DATE NULL,                    --发送时间
  MS_ReceivedTime DATE NULL,                  --收到时间
  MS_Content LONG /*TEXT*/ NULL,              --消息内容(未加校验码和头尾标志)
  PRIMARY KEY(MS_ID)
)
/
CREATE INDEX I_MS_Receive_UserNumber on PH_Message(MS_Receive_UserNumber, MS_CreatedTime)
/
CREATE TABLE PH_Friends (                --好友
  FE_ID NUMERIC(15) NOT NULL,
  FE_A_US_ID NUMERIC(15) NOT NULL,       --A用户
  FE_B_US_ID NUMERIC(15) NOT NULL,       --B用户
  FE_CreatedTime DATE NOT NULL,          --创建时间
  FE_Creater_US_ID NUMERIC(15) NOT NULL, --创建人
  FE_AcceptedTime DATE NULL,             --确认时间
  FE_RemovedTime DATE NULL,              --移除时间
  FE_Remover_US_ID NUMERIC(15) NULL,     --移除人
  PRIMARY KEY(FE_ID),
  UNIQUE(FE_A_US_ID, FE_B_US_ID)
)
/
CREATE TABLE PH_Group (                  --组
  GP_ID NUMERIC(15) NOT NULL,
  GP_Name VARCHAR(100) NOT NULL,         --名称
  GP_CreatedTime DATE NOT NULL,          --创建时间
  GP_Creater_US_ID NUMERIC(15) NOT NULL, --创建人
  PRIMARY KEY(GP_ID)
)
/
CREATE TABLE PH_Group_Admin (            --组管理人
  GA_ID NUMERIC(15) NOT NULL,
  GA_GP_ID NUMERIC(15) NOT NULL,         --组
  GA_US_ID NUMERIC(15) NOT NULL,         --用户
  GA_CreatedTime DATE NOT NULL,          --创建时间
  GA_Creater_US_ID NUMERIC(15) NOT NULL, --创建人
  PRIMARY KEY(GA_ID),
  UNIQUE(GA_GP_ID, GA_US_ID)
)
/
CREATE TABLE PH_Group_Members (          --组成员
  GM_ID NUMERIC(15) NOT NULL,
  GM_GP_ID NUMERIC(15) NOT NULL,         --组
  GM_US_ID NUMERIC(15) NOT NULL,         --用户
  GM_CreatedTime DATE NOT NULL,          --创建时间
  GM_Creater_US_ID NUMERIC(15) NOT NULL, --创建人
  PRIMARY KEY(GM_ID),
  UNIQUE(GM_GP_ID, GM_US_ID)
)
/
CREATE TABLE PH_MessageLog (                        --消息日志
  ML_ID NUMERIC(15) NOT NULL,
  ML_US_ID NUMERIC(15) NOT NULL,                    --发送人
  ML_Receiver_US_ID NUMERIC(15) default 0 NOT NULL, --接收人
  ML_Receiver_GP_ID NUMERIC(15) default 0 NOT NULL, --接收方
  ML_Body LONG NOT NULL,                            --报文体(未加校验码和头尾标志)
  ML_Type NUMERIC(5) NOT NULL,                      --报文类型(SchedulerInfomConst.TSendSign)
  ML_Sign NUMERIC(5) DEFAULT 0 NOT NULL,            --处理标识(SchedulerInfomConst.TSendSign)
  ML_CreatedTime DATE NOT NULL,                     --创建时间
  ML_SendedTime DATE NULL,                          --发送时间
  ML_ReceiptedTime DATE NULL,                       --收到时间
  PRIMARY KEY(ML_ID)
)
/
CREATE INDEX I_PH_ML_US_ID on PH_MessageLog (ML_US_ID)
/
CREATE INDEX I_PH_ML_Receiver_US_ID on PH_MessageLog (ML_Receiver_US_ID)
/
CREATE INDEX I_PH_ML_Receiver_GP_ID on PH_MessageLog (ML_Receiver_GP_ID)
/
CREATE INDEX I_PH_ML_CreatedTime on PH_MessageLog (ML_CreatedTime)
/