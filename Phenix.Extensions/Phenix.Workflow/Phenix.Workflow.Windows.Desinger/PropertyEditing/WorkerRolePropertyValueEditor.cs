using System;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.PropertyEditing;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using Phenix.Core.Security.Windows;
using Phenix.Core.Workflow;

namespace Phenix.Workflow.Windows.Desinger.PropertyEditing
{
  internal class WorkerRolePropertyValueEditor : DialogPropertyValueEditor
  {
    #region 方法

    public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
    {
      propertyValue.Value = RoleSelectDialog.Execute(propertyValue.Value as string);
    }

    public static void AddAttributeTable(Type activityType)
    {
      if (typeof(IJointActivity).IsAssignableFrom(activityType))
      {
        PropertyInfo propertyInfo = activityType.GetProperty("WorkerRole");
        AttributeTableBuilder builder = new AttributeTableBuilder();
        builder.AddCustomAttributes(activityType, propertyInfo, new DescriptionAttribute("承担任务的操作角色"));
        builder.AddCustomAttributes(activityType, propertyInfo, new CategoryAttribute("Phenix"));
        builder.AddCustomAttributes(activityType, propertyInfo, new EditorAttribute(typeof(WorkerRolePropertyValueEditor), typeof(System.Activities.Presentation.PropertyEditing.DialogPropertyValueEditor)));
        MetadataStore.AddAttributeTable(builder.CreateTable());
      }
    }

    #endregion
  }
}