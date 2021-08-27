using System.Activities.Presentation.PropertyEditing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Phenix.Workflow.Windows.Desinger.PropertyEditing
{
  internal class DialogPropertyValueEditor : System.Activities.Presentation.PropertyEditing.DialogPropertyValueEditor
  {
    public DialogPropertyValueEditor()
    {
      this.InlineEditorTemplate = new DataTemplate();

      FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(DockPanel));

      FrameworkElementFactory label = new FrameworkElementFactory(typeof(Label));
      label.SetValue(Label.ContentProperty, new Binding("Value"));
      stackPanel.AppendChild(label);

      FrameworkElementFactory editModeSwitch = new FrameworkElementFactory(typeof(EditModeSwitchButton));
      editModeSwitch.SetValue(EditModeSwitchButton.HorizontalAlignmentProperty, HorizontalAlignment.Right);
      stackPanel.AppendChild(editModeSwitch);

      this.InlineEditorTemplate.VisualTree = stackPanel;
    }
  }
}
