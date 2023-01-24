using Microsoft.Win32;
using System.Diagnostics;

namespace OposedPingService
{
    public class Settings
    {
        private static string _registryPath = @"SYSTEM\CurrentControlSet\Services\OposedPing";
        private static RegistryKey _registry = null;

        public static void Init()
        {
            _registry = Registry.LocalMachine.OpenSubKey(_registryPath, RegistryKeyPermissionCheck.ReadWriteSubTree) 
                        ?? Registry.LocalMachine.CreateSubKey(_registryPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
        }

        public static string Get(string key, string defaultValue, EventLog logger, bool allowDefaultValue = false)
        {
            var value = _registry?.GetValue(key);
            if (value == null)
            {
                Set(key, defaultValue);
                value = defaultValue;
            }

            var valueString = value.ToString();
            if (!allowDefaultValue && valueString == defaultValue)
            {
                logger?.WriteEntry($"{key} is default-value. See Windows-Registry: HKEY_LOCAL_MACHINE\\{_registryPath}", EventLogEntryType.Error);
                return null;
            }

            return valueString;
        }

        public static void Set(string key, string value)
        {
            _registry?.SetValue(key, value);
        }
    }
}