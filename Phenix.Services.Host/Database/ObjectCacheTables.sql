{*******************************************************************************
 *
 * Unit Name : ObjectCacheTables.sql
 * Purpose   : 对象缓存表集
 * Author    : wangming
 * Maintainer:
 * History   :
 *
 * CreateDate: 2009-10-17
 *******************************************************************************}

CREATE TABLE PH_CacheAction (         --缓存活动
  CA_ClassName VARCHAR(255) NOT NULL, --类名称
  CA_ActionTime DATE NOT NULL,        --活动时间
  PRIMARY KEY(CA_ClassName)
)
/

