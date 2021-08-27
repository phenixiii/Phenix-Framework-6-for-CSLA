using System;
using Phenix.Core.Mapping;

namespace Phenix.Test.使用指南._25._2
{
  [Serializable()]
  public class GuestUserUpdateCommand : Phenix.Business.CommandBase<GuestUserUpdateCommand>
  {
    public GuestUserUpdateCommand(string guestName)
    {
      GuestName = guestName;
    }

    public string GuestName { get; private set; }

    //注释掉，可以改成通过Phenix.Core.Data.DefaultDatabase执行Execute函数
    ///// <summary>
    ///// 处理执行指令(运行在持久层的程序域里)
    ///// </summary>
    //protected override void DoExecute()
    //{
    //  try
    //  {
    //    Phenix.Core.Data.DbConnectionHelper.Execute(DataSourceKey, AccessData);
    //  }
    //  catch (Exception ex)
    //  {
    //    //如需要, 请在此拦截异常并做处理

    //    throw;
    //  }
    //}
    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// </summary>
    protected override void DoExecute()
    {
      try
      {
        Phenix.Core.Data.DefaultDatabase.Execute(AccessData);
      }
      catch (Exception ex)
      {
        //如需要, 请在此拦截异常并做处理

        throw;
      }
    }

    private void AccessData(System.Data.Common.DbTransaction transaction)
    {
      //请使用业务对象处理逻辑
      //如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
      //如果拦截了异常，请处理后继续抛出，以便交给基类执行Rollback()

      //不要浪费Phenix.Core.Data.DefaultDatabase.Execute()等调用者传入的DbTransaction参数
      //可以使用业务类自带有DbTransaction参数的UpdateRecord等静态函数，它们都是为了在持久层被调用而提供的, 可节省序列化等中间环节带来的执行消耗
      //这段代码的含义是, 用UserNumber作为过滤条件更新Name属性对应的表字段
      UserEasyList.UpdateRecord(transaction, 
        p => p.Usernumber == Phenix.Core.Security.UserIdentity.GuestUserNumber, 
        PropertyValue.Set(UserEasy.NameProperty, GuestName));
    }
  }
}
