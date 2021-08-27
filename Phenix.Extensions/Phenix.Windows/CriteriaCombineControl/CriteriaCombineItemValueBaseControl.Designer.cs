namespace Phenix.Windows
{
  partial class CriteriaCombineItemValueBaseControl
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
      this.criteriaOperateLookUpEdit = new DevExpress.XtraEditors.LookUpEdit();
      this.criteriaExpressionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.criteriaOperateEnumKeyCaptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.criteriaOperateLookUpEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaOperateEnumKeyCaptionBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // criteriaOperateLookUpEdit
      // 
      this.criteriaOperateLookUpEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.criteriaExpressionBindingSource, "Operate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.criteriaOperateLookUpEdit.Dock = System.Windows.Forms.DockStyle.Left;
      this.criteriaOperateLookUpEdit.EnterMoveNextControl = true;
      this.criteriaOperateLookUpEdit.Location = new System.Drawing.Point(0, 0);
      this.criteriaOperateLookUpEdit.Name = "criteriaOperateLookUpEdit";
      this.criteriaOperateLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.criteriaOperateLookUpEdit.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "Key"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Caption", "Caption")});
      this.criteriaOperateLookUpEdit.Properties.DataSource = this.criteriaOperateEnumKeyCaptionBindingSource;
      this.criteriaOperateLookUpEdit.Properties.DisplayMember = "Caption";
      this.criteriaOperateLookUpEdit.Properties.ImmediatePopup = true;
      this.criteriaOperateLookUpEdit.Properties.NullText = "";
      this.criteriaOperateLookUpEdit.Properties.ShowHeader = false;
      this.criteriaOperateLookUpEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
      this.criteriaOperateLookUpEdit.Properties.ValueMember = "Value";
      this.criteriaOperateLookUpEdit.Size = new System.Drawing.Size(100, 20);
      this.criteriaOperateLookUpEdit.TabIndex = 1;
      this.criteriaOperateLookUpEdit.EditValueChanged += new System.EventHandler(this.criteriaOperateLookUpEdit_EditValueChanged);
      // 
      // criteriaExpressionBindingSource
      // 
      this.criteriaExpressionBindingSource.DataSource = typeof(Phenix.Core.Mapping.CriteriaExpression);
      // 
      // criteriaOperateEnumKeyCaptionBindingSource
      // 
      this.criteriaOperateEnumKeyCaptionBindingSource.DataSource = typeof(Phenix.Core.Rule.EnumKeyCaption);
      // 
      // CriteriaCombineItemValueBaseControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.criteriaOperateLookUpEdit);
      this.Name = "CriteriaCombineItemValueBaseControl";
      this.Size = new System.Drawing.Size(427, 22);
      ((System.ComponentModel.ISupportInitialize)(this.criteriaOperateLookUpEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaOperateEnumKeyCaptionBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource criteriaOperateEnumKeyCaptionBindingSource;
    protected DevExpress.XtraEditors.LookUpEdit criteriaOperateLookUpEdit;
    protected System.Windows.Forms.BindingSource criteriaExpressionBindingSource;
  }
}
