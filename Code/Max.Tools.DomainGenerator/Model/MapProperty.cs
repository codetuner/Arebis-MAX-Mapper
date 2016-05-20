using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Remoting.Messaging;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Max.Tools.DomainGenerator.Model
{
    [Serializable]
    public class MapProperty
    {
        public MapProperty()
        { }

        public MapProperty(MapClass owner, XmlNode definition)
        {
            try
            {
                // Set owner:
                this.Owner = owner;
                this.Owner.Properties.Add(this);

                // Custom settings:
                this.Settings = new CustomSettings(definition);

                // Apply definition:
                this.Definition = definition;
                this.Name = definition.Attributes["property"].Value;
                this.name = this.Name.Substring(0, 1).ToLower() + this.Name.Substring(1);
                this.Expression = definition.Attributes["expression"].ValueOr(null);
                if (this.Expression == null)
                {
                    this.IsIdentifier = Convert.ToBoolean(definition.Attributes["identifier"].ValueOr("false"));
                    this.Updatable = Convert.ToBoolean(definition.Attributes["updatable"].ValueOr("true"));
                    this.Source = new PropertyPath(this, this.Owner.SourceType, definition.Attributes["source"].ValueOr(this.Name));
                    this.TypeName = definition.Attributes["type"].ValueOr(this.SourcePropertyTypeName);
                    this.ConversionMethod = definition.Attributes["conversion"].ValueOr("default");
                }
                else
                {
                    this.IsIdentifier = false;
                    this.Updatable = false;
                    this.Source = null;
                    this.TypeName = definition.Attributes["type"].ValueOr(null);
                    this.ConversionMethod = "default";
                }
                this.DataMemberOptions = definition.Attributes["dataMemberOptions"].ValueOr("");
                this.Modifiers = definition.Attributes["modifiers"].ValueOr("public");
                this.OnRemove = definition.Attributes["onRemove"].ValueOr("remove");

                // Apply attributes:
                List<string> attributes = new List<string>();
                foreach (XmlNode attrnode in definition.SelectNodes("attribute"))
                {
                    attributes.Add(attrnode.InnerText);
                }
                this.Attributes = attributes.ToArray();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    String.Format("Error mapping property {0}.{1}", this.Owner.ClassName ?? "UnknownClass", this.Name ?? "UnknownProperty"), 
                    ex);
            }
        }

        public DomainGeneratorSession Session
        {
            get { return this.Owner.Session; }
        }

        public CustomSettings Settings { get; private set; }

        public XmlNode Definition { get; private set; }

        public string[] Attributes { get; private set; }

        public MapClass Owner { get; private set; }

        public string Name { get; private set; }

        public string name { get; private set; }

        public string DataMemberOptions { get; private set; }

        public PropertyPath Source { get; private set; }

        public string TypeName { get; private set; }

        public string Modifiers { get; private set; }

        public string Expression { get; private set; }

        public bool Updatable { get; private set; }

        /// <summary>
        /// Conversion method (none, map, convert)
        /// </summary>
        public string ConversionMethod { get; private set; }

        public bool IsIdentifier { get; private set; }

        public string OnRemove { get; set; }

        /// <summary>
        /// Non-mapped properties not based on expressions and with a simple source.
        /// </summary>
        public bool IsSimpleProperty
        {
            get { 
                return (
                    (new String[] { "none", "cast", "convert", "custom" }.Contains(this.ConversionMethod)) 
                    || (this.Expression != null) 
                    || (this.Source.IsSimpleProperty)
                ); 
            }
        }

        public bool IsCollection 
        { 
            get {
                return (
                    ((this.ConversionMethod != "none") && (this.Expression == null) && (this.Source.IsCollection))
                );
            } 
        }

        public bool IsString
        {
            get { return (this.Source.PropertyType == "System.String"); }
        }

        public string SourcePropertyTypeName
        {
            get
            {
                return this.Source.PropertyType;
            }
        }

        internal void Validate()
        {
            if (this.ConversionMethod == "map" && this.Owner.Mapping.GetClass(this.TypeName) == null)
            {
                throw new InvalidOperationException(String.Format("Cannot map property {0}.{1} to unknown type {2}.", this.Owner.ClassName, this.Name, this.TypeName));
            }
            if (this.Expression != null && this.TypeName == null)
            {
                throw new InvalidOperationException(String.Format("Property {0}.{1} using an expression should have a type defined explicitely.", this.Owner.ClassName, this.Name));
            }
            if (!this.IsCollection && this.OnRemove != "remove")
            {
                throw new InvalidOperationException(String.Format("Property {0}.{1} cannot customize the OnRemove action as it is not a collection.", this.Owner.ClassName, this.Name));
            }
        }
    }
}
