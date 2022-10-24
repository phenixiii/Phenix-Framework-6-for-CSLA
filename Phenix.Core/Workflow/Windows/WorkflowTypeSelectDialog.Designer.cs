namespace Phenix.Core.Workflow.Windows
{
  partial class WorkflowTypeSelectDialog
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
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.workflowIdentityInfoListBox = new System.Windows.Forms.ListBox();
      this.SuspendLayout();
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(392, 49);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 2;
      this.cancelButton.Text = "取消";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(392, 20);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 1;
      this.okButton.Text = "确认";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // workflowIdentityInfoListBox
      // 
      this.workflowIdentityInfoListBox.FormattingEnabled = true;
      this.workflowIdentityInfoListBox.ItemHeight = 12;
      this.workflowIdentityInfoListBox.Location = new System.Drawing.Point(25, 20);
      this.workflowIdentityInfoListBox.Name = "workflowIdentityInfoListBox";
      this.workflowIdentityInfoListBox.Size = new System.Drawing.Size(345, 220);
      this.workflowIdentityInfoListBox.Sorted = true;
      this.workflowIdentityInfoListBox.TabIndex = 0;
      this.workflowIdentityInfoListBox.DoubleClick += new System.EventHandler(this.workflowIdentityInfoListBox_DoubleClick);
      // 
      // WorkflowTypeSelectDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(501, 273);
      this.Controls.Add(this.workflowIdentityInfoListBox);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Name = "WorkflowTypeSelectDialog";
      this.Text = "选择类名";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.ListBox workflowIdentityInfoListBox;
  }
}