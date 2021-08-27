using System;
using Phenix.Core.Rule;

namespace Phenix.Test.使用指南._12._10._3
{
    /// <summary>
    /// 客户信息
    /// </summary>
    [Serializable]
    public class CustomerInfo : CustomerInfo<CustomerInfo>
    {
        #region 勾选

        /// <summary>
        /// 已分配的身份类型
        /// </summary>
        public CustomerIdentityList CustomerIdentityList
        {
            get { return GetCompositionDetail<CustomerIdentityList, CustomerIdentity>(CustomerIdentityList.Fetch(false)); }
        }

        /// <summary>
        /// 供勾选的身份类型
        /// </summary>
        public CustomerIdentityList SelectableCustomerIdentityList
        {
            get { return CustomerIdentityList.CollatingSelectableList(EnumKeyCaptionCollection.Fetch<Identity>()); }
        }

        #endregion
    }

    /// <summary>
    /// 客户信息清单
    /// </summary>
    [Serializable]
    public class CustomerInfoList : Phenix.Business.BusinessListBase<CustomerInfoList, CustomerInfo>
    {
    }

    /// <summary>
    /// 客户信息
    /// </summary>
    [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("客户信息"), Phenix.Core.Mapping.ClassAttribute("CSR_CUSTOMER_INFO", FriendlyName = "客户信息")]
    public abstract class CustomerInfo<T> : Phenix.Business.BusinessBase<T> where T : CustomerInfo<T>
    {
        #region 属性
        /// <summary>
        /// CTI_ID
        /// </summary>
        public static readonly Phenix.Business.PropertyInfo<long?> CTI_IDProperty = RegisterProperty<long?>(c => c.CTI_ID);
        [Phenix.Core.Mapping.Field(FriendlyName = "CTI_ID", TableName = "CSR_CUSTOMER_INFO", ColumnName = "CTI_ID", IsPrimaryKey = true, NeedUpdate = true)]
        private long? _CTI_ID;
        /// <summary>
        /// CTI_ID
        /// </summary>
        [System.ComponentModel.DisplayName("CTI_ID")]
        public long? CTI_ID
        {
            get { return GetProperty(CTI_IDProperty, _CTI_ID); }
            internal set { SetProperty(CTI_IDProperty, ref _CTI_ID, value); }
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
        public override string PrimaryKey
        {
            get { return String.Format("{0}", CTI_ID); }
        }

        /// <summary>
        /// 代码
        /// </summary>
        public static readonly Phenix.Business.PropertyInfo<string> CodeProperty = RegisterProperty<string>(c => c.Code);
        [Phenix.Core.Mapping.FieldRuleAttribute(StringTrim = true, StringUpperCase = true, StringOnImeMode = false)]
        [Phenix.Core.Mapping.Field(FriendlyName = "代码", Alias = "CTI_CODE", TableName = "CSR_CUSTOMER_INFO", ColumnName = "CTI_CODE", NeedUpdate = true, InLookUpColumn = true, InLookUpColumnSelect = true)]
        private string _code;
        /// <summary>
        /// 代码
        /// </summary>
        [System.ComponentModel.DisplayName("代码")]
        public string Code
        {
            get { return GetProperty(CodeProperty, _code); }
            set { SetProperty(CodeProperty, ref _code, value); }
        }

        #endregion
    }
}