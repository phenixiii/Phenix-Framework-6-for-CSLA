namespace Phenix.Windows
{
  partial class CriteriaCombineItemControl
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
      this.leftNodeFieldMapInfoLookUpEdit = new DevExpress.XtraEditors.LookUpEdit();
      this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
      ((System.ComponentModel.ISupportInitialize)(this.leftNodeFieldMapInfoLookUpEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
      this.splitContainerControl.SuspendLayout();
      this.SuspendLayout();
      // 
      // leftNodeFieldMapInfoLookUpEdit
      // 
      this.leftNodeFieldMapInfoLookUpEdit.Dock = System.Windows.Forms.DockStyle.Fill;
      this.leftNodeFieldMapInfoLookUpEdit.Location = new System.Drawing.Point(0, 0);
      this.leftNodeFieldMapInfoLookUpEdit.Name = "leftNodeFieldMapInfoLookUpEdit";
      this.leftNodeFieldMapInfoLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.leftNodeFieldMapInfoLookUpEdit.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Caption", "Caption")});
      this.leftNodeFieldMapInfoLookUpEdit.Properties.DataSource = this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource;
      this.leftNodeFieldMapInfoLookUpEdit.Properties.DisplayMember = "Caption";
      this.leftNodeFieldMapInfoLookUpEdit.Properties.ImmediatePopup = true;
      this.leftNodeFieldMapInfoLookUpEdit.Properties.NullText = "";
      this.leftNodeFieldMapInfoLookUpEdit.Properties.ShowHeader = false;
      this.leftNodeFieldMapInfoLookUpEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
      this.leftNodeFieldMapInfoLookUpEdit.Properties.ValueMember = "Value";
      this.leftNodeFieldMapInfoLookUpEdit.Size = new System.Drawing.Size(197, 20);
      this.leftNodeFieldMapInfoLookUpEdit.TabIndex = 0;
      this.leftNodeFieldMapInfoLookUpEdit.EditValueChanged += new System.EventHandler(this.leftNodeFieldMapInfoLookUpEdit_EditValueChanged);
      // 
      // criteriaExpressionPropertyKeyCaptionCollectionBindingSource
      // 
      this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource.DataSource = typeof(Phenix.Core.Rule.CriteriaExpressionPropertyKeyCaptionCollection);
      // 
      // splitContainerControl
      // 
      this.splitContainerControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainerControl.Location = new System.Drawing.Point(0, 0);
      this.splitContainerControl.Name = "splitContainerControl";
      this.splitContainerControl.Panel1.Controls.Add(this.leftNodeFieldMapInfoLookUpEdit);
      this.splitContainerControl.Panel1.Text = "Panel1";
      this.splitContainerControl.Panel2.Text = "Panel2";
      this.splitContainerControl.Size = new System.Drawing.Size(600, 22);
      this.splitContainerControl.SplitterPosition = 197;
      this.splitContainerControl.TabIndex = 1;
      this.splitContainerControl.Text = "splitContainerControl1";
      // 
      // CriteriaCombineItemControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitContainerControl);
      this.Name = "CriteriaCombineItemControl";
      this.Size = new System.Drawing.Size(600, 22);
      ((System.ComponentModel.ISupportInitialize)(this.leftNodeFieldMapInfoLookUpEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
      this.splitContainerControl.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource criteriaExpressionPropertyKeyCaptionCollectionBindingSource;
    private DevExpress.XtraEditors.LookUpEdit leftNodeFieldMapInfoLookUpEdit;
    private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;

  }
}
