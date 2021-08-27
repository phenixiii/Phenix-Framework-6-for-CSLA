namespace Phenix.Windows
{
  partial class CriteriaCombineItemBooleanValueControl
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
      this.valueCheckEdit = new DevExpress.XtraEditors.CheckEdit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaOperateLookUpEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.valueCheckEdit.Properties)).BeginInit();
      this.SuspendLayout();
      // 
      // criteriaOperateLookUpEdit
      // 
      this.criteriaOperateLookUpEdit.Visible = false;
      // 
      // valueCheckEdit
      // 
      this.valueCheckEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.criteriaExpressionBindingSource, "Value", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.valueCheckEdit.Dock = System.Windows.Forms.DockStyle.Fill;
      this.valueCheckEdit.Location = new System.Drawing.Point(100, 0);
      this.valueCheckEdit.Name = "valueCheckEdit";
      this.valueCheckEdit.Properties.AllowGrayed = true;
      this.valueCheckEdit.Properties.Caption = "";
      this.valueCheckEdit.Size = new System.Drawing.Size(327, 19);
      this.valueCheckEdit.TabIndex = 3;
      // 
      // CriteriaCombineItemBooleanValueControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.valueCheckEdit);
      this.Name = "CriteriaCombineItemBooleanValueControl";
      this.Controls.SetChildIndex(this.criteriaOperateLookUpEdit, 0);
      this.Controls.SetChildIndex(this.valueCheckEdit, 0);
      ((System.ComponentModel.ISupportInitialize)(this.criteriaOperateLookUpEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.valueCheckEdit.Properties)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraEditors.CheckEdit valueCheckEdit;

  }
}
