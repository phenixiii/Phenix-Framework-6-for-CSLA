using System.Windows.Forms;

namespace Phenix.Core.Windows
{
  /// <summary>
  /// 窗体接口
  /// </summary>
  public interface IForm
  {
    #region 属性

    /// <summary>
    /// 指示在将键事件传递到具有焦点的控件前窗体是否将接收此键事件
    /// </summary>
    bool KeyPreview { get; set; }

    /// <summary>
    /// 当输入回车键时选择下一个可用控件并使其成为活动控件
    /// </summary>
    bool EnterMoveNextControl { get; set; }

    /// <summary>
    /// 操作数据源
    /// </summary>
    object WorkSource { get; set; }

    /// <summary>
    /// 指定窗口如何显示
    /// </summary>
    FormWindowState WindowState { get; set; }

    /// <summary>
    /// 指定窗体的初始位置
    /// </summary>
    FormStartPosition StartPosition { get; set; }

    #endregion

    #region 方法

    /// <summary>
    /// 分析消息
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    object AnalyseMessage(object message);

    /// <summary>
    /// 激活MDI窗体
    /// </summary>
    /// <param name="mdiParent">主窗体</param>
    void Activate(Form mdiParent);

    /// <summary>
    /// 显示对话框
    /// </summary>
    /// <returns></returns>
    DialogResult ShowDialog();

    #endregion
  }
}
