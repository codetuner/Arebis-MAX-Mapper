using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Max.Tools.DomainGenerator.Model
{
    [Serializable]
    public class CustomSettings
    {
        private Dictionary<string, string> innerDictionary;

        public CustomSettings()
        { }

        public CustomSettings(XmlNode definition)
        { 
            this.innerDictionary = new Dictionary<string,string>();
            foreach (XmlNode refnode in definition.SelectNodes("set"))
                this.innerDictionary[refnode.Attributes["name"].ValueOr("(no-name)")]
                    = refnode.Attributes["value"].ValueOr("null");
        }

        public string this[string name]
        {
            get 
            {
                string result;
                if (this.innerDictionary.TryGetValue(name, out result))
                    return result;
                else
                    return null;
            }
        }

        public string ValueOr(string key, string defaultValue)
        {
            return this[key] ?? defaultValue;
        }
    }
}
