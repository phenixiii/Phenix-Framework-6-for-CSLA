namespace Phenix.Windows.Helper
{
  partial class PreviewForm
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
      this.components = new System.ComponentModel.Container();
      this.templateFilesBindingSource = new System.Windows.Forms.BindingSource(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.fController)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.templateFilesBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // fController
      // 
      this.fController.PropertiesBar.DefaultGlyphSize = new System.Drawing.Size(16, 16);
      this.fController.PropertiesBar.DefaultLargeGlyphSize = new System.Drawing.Size(32, 32);
      // 
      // templateFilesBindingSource
      // 
      this.templateFilesBindingSource.DataSource = typeof(Phenix.Windows.Helper.TemplateFile);
      // 
      // PreviewForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(904, 520);
      this.Location = new System.Drawing.Point(0, 0);
      this.Name = "PreviewForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "打印预览";
      ((System.ComponentModel.ISupportInitialize)(this.fController)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.templateFilesBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource templateFilesBindingSource;
  }
}