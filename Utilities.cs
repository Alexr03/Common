using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alexr03.Common
{
    public class Utilities
    {
        public static readonly JsonSerializerSettings NoErrorJsonSettings = new JsonSerializerSettings
        {
            Error = (sender, args) => { args.ErrorContext.Handled = true; },
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        public static bool IsRunningOnTcAdmin
        {
            get
            {
                try
                {
                    var _ = Type.GetType("TCAdmin.SDK.Misc.Random, TCAdmin.SDK");
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        
        public static bool IsRunningOn(ERunningOn runningOn)
        {
            switch (runningOn)
            {
                case ERunningOn.IIS:
                    return Process.GetCurrentProcess().ProcessName.Contains("w3wp");
                case ERunningOn.IISExpress:
                    return Process.GetCurrentProcess().ProcessName.Contains("iisexpress");
                default:
                    throw new ArgumentOutOfRangeException(nameof(runningOn), runningOn, null);
            }
        }
    }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum ERunningOn
    {
        IIS,
        IISExpress,
    }
}