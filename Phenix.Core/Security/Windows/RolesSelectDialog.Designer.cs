namespace Phenix.Core.Security.Windows
{
  partial class RolesSelectDialog
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
      this.unselectRolesListBox = new System.Windows.Forms.ListBox();
      this.selectedRolesListBox = new System.Windows.Forms.ListBox();
      this.unselectedRolesLabel = new System.Windows.Forms.Label();
      this.selectedRolesLabel = new System.Windows.Forms.Label();
      this.selectAllButton = new System.Windows.Forms.Button();
      this.selectButton = new System.Windows.Forms.Button();
      this.unselectButton = new System.Windows.Forms.Button();
      this.unselectAllButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // unselectRolesListBox
      // 
      this.unselectRolesListBox.FormattingEnabled = true;
      this.unselectRolesListBox.ItemHeight = 12;
      this.unselectRolesListBox.Location = new System.Drawing.Point(22, 38);
      this.unselectRolesListBox.Name = "unselectRolesListBox";
      this.unselectRolesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.unselectRolesListBox.Size = new System.Drawing.Size(150, 196);
      this.unselectRolesListBox.TabIndex = 1;
      this.unselectRolesListBox.DoubleClick += new System.EventHandler(this.unselectRolesListBox_DoubleClick);
      // 
      // selectedRolesListBox
      // 
      this.selectedRolesListBox.FormattingEnabled = true;
      this.selectedRolesListBox.ItemHeight = 12;
      this.selectedRolesListBox.Location = new System.Drawing.Point(212, 38);
      this.selectedRolesListBox.Name = "selectedRolesListBox";
      this.selectedRolesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.selectedRolesListBox.Size = new System.Drawing.Size(150, 196);
      this.selectedRolesListBox.TabIndex = 7;
      this.selectedRolesListBox.DoubleClick += new System.EventHandler(this.selectedRolesListBox_DoubleClick);
      // 
      // unselectedRolesLabel
      // 
      this.unselectedRolesLabel.AutoSize = true;
      this.unselectedRolesLabel.Location = new System.Drawing.Point(22, 20);
      this.unselectedRolesLabel.Name = "unselectedRolesLabel";
      this.unselectedRolesLabel.Size = new System.Drawing.Size(53, 12);
      this.unselectedRolesLabel.TabIndex = 0;
      this.unselectedRolesLabel.Text = "可选角色";
      // 
      // selectedRolesLabel
      // 
      this.selectedRolesLabel.AutoSize = true;
      this.selectedRolesLabel.Location = new System.Drawing.Point(210, 20);
      this.selectedRolesLabel.Name = "selectedRolesLabel";
      this.selectedRolesLabel.Size = new System.Drawing.Size(53, 12);
      this.selectedRolesLabel.TabIndex = 6;
      this.selectedRolesLabel.Text = "已选角色";
      // 
      // selectAllButton
      // 
      this.selectAllButton.Location = new System.Drawing.Point(178, 51);
      this.selectAllButton.Name = "selectAllButton";
      this.selectAllButton.Size = new System.Drawing.Size(28, 25);
      this.selectAllButton.TabIndex = 2;
      this.selectAllButton.Text = ">>";
      this.selectAllButton.UseVisualStyleBackColor = true;
      this.selectAllButton.Click += new System.EventHandler(this.selectAllButton_Click);
      // 
      // selectButton
      // 
      this.selectButton.Location = new System.Drawing.Point(178, 82);
      this.selectButton.Name = "selectButton";
      this.selectButton.Size = new System.Drawing.Size(28, 25);
      this.selectButton.TabIndex = 3;
      this.selectButton.Text = ">";
      this.selectButton.UseVisualStyleBackColor = true;
      this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
      // 
      // unselectButton
      // 
      this.unselectButton.Location = new System.Drawing.Point(178, 113);
      this.unselectButton.Name = "unselectButton";
      this.unselectButton.Size = new System.Drawing.Size(28, 25);
      this.unselectButton.TabIndex = 4;
      this.unselectButton.Text = "<";
      this.unselectButton.UseVisualStyleBackColor = true;
      this.unselectButton.Click += new System.EventHandler(this.unselectButton_Click);
      // 
      // unselectAllButton
      // 
      this.unselectAllButton.Location = new System.Drawing.Point(178, 144);
      this.unselectAllButton.Name = "unselectAllButton";
      this.unselectAllButton.Size = new System.Drawing.Size(28, 25);
      this.unselectAllButton.TabIndex = 5;
      this.unselectAllButton.Text = "<<";
      this.unselectAllButton.UseVisualStyleBackColor = true;
      this.unselectAllButton.Click += new System.EventHandler(this.unselectAllButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(392, 49);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 9;
      this.cancelButton.Text = "取消";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(392, 20);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 8;
      this.okButton.Text = "确认";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // AllowRolesSetupDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(501, 273);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.unselectAllButton);
      this.Controls.Add(this.unselectButton);
      this.Controls.Add(this.selectButton);
      this.Controls.Add(this.selectAllButton);
      this.Controls.Add(this.selectedRolesLabel);
      this.Controls.Add(this.unselectedRolesLabel);
      this.Controls.Add(this.selectedRolesListBox);
      this.Controls.Add(this.unselectRolesListBox);
      this.Name = "AllowRolesSetupDialog";
      this.Text = "选择角色";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListBox unselectRolesListBox;
    private System.Windows.Forms.ListBox selectedRolesListBox;
    private System.Windows.Forms.Label unselectedRolesLabel;
    private System.Windows.Forms.Label selectedRolesLabel;
    private System.Windows.Forms.Button selectAllButton;
    private System.Windows.Forms.Button selectButton;
    private System.Windows.Forms.Button unselectButton;
    private System.Windows.Forms.Button unselectAllButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
  }
}