
namespace Phenix.Security.Business
{
  /// <summary>
  /// 重置口令工作流启动指令
  /// </summary>
  [Phenix.Core.Workflow.WorkflowIdentityAttribute("Phenix.Security.Business.Workflow", "UserResetPasswordWorkflowStartCommand", "重置口令工作流启动指令"), System.SerializableAttribute()]
  public class UserResetPasswordWorkflowStartCommand : Phenix.Business.Workflow.StartCommandBase<UserResetPasswordWorkflowStartCommand>
  {
  }
}
