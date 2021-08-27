using System;

namespace Phenix.Test.使用指南._25._2
{
  [Serializable()]
  public class GuestUserFetchCommand : Phenix.Business.CommandBase<GuestUserFetchCommand>
  {
    public GuestUserFetchCommand()
    {
    }

    public User GuestUser { get; private set; }

    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// </summary>
    protected override void DoExecute()
    {
      try
      {
        Phenix.Core.Data.DbConnectionHelper.Execute(DataSourceKey, AccessData);
      }
      catch (Exception ex)
      {
        //如需要, 请在此拦截异常并做处理

        throw;
      }
    }

    //注释掉，因为本Command类的用处就是Fetch对象, 无需transaction
    //private void AccessData(System.Data.Common.DbTransaction transaction)
    //{
    //  //请使用业务对象处理逻辑
    //  //如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
    //  //如果拦截了异常，请处理后继续抛出，以便交给基类执行Rollback()
    //  using (System.Data.Common.DbCommand command = Phenix.Core.Data.DbCommandHelper.CreateCommand(transaction, SQL语句))
    //  {
    //    Phenix.Core.Data.DbCommandHelper.ExecuteNonQuery(command, false);
    //  }
    //}
    private void AccessData(System.Data.Common.DbConnection connection)
    {
      //不要浪费Phenix.Core.Data.DefaultDatabase.Execute()等调用者传入的DbConnection参数
      //可以使用业务类自带有DbConnection参数的Fetch等静态函数，它们都是为了在持久层被调用而提供的, 可节省序列化等中间环节带来的执行消耗
      GuestUser = User.Fetch(connection, p => p.Usernumber == Phenix.Core.Security.UserIdentity.GuestUserNumber);
      //函数结束时, 不要释放connection, 请交给传入connection的调用者负责收尾工作, 也就是'谁构造的资源就由谁负责释放'
      //注意: 这段文字的逻辑, 也适用于那些专门用来构造资源对象供调用者使用的工厂函数, 比如Phenix.Core.Data.DbCommandHelper.CreateCommand()函数
      //案例如上被注释掉的代码, 释放资源的职责是调用这些工厂函数的代码段, 工厂函数只负责构造资源给到调用者自由支配
      //一般都是函数调用链的外圈代码负责资源的释放, 不管是它从内圈函数获取资源还是自己构造资源给到内圈函数
    }
  }
}
