using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Http.Dispatcher;
using Phenix.Core;

namespace Phenix.Services.Host.Core
{
    internal class PluginAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            string value = AppSettings.ReadValue(MainForm.PLUGINS_KEY, false, true);
            if (!String.IsNullOrEmpty(value))
                foreach (string s in value.Split(new Char[] {AppConfig.VALUE_SEPARATOR}, StringSplitOptions.RemoveEmptyEntries))
                {
                    string fileName = Path.GetFileName(s);
                    if (String.IsNullOrEmpty(fileName) || !File.Exists(s))
                        continue;
                    string assemblyPath = Path.Combine(AppConfig.BaseDirectory, fileName);
                    if (String.Compare(s, fileName, StringComparison.OrdinalIgnoreCase) != 0 &&
                        String.Compare(s, assemblyPath, StringComparison.OrdinalIgnoreCase) != 0)
                        File.Copy(s, assemblyPath, true);
                    AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(fileName));
                }

            return base.GetAssemblies();
        }
    }
}
