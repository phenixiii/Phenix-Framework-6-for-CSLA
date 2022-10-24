using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Phenix.Core.Windows;

namespace Phenix.Core.Dictionary.Windows
{
  /// <summary>
  /// 管理业务码格式对话框
  /// </summary>
  public partial class BusinessCodeFormatManageDialog : DialogForm
  {
    private BusinessCodeFormatManageDialog()
    {
      InitializeComponent();

      this.addBusinessCodeFormatToolStripButton.Text = Resources.Add;
      this.editBusinessCodeFormatToolStripButton.Text = Resources.Edit;
      this.deleteBusinessCodeFormatToolStripButton.Text = Resources.Delete;
      this.exitToolStripButton.Text = Resources.Exit;
      this.businessCodeNameTextBoxColumn.HeaderText = Resources.Name;
      this.captionTextBoxColumn.HeaderText = Resources.Caption;
      this.formatStringTextBoxColumn.HeaderText = Resources.FormatString;
      this.fillOnSavingCheckBoxColumn.HeaderText = Resources.FillOnSaving;

      this.businessCodeFormatsBindingSource.DataSource = new List<BusinessCodeFormat>(DataDictionaryHub.BusinessCodeFormats.Values);
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static void Execute()
    {
      Execute(null);
    }

    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="doMessageNotify">消息通报事件</param>
    public static void Execute(Action<MessageNotifyEventArgs> doMessageNotify)
    {
      using (BusinessCodeFormatManageDialog dialog = new BusinessCodeFormatManageDialog())
      {
        if (doMessageNotify != null)
          dialog._messageNotify = doMessageNotify;
        dialog.Text = Resources.BusinessCodeFormatManage;
        dialog.ShowDialog();
      }
    }

    #endregion

    #region 属性

    private List<BusinessCodeFormat> WorkingBusinessCodeFormats
    {
      get { return BindingSourceHelper.GetDataSourceList(this.businessCodeFormatsBindingSource) as List<BusinessCodeFormat>; }
    }

    private BusinessCodeFormat WorkingBusinessCodeFormat
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.businessCodeFormatsBindingSource) as BusinessCodeFormat; }
    }

    #endregion

    #region 事件

    private event Action<MessageNotifyEventArgs> _messageNotify;
    private void OnMessageNotify(MessageNotifyEventArgs e)
    {
      Action<MessageNotifyEventArgs> handler = _messageNotify;
      if (handler != null)
        handler(e);
    }

    #endregion

    #region 方法

    private void Humanistic()
    {
      if (WorkingBusinessCodeFormat == null)
      {
        this.hintToolStripStatusLabel.Text = null;
        this.filterToolStripTextBox.Text = null;
      }
      else
      {
        this.hintToolStripStatusLabel.Text = WorkingBusinessCodeFormat.Caption;
        string name = BusinessCodeFormat.ExtractName(WorkingBusinessCodeFormat.BusinessCodeName);
        if (String.CompareOrdinal(name, this.filterToolStripTextBox.Text) != 0)
          this.filterToolStripTextBox.Text = name;
      }
    }

    private void ApplyRules()
    {
      this.addBusinessCodeFormatToolStripButton.Enabled = WorkingBusinessCodeFormat != null && !WorkingBusinessCodeFormat.IsDefault;
      this.editBusinessCodeFormatToolStripButton.Enabled = WorkingBusinessCodeFormat != null;
      this.deleteBusinessCodeFormatToolStripButton.Enabled = WorkingBusinessCodeFormat != null;
    }

    private void ResetBusinessCodeFormatsBindingSource()
    {
      this.businessCodeFormatsBindingSource.ResetBindings(false);
    }

    private void FilterBusinessCodeFormats(string like)
    {
      if (WorkingBusinessCodeFormat != null && WorkingBusinessCodeFormat.BusinessCodeName.IndexOf(like, StringComparison.Ordinal) == 0)
        return;
      List<BusinessCodeFormat> businessCodeFormats = new List<BusinessCodeFormat>();
      foreach (KeyValuePair<string, BusinessCodeFormat> kvp in DataDictionaryHub.BusinessCodeFormats)
        if (kvp.Key.IndexOf(like, StringComparison.Ordinal) == 0)
          businessCodeFormats.Add(kvp.Value);
      this.businessCodeFormatsBindingSource.DataSource = businessCodeFormats;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void AddBusinessCodeFormat()
    {
      BusinessCodeFormat value = (BusinessCodeFormat)Phenix.Core.Reflection.Utilities.Clone(WorkingBusinessCodeFormat);
      value.CriteriaPropertyValue = Resources.InputCriteriaPropertyValue;
      value = BusinessCodeFormatEditDialog.Execute(value, new Action(AddBusinessCodeFormat));
      if (value != null)
        try
        {
          value.Save();
          WorkingBusinessCodeFormats.Add(value);
          ResetBusinessCodeFormatsBindingSource();
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, this.Text,
            String.Format(Resources.AddBusinessCodeFormatSucceed, value.Caption, value.FormatString)));
        }
        catch (Exception ex)
        {
          MessageBox.Show(AppUtilities.GetErrorHint(ex), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void EditBusinessCodeFormat()
    {
      BusinessCodeFormat value = (BusinessCodeFormat)Phenix.Core.Reflection.Utilities.Clone(WorkingBusinessCodeFormat);
      value = BusinessCodeFormatEditDialog.Execute(value, new Action(AddBusinessCodeFormat));
      if (value == null)
        return;
      foreach (BusinessCodeFormat item in WorkingBusinessCodeFormats)
        if (String.CompareOrdinal(item.BusinessCodeName, value.BusinessCodeName) == 0)
        {
          try
          {
            value.Save();
            item.FormatString = value.FormatString;
            item.FillOnSaving = value.FillOnSaving;
            ResetBusinessCodeFormatsBindingSource();
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, this.Text,
              String.Format(Resources.EditBusinessCodeFormatSucceed, item.Caption, item.FormatString)));
          }
          catch (Exception ex)
          {
            MessageBox.Show(AppUtilities.GetErrorHint(ex), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          break;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void DeleteBusinessCodeFormat()
    {
      string caption = WorkingBusinessCodeFormat.Caption;
      if (MessageBox.Show(String.Format(Resources.DeleteBusinessCodeFormatAsk, caption),
        this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        try
        {
          DataDictionaryHub.RemoveBusinessCodeFormat(WorkingBusinessCodeFormat);
          WorkingBusinessCodeFormats.Remove(WorkingBusinessCodeFormat);
          ResetBusinessCodeFormatsBindingSource();
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, this.Text,
            String.Format(Resources.DeleteBusinessCodeFormatSucceed, caption)));
        }
        catch (Exception ex)
        {
          MessageBox.Show(AppUtilities.GetErrorHint(ex), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    #endregion
    
    private void businessCodeFormatBindingSource_PositionChanged(object sender, EventArgs e)
    {
      Humanistic();
      ApplyRules();
    }

    private void filterToolStripTextBox_TextChanged(object sender, EventArgs e)
    {
      FilterBusinessCodeFormats(filterToolStripTextBox.Text);
    }

    private void addBusinessCodeFormatToolStripButton_Click(object sender, EventArgs e)
    {
      AddBusinessCodeFormat();
    }

    private void editBusinessCodeFormatToolStripButton_Click(object sender, EventArgs e)
    {
      EditBusinessCodeFormat();
    }

    private void deleteBusinessCodeFormatToolStripButton_Click(object sender, EventArgs e)
    {
      DeleteBusinessCodeFormat();
    }

    private void businessCodeFormatDataGridView_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      EditBusinessCodeFormat();
    }

    private void BusinessCodeFormatManageDialog_Shown(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void exitToolStripButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
