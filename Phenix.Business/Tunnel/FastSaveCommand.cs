using System;
using System.Collections.Generic;
using System.Data.Common;
using Phenix.Business.Core;
using Phenix.Core;
using Phenix.Core.Data;

namespace Phenix.Business.Tunnel
{
  /// <summary>
  /// 快速提交业务对象，允许不检查业务规则
  /// </summary>
  [Serializable]
  public class FastSaveCommand : Csla.CommandBase<FastSaveCommand>
  {
    /// <summary>
    /// 初始化
    /// dataSourceKey = null
    /// aloneTransaction = false
    /// </summary>
    public FastSaveCommand()
      : this(null, false, null) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="dataSourceKey">数据源键</param>
    /// <param name="aloneTransaction">是否Businesses各自使用独立事务</param>
    public FastSaveCommand(string dataSourceKey, bool aloneTransaction)
      : this(dataSourceKey, aloneTransaction, null) { }

    /// <summary>
    /// 初始化
    /// aloneTransaction = false
    /// </summary>
    /// <param name="dataSourceKey">数据源键</param>
    public FastSaveCommand(string dataSourceKey)
      : this(dataSourceKey, false, null) { }

    /// <summary>
    /// 初始化
    /// dataSourceKey = null
    /// </summary>
    /// <param name="aloneTransaction">是否Businesses各自使用独立事务</param>
    public FastSaveCommand(bool aloneTransaction)
      : this(null, aloneTransaction, null) { }

    /// <summary>
    /// 初始化
    /// dataSourceKey = null
    /// aloneTransaction = false
    /// </summary>
    /// <param name="businesses">需提交后台的业务对象</param>
    public FastSaveCommand(IList<IBusiness> businesses)
      : this(null, false, businesses) { }

    /// <summary>
    /// 初始化
    /// aloneTransaction = false
    /// </summary>
    /// <param name="dataSourceKey">数据源键</param>
    /// <param name="businesses">需提交后台的业务对象</param>
    public FastSaveCommand(string dataSourceKey, IList<IBusiness> businesses)
      : this(dataSourceKey, false, businesses) { }

    /// <summary>
    /// 初始化
    /// dataSourceKey = null
    /// </summary>
    /// <param name="aloneTransaction">是否Businesses各自使用独立事务</param>
    /// <param name="businesses">需提交后台的业务对象</param>
    public FastSaveCommand(bool aloneTransaction, IList<IBusiness> businesses)
      : this(null, aloneTransaction, businesses) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="dataSourceKey">数据源键</param>
    /// <param name="aloneTransaction">是否Businesses各自使用独立事务</param>
    /// <param name="businesses">需提交后台的业务对象</param>
    public FastSaveCommand(string dataSourceKey, bool aloneTransaction, IList<IBusiness> businesses)
      : base()
    {
      _dataSourceKey = dataSourceKey;
      _aloneTransaction = aloneTransaction;
      _businesses = businesses ?? new List<IBusiness>();
    }
    
    #region 属性

    [Csla.NotUndoable]
    private readonly string _dataSourceKey;
    /// <summary>
    /// 数据源键
    /// 缺省为 null
    /// </summary>
    public string DataSourceKey
    {
      get { return _dataSourceKey; }
    }

    [Csla.NotUndoable]
    private readonly bool _aloneTransaction;
    /// <summary>
    /// 是否Businesses各自使用独立事务
    /// 缺省为 false
    /// </summary>
    public bool AloneTransaction
    {
      get { return _aloneTransaction; }
    }

    [Csla.NotUndoable]
    private readonly IList<IBusiness> _businesses;
    /// <summary>
    /// 需提交后台的业务对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IList<IBusiness> Businesses
    {
      get { return _businesses; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 执行指令
    /// </summary>
    public void Execute()
    {
      Csla.DataPortal.Update(this);
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      foreach (IRefinedly item in _businesses)
        item.CompletelyApplyEdit(true, false, false, true, ref ignoreLinks);
    }

    #region Data Access

    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void DataPortal_Execute()
    {
      DbConnectionHelper.Execute(DataSourceKey, DoExecute);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void DoExecute(DbTransaction transaction)
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      if (AloneTransaction)
      {
        List<ExceptionEventArgs> saveErrors = new List<ExceptionEventArgs>();
        foreach (IRefinedly item in _businesses)
          try
          {
            item.SaveSelf(null, ref ignoreLinks);
          }
          catch (Exception ex)
          {
            saveErrors.Add(new ExceptionEventArgs(item, ex));
          }
        if (saveErrors.Count > 0)
          throw new SaveException(saveErrors);
      }
      else
      {
        foreach (IRefinedly item in _businesses)
          item.SaveSelf(transaction, ref ignoreLinks);
      }
    }

    #endregion

    #endregion
  }
}
