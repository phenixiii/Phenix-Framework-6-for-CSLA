using System;
using System.Security.Authentication;
using System.Windows.Forms;
using Phenix.Business;
using Phenix.Core;
using Phenix.Core.Mapping;

namespace Phenix.Windows.Helper
{
  /// <summary>
  /// 持久化助手
  /// </summary>
  public static class PersistentHelper
  {
    #region Action

    /// <summary>
    /// 提交数据
    /// throwIfException = false
    /// </summary>
    /// <param name="doExecute">执行数据库操作处理函数</param>
    /// <param name="hint">提示信息</param>
    public static bool Execute(Action doExecute, string hint)
    {
      return Execute(doExecute, hint, false);
    }

    /// <summary>
    /// 提交数据
    /// </summary>
    /// <param name="doExecute">执行数据库操作处理函数</param>
    /// <param name="hint">提示信息</param>
    /// <param name="throwIfException">如果为 true, 则会在截获异常时抛出; 如果为 false, 则在处理完异常后返回 false</param>
    public static bool Execute(Action doExecute, string hint, bool throwIfException)
    {
      if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmExecute, hint), Phenix.Windows.Properties.Resources.ExecuteMethod,
        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return false;
      try
      {
        using (new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.Executing, hint), Phenix.Core.Properties.Resources.PleaseWait))
        {
          doExecute();
        }
        MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecuteSucceed, hint), Phenix.Windows.Properties.Resources.ExecuteMethod, 
          MessageBoxButtons.OK, MessageBoxIcon.Information);
        return true;
      }
      catch (AuthenticationException ex)
      {
        MessageBox.Show(AppUtilities.GetErrorHint(ex), Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
        return false;
      }
      catch (Exception ex)
      {
        if (throwIfException)
          throw;
        MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecutAborted,
          hint, AppUtilities.GetErrorHint(ex, typeof(Csla.DataPortalException), typeof(Csla.Reflection.CallMethodException))),
          Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
    }

    #endregion

    #region Func<bool result>

    /// <summary>
    /// 提交数据
    /// throwIfException = false
    /// </summary>
    /// <param name="doExecute">执行数据库操作处理函数</param>
    /// <param name="hint">提示信息</param>
    public static bool Execute(Func<bool> doExecute, string hint)
    {
      return Execute(doExecute, hint, false);
    }

    /// <summary>
    /// 提交数据
    /// </summary>
    /// <param name="doExecute">执行数据库操作处理函数</param>
    /// <param name="hint">提示信息</param>
    /// <param name="throwIfException">如果为 true, 则会在截获异常时抛出; 如果为 false, 则在处理完异常后返回 false</param>
    public static bool Execute(Func<bool> doExecute, string hint, bool throwIfException)
    {
      if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmExecute, hint), Phenix.Windows.Properties.Resources.ExecuteMethod,
        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return false;
      try
      {
        using (new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.Executing, hint), Phenix.Core.Properties.Resources.PleaseWait))
        {
          if (doExecute())
          {
            MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecuteSucceed, hint), Phenix.Windows.Properties.Resources.ExecuteMethod,
              MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
          }
          return false;
        }
      }
      catch (AuthenticationException ex)
      {
        MessageBox.Show(AppUtilities.GetErrorHint(ex), Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
        return false;
      }
      catch (Exception ex)
      {
        if (throwIfException)
          throw;
        MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecutAborted,
          hint, AppUtilities.GetErrorHint(ex, typeof(Csla.DataPortalException), typeof(Csla.Reflection.CallMethodException))),
          Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
    }

    #endregion

    #region Action<bool needCheckDirty>

    /// <summary>
    /// 提交数据
    /// throwIfException = false
    /// </summary>
    /// <param name="doExecute">执行数据库操作处理函数</param>
    /// <param name="hint">提示信息</param>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    public static bool Execute(Action<bool> doExecute, string hint, bool needCheckDirty)
    {
      return Execute(doExecute, hint, needCheckDirty, false);
    }

    /// <summary>
    /// 提交数据
    /// </summary>
    /// <param name="doExecute">执行数据库操作处理函数</param>
    /// <param name="hint">提示信息</param>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    /// <param name="throwIfException">如果为 true, 则会在截获异常时抛出; 如果为 false, 则在处理完异常后返回 false</param>
    public static bool Execute(Action<bool> doExecute, string hint, bool needCheckDirty, bool throwIfException)
    {
      if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmExecute, hint), Phenix.Windows.Properties.Resources.ExecuteMethod,
        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return false;
      do
      {
        try
        {
          using (new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.Executing, hint), Phenix.Core.Properties.Resources.PleaseWait))
          {
            doExecute(needCheckDirty);
          }
          MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecuteSucceed, hint), Phenix.Windows.Properties.Resources.ExecuteMethod,
            MessageBoxButtons.OK, MessageBoxIcon.Information);
          return true;
        }
        catch (CheckDirtyException ex)
        {
          if (throwIfException)
            throw;
          if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.DataSaveForcibly, hint, null, AppUtilities.GetErrorHint(ex)),
            Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return false;
          needCheckDirty = false;
        }
        catch (AuthenticationException ex)
        {
          MessageBox.Show(AppUtilities.GetErrorHint(ex), Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
          Application.Exit();
          return false;
        }
        catch (Exception ex)
        {
          if (throwIfException)
            throw;
          MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecutAborted,
            hint, AppUtilities.GetErrorHint(ex, typeof(Csla.DataPortalException), typeof(Csla.Reflection.CallMethodException))),
            Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
      } while (true);
    }

    #endregion

    #region Func<bool result, bool needCheckDirty>

    /// <summary>
    /// 提交数据
    /// throwIfException = false
    /// </summary>
    /// <param name="doExecute">执行数据库操作处理函数</param>
    /// <param name="hint">提示信息</param>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    public static bool Execute(Func<bool, bool> doExecute, string hint, bool needCheckDirty)
    {
      return Execute(doExecute, hint, needCheckDirty, false);
    }

    /// <summary>
    /// 提交数据
    /// </summary>
    /// <param name="doExecute">执行数据库操作处理函数</param>
    /// <param name="hint">提示信息</param>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    /// <param name="throwIfException">如果为 true, 则会在截获异常时抛出; 如果为 false, 则在处理完异常后返回 false</param>
    public static bool Execute(Func<bool, bool> doExecute, string hint, bool needCheckDirty, bool throwIfException)
    {
      if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmExecute, hint), Phenix.Windows.Properties.Resources.ExecuteMethod,
        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return false;
      do
      {
        try
        {
          using (new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.Executing, hint), Phenix.Core.Properties.Resources.PleaseWait))
          {
            if (doExecute(needCheckDirty))
            {
              MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecuteSucceed, hint), Phenix.Windows.Properties.Resources.ExecuteMethod,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
              return true;
            }
            return false;
          }
        }
        catch (CheckDirtyException ex)
        {
          if (throwIfException)
            throw;
          if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.DataSaveForcibly, hint, null, AppUtilities.GetErrorHint(ex)),
            Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return false;
          needCheckDirty = false;
        }
        catch (AuthenticationException ex)
        {
          MessageBox.Show(AppUtilities.GetErrorHint(ex), Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
          Application.Exit();
          return false;
        }
        catch (Exception ex)
        {
          if (throwIfException)
            throw;
          MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecutAborted,
            hint, AppUtilities.GetErrorHint(ex, typeof(Csla.DataPortalException), typeof(Csla.Reflection.CallMethodException))),
            Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
      } while (true);
    }

    #endregion

    #region Func<T : CommandBase<T>>

    /// <summary>
    /// 提交数据
    /// throwIfException = false
    /// </summary>
    /// <param name="doExecute">执行数据库操作处理函数</param>
    /// <param name="hint">提示信息</param>
    public static T Execute<T>(Func<T> doExecute, string hint)
       where T : CommandBase<T>
    {
      return Execute(doExecute, hint, false);
    }

    /// <summary>
    /// 提交数据
    /// </summary>
    /// <param name="doExecute">执行数据库操作处理函数</param>
    /// <param name="hint">提示信息</param>
    /// <param name="throwIfException">如果为 true, 则会在截获异常时抛出; 如果为 false, 则在处理完异常后返回 false</param>
    public static T Execute<T>(Func<T> doExecute, string hint, bool throwIfException)
      where T : CommandBase<T>
    {
      if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmExecute, hint), Phenix.Windows.Properties.Resources.ExecuteMethod,
        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return null;
      try
      {
        using (new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.Executing, hint), Phenix.Core.Properties.Resources.PleaseWait))
        {
          T result = doExecute();
          if (result.ExecuteResult.Applied && result.ExecuteResult.Succeed)
            MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecuteSucceed, hint), Phenix.Windows.Properties.Resources.ExecuteMethod,
              MessageBoxButtons.OK, MessageBoxIcon.Information);
          return result;
        }
      }
      catch (AuthenticationException ex)
      {
        MessageBox.Show(AppUtilities.GetErrorHint(ex), Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
        return null;
      }
      catch (Exception ex)
      {
        if (throwIfException)
          throw;
        MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecutAborted,
          hint, AppUtilities.GetErrorHint(ex, typeof(Csla.DataPortalException), typeof(Csla.Reflection.CallMethodException))),
          Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null;
      }
    }

    #endregion

    #region T : CommandBase<T>

    /// <summary>
    /// 提交数据
    /// throwIfException = false
    /// </summary>
    /// <param name="command">指令</param>
    /// <param name="hint">提示信息</param>
    public static T Execute<T>(T command, string hint)
       where T : CommandBase<T>
    {
      return Execute(command, hint, false);
    }

    /// <summary>
    /// 提交数据
    /// </summary>
    /// <param name="command">指令</param>
    /// <param name="hint">提示信息</param>
    /// <param name="throwIfException">如果为 true, 则会在截获异常时抛出; 如果为 false, 则在处理完异常后返回 false</param>
    public static T Execute<T>(T command, string hint, bool throwIfException)
      where T : CommandBase<T>
    {
      if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmExecute, hint), Phenix.Windows.Properties.Resources.ExecuteMethod,
        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return null;
      try
      {
        using (new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.Executing, hint), Phenix.Core.Properties.Resources.PleaseWait))
        {
          command.Execute();
          if (command.ExecuteResult.Applied && command.ExecuteResult.Succeed)
            MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecuteSucceed, hint), Phenix.Windows.Properties.Resources.ExecuteMethod,
              MessageBoxButtons.OK, MessageBoxIcon.Information);
          return command;
        }
      }
      catch (AuthenticationException ex)
      {
        MessageBox.Show(AppUtilities.GetErrorHint(ex), Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
        return null;
      }
      catch (Exception ex)
      {
        if (throwIfException)
          throw;
        MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ExecutAborted,
          hint, AppUtilities.GetErrorHint(ex, typeof(Csla.DataPortalException), typeof(Csla.Reflection.CallMethodException))),
          Phenix.Windows.Properties.Resources.ExecuteMethod, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null;
      }
    }

    #endregion
  }
}
