using System;
using System.Threading;

namespace Phenix.Core.Operate
{
    /// <summary>
    /// "键-标签"标签
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
    public sealed class KeyCaptionAttribute : Attribute
    {
        #region 属性

        private string _enFriendlyName;

        /// <summary>
        /// 指示该类的友好名
        /// </summary>
        public string EnFriendlyName
        {
            get { return _enFriendlyName; }
            set { _enFriendlyName = value; }
        }

        private string _friendlyName;

        /// <summary>
        /// 指示该类的友好名
        /// 用于提示信息中
        /// Thread.CurrentThread.CurrentCulture.Name 为非'zh-'时返回 EnFriendlyName
        /// </summary>
        public string FriendlyName
        {
            get { return Thread.CurrentThread.CurrentCulture.Name.IndexOf("zh-", StringComparison.OrdinalIgnoreCase) == 0 ? _friendlyName : _enFriendlyName; }
            set { _friendlyName = value; }
        }

        #endregion
    }
}
