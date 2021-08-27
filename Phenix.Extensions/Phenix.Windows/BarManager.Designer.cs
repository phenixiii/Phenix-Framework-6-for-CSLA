namespace Phenix.Windows
{
  partial class BarManager
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BarManager));
      this.imageList = new System.Windows.Forms.ImageList(this.components);
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      // 
      // imageList
      // 
      this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
      this.imageList.TransparentColor = System.Drawing.Color.Transparent;
      this.imageList.Images.SetKeyName(0, "Fetch.ico");
      this.imageList.Images.SetKeyName(1, "Reset.ico");
      this.imageList.Images.SetKeyName(2, "Add.ico");
      this.imageList.Images.SetKeyName(3, "Modify.ico");
      this.imageList.Images.SetKeyName(4, "Delete.ico");
      this.imageList.Images.SetKeyName(5, "Cancel.ico");
      this.imageList.Images.SetKeyName(6, "Save.ico");
      this.imageList.Images.SetKeyName(7, "Import.ico");
      this.imageList.Images.SetKeyName(8, "Export.ico");
      this.imageList.Images.SetKeyName(9, "Print.ico");
      this.imageList.Images.SetKeyName(10, "Help.ico");
      this.imageList.Images.SetKeyName(11, "Exit.ico");
      this.imageList.Images.SetKeyName(12, "Locate.ico");
      this.imageList.Images.SetKeyName(13, "Setup.ico");
      this.imageList.Images.SetKeyName(14, "AddClone.ico");
      this.imageList.Images.SetKeyName(15, "Restore.ico");
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private System.Windows.Forms.ImageList imageList;
  }
}
