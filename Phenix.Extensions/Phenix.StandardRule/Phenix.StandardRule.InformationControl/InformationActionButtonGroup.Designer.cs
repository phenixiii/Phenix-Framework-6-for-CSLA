namespace Phenix.StandardRule.InformationControl
{
    partial class InformationActionButtonGroup
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.btnApply = new DevExpress.XtraEditors.SimpleButton();
      this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
      this.btnInvalid = new DevExpress.XtraEditors.SimpleButton();
      this.btnSubmitVerify = new DevExpress.XtraEditors.SimpleButton();
      this.btnVerifyNotPass = new DevExpress.XtraEditors.SimpleButton();
      this.btnMerge = new DevExpress.XtraEditors.SimpleButton();
      this.btnVerifyPass = new DevExpress.XtraEditors.SimpleButton();
      this.btnSuspend = new DevExpress.XtraEditors.SimpleButton();
      this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
      this.layoutControlItemApply = new DevExpress.XtraLayout.LayoutControlItem();
      this.layoutControlItemSuspend = new DevExpress.XtraLayout.LayoutControlItem();
      this.layoutControlItemVerifyPass = new DevExpress.XtraLayout.LayoutControlItem();
      this.layoutControlItemSubmitVerify = new DevExpress.XtraLayout.LayoutControlItem();
      this.layoutControlItemInvalid = new DevExpress.XtraLayout.LayoutControlItem();
      this.layoutControlItemVerifyNotPass = new DevExpress.XtraLayout.LayoutControlItem();
      this.layoutControlItemMerge = new DevExpress.XtraLayout.LayoutControlItem();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
      this.layoutControl.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemApply)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSuspend)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemVerifyPass)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSubmitVerify)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemInvalid)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemVerifyNotPass)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMerge)).BeginInit();
      this.SuspendLayout();
      // 
      // btnApply
      // 
      this.btnApply.Location = new System.Drawing.Point(551, 2);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new System.Drawing.Size(85, 22);
      this.btnApply.StyleController = this.layoutControl;
      this.btnApply.TabIndex = 6;
      this.btnApply.Text = "恢复使用(&A)";
      // 
      // layoutControl
      // 
      this.layoutControl.AllowCustomizationMenu = false;
      this.layoutControl.Controls.Add(this.btnInvalid);
      this.layoutControl.Controls.Add(this.btnSubmitVerify);
      this.layoutControl.Controls.Add(this.btnVerifyNotPass);
      this.layoutControl.Controls.Add(this.btnApply);
      this.layoutControl.Controls.Add(this.btnMerge);
      this.layoutControl.Controls.Add(this.btnVerifyPass);
      this.layoutControl.Controls.Add(this.btnSuspend);
      this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.layoutControl.Location = new System.Drawing.Point(0, 0);
      this.layoutControl.Name = "layoutControl";
      this.layoutControl.Root = this.layoutControlGroup;
      this.layoutControl.Size = new System.Drawing.Size(638, 26);
      this.layoutControl.TabIndex = 8;
      // 
      // btnInvalid
      // 
      this.btnInvalid.Location = new System.Drawing.Point(288, 2);
      this.btnInvalid.Name = "btnInvalid";
      this.btnInvalid.Size = new System.Drawing.Size(81, 22);
      this.btnInvalid.StyleController = this.layoutControl;
      this.btnInvalid.TabIndex = 4;
      this.btnInvalid.Text = "作废资料(&I)";
      // 
      // btnSubmitVerify
      // 
      this.btnSubmitVerify.Location = new System.Drawing.Point(2, 2);
      this.btnSubmitVerify.Name = "btnSubmitVerify";
      this.btnSubmitVerify.Size = new System.Drawing.Size(92, 22);
      this.btnSubmitVerify.StyleController = this.layoutControl;
      this.btnSubmitVerify.TabIndex = 1;
      this.btnSubmitVerify.Text = "提交审核(&S)";
      // 
      // btnVerifyNotPass
      // 
      this.btnVerifyNotPass.Location = new System.Drawing.Point(187, 2);
      this.btnVerifyNotPass.Name = "btnVerifyNotPass";
      this.btnVerifyNotPass.Size = new System.Drawing.Size(97, 22);
      this.btnVerifyNotPass.StyleController = this.layoutControl;
      this.btnVerifyNotPass.TabIndex = 3;
      this.btnVerifyNotPass.Text = "未通过审核(&N)";
      // 
      // btnMerge
      // 
      this.btnMerge.Location = new System.Drawing.Point(373, 2);
      this.btnMerge.Name = "btnMerge";
      this.btnMerge.Size = new System.Drawing.Size(86, 22);
      this.btnMerge.StyleController = this.layoutControl;
      this.btnMerge.TabIndex = 0;
      this.btnMerge.Text = "合并资料(&M)";
      // 
      // btnVerifyPass
      // 
      this.btnVerifyPass.Location = new System.Drawing.Point(98, 2);
      this.btnVerifyPass.Name = "btnVerifyPass";
      this.btnVerifyPass.Size = new System.Drawing.Size(85, 22);
      this.btnVerifyPass.StyleController = this.layoutControl;
      this.btnVerifyPass.TabIndex = 2;
      this.btnVerifyPass.Text = "通过审核(&V)";
      // 
      // btnSuspend
      // 
      this.btnSuspend.Location = new System.Drawing.Point(463, 2);
      this.btnSuspend.Name = "btnSuspend";
      this.btnSuspend.Size = new System.Drawing.Size(84, 22);
      this.btnSuspend.StyleController = this.layoutControl;
      this.btnSuspend.TabIndex = 5;
      this.btnSuspend.Text = "暂停使用(&S)";
      // 
      // layoutControlGroup
      // 
      this.layoutControlGroup.CustomizationFormText = "layoutControlGroup";
      this.layoutControlGroup.GroupBordersVisible = false;
      this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemApply,
            this.layoutControlItemSuspend,
            this.layoutControlItemVerifyPass,
            this.layoutControlItemSubmitVerify,
            this.layoutControlItemInvalid,
            this.layoutControlItemVerifyNotPass,
            this.layoutControlItemMerge});
      this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
      this.layoutControlGroup.Name = "layoutControlGroup";
      this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      this.layoutControlGroup.Size = new System.Drawing.Size(638, 26);
      this.layoutControlGroup.Text = "layoutControlGroup";
      this.layoutControlGroup.TextVisible = false;
      // 
      // layoutControlItemApply
      // 
      this.layoutControlItemApply.Control = this.btnApply;
      this.layoutControlItemApply.CustomizationFormText = "layoutControlItemApply";
      this.layoutControlItemApply.Location = new System.Drawing.Point(549, 0);
      this.layoutControlItemApply.Name = "layoutControlItemApply";
      this.layoutControlItemApply.Size = new System.Drawing.Size(89, 26);
      this.layoutControlItemApply.Text = "layoutControlItemApply";
      this.layoutControlItemApply.TextSize = new System.Drawing.Size(0, 0);
      this.layoutControlItemApply.TextToControlDistance = 0;
      this.layoutControlItemApply.TextVisible = false;
      // 
      // layoutControlItemSuspend
      // 
      this.layoutControlItemSuspend.Control = this.btnSuspend;
      this.layoutControlItemSuspend.CustomizationFormText = "layoutControlItemSuspend";
      this.layoutControlItemSuspend.Location = new System.Drawing.Point(461, 0);
      this.layoutControlItemSuspend.Name = "layoutControlItemSuspend";
      this.layoutControlItemSuspend.Size = new System.Drawing.Size(88, 26);
      this.layoutControlItemSuspend.Text = "layoutControlItemSuspend";
      this.layoutControlItemSuspend.TextSize = new System.Drawing.Size(0, 0);
      this.layoutControlItemSuspend.TextToControlDistance = 0;
      this.layoutControlItemSuspend.TextVisible = false;
      // 
      // layoutControlItemVerifyPass
      // 
      this.layoutControlItemVerifyPass.Control = this.btnVerifyPass;
      this.layoutControlItemVerifyPass.CustomizationFormText = "layoutControlItemVerifyPass";
      this.layoutControlItemVerifyPass.Location = new System.Drawing.Point(96, 0);
      this.layoutControlItemVerifyPass.Name = "layoutControlItemVerifyPass";
      this.layoutControlItemVerifyPass.Size = new System.Drawing.Size(89, 26);
      this.layoutControlItemVerifyPass.Text = "layoutControlItemVerifyPass";
      this.layoutControlItemVerifyPass.TextSize = new System.Drawing.Size(0, 0);
      this.layoutControlItemVerifyPass.TextToControlDistance = 0;
      this.layoutControlItemVerifyPass.TextVisible = false;
      // 
      // layoutControlItemSubmitVerify
      // 
      this.layoutControlItemSubmitVerify.Control = this.btnSubmitVerify;
      this.layoutControlItemSubmitVerify.CustomizationFormText = "layoutControlItemSubmitVerify";
      this.layoutControlItemSubmitVerify.Location = new System.Drawing.Point(0, 0);
      this.layoutControlItemSubmitVerify.Name = "layoutControlItemSubmitVerify";
      this.layoutControlItemSubmitVerify.Size = new System.Drawing.Size(96, 26);
      this.layoutControlItemSubmitVerify.Text = "layoutControlItemSubmitVerify";
      this.layoutControlItemSubmitVerify.TextSize = new System.Drawing.Size(0, 0);
      this.layoutControlItemSubmitVerify.TextToControlDistance = 0;
      this.layoutControlItemSubmitVerify.TextVisible = false;
      // 
      // layoutControlItemInvalid
      // 
      this.layoutControlItemInvalid.Control = this.btnInvalid;
      this.layoutControlItemInvalid.CustomizationFormText = "layoutControlItemInvalid";
      this.layoutControlItemInvalid.Location = new System.Drawing.Point(286, 0);
      this.layoutControlItemInvalid.Name = "layoutControlItemInvalid";
      this.layoutControlItemInvalid.Size = new System.Drawing.Size(85, 26);
      this.layoutControlItemInvalid.Text = "layoutControlItemInvalid";
      this.layoutControlItemInvalid.TextSize = new System.Drawing.Size(0, 0);
      this.layoutControlItemInvalid.TextToControlDistance = 0;
      this.layoutControlItemInvalid.TextVisible = false;
      // 
      // layoutControlItemVerifyNotPass
      // 
      this.layoutControlItemVerifyNotPass.Control = this.btnVerifyNotPass;
      this.layoutControlItemVerifyNotPass.CustomizationFormText = "layoutControlItemVerifyNotPass";
      this.layoutControlItemVerifyNotPass.Location = new System.Drawing.Point(185, 0);
      this.layoutControlItemVerifyNotPass.Name = "layoutControlItemVerifyNotPass";
      this.layoutControlItemVerifyNotPass.Size = new System.Drawing.Size(101, 26);
      this.layoutControlItemVerifyNotPass.Text = "layoutControlItemVerifyNotPass";
      this.layoutControlItemVerifyNotPass.TextSize = new System.Drawing.Size(0, 0);
      this.layoutControlItemVerifyNotPass.TextToControlDistance = 0;
      this.layoutControlItemVerifyNotPass.TextVisible = false;
      // 
      // layoutControlItemMerge
      // 
      this.layoutControlItemMerge.Control = this.btnMerge;
      this.layoutControlItemMerge.CustomizationFormText = "layoutControlItemMerge";
      this.layoutControlItemMerge.Location = new System.Drawing.Point(371, 0);
      this.layoutControlItemMerge.Name = "layoutControlItemMerge";
      this.layoutControlItemMerge.Size = new System.Drawing.Size(90, 26);
      this.layoutControlItemMerge.Text = "layoutControlItemMerge";
      this.layoutControlItemMerge.TextSize = new System.Drawing.Size(0, 0);
      this.layoutControlItemMerge.TextToControlDistance = 0;
      this.layoutControlItemMerge.TextVisible = false;
      // 
      // InformationActionButtonGroup
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.Controls.Add(this.layoutControl);
      this.Name = "InformationActionButtonGroup";
      this.Size = new System.Drawing.Size(638, 26);
      ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
      this.layoutControl.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemApply)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSuspend)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemVerifyPass)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSubmitVerify)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemInvalid)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemVerifyNotPass)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMerge)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnApply;
        private DevExpress.XtraEditors.SimpleButton btnSuspend;
        private DevExpress.XtraEditors.SimpleButton btnVerifyPass;
        private DevExpress.XtraEditors.SimpleButton btnMerge;
        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemApply;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSuspend;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemVerifyPass;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemMerge;
        private DevExpress.XtraEditors.SimpleButton btnVerifyNotPass;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemVerifyNotPass;
        private DevExpress.XtraEditors.SimpleButton btnInvalid;
        private DevExpress.XtraEditors.SimpleButton btnSubmitVerify;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSubmitVerify;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemInvalid;
    }
}
