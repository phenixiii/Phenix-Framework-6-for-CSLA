using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Phenix.Business;
using Phenix.Core;
using Phenix.Core.Windows;
using Phenix.StandardRule.Information;

namespace Phenix.StandardRule.InformationControl
{
  /// <summary>
  /// 资料活动按钮组
  /// </summary>
  public partial class InformationActionButtonGroup : DevExpress.XtraEditors.XtraUserControl
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public InformationActionButtonGroup()
    {
      InitializeComponent();

      this.btnSubmitVerify.Click += new EventHandler(SubmitVerifyButton_Click);
      this.btnVerifyPass.Click += new EventHandler(VerifyPassButton_Click);
      this.btnVerifyNotPass.Click += new EventHandler(VerifyNotPassButton_Click);
      this.btnMerge.Click += new EventHandler(MergeButton_Click);
      this.btnInvalid.Click += new EventHandler(InvalidButton_Click);
      this.btnSuspend.Click += new EventHandler(SuspendButton_Click);
      this.btnApply.Click += new EventHandler(ApplyButton_Click);
    }

    #region 成员变量及属性

    private BindingSource _bindingSource;
    /// <summary>
    /// 数据源
    /// </summary>
    [DefaultValue(null), Description("消息控件对应的操作数据源\n注：需实现IInformation接口"), Category("Data")]
    public BindingSource BindingSource
    {
      get { return _bindingSource; }
      set
      {
        if (value == _bindingSource)
          return;
        if (!DesignMode && _bindingSource != null)
        {
          _bindingSource.DataSourceChanged -= new EventHandler(BindingSource_DataSourceChanged);
          _bindingSource.ListChanged -= new ListChangedEventHandler(BindingSource_ListChanged);
          _bindingSource.PositionChanged -= new EventHandler(BindingSource_PositionChanged);
        }
        _bindingSource = value;
        if (DesignMode)
        {
          if (value != null)
          {
            Type coreType = BindingSourceHelper.GetDataSourceCoreType(value);
            if (coreType != null && !typeof(IInformation).IsAssignableFrom(coreType))
            {
              _bindingSource = null;
              throw new InvalidOperationException(String.Format("请为类{0}实现{1}接口", value.GetType().FullName, typeof(IInformation).FullName));
            }
          }
        }
        else
        {
          if (value != null)
          {
            value.DataSourceChanged += new EventHandler(BindingSource_DataSourceChanged);
            value.ListChanged += new ListChangedEventHandler(BindingSource_ListChanged);
            value.PositionChanged += new EventHandler(BindingSource_PositionChanged);
          }
          ResetButtonStatus();
        }
      }
    }

    /// <summary>
    /// 当前资料清单
    /// </summary>
    [Browsable(false)]
    public IInformationList<IInformation> CurrentInformationList
    {
      get { return BindingSourceHelper.GetDataSourceList(BindingSource) as IInformationList<IInformation>; }
    }

    /// <summary>
    /// 当前资料
    /// </summary>
    [Browsable(false)]
    public IInformation CurrentInformation
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(BindingSource) as IInformation; }
    }

    /// <summary>
    /// 当前业务对象
    /// </summary>
    [Browsable(false)]
    public IBusiness CurrentBusiness
    {
      get
      {
        if (CurrentInformationList != null)
          return CurrentInformationList;
        return CurrentInformation;
      }
    }

    private new bool DesignMode
    {
      get { return base.DesignMode || AppConfig.DesignMode; }
    }

    #endregion

    #region 事件

    ///<summary>
    /// 资料状态发生变化前
    ///</summary>
    public event EventHandler<InformationStatusChangingEventArgs> InformationTool_InformationStatusChanging;

    ///<summary>
    /// 资料状态发生变化后
    ///</summary>
    public event EventHandler<InformationStatusChangedEventArgs> InformationTool_InformationStatusChanged;

    #region 按钮触发执行

    private void SubmitVerifyButton_Click(object sender, EventArgs e)
    {
      Execute(InformationAction.SubmitVerify);
    }

    private void VerifyPassButton_Click(object sender, EventArgs e)
    {
      Execute(InformationAction.VerifyPass);
    }

    private void VerifyNotPassButton_Click(object sender, EventArgs e)
    {
      Execute(InformationAction.VerifyNotPass);
    }

    private void MergeButton_Click(object sender, EventArgs e)
    {
      Execute(InformationAction.Merge);
    }

    private void InvalidButton_Click(object sender, EventArgs e)
    {
      Execute(InformationAction.Invalid);
    }

    private void SuspendButton_Click(object sender, EventArgs e)
    {
      Execute(InformationAction.Suspend);
    }

    private void ApplyButton_Click(object sender, EventArgs e)
    {
      Execute(InformationAction.Apply);
    }

    private void BindingSource_DataSourceChanged(object sender, EventArgs e)
    {
      ResetButtonStatus();
    }

    private void BindingSource_ListChanged(object sender, ListChangedEventArgs e)
    {
      ResetButtonStatus();
    }

    private void BindingSource_PositionChanged(object sender, EventArgs e)
    {
      ResetButtonStatus();
    }

    #endregion

    #endregion

    #region 方法

    private void ResetButtonStatus()
    {
      if (CurrentInformationList != null && CurrentInformationList.SelectedItems.Count > 0)
      {
        IEnumerable<IInformation> informations = (IEnumerable<IInformation>)CurrentInformationList.SelectedItems;
        this.btnSubmitVerify.Enabled = InformationTool.GetAllowSubmitVerify(informations);
        this.btnVerifyPass.Enabled = InformationTool.GetAllowVerify(informations);
        this.btnVerifyNotPass.Enabled = InformationTool.GetAllowVerify(informations);
        this.btnMerge.Enabled = InformationTool.GetAllowMergeInvalid(informations);
        this.btnInvalid.Enabled = InformationTool.GetAllowMergeInvalid(informations);
        this.btnSuspend.Enabled = InformationTool.GetAllowSuspend(informations);
        this.btnApply.Enabled = InformationTool.GetAllowApply(informations);
      }
      else
      {
        this.btnSubmitVerify.Enabled = InformationTool.GetAllowSubmitVerify(CurrentInformation);
        this.btnVerifyPass.Enabled = InformationTool.GetAllowVerify(CurrentInformation);
        this.btnVerifyNotPass.Enabled = InformationTool.GetAllowVerify(CurrentInformation);
        this.btnMerge.Enabled = InformationTool.GetAllowMergeInvalid(CurrentInformation);
        this.btnInvalid.Enabled = InformationTool.GetAllowMergeInvalid(CurrentInformation);
        this.btnSuspend.Enabled = InformationTool.GetAllowSuspend(CurrentInformation);
        this.btnApply.Enabled = InformationTool.GetAllowApply(CurrentInformation);
      }
    }

    private void DoExecute(InformationTool tool, IInformation information, InformationAction action)
    {
      if (InformationTool_InformationStatusChanging != null)
        tool.InformationStatusChanging += new EventHandler<InformationStatusChangingEventArgs>(InformationTool_InformationStatusChanging);
      if (InformationTool_InformationStatusChanged != null)
        tool.InformationStatusChanged += new EventHandler<InformationStatusChangedEventArgs>(InformationTool_InformationStatusChanged);
      try
      {
        tool.Information = information;
        tool.Execute(action);
      }
      finally
      {
        if (InformationTool_InformationStatusChanging != null)
          tool.InformationStatusChanging -= new EventHandler<InformationStatusChangingEventArgs>(InformationTool_InformationStatusChanging);
        if (InformationTool_InformationStatusChanged != null)
          tool.InformationStatusChanged -= new EventHandler<InformationStatusChangedEventArgs>(InformationTool_InformationStatusChanged);
      }
    }

    private void Execute(InformationAction action)
    {
      if (CurrentBusiness == null)
        return;
      //如果不处于编辑状态则置为编辑状态
      bool editMode = CurrentBusiness.Root.EditMode;
      if (!editMode)
        CurrentBusiness.Root.BeginEdit();
      //对当前对象及被选对象，变更资料状态
      InformationTool tool = new InformationTool();
      if (CurrentInformationList != null && CurrentInformationList.SelectedItems.Count > 0)
      {
        foreach (IInformation item in CurrentInformationList.SelectedItems)
          DoExecute(tool, item, action);
      }
      else
        DoExecute(tool, CurrentInformation, action);
      //如果原状态为非编辑状态则保存
      if (!editMode)
        CurrentBusiness.Root.Save();
      //重置按钮组状态
      ResetButtonStatus();
    }

    #endregion
  }
}