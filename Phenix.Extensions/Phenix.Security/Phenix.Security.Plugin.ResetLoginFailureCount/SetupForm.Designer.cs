namespace Phenix.Security.Plugin.ResetLoginFailureCount
{
  partial class SetupForm
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
      this.resetIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.resetIntervalNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // resetIntervalNumericUpDown
      // 
      this.resetIntervalNumericUpDown.Location = new System.Drawing.Point(180, 39);
      this.resetIntervalNumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
      this.resetIntervalNumericUpDown.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
      this.resetIntervalNumericUpDown.Name = "resetIntervalNumericUpDown";
      this.resetIntervalNumericUpDown.Size = new System.Drawing.Size(93, 21);
      this.resetIntervalNumericUpDown.TabIndex = 7;
      this.resetIntervalNumericUpDown.Value = new decimal(new int[] {
            1440,
            0,
            0,
            0});
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(31, 41);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(143, 12);
      this.label1.TabIndex = 5;
      this.label1.Text = "Reset Time Span Minutes";
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(165, 97);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 6;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.Location = new System.Drawing.Point(50, 97);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 4;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // SetupForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(313, 150);
      this.Controls.Add(this.resetIntervalNumericUpDown);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Name = "SetupForm";
      this.Text = "Setup";
      ((System.ComponentModel.ISupportInitialize)(this.resetIntervalNumericUpDown)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.NumericUpDown resetIntervalNumericUpDown;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
  }
}