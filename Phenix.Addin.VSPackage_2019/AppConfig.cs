using System;

namespace Phenix.Addin.VSPackage
{
  internal static class AppConfig
  {
    public static readonly Guid C_FRAMEWORK_SUBMENU_GUID = new Guid("EA2ACFA5-5468-486F-9D33-B18FB9DD4713");
    public const uint C_FRAMEWORK_SUBMENU_GROUP = 0x1100;
    public const uint C_SET_CONNECTION_INFO = 0x1120;
    public const uint C_CLEAR_DATA_DICTIONARY_CACHE = 0x1121;
    public const uint C_REFRESH_VIEW_FILE = 0x1122;
    public static readonly Guid C_FRAMEWORK_CLASS_MENU_GUID = new Guid("5AD8F0E8-A6E2-421B-AC93-83C08BB386C5");
    public const uint C_FRAMEWORK_CLASS_MENU_GROUP = 0x1010;
    public const uint C_FRAMEWORK_CLASS_MENU = 0x1011;
    public const uint C_BUILD_BUSINESS_CLASS = 0x0102;
    public const uint C_BUILD_CRITERIA_CLASS = 0x0103;
    public const uint C_BUILD_COMMAND_CLASS = 0x0104;
    public const uint C_ADD_ENUM_ATTRIBUTE = 0x0105;
    public const uint C_BRIDGING_DETAIL_PROPERTY = 0x0106;
    public static readonly Guid C_FRAMEWORK_RULE_MENU_GUID = new Guid("028921CC-B954-447E-98F5-BEDFB57DE002");
    public const uint C_FRAMEWORK_RULE_MENU_GROUP = 0x1020;
    public const uint C_FRAMEWORK_RULE_MENU = 0x1021;
    public const uint C_BUILD_COMMON_BUSINESS_RULE_CLASS = 0x0107;
    public const uint C_BUILD_OBJECT_RULE_CLASS = 0x0108;
    public const uint C_ADD_VALIDATION_RULE_ATTRIBUTE = 0x0109;
    public const uint C_BUILD_EDIT_VALIDATION_RULE_CLASS = 0x0110;
    public const uint C_BUILD_READ_AUTHORIZATION_RULE_CLASS = 0x0111;
    public const uint C_BUILD_WRITE_AUTHORIZATION_RULE_CLASS = 0x0112;
    public const uint C_BUILD_EXECUTE_AUTHORIZATION_RULE_CLASS = 0x0113;
    public static readonly Guid C_FRAMEWORK_CONFIG_MENU_GUID = new Guid("B3351075-E78D-40BB-AEFA-3CC5C85036E7");
    public const uint C_FRAMEWORK_CONFIG_MENU_GROUP = 0x1030;
    public const uint C_FRAMEWORK_CONFIG_MENU = 0x1031;
    public const uint C_MAKE_OBJECT_LOCAL_CONFIG = 0x0114;
    public const uint C_MAKE_OBJECT_PUBLIC_CONFIG = 0x0115;
    public const uint C_MAKE_CLASS_LOCAL_CONFIG = 0x0116;
    public const uint C_MAKE_CLASS_PUBLIC_CONFIG = 0x0117;
    public static readonly Guid C_FRAMEWORK_PLUGIN_MENU_GUID = new Guid("D2AAB83D-79C4-493D-9054-6EB060120EA0");
    public const uint C_FRAMEWORK_PLUGIN_MENU_GROUP = 0x1040;
    public const uint C_FRAMEWORK_PLUGIN_MENU = 0x1041;
    public const uint C_ADD_WINDOW_PLUGIN = 0x0118;
    public const uint C_BUILD_WORKFLOW_START_COMMAND_CLASS = 0x0119;
    public static readonly Guid C_FRAMEWORK_WEBAPI_MENU_GUID = new Guid("13441521-7173-4B46-9D41-256E5023F659");
    public const uint C_FRAMEWORK_WEBAPI_MENU_GROUP = 0x1050;
    public const uint C_FRAMEWORK_WEBAPI_MENU = 0x1051;
    public const uint C_BUILD_API_CONTROLLER = 0x0120;

    public static readonly Guid S_FRAMEWORK_SUBMENU_GUID = new Guid("5B535B09-B755-4F1F-BCDE-F76FC7C578F8");
    public const uint S_FRAMEWORK_SUBMENU_GROUP = 0x2100;
    public const uint S_SET_CONNECTION_INFO = 0x2120;
    public const uint S_CLEAR_DATA_DICTIONARY_CACHE = 0x2121;
    public const uint S_REFRESH_VIEW_FILES = 0x2122;
    public const uint S_DELETE_DEV_LICX = 0x2123;

    public static readonly Guid P_FRAMEWORK_SUBMENU_GUID = new Guid("D636119B-5057-46BD-B96F-33AB6558C291");
    public const uint P_FRAMEWORK_SUBMENU_GROUP = 0x3100;
    public const uint P_SET_CONNECTION_INFO = 0x3120;
    public const uint P_CLEAR_DATA_DICTIONARY_CACHE = 0x3121;
    public const uint P_REFRESH_VIEW_FILES = 0x3122;
    public const uint P_DELETE_DEV_LICX = 0x3123;

    public static readonly Guid C_TEAMWORK_SUBMENU_GUID = new Guid("A5F45733-7245-4360-89C3-086C97EF5FCC");
    public const uint C_TEAMWORK_SUBMENU_GROUP = 0x5100;
    public const uint C_WEAVE_PROBER = 0x5120;

    public static readonly Guid S_TEAMWORK_SUBMENU_GUID = new Guid("2EE40385-88EF-4C7A-A59B-C14EFA0200FA");
    public const uint S_TEAMWORK_SUBMENU_GROUP = 0x6100;
    public const uint S_WEAVE_PROBER = 0x6120;

    public static readonly Guid P_TEAMWORK_SUBMENU_GUID = new Guid("C0CC70E7-3D6C-47BF-86F0-8EF13F70FF85");
    public const uint P_TEAMWORK_SUBMENU_GROUP = 0x7100;
    public const uint P_WEAVE_PROBER = 0x7120;
  }
}