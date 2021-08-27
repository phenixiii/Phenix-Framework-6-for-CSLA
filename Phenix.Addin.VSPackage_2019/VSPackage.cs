using System;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Phenix.Addin.VSPackage
{
  [ProvideAutoLoad(UIContextGuids.CodeWindow)]
  [ProvideAutoLoad(UIContextGuids.SolutionExists)]
  [PackageRegistration(UseManagedResourcesOnly = true)]
  [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] //与VSPackage.resx保持一致
  [ProvideMenuResource("Menus.ctmenu", 1)] //与Phenix.VSPackage.csproj的<ResourceName>Menus.ctmenu</ResourceName>保持一致
  [Guid("2EC09DC7-D1A6-48DC-81A2-86754736949D")] //与source.extension.vsixmanifest的ProductID保持一致
  public sealed class VSPackage : Package
  {
    #region 属性

    private DTE2 _applicationObject;
    private DTE2 ApplicationObject
    {
      get
      {
        if (_applicationObject == null)
          _applicationObject = GetService(typeof(DTE)) as DTE2;
        return _applicationObject;
      }
    }

    private Connect _connect;
    private Connect Connect
    {
      get
      {
        if (_connect == null)
        {
          if (ApplicationObject != null)
            _connect = new Connect(ApplicationObject);
        }
        return _connect;
      }
    }

    #endregion

    #region 方法

    protected override void Initialize()
    {
      base.Initialize();

      if (GetService(typeof(IMenuCommandService)) is OleMenuCommandService menuCommandService)
      {
        OleMenuCommand command = new OleMenuCommand(SetConnectionInfoCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_SUBMENU_GUID, (int)AppConfig.C_SET_CONNECTION_INFO));
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(ClearDataDictionaryCacheCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_SUBMENU_GUID, (int)AppConfig.C_CLEAR_DATA_DICTIONARY_CACHE));
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(RefreshViewFilesCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_SUBMENU_GUID, (int)AppConfig.C_REFRESH_VIEW_FILE));
        command.BeforeQueryStatus += new System.EventHandler(RefreshViewFileCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BuildBusinessClassCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_CLASS_MENU_GUID, (int)AppConfig.C_BUILD_BUSINESS_CLASS));
        command.BeforeQueryStatus += new System.EventHandler(BuildBusinessClassCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BuildCriteriaClassCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_CLASS_MENU_GUID, (int)AppConfig.C_BUILD_CRITERIA_CLASS));
        command.BeforeQueryStatus += new System.EventHandler(BuildCriteriaClassCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BuildCommandClassCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_CLASS_MENU_GUID, (int)AppConfig.C_BUILD_COMMAND_CLASS));
        command.BeforeQueryStatus += new System.EventHandler(BuildCommandClassCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(AddEnumAttributeCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_CLASS_MENU_GUID, (int)AppConfig.C_ADD_ENUM_ATTRIBUTE));
        command.BeforeQueryStatus += new System.EventHandler(AddEnumAttributeCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BridgingDetailPropertyCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_CLASS_MENU_GUID, (int)AppConfig.C_BRIDGING_DETAIL_PROPERTY));
        command.BeforeQueryStatus += new System.EventHandler(BridgingDetailPropertyCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BuildCommonBusinessRuleClassCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_RULE_MENU_GUID, (int)AppConfig.C_BUILD_COMMON_BUSINESS_RULE_CLASS));
        command.BeforeQueryStatus += new System.EventHandler(BuildCommonBusinessRuleClassCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BuildObjectRuleClassCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_RULE_MENU_GUID, (int)AppConfig.C_BUILD_OBJECT_RULE_CLASS));
        command.BeforeQueryStatus += new System.EventHandler(BuildObjectRuleClassCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(AddValidationRuleAttributeCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_RULE_MENU_GUID, (int)AppConfig.C_ADD_VALIDATION_RULE_ATTRIBUTE));
        command.BeforeQueryStatus += new System.EventHandler(AddValidationRuleAttributeCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BuildEditValidationRuleClassCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_RULE_MENU_GUID, (int)AppConfig.C_BUILD_EDIT_VALIDATION_RULE_CLASS));
        command.BeforeQueryStatus += new System.EventHandler(BuildEditValidationRuleClassCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BuildReadAuthorizationRuleClassCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_RULE_MENU_GUID, (int)AppConfig.C_BUILD_READ_AUTHORIZATION_RULE_CLASS));
        command.BeforeQueryStatus += new System.EventHandler(BuildReadAuthorizationRuleClassCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BuildWriteAuthorizationRuleClassCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_RULE_MENU_GUID, (int)AppConfig.C_BUILD_WRITE_AUTHORIZATION_RULE_CLASS));
        command.BeforeQueryStatus += new System.EventHandler(BuildWriteAuthorizationRuleClassCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BuildExecuteAuthorizationRuleClassCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_RULE_MENU_GUID, (int)AppConfig.C_BUILD_EXECUTE_AUTHORIZATION_RULE_CLASS));
        command.BeforeQueryStatus += new System.EventHandler(BuildExecuteAuthorizationRuleClassCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(MakeObjectLocalConfigCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_CONFIG_MENU_GUID, (int)AppConfig.C_MAKE_OBJECT_LOCAL_CONFIG));
        command.BeforeQueryStatus += new System.EventHandler(MakeObjectLocalConfigCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(MakeObjectPublicConfigCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_CONFIG_MENU_GUID, (int)AppConfig.C_MAKE_OBJECT_PUBLIC_CONFIG));
        command.BeforeQueryStatus += new System.EventHandler(MakeObjectPublicConfigCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(MakeClassLocalConfigCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_CONFIG_MENU_GUID, (int)AppConfig.C_MAKE_CLASS_LOCAL_CONFIG));
        command.BeforeQueryStatus += new System.EventHandler(MakeClassLocalConfigCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(MakeClassPublicConfigCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_CONFIG_MENU_GUID, (int)AppConfig.C_MAKE_CLASS_PUBLIC_CONFIG));
        command.BeforeQueryStatus += new System.EventHandler(MakeClassPublicConfigCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(AddWindowPluginCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_PLUGIN_MENU_GUID, (int)AppConfig.C_ADD_WINDOW_PLUGIN));
        command.BeforeQueryStatus += new System.EventHandler(AddWindowPluginCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BuildWorkflowStartCommandClassCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_PLUGIN_MENU_GUID, (int)AppConfig.C_BUILD_WORKFLOW_START_COMMAND_CLASS));
        command.BeforeQueryStatus += new System.EventHandler(BuildWorkflowStartCommandClassCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(BuildApiControllerCommandCallback,
          new CommandID(AppConfig.C_FRAMEWORK_WEBAPI_MENU_GUID, (int)AppConfig.C_BUILD_API_CONTROLLER));
        command.BeforeQueryStatus += new System.EventHandler(BuildApiControllerCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(SetConnectionInfoCommandCallback,
          new CommandID(AppConfig.S_FRAMEWORK_SUBMENU_GUID, (int)AppConfig.S_SET_CONNECTION_INFO));
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(ClearDataDictionaryCacheCommandCallback,
          new CommandID(AppConfig.S_FRAMEWORK_SUBMENU_GUID, (int)AppConfig.S_CLEAR_DATA_DICTIONARY_CACHE));
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(RefreshSolutionViewFilesCommandCallback,
          new CommandID(AppConfig.S_FRAMEWORK_SUBMENU_GUID, (int)AppConfig.S_REFRESH_VIEW_FILES));
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(DeleteSolutionDevLicxCommandCallback,
          new CommandID(AppConfig.S_FRAMEWORK_SUBMENU_GUID, (int)AppConfig.S_DELETE_DEV_LICX));
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(SetConnectionInfoCommandCallback,
          new CommandID(AppConfig.P_FRAMEWORK_SUBMENU_GUID, (int)AppConfig.P_SET_CONNECTION_INFO));
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(ClearDataDictionaryCacheCommandCallback,
          new CommandID(AppConfig.P_FRAMEWORK_SUBMENU_GUID, (int)AppConfig.P_CLEAR_DATA_DICTIONARY_CACHE));
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(RefreshProjectViewFilesCommandCallback,
          new CommandID(AppConfig.P_FRAMEWORK_SUBMENU_GUID, (int)AppConfig.P_REFRESH_VIEW_FILES));
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(DeleteProjectDevLicxCommandCallback,
          new CommandID(AppConfig.P_FRAMEWORK_SUBMENU_GUID, (int)AppConfig.P_DELETE_DEV_LICX));
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(WeaveProberCommandCallback,
          new CommandID(AppConfig.C_TEAMWORK_SUBMENU_GUID, (int)AppConfig.C_WEAVE_PROBER));
        command.BeforeQueryStatus += new System.EventHandler(WeaveProberCommand_BeforeQueryStatus);
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(WeaveSolutionProberCommandCallback,
          new CommandID(AppConfig.S_TEAMWORK_SUBMENU_GUID, (int)AppConfig.S_WEAVE_PROBER));
        menuCommandService.AddCommand(command);

        command = new OleMenuCommand(WeaveProjectProberCommandCallback,
          new CommandID(AppConfig.P_TEAMWORK_SUBMENU_GUID, (int)AppConfig.P_WEAVE_PROBER));
        menuCommandService.AddCommand(command);
      }
    }

    private void SetConnectionInfoCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.SET_CONNECTION_INFO_CMD_NAME);
    }

    private void ClearDataDictionaryCacheCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.CLEAR_DATA_DICTIONARY_CACHE_CMD_NAME);
    }

    private void RefreshViewFilesCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.REFRESH_VIEW_FILES_CMD_NAME);
    }

    private void RefreshViewFileCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryRefreshViewFileCommandStatus() ?? false;
    }

    private void BuildBusinessClassCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BUILD_BUSINESS_CLASS_CMD_NAME);
    }

    private void BuildBusinessClassCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBuildBusinessClassCommandStatus() ?? false;
    }

    private void BuildCriteriaClassCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BUILD_CRITERIA_CLASS_CMD_NAME);
    }

    private void BuildCriteriaClassCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBuildCriteriaClassCommandStatus() ?? false;
    }

    private void BuildCommandClassCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BUILD_COMMAND_CLASS_CMD_NAME);
    }

    private void BuildCommandClassCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBuildCommandClassCommandStatus() ?? false;
    }

    private void AddEnumAttributeCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.ADD_ENUM_ATTRIBUTE_CMD_NAME);
    }

    private void AddEnumAttributeCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryAddEnumAttributeCommandStatus() ?? false;
    }

    private void BridgingDetailPropertyCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BRIDGING_DETAIL_PROPERTY_CMD_NAME);
    }

    private void BridgingDetailPropertyCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBridgingDetailPropertyCommandStatus() ?? false;
    }

    private void BuildCommonBusinessRuleClassCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BUILD_COMMON_BUSINESS_RULE_CLASS_CMD_NAME);
    }

    private void BuildCommonBusinessRuleClassCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBuildCommonBusinessRuleClassCommandStatus() ?? false;
    }

    private void BuildObjectRuleClassCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BUILD_OBJECT_RULE_CLASS_CMD_NAME);
    }

    private void BuildObjectRuleClassCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBuildObjectRuleClassCommandStatus() ?? false;
    }

    private void AddValidationRuleAttributeCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.ADD_VALIDATION_RULE_ATTRIBUTE_CMD_NAME);
    }

    private void AddValidationRuleAttributeCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryAddValidationRuleAttributeCommandStatus() ?? false;
    }

    private void BuildEditValidationRuleClassCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BUILD_EDIT_VALIDATION_RULE_CLASS_CMD_NAME);
    }

    private void BuildEditValidationRuleClassCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBuildEditValidationRuleClassCommandStatus() ?? false;
    }

    private void BuildReadAuthorizationRuleClassCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BUILD_READ_AUTHORIZATION_RULE_CLASS_CMD_NAME);
    }

    private void BuildReadAuthorizationRuleClassCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBuildReadAuthorizationRuleClassCommandStatus() ?? false;
    }

    private void BuildWriteAuthorizationRuleClassCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BUILD_WRITE_AUTHORIZATION_RULE_CLASS_CMD_NAME);
    }

    private void BuildWriteAuthorizationRuleClassCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBuildWriteAuthorizationRuleClassCommandStatus() ?? false;
    }

    private void BuildExecuteAuthorizationRuleClassCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BUILD_EXECUTE_AUTHORIZATION_RULE_CLASS_CMD_NAME);
    }

    private void BuildExecuteAuthorizationRuleClassCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBuildExecuteAuthorizationRuleClassCommandStatus() ?? false;
    }

    private void MakeObjectLocalConfigCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.MAKE_OBJECT_LOCAL_CONFIG_CMD_NAME);
    }

    private void MakeObjectLocalConfigCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryMakeObjectConfigCommandStatus() ?? false;
    }

    private void MakeObjectPublicConfigCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.MAKE_OBJECT_PUBLIC_CONFIG_CMD_NAME);
    }

    private void MakeObjectPublicConfigCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryMakeObjectConfigCommandStatus() ?? false;
    }

    private void MakeClassLocalConfigCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.MAKE_CLASS_LOCAL_CONFIG_CMD_NAME);
    }

    private void MakeClassLocalConfigCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryMakeClassConfigCommandStatus() ?? false;
    }

    private void MakeClassPublicConfigCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.MAKE_CLASS_PUBLIC_CONFIG_CMD_NAME);
    }

    private void MakeClassPublicConfigCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryMakeClassConfigCommandStatus() ?? false;
    }

    private void AddWindowPluginCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.ADD_WINDOW_PLUGIN_CMD_NAME);
    }

    private void AddWindowPluginCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryAddWindowPluginCommandStatus() ?? false;
    }

    private void BuildWorkflowStartCommandClassCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BUILD_WORKFLOW_START_COMMAND_CLASS_CMD_NAME);
    }

    private void BuildWorkflowStartCommandClassCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBuildWorkflowStartCommandClassCommandStatus() ?? false;
    }

    private void BuildApiControllerCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.BUILD_API_CONTROLLER_CMD_NAME);
    }

    private void BuildApiControllerCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryBuildApiControllerCommandStatus() ?? false;
    }

    private void RefreshSolutionViewFilesCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.REFRESH_SOLUTION_VIEW_FILES_CMD_NAME);
    }

    private void RefreshProjectViewFilesCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.REFRESH_PROJECT_VIEW_FILES_CMD_NAME);
    }

    private void DeleteSolutionDevLicxCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.DELETE_SOLUTION_DEV_LICX_CMD_NAME);
    }

    private void DeleteProjectDevLicxCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.DELETE_PROJECT_DEV_LICX_CMD_NAME);
    }
    
    private void WeaveProberCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.WEAVE_PROBER_CMD_NAME);
    }

    private void WeaveProberCommand_BeforeQueryStatus(object sender, EventArgs e)
    {
      ((OleMenuCommand)sender).Enabled = Connect.QueryWeaveProberCommandStatus() ?? false;
    }

    private void WeaveSolutionProberCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.WEAVE_SOLUTION_PROBER_CMD_NAME);
    }

    private void WeaveProjectProberCommandCallback(object sender, EventArgs e)
    {
      Connect.Exec(Connect.WEAVE_PROJECT_PROBER_CMD_NAME);
    }

    #endregion
  }
}
