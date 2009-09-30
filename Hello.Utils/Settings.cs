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
            get { return SettingsImplementation.GetString("TwitterBotUsername"); }
        }

        public static string TwitterBotPassword
        {
            get { return SettingsImplementation.GetString("TwitterBotPassword"); }
        }

        public static string TwitterHashTag
        {
            get { return SettingsImplementation.GetString("TwitterHashTag"); }
        }

        public static string DefaultImageURL
        {
            get { return SettingsImplementation.GetString("DefaultImageURL"); }
        }

        public static string ConnectionString
        {
            get { return SettingsImplementation.GetString("ConnectionString"); }
        }

        public static int MaxSearchResults
        {
            get { return SettingsImplementation.GetInt("MaxSearchResults"); }
        }

        public static int MaxMessages
        {
            get { return SettingsImplementation.GetInt("MaxMessages"); }
        }

        public static int MaxTags
        {
            get { return SettingsImplementation.GetInt("MaxTags"); }
        }

        public static Dictionary<int, string> TagSizes
        {
            get
            {
                return new Dictionary<int, string> {
                    { 0, "largest" },
                    { 1,  "larger" },
                    { 2,  "large" },
                    { 3,  "medium" },
                    { 4,  "small" },
                    { 5,  "smaller" }
                };
            }
        }

        public static string GetHeatColour(int hottness)
        {
            if (hottness >= 1000)
                return "DC3D2A";
            if (hottness >= 250)
                return "DCC12A";
            if (hottness >= 100)
                return "4FA431";
            if (hottness >= 50)
                return "319DB8";
            if (hottness >= 20)
                return "8B4591";
            return "000000";
        }

        public static string DefaultTagSize
        {
            get { return "smallest"; }
        }

        public static string EventSlugRegex
        {
            get { return @"[\w\-_]+"; }
        }

        public static PointsConfig Points
        {
            get { return new PointsConfig(); }
        }

        public class PointsConfig
        {
            public int Met
            {
                get { return SettingsImplementation.GetInt("PointsForMet"); }
            }

            public int Sat
            {
                get { return SettingsImplementation.GetInt("PointsForSat"); }
            }

            public int HiFive
            {
                get { return SettingsImplementation.GetInt("PointsForHiFive"); }
            }
        }

        public static ThresholdsConfig Thresholds
        {
            get { return new ThresholdsConfig(); }
        }

        public class ThresholdsConfig
        {
            public int Bronze
            {
                get { return SettingsImplementation.GetInt("BronzeThreshold"); }
            }

            public int Silver
            {
                get { return SettingsImplementation.GetInt("SilverThreshold"); }
            }

            public int Gold
            {
                get { return SettingsImplementation.GetInt("GoldThreshold"); }
            }

            public int Diamond
            {
                get { return SettingsImplementation.GetInt("DiamondThreshold"); }
            }

            public int Smiley
            {
                get { return SettingsImplementation.GetInt("SmileyThreshold"); }
            }
        }

        public static AdminConfig Admin
        {
            get { return new AdminConfig(); }
        }

        public class AdminConfig
        {
            public int MaxTideMarks
            {
                get { return SettingsImplementation.GetInt("AdminMaxTideMarks"); }
            }
        }
    }

    public class SettingsImpl : ISettingsImpl
    {
        public string GetString(string value)
        {
            if (value == "ConnectionString")
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            return ConfigurationManager.AppSettings[value];
        }

        public int GetInt(string value)
        {
            int result = 0;
            if (Int32.TryParse(ConfigurationManager.AppSettings[value], out result))
                return result;
            else throw new HelloException("Must specify an integer value for " + value + " in the application or web settings.");
        }
    }

    public interface ISettingsImpl
    {
        string GetString(string value);
        int GetInt(string value);
    }
}
