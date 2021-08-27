namespace Phenix.Windows
{
  partial class ProcessControl
  {
    /// <summary> 
    /// 必需的设计器变量。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// 清理所有正在使用的资源。
    /// </summary>
    /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region 组件设计器生成的代码

    /// <summary> 
    /// 设计器支持所需的方法 - 不要
    /// 使用代码编辑器修改此方法的内容。
    /// </summary>
    private void InitializeComponent()
    {
      this.groupControl = new DevExpress.XtraEditors.GroupControl();
      this.progressBarControl = new DevExpress.XtraEditors.ProgressBarControl();
      this.messageLabelControl = new DevExpress.XtraEditors.LabelControl();
      ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
      this.groupControl.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.progressBarControl.Properties)).BeginInit();
      this.SuspendLayout();
      // 
      // groupControl
      // 
      this.groupControl.Controls.Add(this.progressBarControl);
      this.groupControl.Controls.Add(this.messageLabelControl);
      this.groupControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupControl.Location = new System.Drawing.Point(0, 0);
      this.groupControl.Name = "groupControl";
      this.groupControl.Size = new System.Drawing.Size(354, 80);
      this.groupControl.TabIndex = 0;
      this.groupControl.Text = "title";
      // 
      // progressBarControl
      // 
      this.progressBarControl.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.progressBarControl.Location = new System.Drawing.Point(2, 47);
      this.progressBarControl.Name = "progressBarControl";
      this.progressBarControl.Properties.DisplayFormat.FormatString = "当前进度：{0:N0}%";
      this.progressBarControl.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
      this.progressBarControl.Properties.ShowTitle = true;
      this.progressBarControl.Properties.Step = 1;
      this.progressBarControl.Size = new System.Drawing.Size(350, 31);
      this.progressBarControl.TabIndex = 1;
      // 
      // messageLabelControl
      // 
      this.messageLabelControl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
      this.messageLabelControl.Dock = System.Windows.Forms.DockStyle.Top;
      this.messageLabelControl.Location = new System.Drawing.Point(2, 23);
      this.messageLabelControl.Name = "messageLabelControl";
      this.messageLabelControl.Size = new System.Drawing.Size(350, 14);
      this.messageLabelControl.TabIndex = 0;
      // 
      // ProcessControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupControl);
      this.Name = "ProcessControl";
      this.Size = new System.Drawing.Size(354, 80);
      ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
      this.groupControl.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.progressBarControl.Properties)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraEditors.GroupControl groupControl;
    private DevExpress.XtraEditors.ProgressBarControl progressBarControl;
    private DevExpress.XtraEditors.LabelControl messageLabelControl;
  }
}