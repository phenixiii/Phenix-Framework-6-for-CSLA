using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Phenix.Windows
{
  ///<summary>
  /// 进度条
  ///</summary>
  [ToolboxItem(false)]
  public partial class ProcessControl : UserControl
  {
    ///<summary>
    /// 进度条
    ///</summary>
    public ProcessControl()
    {
      InitializeComponent();
    }

    ///<summary>
    /// 进度条
    ///</summary>
    public ProcessControl(string title, int maxValue, int minValue, int step, Form parentForm)
      : this()
    {
      this.groupControl.Text = title;
      this.progressBarControl.Properties.Maximum = maxValue;
      this.progressBarControl.Properties.Minimum = minValue;
      this.progressBarControl.Properties.Step = step;

      parentForm.Controls.Add(this);

      this.Location = new Point((parentForm.Size.Width - this.Size.Width) / 2, (parentForm.Size.Height - this.Size.Height) / 2);
      this.BringToFront();
    }

    ///<summary>
    /// 重置
    ///</summary>
    public void Reset(string title, int maxValue, int minValue, int step)
    {
      this.groupControl.Text = title;
      this.progressBarControl.Properties.Maximum = maxValue;
      this.progressBarControl.Properties.Minimum = minValue;
      this.progressBarControl.Properties.Step = step;
    }

    ///<summary>
    /// 执行
    ///</summary>
    public void PerformStep(string message)
    {
      this.messageLabelControl.Text = message;
      this.progressBarControl.PerformStep();
      Application.DoEvents();
    }
  }
}