using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Alexr03.Common
{
    public class Utilities
    {
        public static readonly JsonSerializerSettings NoErrorJsonSettings = new JsonSerializerSettings
        {
            Error = (sender, args) => { args.ErrorContext.Handled = true; }
        };

        private static bool _hasRunIsRunningOnTcAdmin;
        private static bool _isRunningOnTcAdmin;
        public static bool IsRunningOnTcAdmin
        {
            get
            {
                if (_hasRunIsRunningOnTcAdmin)
                {
                    return _isRunningOnTcAdmin;
                }
                
                _hasRunIsRunningOnTcAdmin = true;
                try
                {
                    var databaseProvider = global::TCAdmin.SDK.Database.DatabaseManager.CreateDatabaseManager(true);
                    databaseProvider.Disconnect();
                    _isRunningOnTcAdmin = true;
                    return true;
                }
                catch
                {
                    _isRunningOnTcAdmin = false;
                    return false;
                }
            }
        }
        
        public static bool IsRunningOn(ERunningOn runningOn)
        {
            switch (runningOn)
            {
                case ERunningOn.Iis:
                    return Process.GetCurrentProcess().ProcessName.Contains("w3wp");
                case ERunningOn.IisExpress:
                    return Process.GetCurrentProcess().ProcessName.Contains("iisexpress");
                default:
                    throw new ArgumentOutOfRangeException(nameof(runningOn), runningOn, null);
            }
        }
    }
    
    public enum ERunningOn
    {
        Iis,
        IisExpress,
    }
}