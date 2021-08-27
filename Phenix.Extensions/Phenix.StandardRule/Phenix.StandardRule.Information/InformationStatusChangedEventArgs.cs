using System;

namespace Phenix.StandardRule.Information
{
  /// <summary>
  /// 资料状态变更后事件内容
  /// </summary>
  public class InformationStatusChangedEventArgs : EventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="informationStatusChangingEventArgs">资料状态变更前事件内容</param>
    internal InformationStatusChangedEventArgs(InformationStatusChangingEventArgs informationStatusChangingEventArgs)
      : base()
    {
      _information = informationStatusChangingEventArgs.Information;
      _oldStatus = informationStatusChangingEventArgs.OldStatus;
      _newStatus = informationStatusChangingEventArgs.NewStatus;
      _action = informationStatusChangingEventArgs.Action;
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
      get { return _action; }
    }

    #endregion
  }
}

