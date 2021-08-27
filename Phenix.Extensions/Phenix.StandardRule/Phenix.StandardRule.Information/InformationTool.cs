using System;
using System.Collections.Generic;
using System.Reflection;

namespace Phenix.StandardRule.Information
{
  /// <summary>
  /// 资料工具
  /// </summary>
  public class InformationTool
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public InformationTool()
    {
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="information">资料</param>
    public InformationTool(IInformation information)
    {
      Information = information;
    }

    #region 属性

    #region 可操作条件

    /// <summary>
    /// 是否允许提交审核
    /// </summary>
    public bool AllowSubmitVerify
    {
      get { return GetAllowSubmitVerify(Information); }
    }

    /// <summary>
    /// 是否允许审核资料
    /// </summary>
    public bool AllowVerify
    {
      get { return GetAllowVerify(Information); }
    }
    
    /// <summary>
    /// 是否允许合并作废资料
    /// </summary>
    public bool AllowMergeInvalid
    {
      get { return GetAllowMergeInvalid(Information); }
    }

    /// <summary>
    /// 是否允许暂停使用资料
    /// </summary>
    public bool AllowSuspend
    {
      get { return GetAllowSuspend(Information); }
    }

    /// <summary>
    /// 是否允许恢复使用资料
    /// </summary>
    public bool AllowApply
    {
      get { return GetAllowApply(Information); }
    }

    #endregion

    private IInformation _information;
    /// <summary>
    /// 资料
    /// </summary>
    public IInformation Information
    {
      get { return _information; }
      set
      {
        if (_information == value)
          return;
        
        _information = value;

        InitializeInformation();
      }
    }

    /// <summary>
    /// 资料标签
    /// </summary>
    public string InformationCaption
    {
      get { return _information != null ? _information.Caption : String.Empty; }
    }

    #endregion

    #region 事件

    ///<summary>
    /// 资料状态发生变化前
    ///</summary>
    public event EventHandler<InformationStatusChangingEventArgs> InformationStatusChanging;

    ///<summary>
    /// 资料状态发生变化后
    ///</summary>
    public event EventHandler<InformationStatusChangedEventArgs> InformationStatusChanged;

    /// <summary>
    /// 资料状态发生变化前
    /// </summary>
    /// <param name="e">资料状态变更事件内容</param>
    protected void OnInformationStatusChanging(InformationStatusChangingEventArgs e)
    {
      EventHandler<InformationStatusChangingEventArgs> handler = InformationStatusChanging;
      if (handler != null)
        handler(null, e);
      e.Information.OnInformationStatusChanging(e);
    }

    /// <summary>
    /// 资料状态发生变化后
    /// </summary>
    /// <param name="e">资料状态变更事件内容</param>
    protected void OnInformationStatusChanged(InformationStatusChangedEventArgs e)
    {
      EventHandler<InformationStatusChangedEventArgs> handler = InformationStatusChanged;
      if (handler != null)
        handler(null, e);
      e.Information.OnInformationStatusChanged(e);
    }

    #endregion

    #region 方法

    #region 可操作条件

    /// <summary>
    /// 是否允许提交审核
    /// </summary>
    /// <param name="information">资料</param>
    public static bool GetAllowSubmitVerify(IInformation information)
    {
      return information != null &&
        information.AllowEdit &&
        information.InformationStatus == InformationStatus.Registration &&
        information.VerifyStatus != InformationVerifyStatus.Submitted &&
        information.NeedVerify &&
        !information.Disabled;
    }

    /// <summary>
    /// 是否允许提交审核
    /// </summary>
    /// <param name="informations">资料队列</param>
    public static bool GetAllowSubmitVerify(IEnumerable<IInformation> informations)
    {
      foreach (IInformation item in informations)
        if (!GetAllowSubmitVerify(item))
          return false;
      return true;
    }

    /// <summary>
    /// 是否允许审核资料
    /// </summary>
    /// <param name="information">资料</param>
    public static bool GetAllowVerify(IInformation information)
    {
      return information != null &&
        information.AllowEdit &&
        information.InformationStatus == InformationStatus.Registration &&
        information.VerifyStatus == InformationVerifyStatus.Submitted &&
        information.NeedVerify &&
        !information.Disabled;
    }
    
    /// <summary>
    /// 是否允许审核资料
    /// </summary>
    /// <param name="informations">资料队列</param>
    public static bool GetAllowVerify(IEnumerable<IInformation> informations)
    {
      foreach (IInformation item in informations)
        if (!GetAllowVerify(item))
          return false;
      return true;
    }

    /// <summary>
    /// 是否允许合并作废资料
    /// </summary>
    /// <param name="information">资料</param>
    public static bool GetAllowMergeInvalid(IInformation information)
    {
      return information != null &&
        information.AllowEdit &&
        information.InformationStatus == InformationStatus.Registration &&
        information.VerifyStatus == InformationVerifyStatus.NotPass &&
        information.NeedVerify &&
        !information.Disabled;
    }

    /// <summary>
    /// 是否允许合并作废资料
    /// </summary>
    /// <param name="informations">资料队列</param>
    public static bool GetAllowMergeInvalid(IEnumerable<IInformation> informations)
    {
      foreach (IInformation item in informations)
        if (!GetAllowMergeInvalid(item))
          return false;
      return true;
    }

    /// <summary>
    /// 是否允许暂停使用资料
    /// </summary>
    /// <param name="information">资料</param>
    public static bool GetAllowSuspend(IInformation information)
    {
      return information != null &&
        information.AllowEdit &&
        information.InformationStatus == InformationStatus.Formal &&
        !information.Disabled;
    }

    /// <summary>
    /// 是否允许暂停使用资料
    /// </summary>
    /// <param name="informations">资料队列</param>
    public static bool GetAllowSuspend(IEnumerable<IInformation> informations)
    {
      foreach (IInformation item in informations)
        if (!GetAllowSuspend(item))
          return false;
      return true;
    }

    /// <summary>
    /// 是否允许恢复使用资料
    /// </summary>
    /// <param name="information">资料</param>
    public static bool GetAllowApply(IInformation information)
    {
      return information != null &&
        information.AllowEdit &&
        information.InformationStatus == InformationStatus.Suspended &&
        !information.Disabled;
    }

    /// <summary>
    /// 是否允许恢复使用资料
    /// </summary>
    /// <param name="informations">资料队列</param>
    public static bool GetAllowApply(IEnumerable<IInformation> informations)
    {
      foreach (IInformation item in informations)
        if (!GetAllowApply(item))
          return false;
      return true;
    }

    #endregion

    /// <summary>
    /// 执行
    /// </summary>
    public void Execute(InformationAction action)
    {
      switch (action)
      {
        case InformationAction.SubmitVerify:
          SubmitVerify();
          break;
        case InformationAction.VerifyPass:
          VerifyPass();
          break;
        case InformationAction.VerifyNotPass:
          VerifyNotPass();
          break;
        case InformationAction.Merge:
          Merge();
          break;
        case InformationAction.Invalid:
          Invalid();
          break;
        case InformationAction.Suspend:
          Suspend();
          break;
        case InformationAction.Apply:
          Apply();
          break;
        default:
          InitializeInformation();
          break;
      }
    }

    /// <summary>
    /// 提交审核
    /// </summary>
    public virtual void SubmitVerify()
    {
      if (!AllowSubmitVerify)
        throw new InformationException(InformationAction.SubmitVerify, String.Format(Properties.Resources.NotAllowSubmitVerify, InformationCaption));

      ChangeInformationVerifyStatus(InformationVerifyStatus.Submitted);
    }

    /// <summary>
    /// 通过审核
    /// </summary>
    public virtual void VerifyPass()
    {
      if (!AllowVerify)
        throw new InformationException(InformationAction.VerifyPass, String.Format(Properties.Resources.NotAllowVerify, InformationCaption));

      ChangeInformationStatus(InformationStatus.Formal, InformationAction.VerifyPass);
      ChangeInformationVerifyStatus(InformationVerifyStatus.Pass);
    }

    /// <summary>
    /// 未通过审核
    /// </summary>
    public virtual void VerifyNotPass()
    {
      if (!AllowVerify)
        throw new InformationException(InformationAction.VerifyNotPass, String.Format(Properties.Resources.NotAllowVerify, InformationCaption));

      ChangeInformationVerifyStatus(InformationVerifyStatus.NotPass);
    }

    /// <summary>
    /// 合并资料
    /// </summary>
    public virtual void Merge()
    {
      if (!AllowMergeInvalid)
        throw new InformationException(InformationAction.Merge, String.Format(Properties.Resources.NotAllowMergeInvalid, InformationCaption));

      ChangeInformationStatus(InformationStatus.Merged, InformationAction.Merge);
    }

    /// <summary>
    /// 作废资料
    /// </summary>
    public virtual void Invalid()
    {
      if (!AllowMergeInvalid)
        throw new InformationException(InformationAction.Invalid, String.Format(Properties.Resources.NotAllowMergeInvalid, InformationCaption));

      ChangeInformationStatus(InformationStatus.Invalided, InformationAction.Invalid);
    }
    
    /// <summary>
    /// 暂停使用资料
    /// </summary>
    /// <exception cref="InformationException"></exception>
    public virtual void Suspend()
    {
      if (!AllowSuspend)
        throw new InformationException(InformationAction.Suspend, String.Format(Properties.Resources.NotAllowSuspend, InformationCaption));

      ChangeInformationStatus(InformationStatus.Suspended, InformationAction.Suspend);
    }

    /// <summary>
    /// 恢复使用资料
    /// </summary>
    /// <exception cref="InformationException"></exception>
    public virtual void Apply()
    {
      if (!AllowApply)
        throw new InformationException(InformationAction.Apply, String.Format(Properties.Resources.NotAllowApply, InformationCaption));

      ChangeInformationStatus(InformationStatus.Formal, InformationAction.Apply);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void InitializeInformation()
    {
      if (Information == null || Information.InformationStatus != InformationStatus.Init)
        return;

      ChangeInformationStatus(Information.NeedVerify ? InformationStatus.Registration : InformationStatus.Formal, InformationAction.Create);
    }

    private void ChangeInformationStatus(InformationStatus value, InformationAction action)
    {
      InformationStatusChangingEventArgs args = new InformationStatusChangingEventArgs(
        Information, Information.InformationStatus, value, action);
      OnInformationStatusChanging(args);
      if (args.Stop)
        return;
      if (!args.Applied)
      {
        //将Information全部InformationStatus类型的属性值设置成value
        foreach (PropertyInfo item in Information.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
          if (item.PropertyType == typeof(InformationStatus) && item.CanWrite)
          {
            item.SetValue(Information, value, null);
            //break;
          }
      }
      if (args.Succeed)
        OnInformationStatusChanged(new InformationStatusChangedEventArgs(args));
    }

    private void ChangeInformationVerifyStatus(InformationVerifyStatus value)
    {
      //将Information全部InformationVerifyStatus类型的属性值设置成value
      foreach (PropertyInfo item in Information.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        if (item.PropertyType == typeof(InformationVerifyStatus) && item.CanWrite)
        {
          item.SetValue(Information, value, null);
          //break;
        }
    }

    #endregion
  }
}
