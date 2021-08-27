using System;

namespace Phenix.StandardRule.Information
{
  /// <summary>
  /// 资料状态变更时事件内容
  /// </summary>
  public class InformationStatusChangingEventArgs : Phenix.Core.ShallEventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="information">资料</param>
    /// <param name="oldStatus">旧的资料状态</param>
    /// <param name="newStatus">新的资料状态</param>
    /// <param name="action">动作</param>
    internal InformationStatusChangingEventArgs(IInformation information,
      InformationStatus oldStatus, InformationStatus newStatus, InformationAction action)
      : base()
    {
      _information = information;
      _oldStatus = oldStatus;
      _newStatus = newStatus;
      _action = action;
    }

    #region 属性

    private readonly IInformation _information;
    /// <summary>
    /// 资料
    /// </summary>
    public IInformation Information
    {
      get { return _information; }
    }

    private readonly InformationStatus _oldStatus;
    /// <summary>
    /// 旧的资料状态
    /// </summary>
    public InformationStatus OldStatus
    {
      get { return _oldStatus; }
    }

    private readonly InformationStatus _newStatus;
    /// <summary>
    /// 新的资料状态
    /// </summary>
    public InformationStatus NewStatus
    {
      get { return _newStatus; }
    }

    private readonly InformationAction _action;
    /// <summary>
    /// 动作
    /// </summary>
    public InformationAction Action
    {
      get { return this._action; }
    }

    #endregion
  }
}

