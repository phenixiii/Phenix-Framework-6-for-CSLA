using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using DevExpress.XtraNavBar;
using Phenix.Core;
using Phenix.Core.Mapping;
using Phenix.Core.Rule;
using Phenix.Core.Windows;

namespace Phenix.Windows
{
  /// <summary>
  /// 查询组合框控件
  /// </summary>
  [Description("查询组合框")]
  [ToolboxItem(true), ToolboxBitmap(typeof(CriteriaCombineControl), "Phenix.Windows.CriteriaCombineControl")]
  public sealed partial class CriteriaCombineControl : UserControl, ISupportInitialize
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public CriteriaCombineControl()
    {
      InitializeComponent();
    }

    #region 属性

    private new bool DesignMode
    {
      get { return base.DesignMode || AppConfig.DesignMode; }
    }

    private Control _host;
    /// <summary>
    /// 所属容器
    /// </summary>
    [DefaultValue(null), Browsable(false)]
    public Control Host
    {
      get
      {
        if (_host == null)
        {
          if (DesignMode)
          {
            IDesignerHost designer = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (designer != null)
              _host = designer.RootComponent as Control;
          }
        }
        return _host;
      }
      set
      {
        if (!DesignMode && _host != null)
          throw new InvalidOperationException("运行期不允许修改Host");
        _host = value;
        if (!DesignMode)
        {
          Form form = value as Form;
          if (form != null)
            form.Shown += new EventHandler(Host_Shown);
        }
      }
    }

    /// <summary>
    /// 数据源项的类型
    /// </summary>
    [DefaultValue(null), Description("数据源项的类型\n设置为绑定的业务类型"), Category("Data")]
    [Editor(typeof(Phenix.Services.Client.Design.BusinessSelectorEditor), typeof(UITypeEditor))]
    public Type OperateClassType { get; set; }

    /// <summary>
    /// 条件表达式名称
    /// </summary>
    [DefaultValue(null), Description("条件表达式名称\n缺省情况下为所在窗体类全名+控件名+操作类全名"), Category("Design")]
    public string CriteriaExpressionName { get; set; }
    /// <summary>
    /// 条件表达式名称
    /// </summary>
    private string CriteriaExpressionNameValue
    {
      get
      {
        if (String.IsNullOrEmpty(CriteriaExpressionName))
          return String.Format("{0}.{1}.{2}", Host.GetType().FullName, Name, OperateClassType != null ? OperateClassType.FullName : String.Empty);
        return CriteriaExpressionName;
      }
    }

    private ReadLevel _defaultReadLevel = ReadLevel.Public;
    /// <summary>
    /// 缺省读取级别
    /// </summary>
    [DefaultValue(ReadLevel.Public), Description("缺省读取级别\n当新增条件表达式时缺省定义的读取级别"), Category("Data")]
    public ReadLevel DefaultReadLevel
    {
      get { return _defaultReadLevel; }
      set { _defaultReadLevel = value; }
    }

    private CriteriaExpressionKeyCaptionCollection CriteriaExpressionKeyCaptionCollection
    {
      get { return BindingSourceHelper.GetDataSourceList(this.criteriaExpressionKeyCaptionCollectionBindingSource) as CriteriaExpressionKeyCaptionCollection; }
      set { this.criteriaExpressionKeyCaptionCollectionBindingSource.DataSource = value; }
    }

    private CriteriaExpressionKeyCaption WorkingCriteriaExpressionKeyCaption
    {
      get { return CriteriaExpressionKeyCaptionCollection.FindByValue(WorkingCriteriaExpression); }
      set { WorkingCriteriaExpression = value != null ? value.Value : null; }
    }

    /// <summary>
    /// 当前操作的条件表达式
    /// </summary>
    [Browsable(false)]
    public CriteriaExpression WorkingCriteriaExpression
    {
      get { return this.criteriaExpressionLookUpBarEditItem.EditValue as CriteriaExpression; }
      private set { this.criteriaExpressionLookUpBarEditItem.EditValue = value; }
    }

    private CriteriaCombineItemControl ActiveCriteriaCombineItemControl { get; set; }

    private NavBarGroup ActiveCriteriaCombineNavBarGroup
    {
      get
      {
        if (ActiveCriteriaCombineItemControl == null || ActiveCriteriaCombineItemControl.Parent == null)
          return null;
        return ((NavBarGroupControlContainer)ActiveCriteriaCombineItemControl.Parent).OwnerGroup;
      }
    }

    #endregion

    #region 事件

    private void CriteriaCombineItemControl_Enter(object sender, EventArgs e)
    {
      ActiveCriteriaCombineItemControl = (CriteriaCombineItemControl)sender;
      ApplyRules();
    }
   
    #endregion

    #region 方法

    #region ISupportInitialize 成员

    ///<summary>
    /// 开始初始化
    ///</summary>
    public void BeginInit()
    {
    }

    ///<summary>
    /// 结束初始化
    ///</summary>
    public void EndInit()
    {
      if (!DesignMode && !(Host is Form))
        Host_Shown(null, null);
    }

    #endregion

    private void ApplyRules()
    {
      this.criteriaExpressionNavBarControl.Visible = WorkingCriteriaExpression != null;
      this.criteriaExpressionDeleteBarButtonItem.Enabled = WorkingCriteriaExpression != null;
      this.criteriaExpressionUpdateBarButtonItem.Enabled = WorkingCriteriaExpression != null;
      this.addGroupBarButtonItem.Enabled = WorkingCriteriaExpression != null;
      this.addBarButtonItem.Enabled = WorkingCriteriaExpression != null &&
        ActiveCriteriaCombineNavBarGroup != null;
      this.deleteGroupBarButtonItem.Enabled = WorkingCriteriaExpression != null &&
        ActiveCriteriaCombineNavBarGroup != null && this.criteriaExpressionNavBarControl.Groups.IndexOf(ActiveCriteriaCombineNavBarGroup) != 0;
    }

    private void InitializeCriteriaExpressionKeyCaptionCollection()
    {
      if (OperateClassType == null)
        throw new InvalidOperationException("由于OperateClassType属性为空无法获取查询条件");
      CriteriaExpressionKeyCaptionCollection = CriteriaExpressionKeyCaptionCollection.Fetch(OperateClassType, CriteriaExpressionNameValue);
      WorkingCriteriaExpressionKeyCaption = CriteriaExpressionKeyCaptionCollection != null && CriteriaExpressionKeyCaptionCollection.Count > 0 ? CriteriaExpressionKeyCaptionCollection[0] : null;
    }

    private void ClearCriteriaExpressionControl()
    {
      for (int i = this.criteriaExpressionNavBarControl.Groups.Count - 1; i >= 0; i--)
        ClearCriteriaExpressionControl(this.criteriaExpressionNavBarControl.Groups[i]);
    }

    private void ClearCriteriaExpressionControl(NavBarGroup groupControl)
    {
      NavBarGroupControlContainer groupControlContainer = groupControl.ControlContainer;
      for (int i = groupControlContainer.Controls.Count - 1; i >= 0; i--)
      {
        Control control = groupControlContainer.Controls[i];
        groupControlContainer.Controls.Remove(control);
        control.Dispose();
      }
      groupControl.ControlContainer = null;
      this.criteriaExpressionNavBarControl.Controls.Remove(groupControlContainer);
      groupControlContainer.Dispose();
      this.criteriaExpressionNavBarControl.Groups.Remove(groupControl);
      groupControl.Dispose();
    }

    private void ResetWorkCriteriaExpressionControl(CriteriaExpression criteriaExpression)
    {
      if ((object)criteriaExpression != null && criteriaExpression.Left.CriteriaExpressionType == CriteriaExpressionType.CriteriaOperate)
        AddCriteriaExpressionGroupControl(CriteriaLogical.And, criteriaExpression);
      else
      {
        CriteriaExpression p = criteriaExpression;
        while ((object)p != null && p.CriteriaExpressionType != CriteriaExpressionType.Short)
        {
          AddCriteriaExpressionGroupControl(p.Logical, p.Right);
          p = p.Left;
        }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    private void AddCriteriaExpressionGroupControl(CriteriaLogical logical, CriteriaExpression criteriaExpression)
    {
      NavBarGroup groupControl = new NavBarGroup();
      groupControl.GroupStyle = NavBarGroupStyle.ControlContainer;
      NavBarGroupControlContainer groupControlContainer = new NavBarGroupControlContainer();
      groupControl.ControlContainer = groupControlContainer;
      this.criteriaExpressionNavBarControl.Groups.Add(groupControl);
      this.criteriaExpressionNavBarControl.Controls.Add(groupControlContainer);
      groupControl.Visible = false;
      AddCriteriaExpressionControl(groupControl, criteriaExpression, CriteriaExpressionKeyCaptionCollection.FetchCriteriaExpressionProperties(criteriaExpression).GetSelectedItems());
      if (groupControl.ControlContainer.Controls.Count > 0)
      {
        groupControl.ControlContainer.Tag = criteriaExpression;
        groupControl.Caption = EnumKeyCaption.GetCaption(logical);
        groupControl.Visible = true;
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    private void AddCriteriaExpressionControl(NavBarGroup groupControl, CriteriaExpression criteriaExpression, CriteriaExpressionPropertyKeyCaptionCollection selectedProperties)
    {
      CriteriaExpression p = criteriaExpression;
      while ((object)p != null && p.CriteriaExpressionType != CriteriaExpressionType.Short)
      {
        CriteriaCombineItemControl criteriaCombineItemControl = new CriteriaCombineItemControl(p.Left, selectedProperties, p.Left.LeftNode.FieldMapInfo);
        criteriaCombineItemControl.Enter += new EventHandler(CriteriaCombineItemControl_Enter);
        criteriaCombineItemControl.Focus();
        groupControl.ControlContainer.Controls.Add(criteriaCombineItemControl);
        groupControl.GroupClientHeight = groupControl.ControlContainer.Controls.Count * 25;
        groupControl.Expanded = true;
        criteriaCombineItemControl.Dock = DockStyle.Top;
        p = p.Right;
      }
    }

    #endregion

    private void Host_Shown(object sender, EventArgs e)
    {
      InitializeCriteriaExpressionKeyCaptionCollection();
      ApplyRules();
    }
    
    private void criteriaExpressionLookUpBarEditItem_EditValueChanged(object sender, EventArgs e)
    {
      ClearCriteriaExpressionControl();
      ResetWorkCriteriaExpressionControl(WorkingCriteriaExpression);
      ApplyRules();
    }

    private void criteriaExpressionNewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      CriteriaExpressionKeyCaption criteriaExpressionKeyCaption = CriteriaExpressionNewDialog.Execute(CriteriaExpressionKeyCaptionCollection, DefaultReadLevel);
      if (criteriaExpressionKeyCaption != null)
        WorkingCriteriaExpressionKeyCaption = criteriaExpressionKeyCaption;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void criteriaExpressionDeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      CriteriaExpressionKeyCaption criteriaExpressionKeyCaption = WorkingCriteriaExpressionKeyCaption;
      if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmDelete, CriteriaExpressionKeyCaptionCollection.Caption, criteriaExpressionKeyCaption.Caption), Phenix.Windows.Properties.Resources.DataDelete,
        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        try
        {
          CriteriaExpressionKeyCaptionCollection.Remove(criteriaExpressionKeyCaption);
          WorkingCriteriaExpressionKeyCaption = null;
        }
        catch (Exception ex)
        {
          string hint = String.Format(Phenix.Windows.Properties.Resources.DataDeleteAborted, CriteriaExpressionKeyCaptionCollection.Caption, criteriaExpressionKeyCaption.Caption, AppUtilities.GetErrorHint(ex));
          MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataDelete, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void criteriaExpressionUpdateBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      try
      {
        CriteriaExpressionKeyCaptionCollection.Save(WorkingCriteriaExpressionKeyCaption);
        string hint = String.Format(Phenix.Windows.Properties.Resources.DataSaveSucceed, WorkingCriteriaExpressionKeyCaption.Caption);
        MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataSave, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        string hint = String.Format(Properties.Resources.DataSaveAborted, CriteriaExpressionKeyCaptionCollection.Caption, WorkingCriteriaExpressionKeyCaption.Caption, AppUtilities.GetErrorHint(ex));
        MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataSave, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void addGroupBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      CriteriaExpressionPropertyKeyCaptionCollection criteriaExpressionProperties = CriteriaExpressionPropertySelectDialog.Execute(CriteriaExpressionKeyCaptionCollection);
      if (criteriaExpressionProperties != null)
      {
        CriteriaExpressionKeyCaption criteriaExpressionKeyCaption = WorkingCriteriaExpressionKeyCaption;
        AddCriteriaExpressionGroupControl(CriteriaLogical.Or, criteriaExpressionKeyCaption.AddCriteriaExpressionGroup(criteriaExpressionProperties));
        WorkingCriteriaExpressionKeyCaption = criteriaExpressionKeyCaption;
      }
    }
    
    private void addBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      CriteriaExpressionPropertyKeyCaptionCollection criteriaExpressionProperties = CriteriaExpressionPropertySelectDialog.Execute(CriteriaExpressionKeyCaptionCollection);
      if (criteriaExpressionProperties != null)
      {
        NavBarGroupControlContainer groupControlContainer = (NavBarGroupControlContainer)ActiveCriteriaCombineItemControl.Parent;
        CriteriaExpression criteriaExpressionGroup = (CriteriaExpression)groupControlContainer.Tag;
        AddCriteriaExpressionControl(groupControlContainer.OwnerGroup, criteriaExpressionProperties.AddSelectedCriteriaExpression(criteriaExpressionGroup), criteriaExpressionProperties.GetSelectedItems());
      }
    }

    private void deleteGroupBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      NavBarGroupControlContainer groupControlContainer = (NavBarGroupControlContainer)ActiveCriteriaCombineItemControl.Parent;
      CriteriaExpression criteriaExpressionGroup = (CriteriaExpression)groupControlContainer.Tag;
      if (WorkingCriteriaExpressionKeyCaption.RemoveCriteriaExpressionGroup(criteriaExpressionGroup))
        ClearCriteriaExpressionControl(groupControlContainer.OwnerGroup);
    }
  }
}