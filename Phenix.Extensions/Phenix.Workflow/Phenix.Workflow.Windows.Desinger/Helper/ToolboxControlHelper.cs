using System;
using System.Activities.Presentation.Toolbox;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Phenix.Core;
using Phenix.Core.Reflection;
using Phenix.Core.Workflow;
using Phenix.Workflow.Windows.Desinger.PropertyEditing;

namespace Phenix.Workflow.Windows.Desinger.Helper
{
  internal class ToolboxControlHelper
  {
    public static ToolboxControl BuildToolboxControl()
    {
      ToolboxControl result = new ToolboxControl();
      AddToolboxItemWrapperFromBaseDirectory(result);
      AddSystemActivities(result);
      return result;
    }

    public static ToolboxItemWrapper AddToolboxItemWrapper(ToolboxControl toolboxControl, Type toolType)
    {
      PluginAssemblyNamePropertyValueEditor.AddAttributeTable(toolType);
      WorkerRolePropertyValueEditor.AddAttributeTable(toolType);

      AssemblyDescriptionAttribute assemblyDescriptionAttribute = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(toolType.Assembly, typeof(AssemblyDescriptionAttribute));
      string categoryName = assemblyDescriptionAttribute != null
        ? !String.IsNullOrEmpty(assemblyDescriptionAttribute.Description)
            ? assemblyDescriptionAttribute.Description
            : toolType.Namespace
        : toolType.Namespace;
      ToolboxCategory category = toolboxControl.Categories.FirstOrDefault(p => p.CategoryName == categoryName);
      if (category == null)
      {
        category = new ToolboxCategory(categoryName);
        toolboxControl.Categories.Add(category);
      }
      DescriptionAttribute descriptionAttribute = AppUtilities.GetFirstCustomAttribute<DescriptionAttribute>(toolType);
      ToolboxItemWrapper result = new ToolboxItemWrapper(toolType, descriptionAttribute != null ? descriptionAttribute.Description  : toolType.Name);
      category.Add(result);
      return result;
    }

    public static void AddToolboxItemWrapper(ToolboxControl toolboxControl, string fileName)
    {
      foreach (Type item in Utilities.LoadExportedSubclassTypes(fileName, false, typeof(IActivity)))
        AddToolboxItemWrapper(toolboxControl, item);
    }

    public static void AddToolboxItemWrapperFromBaseDirectory(ToolboxControl toolboxControl)
    {
      foreach (Type item in Utilities.LoadExportedSubclassTypesFromBaseDirectory(false, typeof(IActivity)))
        AddToolboxItemWrapper(toolboxControl, item);
    }

    private static void AddSystemActivities(ToolboxControl toolBox)
    {
      ToolboxCategory category = new ToolboxCategory("控制流");
      toolBox.Categories.Add(category);
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Sequence), "Sequence"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.If), "If"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Switch<>), "Switch<>"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.DoWhile), "DoWhile"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.While), "While"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.ForEach<>), "ForEach<>"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Parallel), "Parallel"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.ParallelForEach<>), "ParallelForEach<>"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Pick), "Pick"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.PickBranch), "PickBranch"));

      category = new ToolboxCategory("流程图");
      toolBox.Categories.Add(category);
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Flowchart), "Flowchart"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.FlowDecision), "FlowDecision"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.FlowSwitch<>), "FlowSwitch<>"));

      category = new ToolboxCategory("运行时");
      toolBox.Categories.Add(category);
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Persist), "Persist"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.TerminateWorkflow), "TerminateWorkflow"));

      category = new ToolboxCategory("基元");
      toolBox.Categories.Add(category);
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Assign), "Assign"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Delay), "Delay"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.InvokeAction), "InvokeAction"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.WriteLine), "WriteLine"));

      category = new ToolboxCategory("事务");
      toolBox.Categories.Add(category);
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.CancellationScope), "CancellationScope"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.CompensableActivity), "CompensableActivity"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Compensate), "Compensate"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Confirm), "Confirm"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.TransactionScope), "TransactionScope"));

      category = new ToolboxCategory("集合");
      toolBox.Categories.Add(category);
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.AddToCollection<>), "AddToCollection<>"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.ClearCollection<>), "ClearCollection<>"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.ExistsInCollection<>), "ExistsInCollection<>"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.RemoveFromCollection<>), "RemoveFromCollection<>"));

      category = new ToolboxCategory("错误处理");
      toolBox.Categories.Add(category);
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Rethrow), "Rethrow"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Throw), "Throw"));
      category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.TryCatch), "TryCatch"));
    }
  }
}
