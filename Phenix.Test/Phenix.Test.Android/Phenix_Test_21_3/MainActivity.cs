using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Phenix_Test_21_3
{
  [Activity(Label = "Phenix_Test_21_3", MainLauncher = true, Icon = "@drawable/icon")]
  public class MainActivity : Activity
  {
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.Main);

      // Get our button from the layout resource,
      // and attach an event to it
      Button logOnButton = FindViewById<Button>(Resource.Id.LogOnButton);

      //处理登录按钮触发事件
      logOnButton.Click += async (sender, e) =>
      {
        EditText userNumberEditText = FindViewById<EditText>(Resource.Id.UserNumberEditText);
        EditText passwordEditText = FindViewById<EditText>(Resource.Id.PasswordEditText);

        Phenix.Web.Client.Security.UserIdentity userIdentity = new Phenix.Web.Client.Security.UserIdentity(userNumberEditText.Text, passwordEditText.Text);
        using (Phenix.Web.Client.HttpClient client = new Phenix.Web.Client.HttpClient("10.0.2.2", userIdentity))
        {
          try
          {
            bool succeed = await client.LogOnAsync();
            logOnButton.Text = string.Format("{0}!", succeed ? "成功" : "失败");
          }
          catch (Exception)
          {
            logOnButton.Text = "需事先在本机启动WebAPI服务（启动Bin.Top目录下的Phenix.Services.Host.x86/x64.exe程序）";
          }
        }
      };
    }
  }
}

