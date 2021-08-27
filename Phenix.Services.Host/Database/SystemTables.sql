{*******************************************************************************
 *
 * Unit Name : SystemTables.sql
 * Purpose   : ϵͳ��
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
CREATE TABLE PH_SystemInfo (                          --ϵͳ��Ϣ
  SI_Name VARCHAR(30) NOT NULL,                       --����
  SI_Version VARCHAR(30) NOT NULL,                    --�汾��
  SI_Enterprise VARCHAR(255) NOT NULL,                --��ҵ�������桢����ȴ���ʾ����ҵ��ǩ��
  SI_AssemblyInfoChangedTime DATE NULL,               --�������ϸ���ʱ��
  SI_TableFilterInfoChangedTime DATE NULL,            --����������ϸ���ʱ��
  SI_RoleInfoChangedTime DATE NULL,                   --��ɫ���ϸ���ʱ��
  SI_SectionInfoChangedTime DATE NULL,                --��Ƭ���ϸ���ʱ��
  SI_DepartmentInfoChangedTime DATE NULL,             --�������ϸ���ʱ��
  SI_PositionInfoChangedTime DATE NULL,               --��λ���ϸ���ʱ��
  SI_TableInfoChangedTime DATE NULL,                  --��ṹ����ʱ��
  SI_WorkflowInfoChangedTime DATE NULL,               --���������ϸ���ʱ��
  SI_EmptyRolesIsDeny NUMERIC(1) default 0 NOT NULL,  --(�û�/����)δ���ý�ɫ������Ȩ����Ϊ����
  SI_EasyAuthorization NUMERIC(1) default 1 NOT NULL, --���ɵ���Ȩ
  PRIMARY KEY(SI_Name)
)
/
CREATE TABLE PH_HostInfo (                         --����������Ϣ
  HI_Address VARCHAR(39) NOT NULL,                 --IP��ַ
  HI_Name VARCHAR(255) NOT NULL,         ��        --������
  HI_Active NUMERIC(1) default 0 NOT NULL,         --���
  HI_ActiveTime DATE NOT NULL,                     --�ʱ��
  HI_LinkCount NUMERIC(15) default 0 NOT NULL,     --������
  PRIMARY KEY(HI_Address, HI_Name)
)
/
CREATE INDEX I_PH_HostInfo on PH_HostInfo (
   HI_ActiveTime DESC,
   HI_LinkCount ASC
)
/
CREATE TABLE PH_SequenceMarker (                   --��ű�ʶ
  SM_ID NUMERIC(3) default 0 NOT NULL,             --��ʶID
  SM_HostAddress VARCHAR(39) NOT NULL,             --IP��ַ
  SM_HostName VARCHAR(255) NOT NULL,         ��    --������
  SM_ActiveTime DATE NOT NULL,                     --�ʱ��
  PRIMARY KEY(SM_ID),
  UNIQUE(SM_HostAddress)
)
/
insert into PH_SystemInfo
(SI_Name, SI_Version, SI_Enterprise)
values
('Phenix .NET Framework','1.0.0.0','����д��ҵ����')
/
