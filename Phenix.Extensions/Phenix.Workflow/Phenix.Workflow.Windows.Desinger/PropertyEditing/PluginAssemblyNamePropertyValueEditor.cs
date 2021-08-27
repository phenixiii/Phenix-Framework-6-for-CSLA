using System;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.PropertyEditing;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using Phenix.Core.Plugin;
using Phenix.Core.Plugin.Windows;
using Phenix.Core.Workflow;

namespace Phenix.Workflow.Windows.Desinger.PropertyEditing
{
  internal class PluginAssemblyNamePropertyValueEditor : DialogPropertyValueEditor
  {
    #region 方法

    public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
    {
      PluginInfo pluginInfo = PluginAssemblySelectDialog.Execute(propertyValue.Value as string);
      propertyValue.Value = pluginInfo != null ? pluginInfo.OwnerType.Assembly.GetName().Name : null;
    }

    public static void AddAttributeTable(Type activityType)
    {
      if (typeof(IJointActivity).IsAssignableFrom(activityType))
      {
        PropertyInfo propertyInfo = activityType.GetProperty("PluginAssemblyName");
        AttributeTableBuilder builder = new AttributeTableBuilder();
        builder.AddCustomAttributes(activityType, propertyInfo, new DescriptionAttribute("承担任务的插件界面"));
        builder.AddCustomAttributes(activityType, propertyInfo, new CategoryAttribute("Phenix"));
        builder.AddCustomAttributes(activityType, propertyInfo, new EditorAttribute(typeof(PluginAssemblyNamePropertyValueEditor), typeof(System.Activities.Presentation.PropertyEditing.DialogPropertyValueEditor)));
        MetadataStore.AddAttributeTable(builder.CreateTable());
      }
    }

    #endregion
  }
}