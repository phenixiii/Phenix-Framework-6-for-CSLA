namespace Phenix.Windows
{
  partial class CriteriaCombineItemDateTimeValueControl
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
      this.valueTimeEdit = new DevExpress.XtraEditors.TimeEdit();
      this.valueDateEdit = new DevExpress.XtraEditors.DateEdit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaOperateLookUpEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.valueTimeEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.valueDateEdit.Properties.CalendarTimeProperties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.valueDateEdit.Properties)).BeginInit();
      this.SuspendLayout();
      // 
      // valueTimeEdit
      // 
      this.valueTimeEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.criteriaExpressionBindingSource, "Value", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.valueTimeEdit.Dock = System.Windows.Forms.DockStyle.Fill;
      this.valueTimeEdit.EditValue = new System.DateTime(2011, 6, 26, 0, 0, 0, 0);
      this.valueTimeEdit.EnterMoveNextControl = true;
      this.valueTimeEdit.Location = new System.Drawing.Point(366, 0);
      this.valueTimeEdit.Name = "valueTimeEdit";
      this.valueTimeEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
      this.valueTimeEdit.Size = new System.Drawing.Size(104, 20);
      this.valueTimeEdit.TabIndex = 11;
      // 
      // valueDateEdit
      // 
      this.valueDateEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.criteriaExpressionBindingSource, "Value", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.valueDateEdit.Dock = System.Windows.Forms.DockStyle.Left;
      this.valueDateEdit.EditValue = new System.DateTime(2011, 6, 26, 0, 0, 0, 0);
      this.valueDateEdit.EnterMoveNextControl = true;
      this.valueDateEdit.Location = new System.Drawing.Point(100, 0);
      this.valueDateEdit.Name = "valueDateEdit";
      this.valueDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.valueDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
      this.valueDateEdit.Size = new System.Drawing.Size(266, 20);
      this.valueDateEdit.TabIndex = 10;
      // 
      // CriteriaCombineItemDateTimeValueControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.valueTimeEdit);
      this.Controls.Add(this.valueDateEdit);
      this.Name = "CriteriaCombineItemDateTimeValueControl";
      this.Size = new System.Drawing.Size(470, 22);
      this.Controls.SetChildIndex(this.criteriaOperateLookUpEdit, 0);
      this.Controls.SetChildIndex(this.valueDateEdit, 0);
      this.Controls.SetChildIndex(this.valueTimeEdit, 0);
      ((System.ComponentModel.ISupportInitialize)(this.criteriaOperateLookUpEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.valueTimeEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.valueDateEdit.Properties.CalendarTimeProperties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.valueDateEdit.Properties)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraEditors.TimeEdit valueTimeEdit;
    private DevExpress.XtraEditors.DateEdit valueDateEdit;

  }
}
