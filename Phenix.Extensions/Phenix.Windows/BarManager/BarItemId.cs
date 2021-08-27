using System;

namespace Phenix.Windows
{
  /// <summary>
  /// 菜单项ID
  /// </summary>
  [Serializable]
  public enum BarItemId
  {
    /// <summary>
    /// 无
    /// </summary>
    None = 0,

    /// <summary>
    /// 检索
    /// </summary>
    Fetch = 1000,

    /// <summary>
    /// 复位
    /// </summary>
    Reset = 1001,
    
    /// <summary>
    /// 恢复
    /// </summary>
    Restore = 1015,

    /// <summary>
    /// 新增
    /// </summary>
    Add = 1002,
    
    /// <summary>
    /// 克隆
    /// </summary>
    AddClone = 1014,

    /// <summary>
    /// 编辑
    /// </summary>
    Modify = 1003,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 1004,
    
    /// <summary>
    /// 定位
    /// </summary>
    Locate = 1012,

    /// <summary>
    /// 取消
    /// </summary>
    Cancel = 1005,

    /// <summary>
    /// 保存
    /// </summary>
    Save = 1006,

    /// <summary>
    /// 导入
    /// </summary>
    Import = 1007,

    /// <summary>
    /// 导出
    /// </summary>
    Export = 1008,

    /// <summary>
    /// 打印
    /// </summary>
    Print = 1009,

    /// <summary>
    /// 帮助
    /// </summary>
    Help = 1010,

    /// <summary>
    /// 设置
    /// </summary>
    Setup = 1013,

    /// <summary>
    /// 退出
    /// </summary>
    Exit = 1011,

    /// <summary>
    /// 操作状态
    /// </summary>
    DataOperateState = 2000,

    /// <summary>
    /// 记录状态
    /// </summary>
    DataRecordState = 2001,

    /// <summary>
    /// 提示
    /// </summary>
    Hint = 2002
  }
}
