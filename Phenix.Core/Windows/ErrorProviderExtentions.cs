using System;
using System.ComponentModel;
using System.Windows.Forms;
using Phenix.Core.Data;

namespace Phenix.Core.Windows
{
  /// <summary>
  /// ErrorProvider扩展
  /// </summary>
  public static class ErrorProviderExtentions
  {
    #region 方法

    /// <summary>
    /// 校验控件容器内编辑控件的失效数据
    /// </summary>
    /// <param name="errorProvider">ErrorProvider</param>
    /// <param name="container">控件容器</param>
    /// <param name="source">数据源</param>
    /// <param name="toFocused">聚焦失效控件</param>
    /// <returns>失效数据事件数据</returns>
    public static DataInvalidEventArgs CheckRules(this ErrorProvider errorProvider, Control container, BindingSource source, bool toFocused)
    {
      if (container == null)
        return null;
      foreach (Control item in ControlHelper.FindEditControls(container, source))
      {
        DataInvalidEventArgs result = CheckRule(errorProvider, item, toFocused);
        if (result != null)
          return result;
      }
      return null;
    }

    /// <summary>
    /// 校验编辑控件的失效数据
    /// </summary>
    /// <param name="errorProvider">ErrorProvider</param>
    /// <param name="control">控件</param>
    /// <param name="toFocused">聚焦失效控件</param>
    /// <returns>失效数据事件数据</returns>
    public static DataInvalidEventArgs CheckRule(this ErrorProvider errorProvider, Control control, bool toFocused)
    {
      if (control == null)
        return null;
      string oldMessage = errorProvider.GetError(control);
      foreach (Binding item in control.DataBindings)
      {
        if (String.IsNullOrEmpty(item.BindingMemberInfo.BindingField))
          continue;
        BindingSource bindingSource = item.DataSource as BindingSource;
        if (bindingSource == null)
          continue;
        object obj = BindingSourceHelper.GetDataSourceCurrent(bindingSource);
        IDataInvalidInfo dataInvalidInfo = obj as IDataInvalidInfo;
        if (dataInvalidInfo != null)
          if (dataInvalidInfo.ErrorCount > 0)
          {
            string errorMessage = dataInvalidInfo.GetFirstErrorMessage(item.BindingMemberInfo.BindingField);
            if (String.IsNullOrEmpty(errorMessage))
              goto Label;
            if (toFocused && String.CompareOrdinal(oldMessage, errorMessage) != 0)
              ControlHelper.InvokeSetFocus(control);
            InvokeSetError(errorProvider, control, errorMessage);
            return new DataInvalidEventArgs(bindingSource, bindingSource.Position, control, errorMessage);
          }
          else if (dataInvalidInfo.WarningCount > 0)
          {
            string warningMessage = dataInvalidInfo.GetFirstWarningMessage(item.BindingMemberInfo.BindingField);
            if (String.IsNullOrEmpty(warningMessage))
              goto Label;
            if (toFocused && String.CompareOrdinal(oldMessage, warningMessage) != 0)
              ControlHelper.InvokeSetFocus(control);
            InvokeSetError(errorProvider, control, warningMessage);
            return new DataInvalidEventArgs(bindingSource, bindingSource.Position, control, warningMessage);
          }
          else if (dataInvalidInfo.InformationCount > 0)
          {
            string informationMessage = dataInvalidInfo.GetFirstInformationMessage(item.BindingMemberInfo.BindingField);
            if (String.IsNullOrEmpty(informationMessage))
              goto Label;
            InvokeSetError(errorProvider, control, informationMessage);
            return new DataInvalidEventArgs(bindingSource, bindingSource.Position, control, informationMessage);
          }
        IDataErrorInfo dataErrorInfo = obj as IDataErrorInfo;
        if (dataErrorInfo != null)
        {
          string errorMessage = dataErrorInfo[item.BindingMemberInfo.BindingField];
          if (String.IsNullOrEmpty(errorMessage))
            goto Label;
          if (toFocused && String.CompareOrdinal(oldMessage, errorMessage) != 0)
            ControlHelper.InvokeSetFocus(control);
          InvokeSetError(errorProvider, control, errorMessage);
          return new DataInvalidEventArgs(bindingSource, bindingSource.Position, control, errorMessage);
        }
      }
    Label:
      if (!String.IsNullOrEmpty(oldMessage))
        InvokeSetError(errorProvider, control, String.Empty);
      return null;
    }

    /// <summary>
    /// 显示失效数据信息（线程安全）
    /// </summary>
    /// <param name="errorProvider">DXErrorProvider</param>
    /// <param name="control">控件</param>
    /// <param name="errorMessage">信息</param>
    public static void InvokeSetError(this ErrorProvider errorProvider, Control control, string errorMessage)
    {
      if (control == null)
        return;
      if (control.InvokeRequired)
        control.BeginInvoke(new Action<Control, string>(errorProvider.SetError), new object[] { control, errorMessage });
      else
        errorProvider.SetError(control, errorMessage); ;
    }

    #endregion
  }
}
