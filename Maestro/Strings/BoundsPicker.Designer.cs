﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OSGeo.MapGuide.Maestro.Strings {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class BoundsPicker {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal BoundsPicker() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("OSGeo.MapGuide.Maestro.Strings.BoundsPicker", typeof(BoundsPicker).Assembly);
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
        ///   Looks up a localized string similar to Failed to decode the current bounds,
        ///you may enter new bounds, and these will replace the current.
        ///Error message: {0}.
        /// </summary>
        internal static string BoundsDecodeError {
            get {
                return ResourceManager.GetString("BoundsDecodeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have not entered all the required information..
        /// </summary>
        internal static string IncompleBoundsError {
            get {
                return ResourceManager.GetString("IncompleBoundsError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to convert the Max X value.
        /// </summary>
        internal static string InvalidMaxXError {
            get {
                return ResourceManager.GetString("InvalidMaxXError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to convert the Max Y value.
        /// </summary>
        internal static string InvalidMaxYError {
            get {
                return ResourceManager.GetString("InvalidMaxYError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to convert the Min X value.
        /// </summary>
        internal static string InvalidMinXError {
            get {
                return ResourceManager.GetString("InvalidMinXError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to convert the Min Y value.
        /// </summary>
        internal static string InvalidMinYError {
            get {
                return ResourceManager.GetString("InvalidMinYError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing bounds tag.
        /// </summary>
        internal static string MissingBoundsError {
            get {
                return ResourceManager.GetString("MissingBoundsError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The values you have entered appears to be in local regional format, but are required in US regional format.
        ///Do you want to convert the values?.
        /// </summary>
        internal static string NumbersInRegionalError {
            get {
                return ResourceManager.GetString("NumbersInRegionalError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The coordinates you have entered appears to be invalid, do you want to use them anyway?.
        /// </summary>
        internal static string UseInvalidCoordinatesWarning {
            get {
                return ResourceManager.GetString("UseInvalidCoordinatesWarning", resourceCulture);
            }
        }
    }
}