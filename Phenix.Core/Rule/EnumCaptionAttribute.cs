using System;
using System.Threading;

namespace Phenix.Core.Rule
{
    /// <summary>
    /// "Enum字段"标签
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumCaptionAttribute : Attribute
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="caption">标签</param>
        public EnumCaptionAttribute(string caption)
            : base()
        {
            _caption = caption;
        }

        #region 属性

        /// <summary>
        /// 键, 为 null 时采用枚举Value.ToString("d")
        /// </summary>
        public string Key { get; set; }

        private string _enCaption;

        /// <summary>
        /// 英文标签, 为 null 时采用枚举Value.ToString()
        /// </summary>
        public string EnCaption
        {
            get { return _enCaption; }
            set { _enCaption = value; }
        }

        private readonly string _caption;

        /// <summary>
        /// 标签, 为 null 时采用枚举Value.ToString()
        /// Thread.CurrentThread.CurrentCulture.Name 为非'zh-'时返回 EnCaption
        /// </summary>
        public string Caption
        {
            get { return Thread.CurrentThread.CurrentCulture.Name.IndexOf("zh-", StringComparison.OrdinalIgnoreCase) == 0 ? _caption : _enCaption; }
        }

        /// <summary>
        /// 标记
        /// </summary>
        public string Tag { get; set; }

        #endregion
    }
}