﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NUnitBenchmarker.UIClient.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NUnitBenchmarker.UIClient.Properties.Resources", typeof(Resources).Assembly);
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
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome to the loopback: {0}.
        /// </summary>
        internal static string UI_Communication_welcome_to_the_loopback {
            get {
                return ResourceManager.GetString("UI_Communication_welcome_to_the_loopback", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not start or contact UI process when trying to send message: {0}.
        /// </summary>
        internal static string UI_Message_can_not_start_or_contact_ui_process_when_trying_to_send_message {
            get {
                return ResourceManager.GetString("UI_Message_can_not_start_or_contact_ui_process_when_trying_to_send_message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not start UI process..
        /// </summary>
        internal static string UI_Message_can_not_start_ui_process {
            get {
                return ResourceManager.GetString("UI_Message_can_not_start_ui_process", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UI process successfully started..
        /// </summary>
        internal static string UI_Message_start_ui_process_successfully_started {
            get {
                return ResourceManager.GetString("UI_Message_start_ui_process_successfully_started", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Starting UI process....
        /// </summary>
        internal static string UI_Message_starting_ui_process {
            get {
                return ResourceManager.GetString("UI_Message_starting_ui_process", resourceCulture);
            }
        }
    }
}
