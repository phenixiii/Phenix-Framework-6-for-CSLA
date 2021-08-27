namespace Phenix.Security.Windows.UserManage
{
  partial class UserReport
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

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.Detail = new DevExpress.XtraReports.UI.DetailBand();
      this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
      this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
      this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
      this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
      this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
      this.userListBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.pageFooterBand1 = new DevExpress.XtraReports.UI.PageFooterBand();
      this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
      this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
      this.reportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
      this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
      this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
      this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
      this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
      this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
      this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
      this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
      ((System.ComponentModel.ISupportInitialize)(this.userListBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      // 
      // Detail
      // 
      this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrLabel4,
            this.xrLine1});
      this.Detail.HeightF = 40F;
      this.Detail.Name = "Detail";
      this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
      this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
      // 
      // xrLabel3
      // 
      this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "UserNumber")});
      this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 3.000005F);
      this.xrLabel3.Name = "xrLabel3";
      this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      this.xrLabel3.SizeF = new System.Drawing.SizeF(246.875F, 37F);
      this.xrLabel3.StyleName = "DataField";
      this.xrLabel3.StylePriority.UseTextAlignment = false;
      this.xrLabel3.Text = "xrLabel3";
      this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
      // 
      // xrLabel4
      // 
      this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Name")});
      this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(252.875F, 3.000005F);
      this.xrLabel4.Name = "xrLabel4";
      this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      this.xrLabel4.SizeF = new System.Drawing.SizeF(391.125F, 36.99999F);
      this.xrLabel4.StyleName = "DataField";
      this.xrLabel4.StylePriority.UseTextAlignment = false;
      this.xrLabel4.Text = "xrLabel4";
      this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
      // 
      // xrLine1
      // 
      this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(6F, 3F);
      this.xrLine1.Name = "xrLine1";
      this.xrLine1.SizeF = new System.Drawing.SizeF(638F, 2F);
      // 
      // xrLabel1
      // 
      this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 39F);
      this.xrLabel1.Name = "xrLabel1";
      this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      this.xrLabel1.SizeF = new System.Drawing.SizeF(246.875F, 29.41667F);
      this.xrLabel1.StyleName = "FieldCaption";
      this.xrLabel1.StylePriority.UseTextAlignment = false;
      this.xrLabel1.Text = "工号";
      this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
      // 
      // xrLabel2
      // 
      this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(252.875F, 39F);
      this.xrLabel2.Name = "xrLabel2";
      this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      this.xrLabel2.SizeF = new System.Drawing.SizeF(391.125F, 29.41667F);
      this.xrLabel2.StyleName = "FieldCaption";
      this.xrLabel2.StylePriority.UseTextAlignment = false;
      this.xrLabel2.Text = "姓名";
      this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
      // 
      // userListBindingSource
      // 
      this.userListBindingSource.DataSource = typeof(Phenix.Security.Business.UserList);
      // 
      // pageFooterBand1
      // 
      this.pageFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrPageInfo2});
      this.pageFooterBand1.HeightF = 29F;
      this.pageFooterBand1.Name = "pageFooterBand1";
      // 
      // xrPageInfo1
      // 
      this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(6F, 6F);
      this.xrPageInfo1.Name = "xrPageInfo1";
      this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
      this.xrPageInfo1.SizeF = new System.Drawing.SizeF(313F, 23F);
      this.xrPageInfo1.StyleName = "PageInfo";
      // 
      // xrPageInfo2
      // 
      this.xrPageInfo2.Format = "Page {0} of {1}";
      this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(331F, 6F);
      this.xrPageInfo2.Name = "xrPageInfo2";
      this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      this.xrPageInfo2.SizeF = new System.Drawing.SizeF(313F, 23F);
      this.xrPageInfo2.StyleName = "PageInfo";
      this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
      // 
      // reportHeaderBand1
      // 
      this.reportHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrLabel1,
            this.xrLabel2});
      this.reportHeaderBand1.HeightF = 68.41667F;
      this.reportHeaderBand1.Name = "reportHeaderBand1";
      // 
      // xrLabel5
      // 
      this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(6F, 6F);
      this.xrLabel5.Name = "xrLabel5";
      this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      this.xrLabel5.SizeF = new System.Drawing.SizeF(638F, 33F);
      this.xrLabel5.StyleName = "Title";
      this.xrLabel5.StylePriority.UseBorders = false;
      this.xrLabel5.StylePriority.UseTextAlignment = false;
      this.xrLabel5.Text = "用户清单";
      this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
      // 
      // Title
      // 
      this.Title.BackColor = System.Drawing.Color.Transparent;
      this.Title.BorderColor = System.Drawing.Color.Black;
      this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
      this.Title.BorderWidth = 1;
      this.Title.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold);
      this.Title.ForeColor = System.Drawing.Color.Maroon;
      this.Title.Name = "Title";
      // 
      // FieldCaption
      // 
      this.FieldCaption.BackColor = System.Drawing.Color.Transparent;
      this.FieldCaption.BorderColor = System.Drawing.Color.Black;
      this.FieldCaption.Borders = DevExpress.XtraPrinting.BorderSide.None;
      this.FieldCaption.BorderWidth = 1;
      this.FieldCaption.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
      this.FieldCaption.ForeColor = System.Drawing.Color.Maroon;
      this.FieldCaption.Name = "FieldCaption";
      // 
      // PageInfo
      // 
      this.PageInfo.BackColor = System.Drawing.Color.Transparent;
      this.PageInfo.BorderColor = System.Drawing.Color.Black;
      this.PageInfo.Borders = DevExpress.XtraPrinting.BorderSide.None;
      this.PageInfo.BorderWidth = 1;
      this.PageInfo.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
      this.PageInfo.ForeColor = System.Drawing.Color.Black;
      this.PageInfo.Name = "PageInfo";
      // 
      // DataField
      // 
      this.DataField.BackColor = System.Drawing.Color.Transparent;
      this.DataField.BorderColor = System.Drawing.Color.Black;
      this.DataField.Borders = DevExpress.XtraPrinting.BorderSide.None;
      this.DataField.BorderWidth = 1;
      this.DataField.Font = new System.Drawing.Font("Times New Roman", 10F);
      this.DataField.ForeColor = System.Drawing.Color.Black;
      this.DataField.Name = "DataField";
      this.DataField.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      // 
      // topMarginBand1
      // 
      this.topMarginBand1.HeightF = 59.375F;
      this.topMarginBand1.Name = "topMarginBand1";
      // 
      // bottomMarginBand1
      // 
      this.bottomMarginBand1.Name = "bottomMarginBand1";
      // 
      // UserReport
      // 
      this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.pageFooterBand1,
            this.reportHeaderBand1,
            this.topMarginBand1,
            this.bottomMarginBand1});
      this.DataSource = this.userListBindingSource;
      this.Margins = new System.Drawing.Printing.Margins(100, 100, 59, 100);
      this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.FieldCaption,
            this.PageInfo,
            this.DataField});
      this.Version = "12.2";
      ((System.ComponentModel.ISupportInitialize)(this.userListBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private DevExpress.XtraReports.UI.DetailBand Detail;
    private System.Windows.Forms.BindingSource userListBindingSource;
    private DevExpress.XtraReports.UI.XRLabel xrLabel1;
    private DevExpress.XtraReports.UI.XRLabel xrLabel2;
    private DevExpress.XtraReports.UI.XRLabel xrLabel3;
    private DevExpress.XtraReports.UI.XRLabel xrLabel4;
    private DevExpress.XtraReports.UI.XRLine xrLine1;
    private DevExpress.XtraReports.UI.PageFooterBand pageFooterBand1;
    private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
    private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
    private DevExpress.XtraReports.UI.ReportHeaderBand reportHeaderBand1;
    private DevExpress.XtraReports.UI.XRLabel xrLabel5;
    private DevExpress.XtraReports.UI.XRControlStyle Title;
    private DevExpress.XtraReports.UI.XRControlStyle FieldCaption;
    private DevExpress.XtraReports.UI.XRControlStyle PageInfo;
    private DevExpress.XtraReports.UI.XRControlStyle DataField;
    private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
    private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
  }
}
