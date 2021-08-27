using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using Phenix.Business;
using Phenix.Core;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;
using Phenix.Core.Windows;

namespace Phenix.Windows
{
  /// <summary>
  /// TreeList管理组件
  /// 与IBusinessTreeNode互动
  /// </summary>
  [Description("TreeList管理组件")]
  [Designer(typeof(TreeListManagerDesigner))]
  [ProvideProperty("Apply", typeof(TreeList))] //被应用到TreeList管理
  [ToolboxItem(true), ToolboxBitmap(typeof(TreeListManager), "Phenix.Windows.TreeListManager")]
  public sealed class TreeListManager : Component, IExtenderProvider, ISupportInitialize
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public TreeListManager()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="container">组件容器</param>
    public TreeListManager(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container");
      container.Add(this);
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

    private readonly Dictionary<TreeList, TreeListDragDropRuleStatus> _ruleStatuses = new Dictionary<TreeList, TreeListDragDropRuleStatus>();

    #endregion

    #region 扩展程序属性

    /// <summary>
    /// 被应用到TreeList管理
    /// </summary>
    [Description("被应用到TreeList管理"), Category("Phenix")]
    public bool GetApply(TreeList source)
    {
      TreeListDragDropRuleStatus result;
      if (_ruleStatuses.TryGetValue(source, out result))
        return result.Apply;
      return true;
    }

    /// <summary>
    /// 被应用到TreeList管理
    /// </summary>
    public void SetApply(TreeList source, bool value)
    {
      TreeListDragDropRuleStatus status;
      if (_ruleStatuses.TryGetValue(source, out status))
        status.Apply = value;
      else
        _ruleStatuses.Add(source, new TreeListDragDropRuleStatus { Apply = value });
    }
    
    #endregion

    #region 事件

    private void Host_Shown(object sender, EventArgs e)
    {
      InitializeRule();
    }

    #region TreeList 事件

    private void TreeList_BeforeDragNode(object sender, BeforeDragNodeEventArgs e)
    {
      TreeList treeList = (TreeList)sender;
      TreeListDragDropRuleStatus status;
      if (_ruleStatuses.TryGetValue(treeList, out status))
      {
        status.SourceNode = e.Node;
        status.TargetNode = null;
        IBusinessTreeNode sourceBusiness = treeList.GetDataRecordByNode(status.SourceNode) as IBusinessTreeNode;
        status.CanDrag = sourceBusiness != null && sourceBusiness.AllowMove;
        e.CanDrag = status.CanDrag;
      }
    }

    private void TreeList_CalcNodeDragImageIndex(object sender, CalcNodeDragImageIndexEventArgs e)
    {
      TreeList treeList = (TreeList)sender;
      TreeListDragDropRuleStatus status;
      if (_ruleStatuses.TryGetValue(treeList, out status))
      {
        status.CanDrag = false;
        status.SourceNode = e.DragArgs.Data.GetData(typeof(TreeListNode)) as TreeListNode;
        status.TargetNode = e.Node;
        if (status.SourceNode == null || status.TargetNode == null)
          return;
        IBusinessTreeNode sourceBusiness = treeList.GetDataRecordByNode(status.SourceNode) as IBusinessTreeNode;
        IBusinessTreeNode targetBussiness = treeList.GetDataRecordByNode(status.TargetNode) as IBusinessTreeNode;
        if (sourceBusiness == null || targetBussiness == null)
          return;

        switch (e.ImageIndex)
        {
          case 0: //下级
            status.CanDrag = sourceBusiness.AllowMove && targetBussiness.AllowAddChild(sourceBusiness);
            break;
          case 1: //前面
          case 2: //后面
            status.CanDrag = sourceBusiness.AllowMove && targetBussiness.AllowTogether(sourceBusiness);
            break;
        }
      }
    }

    private void TreeList_DragOver(object sender, DragEventArgs e)
    {
      if (e.Effect == DragDropEffects.Move)
      {
        TreeListDragDropRuleStatus status;
        if (_ruleStatuses.TryGetValue((TreeList)sender, out status))
          if (!status.CanDrag)
            e.Effect = DragDropEffects.None;
      }
    }

    private void TreeList_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Effect == DragDropEffects.Move)
      {
        TreeListDragDropRuleStatus status;
        if (_ruleStatuses.TryGetValue((TreeList)sender, out status))
          if (!status.CanDrag)
            e.Effect = DragDropEffects.None;
      }
    }

    #endregion

    #endregion

    #region 方法

    #region IExtenderProvider 成员

    /// <summary>
    /// 是否可以将扩展程序属性提供给指定的对象
    /// </summary>
    /// <param name="extendee">要接收扩展程序属性的对象</param>
    public bool CanExtend(object extendee)
    {
      return extendee is TreeList;
    }

    #endregion

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

    private void InitializeRule()
    {
      foreach (KeyValuePair<TreeList, TreeListDragDropRuleStatus> kvp in _ruleStatuses)
        if (kvp.Value.Apply)
        {
          kvp.Key.OptionsBehavior.Editable = false;
          kvp.Key.DragNodesMode = TreeListDragNodesMode.Advanced;
          kvp.Key.BeforeDragNode += new BeforeDragNodeEventHandler(TreeList_BeforeDragNode);
          kvp.Key.CalcNodeDragImageIndex += new CalcNodeDragImageIndexEventHandler(TreeList_CalcNodeDragImageIndex);
          kvp.Key.DragOver += new DragEventHandler(TreeList_DragOver);
          kvp.Key.DragDrop += new DragEventHandler(TreeList_DragDrop);
        }
    }

    /// <summary>
    /// 重置执行授权
    /// </summary>
    public void ResetAuthorizationRules(bool editMode)
    {
      foreach (KeyValuePair<TreeList, TreeListDragDropRuleStatus> kvp in _ruleStatuses)
        if (kvp.Value.Apply)
          kvp.Key.OptionsBehavior.DragNodes = editMode;
    }

    #region Rules

    internal static string ResetRules(Control container)
    {
      if (container == null)
        throw new ArgumentNullException("container");
      string result = ResetRules(container as TreeList);
      foreach (Control item in container.Controls)
        result += ResetRules(item);
      if (!String.IsNullOrEmpty(result))
        result = container.Name + Environment.NewLine + result + Environment.NewLine;
      return result;
    }

    //for Developer Express .NET
    private static string ResetRules(TreeList treeList)
    {
      string result = String.Empty;
      if (treeList == null)
        return result;
      Type listItemType = Utilities.FindListItemType(BindingSourceHelper.GetDataSourceType(treeList.DataSource as BindingSource));
      if (listItemType == null)
        return result;
      foreach (KeyValuePair<FieldMapInfo, FieldMapInfo> kvp in ClassMemberHelper.GetDetailMasterFieldMapInfos(listItemType, listItemType, null, false))
      {
        treeList.KeyFieldName = kvp.Value.PropertyName;
        treeList.ParentFieldName = kvp.Key.PropertyName;
        treeList.RootValue = null;
        break;
      }
      result += String.Format("change {0}{1}", treeList.Name, Environment.NewLine);
      return result;
    }
    
    #endregion

    #endregion

    #region 内嵌类

    [Serializable]
    private class TreeListDragDropRuleStatus
    {
      private bool _apply = true;
      public bool Apply
      {
        get { return _apply; }
        set { _apply = value; }
      }

      public bool CanDrag { get; set; }
      public TreeListNode SourceNode { get; set; }
      public TreeListNode TargetNode { get; set; }
    }

    #endregion
  }
}