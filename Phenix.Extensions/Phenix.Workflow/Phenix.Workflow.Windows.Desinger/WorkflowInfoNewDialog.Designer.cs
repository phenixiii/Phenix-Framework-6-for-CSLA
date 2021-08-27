namespace Phenix.Workflow.Windows.Desinger
{
  partial class WorkflowInfoNewDialog
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
      this.typeNamespaceTextBox = new System.Windows.Forms.TextBox();
      this.typeNamespaceLabel = new System.Windows.Forms.Label();
      this.captionTextBox = new System.Windows.Forms.TextBox();
      this.captionLabel = new System.Windows.Forms.Label();
      this.typeNameTextBox = new System.Windows.Forms.TextBox();
      this.typeNameLabel = new System.Windows.Forms.Label();
      this.importButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(336, 51);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 7;
      this.cancelButton.Text = "取消";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(336, 22);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 6;
      this.okButton.Text = "确认";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // typeNamespaceTextBox
      // 
      this.typeNamespaceTextBox.Enabled = false;
      this.typeNamespaceTextBox.Location = new System.Drawing.Point(89, 24);
      this.typeNamespaceTextBox.MaxLength = 30;
      this.typeNamespaceTextBox.Name = "typeNamespaceTextBox";
      this.typeNamespaceTextBox.Size = new System.Drawing.Size(208, 21);
      this.typeNamespaceTextBox.TabIndex = 1;
      this.typeNamespaceTextBox.TextChanged += new System.EventHandler(this.typeNamespaceTextBox_TextChanged);
      // 
      // typeNamespaceLabel
      // 
      this.typeNamespaceLabel.AutoSize = true;
      this.typeNamespaceLabel.Location = new System.Drawing.Point(30, 28);
      this.typeNamespaceLabel.Name = "typeNamespaceLabel";
      this.typeNamespaceLabel.Size = new System.Drawing.Size(53, 12);
      this.typeNamespaceLabel.TabIndex = 0;
      this.typeNamespaceLabel.Text = "命名空间";
      // 
      // captionTextBox
      // 
      this.captionTextBox.Location = new System.Drawing.Point(89, 78);
      this.captionTextBox.MaxLength = 30;
      this.captionTextBox.Name = "captionTextBox";
      this.captionTextBox.Size = new System.Drawing.Size(208, 21);
      this.captionTextBox.TabIndex = 5;
      this.captionTextBox.TextChanged += new System.EventHandler(this.captionTextBox_TextChanged);
      // 
      // captionLabel
      // 
      this.captionLabel.AutoSize = true;
      this.captionLabel.Location = new System.Drawing.Point(30, 82);
      this.captionLabel.Name = "captionLabel";
      this.captionLabel.Size = new System.Drawing.Size(53, 12);
      this.captionLabel.TabIndex = 4;
      this.captionLabel.Text = "标    签";
      // 
      // typeNameTextBox
      // 
      this.typeNameTextBox.Enabled = false;
      this.typeNameTextBox.Location = new System.Drawing.Point(89, 51);
      this.typeNameTextBox.MaxLength = 30;
      this.typeNameTextBox.Name = "typeNameTextBox";
      this.typeNameTextBox.Size = new System.Drawing.Size(208, 21);
      this.typeNameTextBox.TabIndex = 3;
      this.typeNameTextBox.TextChanged += new System.EventHandler(this.typeNameTextBox_TextChanged);
      // 
      // typeNameLabel
      // 
      this.typeNameLabel.AutoSize = true;
      this.typeNameLabel.Location = new System.Drawing.Point(30, 55);
      this.typeNameLabel.Name = "typeNameLabel";
      this.typeNameLabel.Size = new System.Drawing.Size(53, 12);
      this.typeNameLabel.TabIndex = 2;
      this.typeNameLabel.Text = "类型名称";
      // 
      // importButton
      // 
      this.importButton.Location = new System.Drawing.Point(336, 80);
      this.importButton.Name = "importButton";
      this.importButton.Size = new System.Drawing.Size(90, 23);
      this.importButton.TabIndex = 8;
      this.importButton.Text = "导入";
      this.importButton.UseVisualStyleBackColor = true;
      this.importButton.Click += new System.EventHandler(this.importButton_Click);
      // 
      // NewWorkflowInfoDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(455, 143);
      this.Controls.Add(this.importButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.typeNamespaceTextBox);
      this.Controls.Add(this.typeNamespaceLabel);
      this.Controls.Add(this.captionTextBox);
      this.Controls.Add(this.captionLabel);
      this.Controls.Add(this.typeNameTextBox);
      this.Controls.Add(this.typeNameLabel);
      this.Name = "NewWorkflowInfoDialog";
      this.Text = "新建工作流";
      this.Shown += new System.EventHandler(this.NewWorkflowDialog_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.TextBox typeNamespaceTextBox;
    private System.Windows.Forms.Label typeNamespaceLabel;
    private System.Windows.Forms.TextBox captionTextBox;
    private System.Windows.Forms.Label captionLabel;
    private System.Windows.Forms.TextBox typeNameTextBox;
    private System.Windows.Forms.Label typeNameLabel;
    private System.Windows.Forms.Button importButton;
  }
}