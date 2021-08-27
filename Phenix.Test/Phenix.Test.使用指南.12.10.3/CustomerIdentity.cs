using System;

namespace Phenix.Test.使用指南._12._10._3
{
    /// <summary>
    /// 客户身份类别
    /// </summary>
    [Serializable]
    public class CustomerIdentity : CustomerIdentity<CustomerIdentity>
    {
    }

    /// <summary>
    /// 客户身份类别清单
    /// </summary> 
    [Serializable]
    public class CustomerIdentityList : Phenix.Business.BusinessListBase<CustomerIdentityList, CustomerIdentity>
    {
    }

    /// <summary>
    /// 客户身份类别
    /// </summary>
    [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("客户身份类别"), Phenix.Core.Mapping.Class("CSR_CUSTOMER_IDENTITY", FriendlyName = "客户身份类别")]
    public abstract class CustomerIdentity<T> : Phenix.Business.BusinessBase<T> where T : CustomerIdentity<T>
    {

        /// <summary>
        /// 客户身份类别
        /// </summary>
        public static readonly Phenix.Business.PropertyInfo<Identity?> IdentityProperty = RegisterProperty<Identity?>(c => c.Identity);

        [Phenix.Core.Mapping.Field(PropertyName = "Identity", FriendlyName = "客户身份类别", Alias = "CCI_CUSTOMER_IDENTITY_FG", TableName = "CSR_CUSTOMER_IDENTITY", ColumnName = "CCI_CUSTOMER_IDENTITY_FG", IsPrimaryKey = true, NeedUpdate = true)]
        private Identity? _identity;

        /// <summary>
        /// 客户身份类别
        /// </summary>
        [System.ComponentModel.DisplayName("客户身份类别")]
        public Identity? Identity
        {
            get { return GetProperty(IdentityProperty, _identity); }
            set { SetProperty(IdentityProperty, ref _identity, value); }
        }

        /// <summary>
        /// 客户身份类别
        /// </summary>
        [System.ComponentModel.DisplayName("客户身份类别")]
        public string IdentityCaption
        {
            get { return Phenix.Core.Rule.EnumKeyCaption.GetCaption(_identity); }
        }

        /// <summary>
        /// 客户信息
        /// </summary>
        public static readonly Phenix.Business.PropertyInfo<long?> CCI_CTI_IDProperty = RegisterProperty<long?>(c => c.CCI_CTI_ID);

        [Phenix.Core.Mapping.Field(PropertyName = "CCI_CTI_ID", FriendlyName = "客户信息", TableName = "CSR_CUSTOMER_IDENTITY", ColumnName = "CCI_CTI_ID", IsPrimaryKey = true, NeedUpdate = true)]
        private long? _CCI_CTI_ID;

        /// <summary>
        /// 客户信息
        /// </summary>
        [System.ComponentModel.DisplayName("客户信息")]
        public long? CCI_CTI_ID
        {
            get { return GetProperty(CCI_CTI_IDProperty, _CCI_CTI_ID); }
            set { SetProperty(CCI_CTI_IDProperty, ref _CCI_CTI_ID, value); }
        }

        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
        public override string PrimaryKey
        {
            get { return String.Format("{0},{1},", Identity, CCI_CTI_ID); }
        }
    }
}