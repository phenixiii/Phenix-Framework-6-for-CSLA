namespace Phenix.Core.Windows
{
  /// <summary>
  /// 消息框
  /// </summary>
  public sealed partial class ShowMessageDialog : DialogForm
  {
    /// <summary>
    /// 消息框
    /// </summary>
    public ShowMessageDialog(string caption)
    {
      InitializeComponent();

      this.Text = caption;
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static void Execute(string caption, string message)
    {
      using (ShowMessageDialog dialog = new ShowMessageDialog(caption))
      {
        dialog.AddMessage(message);
        dialog.ShowDialog();
      }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 添加信息
    /// </summary>
    public void AddMessage(string message)
    {
      messageRichTextBox.AppendText(message + "\r\n");
    }

    #endregion
  }
}
