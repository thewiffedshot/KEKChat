﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KEKChat {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class language_strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal language_strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KEKChat.language_strings", typeof(language_strings).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Повтори парола.
        /// </summary>
        public static string ConfirmPasswordFieldLabel {
            get {
                return ResourceManager.GetString("ConfirmPasswordFieldLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to И-мейл.
        /// </summary>
        public static string EmailFieldLabel {
            get {
                return ResourceManager.GetString("EmailFieldLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to И-мейла може да съдържа само (А-Z) (a-z) (0-9) ( . ) ( _ )..
        /// </summary>
        public static string EmailInvalidError {
            get {
                return ResourceManager.GetString("EmailInvalidError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Паролите не съвпадат..
        /// </summary>
        public static string PasswordConfirmationError {
            get {
                return ResourceManager.GetString("PasswordConfirmationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Парола.
        /// </summary>
        public static string PasswordFieldLabel {
            get {
                return ResourceManager.GetString("PasswordFieldLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Паролата трябва да е поне 6 символа..
        /// </summary>
        public static string PasswordLengthError {
            get {
                return ResourceManager.GetString("PasswordLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Полето е задължително..
        /// </summary>
        public static string RequiredFieldError {
            get {
                return ResourceManager.GetString("RequiredFieldError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Псевдоним.
        /// </summary>
        public static string UsernameFieldLabel {
            get {
                return ResourceManager.GetString("UsernameFieldLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Името може да съдържа само (А-Z) (a-z) (0-9) ( _ ) и не може да започва с цифра..
        /// </summary>
        public static string UsernameInvalidError {
            get {
                return ResourceManager.GetString("UsernameInvalidError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Името трябва да е между 4 и 16 символа..
        /// </summary>
        public static string UsernameLengthError {
            get {
                return ResourceManager.GetString("UsernameLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Името вече е заето..
        /// </summary>
        public static string UsernameTakenError {
            get {
                return ResourceManager.GetString("UsernameTakenError", resourceCulture);
            }
        }
    }
}
