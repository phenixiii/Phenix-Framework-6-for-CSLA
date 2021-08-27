using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Phenix.Business;
using Phenix.Core.Log;

namespace Phenix.Windows
{
  internal partial class ShowExecuteActionForm : Form
  {
    private ShowExecuteActionForm()
    {
      InitializeComponent();
    }

    private static ShowExecuteActionForm _self;

    /// <summary>
    /// 执行
    /// </summary>
    public static void Execute(IBusinessObject business)
    {
      if (_self == null)
        _self = new ShowExecuteActionForm();
      _self.Left = MousePosition.X - _self.Width / 2;
      _self.Top = MousePosition.Y - _self.Height - 8;

      if (_self.Visible)
      {
        _self.Hide();
        return;
      }

      _self.treeView.Nodes.Clear();
      if (business != null)
      {
        IList<ExecuteActionInfo> infos = business.FetchExecuteAction();
        if (infos != null && infos.Count > 0)
        {
          DateTime time = DateTime.Now;
          TreeNode timeNode = null;
          TreeNode keyNode = null;
          foreach (ExecuteActionInfo item in infos)
          {
            if (item.FieldMapInfo.FieldAttribute.IsPrimaryKey)
              continue;
            if (timeNode == null || Math.Abs(time.Subtract(item.Time).Seconds) > 1)
            {
              time = item.Time;
              timeNode = new TreeNode();
              timeNode.Text = item.Time.ToString("u");
              _self.treeView.Nodes.Add(timeNode);
              if (keyNode != null)
              {
                keyNode.ExpandAll();
                keyNode = null;
              }
            }
            string key = String.Format("[{0}] {1} {2}", item.UserNumber, item.ActionCaption, item.EntityCaption);
            if (keyNode == null || String.CompareOrdinal(keyNode.Text, key) != 0)
            {
              keyNode = new TreeNode();
              keyNode.Text = key;
              timeNode.Nodes.Add(keyNode);
            }
            TreeNode changeInfo = new TreeNode();
            changeInfo.Text = item.ChangeInfo;
            keyNode.Nodes.Add(changeInfo);
          }
          if (keyNode != null)
            keyNode.ExpandAll();
          _self.Show();
        }
      }
    }
  }
}
