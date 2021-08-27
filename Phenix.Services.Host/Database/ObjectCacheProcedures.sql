{*******************************************************************************
 *
 * Unit Name : ObjectCacheProcedures.sql
 * Purpose   : 对象缓存存储过程集
 * Author    : wangming
 * Maintainer:
 * History   :
 *
 * CreateDate: 2009-10-17
 *******************************************************************************}

--Oracle
CREATE PROCEDURE PH_Clear_ObjectCache(i_ClassName VARCHAR) as
begin
  update PH_CacheAction set CA_ActionTime = sysdate
  where CA_ClassName in 
    (select distinct(D.AC_Name)
      from PH_AssemblyClass A, PH_AssemblyClass_Group B, PH_AssemblyClass_Group C, PH_AssemblyClass D 
      where i_ClassName = A.AC_Name and A.AC_ID = B.AG_AC_ID
      and B.AG_Name = C.AG_Name and C.AG_AC_ID = D.AC_ID);
end;
/
CREATE PROCEDURE PH_Record_Has_Changed(i_TableName VARCHAR) as
begin
  update PH_CacheAction set CA_ActionTime = sysdate
  where CA_ClassName in 
    (select distinct(A.AC_Name)
      from PH_AssemblyClass A, PH_AssemblyClass_Group B 
      where A.AC_ID = B.AG_AC_ID and upper(B.AG_Name) = upper(i_TableName));
end;
/

--MSSql
CREATE PROCEDURE PH_Clear_ObjectCache @i_ClassName varchar(255) as
begin
  update PH_CacheAction set CA_ActionTime = getdate() 
  where CA_ClassName in 
    (select distinct(D.AC_Name)
      from PH_AssemblyClass A, PH_AssemblyClass_Group B, PH_AssemblyClass_Group C, PH_AssemblyClass D 
      where @i_ClassName = A.AC_Name and A.AC_ID = B.AG_AC_ID
      and B.AG_Name = C.AG_Name and C.AG_AC_ID = D.AC_ID);
end;
/
CREATE PROCEDURE PH_Record_Has_Changed @i_TableName varchar(30) as
begin
  update PH_CacheAction set CA_ActionTime = getdate() 
  where CA_ClassName in 
    (select distinct(A.AC_Name)
      from PH_AssemblyClass A, PH_AssemblyClass_Group B 
      where A.AC_ID = B.AG_AC_ID and upper(B.AG_Name) = upper(@i_TableName));
end;
/