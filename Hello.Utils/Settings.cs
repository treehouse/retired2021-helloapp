using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Hello.Utils
{
    public static class Settings
    {
        private static ISettingsImpl _settings;

        public static ISettingsImpl SettingsImplementation
        {
            get
            {
                if (_settings == null)
                    _settings = new SettingsImpl();
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        public static string TwitterBotUsername
        {
            get { return SettingsImplementation.Get("TwitterBotUsername"); }
        }

        public static string TwitterBotPassword
        {
            get { return SettingsImplementation.Get("TwitterBotPassword"); }
        }

        public static string TwitterHashTag
        {
            get { return SettingsImplementation.Get("TwitterHashTag"); }
        }

        public static string DefaultImageURL
        {
            get { return SettingsImplementation.Get("DefaultImageURL"); }
        }

        public static string ConnectionString
        {
            get { return SettingsImplementation.Get("ConnectionString"); }
        }
    }

    public class SettingsImpl : ISettingsImpl
    {
        public string Get(string value)
        {
            if (value == "ConnectionString")
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            return ConfigurationManager.AppSettings[value];
        }
    }

    public interface ISettingsImpl
    {
        string Get(string value);
    }
}
