create table WGW_DELIVERY_JOB_TICKET_TEST
(
  DJT_ID                  NUMERIC(15) not null,
  DJT_SERIAL              VARCHAR(50) not null,
  DJT_INPUTER             VARCHAR(10) not null,
  DJT_INPUTTIME           DATE not null,
  PRIMARY KEY(DJT_ID)
)
/
-- Add comments to the table 
comment on table WGW_DELIVERY_JOB_TICKET_TEST
  is '出库作业单';
-- Add comments to the columns 
comment on column WGW_DELIVERY_JOB_TICKET_TEST.DJT_SERIAL
  is '出库单号';
comment on column WGW_DELIVERY_JOB_TICKET_TEST.DJT_INPUTER
  is '录入人';
comment on column WGW_DELIVERY_JOB_TICKET_TEST.DJT_INPUTTIME
  is '录入时间';