﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RipLeech_Updater.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RipLeech_Updater.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static System.Drawing.Bitmap bg {
            get {
                object obj = ResourceManager.GetObject("bg", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap logo {
            get {
                object obj = ResourceManager.GetObject("logo", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Windows Registry Editor Version 5.00
        ///
        ///[HKEY_CLASSES_ROOT\RipLeech]
        ///@=&quot;\&quot;URL:RipLeech Protocol\&quot;&quot;
        ///&quot;URL Protocol&quot;=&quot;\&quot;\&quot;&quot;
        ///
        ///[HKEY_CLASSES_ROOT\RipLeech\DefaultIcon]
        ///@=&quot;\&quot;ripleech.exe,1\&quot;&quot;
        ///
        ///[HKEY_CLASSES_ROOT\RipLeech\shell]
        ///
        ///[HKEY_CLASSES_ROOT\RipLeech\shell\open]
        ///
        ///[HKEY_CLASSES_ROOT\RipLeech\shell\open\command]
        ///@=&quot;\&quot;C:\\Program Files\\NiCoding\\RipLeech\\RipLeech_Helper.exe\&quot; \&quot;%1\&quot;&quot;
        ///
        ///.
        /// </summary>
        internal static string protocol {
            get {
                return ResourceManager.GetString("protocol", resourceCulture);
            }
        }
        
        internal static byte[] ripleech {
            get {
                object obj = ResourceManager.GetObject("ripleech", resourceCulture);
                return ((byte[])(obj));
            }
        }
    }
}
