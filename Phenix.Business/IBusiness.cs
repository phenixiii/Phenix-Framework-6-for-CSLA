using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using Phenix.Core.Data;
using Phenix.Core.Mapping;

namespace Phenix.Business
{
  /// <summary>
  /// 业务接口
  /// </summary>
  public interface IBusiness : Csla.Core.ISupportUndo, Csla.Core.ITrackStatus, Csla.Core.ISavable, ICloneable
  {
    #region 属性

    /// <summary>
    /// 数据源键
    /// </summary>
    string DataSourceKey { get; }

    /// <summary>
    /// 条件集
    /// </summary>
    Criterions Criterions { get; }

    /// <summary>
    /// 构建自全部库
    /// null: 仅主库
    /// true: 主库+历史库
    /// false: 历史库
    /// </summary>
    bool? HistoryFetchAll { get; }

    /// <summary>
    /// 标签
    /// </summary>
    string Caption { get; }

    /// <summary>
    /// 是否是根对象
    /// 仅允许从根对象上进行(级联)保存
    /// </summary>
    bool IsRoot { get; }

    /// <summary>
    /// 根对象
    /// </summary>
    IBusiness Root { get; }
    
    /// <summary>
    /// 根业务对象
    /// </summary>
    IBusinessObject RootBusiness { get; }

    /// <summary>
    /// 主业务对象
    /// </summary>
    IBusinessObject MasterBusiness { get; }

    /// <summary>
    /// 分组名
    /// </summary>
    string GroupName { get; }
    
    /// <summary>
    /// 在勾选项清单
    /// </summary>
    bool InSelectableList { get; }

    /// <summary>
    /// 是否只读
    /// </summary>
    bool IsReadOnly { get; }

    /// <summary>
    /// 不参与多级撤销并阻断Detail对象的多级撤销
    /// 缺省为 false
    /// </summary>
    bool NotUndoable { get; }

    /// <summary>
    /// 在编辑状态
    /// </summary>
    bool EditMode { get; }

    /// <summary>
    /// 在编辑状态
    /// </summary>
    BooleanOption EditModeOption { get; }

    /// <summary>
    /// 编辑层级
    /// </summary>
    int EditLevel { get; }

    /// <summary>
    /// 参与事务处理前端的业务队列
    /// </summary>
    Collection<IBusiness> FirstTransactionData { get; }

    /// <summary>
    /// 参与事务处理末端的业务队列
    /// </summary>
    Collection<IBusiness> LastTransactionData { get; }

    /// <summary>
    /// 是否级联Save?
    /// </summary>
    bool CascadingSave { get; }

    /// <summary>
    /// 是否级联Delete?
    /// 如果 CascadingSave = false 则忽略本属性值
    /// </summary>
    bool CascadingDelete { get; }

    /// <summary>
    /// 保留非脏对象以提交到服务端的代码使用
    /// 缺省为 false
    /// </summary>
    bool EnsembleOnSaving { get; }

    /// <summary>
    /// 提交数据后需要刷新本地自己
    /// 缺省为 false
    /// </summary>
    bool NeedRefresh { get; }

    /// <summary>
    /// DbTransaction
    /// </summary>
    DbTransaction DbTransaction { get; }

    #endregion

    #region 方法

    /// <summary>
    /// 校验是否存在重复数据
    /// </summary>
    /// <returns>重复的对象</returns>
    IList<IEntity> CheckRepeated();

    /// <summary>
    /// 校验数据是否有效
    /// onlyOldError = false
    /// onlySelfDirty = false
    /// throwIfException = false
    /// </summary>
    /// <returns>错误信息</returns>
    IDataErrorInfo CheckSelfRules();

    /// <summary>
    /// 校验数据是否有效
    /// onlySelfDirty = false
    /// throwIfException = false
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <returns>错误信息</returns>
    IDataErrorInfo CheckSelfRules(bool onlyOldError);

    /// <summary>
    /// 校验数据是否有效
    /// throwIfException = false
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <param name="onlySelfDirty">仅检查脏数据</param>
    /// <returns>错误信息</returns>
    IDataErrorInfo CheckSelfRules(bool onlyOldError, bool onlySelfDirty);

    /// <summary>
    /// 校验数据是否有效
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <param name="onlySelfDirty">仅检查脏数据</param>
    /// <param name="throwIfException">如果为 true, 则当发现!IsSelfValid时抛出Csla.Rules.ValidationException异常</param>
    /// <returns>错误信息</returns>
    IDataErrorInfo CheckSelfRules(bool onlyOldError, bool onlySelfDirty, bool throwIfException);

    /// <summary>
    /// 校验数据是否有效(仅ObjectRules)
    /// onlySelfDirty = false
    /// throwIfException = false
    /// </summary>
    /// <returns>错误信息</returns>
    IDataErrorInfo CheckSelfObjectRules();

    /// <summary>
    /// 校验数据是否有效(仅ObjectRules)
    /// throwIfException = false
    /// </summary>
    /// <param name="onlySelfDirty">仅检查脏数据</param>
    /// <returns>错误信息</returns>
    IDataErrorInfo CheckSelfObjectRules(bool onlySelfDirty);

    /// <summary>
    /// 校验数据是否有效(仅ObjectRules)
    /// </summary>
    /// <param name="onlySelfDirty">仅检查脏数据</param>
    /// <param name="throwIfException">如果为 true, 则当发现!IsSelfValid时抛出Csla.Rules.ValidationException异常</param>
    /// <returns>错误信息</returns>
    IDataErrorInfo CheckSelfObjectRules(bool onlySelfDirty, bool throwIfException);

    /// <summary>
    /// 校验数据是否有效
    /// onlyOldError = false
    /// onlySelfDirty = false
    /// </summary>
    /// <returns>错误信息</returns>
    string CheckRules();

    /// <summary>
    /// 校验数据是否有效
    /// onlySelfDirty = false
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <returns>错误信息</returns>
    string CheckRules(bool onlyOldError);

    /// <summary>
    /// 校验数据是否有效
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <param name="onlySelfDirty">仅检查脏数据</param>
    /// <returns>错误信息</returns>
    string CheckRules(bool onlyOldError, bool onlySelfDirty);

    /// <summary>
    /// 开始编辑
    /// </summary>
    new void BeginEdit();

    /// <summary>
    /// 取消编辑
    /// </summary>
    new void CancelEdit();

    /// <summary>
    /// 接受编辑
    /// </summary>
    new void ApplyEdit();
    
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    /// <param name="onlySaveSelected">仅提交被勾选的业务对象</param>
    /// <param name="firstTransactionData">参与事务处理前端的业务队列</param>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的对象</returns>
    IBusiness Save(bool needCheckDirty, bool? onlySaveSelected, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData);

    /// <summary>
    /// 保存(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务, 如果为空则将重启新事务</param>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    /// <param name="firstTransactionData">参与事务处理前端的业务队列</param>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的业务对象</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
    IBusiness Save(DbTransaction transaction, bool needCheckDirty, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData);

    /// <summary>
    /// 删除本对象数据之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="limitingCriteriaExpressions">限制保存的条件(not exists 条件语句)</param>
    void OnDeletingSelf(DbTransaction transaction, ref List<CriteriaExpression> limitingCriteriaExpressions);

    /// <summary>
    /// 删除本对象数据之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    void OnDeletedSelf(DbTransaction transaction);

    #endregion
  }
}