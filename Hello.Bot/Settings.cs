using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Hello.Bot
{
    public static class Settings
    {
        private static ISettingsImpl _settings;

        public static ISettingsImpl SettingsImplementation
        {
            get
            {
                Console.WriteLine(_settings == null);
                if (_settings == null)
                    _settings = new SettingsImpl();
                return _settings;
            }
            set
            {
                Console.WriteLine(_settings);
                Console.WriteLine(_settings == null);
                _settings = value;
            }
        }

        public static string TwitterBotUsername
        {
            get { return _settings.Get("TwitterBotUsername"); }
        }

        public static string TwitterBotPassword
        {
            get { return SettingsImplementation.Get("TwitterBotPassword"); }
        }

        public static string TwitterHashTag
        {
            get { return SettingsImplementation.Get("TwitterHashTag"); }
        }
    }

    public class SettingsImpl : ISettingsImpl
    {
        public string Get(string value)
        {
            return ConfigurationManager.AppSettings[value];
        }
    }

    public interface ISettingsImpl
    {
        string Get(string value);
    }
}
