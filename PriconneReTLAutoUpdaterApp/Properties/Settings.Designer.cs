﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PriconneReTLAutoUpdaterApp.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.8.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://api.github.com/repos/ImaterialC/PriconneRe-TL")]
        public string githubApi {
            get {
                return ((string)(this["githubApi"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <string>BepInEx/config/AutoTranslatorConfig.ini</string>
  <string>BepInEx/config/BepInEx.cfg</string>
  <string>BepInEx/config/PriconneTLFixup.cfg</string>
  <string>BepInEx/config/com.sinai.unityexplorer.cfg</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection configFiles {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["configFiles"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <string>BepInEx/Translation/en/Text/_AutoGeneratedTranslations.txt</string>
  <string>BepInEx/Translation/en/Text/_Postprocessors.txt</string>
  <string>BepInEx/Translation/en/Text/_Preprocessors.txt</string>
  <string>BepInEx/Translation/en/Text/_Substitutions.txt</string>
  <string>winhttp.dll</string>
  <string>BepInEx/plugins/PriconneTLFixup.dll</string>
  <string>BepInEx/plugins/XUnity.AutoTranslator/XUnity.AutoTranslator.Plugin.Core.dll</string>
  <string>BepInEx/plugins/UnityExplorer.BIE.Unity.IL2CPP.CoreCLR.dll</string>
  <string>BepInEx/plugins/UniverseLib.BIE.IL2CPP.Interop.dll</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection ignoreFiles {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["ignoreFiles"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsd=\"http://www.w3." +
            "org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n  <s" +
            "tring>dotnet</string>\r\n  <string>BepInEx/core</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection ignoreFolders {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["ignoreFolders"]));
            }
        }
    }
}
