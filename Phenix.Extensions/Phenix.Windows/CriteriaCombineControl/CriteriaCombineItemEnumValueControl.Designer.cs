using System;

namespace Phenix.Windows
{
  partial class CriteriaCombineItemEnumValueControl
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
      this.components = new System.ComponentModel.Container();
      this.valueCheckedComboBoxEdit = new DevExpress.XtraEditors.CheckedComboBoxEdit();
      this.enumKeyCaptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.criteriaOperateLookUpEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.valueCheckedComboBoxEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.enumKeyCaptionBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // valueCheckedComboBoxEdit
      // 
      this.valueCheckedComboBoxEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.criteriaExpressionBindingSource, "Value", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.valueCheckedComboBoxEdit.Dock = System.Windows.Forms.DockStyle.Fill;
      this.valueCheckedComboBoxEdit.EnterMoveNextControl = true;
      this.valueCheckedComboBoxEdit.Location = new System.Drawing.Point(100, 0);
      this.valueCheckedComboBoxEdit.Name = "valueCheckedComboBoxEdit";
      this.valueCheckedComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.valueCheckedComboBoxEdit.Properties.DataSource = this.enumKeyCaptionBindingSource;
      this.valueCheckedComboBoxEdit.Properties.DisplayMember = "Caption";
      this.valueCheckedComboBoxEdit.Properties.ValueMember = "Value";
      this.valueCheckedComboBoxEdit.Size = new System.Drawing.Size(327, 20);
      this.valueCheckedComboBoxEdit.TabIndex = 3;
      // 
      // enumKeyCaptionBindingSource
      // 
      this.enumKeyCaptionBindingSource.DataSource = typeof(Phenix.Core.Rule.EnumKeyCaption);
      // 
      // CriteriaCombineItemEnumValueControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.valueCheckedComboBoxEdit);
      this.Name = "CriteriaCombineItemEnumValueControl";
      this.Controls.SetChildIndex(this.criteriaOperateLookUpEdit, 0);
      this.Controls.SetChildIndex(this.valueCheckedComboBoxEdit, 0);
      ((System.ComponentModel.ISupportInitialize)(this.criteriaOperateLookUpEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.valueCheckedComboBoxEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.enumKeyCaptionBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraEditors.CheckedComboBoxEdit valueCheckedComboBoxEdit;
    private System.Windows.Forms.BindingSource enumKeyCaptionBindingSource;
  }
}
