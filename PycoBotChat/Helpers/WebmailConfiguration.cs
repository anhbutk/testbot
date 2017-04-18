using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PycoBotChat.Helpers
{
    public class WebmailConfiguration : ConfigurationSection
    {
        private static WebmailConfiguration settings = ConfigurationManager.GetSection("dicewebmail") as WebmailConfiguration;

        public static WebmailConfiguration Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("HostNameUri", IsRequired = true)]
        public string HostNameUri
        {
            get { return (string)this["HostNameUri"]; }
            set { this["HostNameUri"] = value; }
        }

        [ConfigurationProperty("AuthenticationKey", IsRequired = true)]
        public string AuthenticationKey
        {
            get { return (string)this["AuthenticationKey"]; }
            set { this["AuthenticationKey"] = value; }
        }

        [ConfigurationProperty("DomainName", IsRequired = true)]
        public string DomainName
        {
            get { return (string)this["DomainName"]; }
            set { this["DomainName"] = value; }
        }

        [ConfigurationProperty("From", IsRequired = false)]
        public string From
        {
            get { return (string)this["From"]; }
            set { this["From"] = value; }
        }
    }
}