namespace Phenix.Windows
{
  partial class ShowExecuteActionForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.treeView = new System.Windows.Forms.TreeView();
      this.SuspendLayout();
      // 
      // treeView
      // 
      this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView.Location = new System.Drawing.Point(0, 0);
      this.treeView.Name = "treeView";
      this.treeView.Size = new System.Drawing.Size(500, 200);
      this.treeView.TabIndex = 0;
      // 
      // ShowExecuteActionForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(500, 200);
      this.ControlBox = false;
      this.Controls.Add(this.treeView);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ShowExecuteActionForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.TopMost = true;
      this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TreeView treeView;
  }
}