using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Remoting.Messaging;

namespace Max.Tools.DomainGenerator.Model
{
    [Serializable]
    public class MapClass
    {
        public MapClass()
        { }

        public MapClass(Mapping mapping, XmlNode definition)
        {
            try
            {
                // Set collections:
                this.Properties = new List<MapProperty>();

                // Custom settings:
                this.Settings = new CustomSettings(definition);

                // Add class to model:
                this.Mapping = mapping;
                this.Mapping.Classes.Add(this);

                // Apply definition:
                this.Definition = definition;
                this.ClassName = definition.Attributes["name"].Value;
                this.IsAbstract = Convert.ToBoolean(definition.Attributes["abstract"].ValueOr("false"));
                this.BaseType = definition.Attributes["baseType"].ValueOr(mapping.DefaultBaseClass);
                this.TypeCondition = definition.Attributes["typeCondition"].ValueOr(null);
                this.HasCustomBaseType = (definition.Attributes["baseType"] != null);
                this.SourceType = definition.Attributes["source"].Value;
                this.IsEnumeration = Session.TypeManager.IsEnumeration(this.SourceType);
                this.DataContractOptions = definition.Attributes["dataContractOptions"].ValueOr("");
                this.ReverseMapping = definition.Attributes["reverseMapping"].ValueOr("none");

                // Apply attributes:
                List<string> attributes = new List<string>();
                foreach (XmlNode attrnode in definition.SelectNodes("attribute"))
                {
                    attributes.Add(attrnode.InnerText);
                }
                this.Attributes = attributes.ToArray();

                // Apply known subtypes:
                List<string> knownSubtypes = new List<string>();
                foreach (XmlNode stnode in definition.SelectNodes("knownSubtype"))
                {
                    knownSubtypes.Add(stnode.InnerText);
                }
                this.KnownSubtypes = knownSubtypes.ToArray();

                // Apply property mappings:
                foreach (XmlNode propnode in definition.SelectNodes("map"))
                {
                    new MapProperty(this, propnode);
                }

                // Get enumeration values:
                if (this.IsEnumeration)
                {
                    this.EnumerationNames = Session.TypeManager.GetEnumerationNames(this.SourceType).ToArray();
                    this.EnumerationValues = Session.TypeManager.GetEnumerationValues(this.SourceType).ToArray();
                }
                else
                {
                    this.EnumerationNames = new string[0];
                    this.EnumerationValues = new int[0];
                }

            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            { 
                foreach(var item in ex.LoaderExceptions)
                {
                    Console.WriteLine("ReflectionTypeLoaderException : " + item.Message);
                }
                throw new ApplicationException(
                    String.Format("Error mapping class {0}", this.ClassName ?? "UnknownClass"),
                    ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    String.Format("Error mapping class {0}", this.ClassName ?? "UnknownClass"),
                    ex);
            }

        }

        public XmlNode Definition { get; private set; }

        public CustomSettings Settings { get; private set; }

        public Mapping Mapping { get; private set; }

        public string ClassName { get; private set; }

        public bool HasCustomBaseType { get; private set; }

        public string BaseType { get; private set; }

        public string SourceType { get; private set; }

        public bool IsAbstract { get; private set; }

        public bool IsEnumeration { get; private set; }

        public string[] EnumerationNames { get; private set; }

        public int[] EnumerationValues { get; private set; }

        public string DataContractOptions { get; private set; }

        public string ReverseMapping { get; private set; }

        public string TypeCondition { get; set; }

        public bool CanMap 
        {
            get { return (this.ReverseMapping != "none"); } 
        }

        public bool CanUpdate
        {
            get { return ((this.ReverseMapping == "update") || (this.ReverseMapping == "createAndUpdate")); }
        }

        public bool CanCreate
        {
            get { return ((this.ReverseMapping == "create") || (this.ReverseMapping == "createAndUpdate")); }
        }

        public string[] KnownSubtypes { get; private set; }

        public string[] Attributes { get; private set; }

        public List<MapProperty> Properties { get; private set; }

        public DomainGeneratorSession Session
        {
            get { return this.Mapping.Session; }
        }

        public MapClass BaseTypeObject
        {
            get
            {
                return this.Mapping.Classes.SingleOrDefault(c => c.ClassName == this.BaseType);
            }
        }

        public IEnumerable<MapProperty> CollectionProperties
        {
            get
            {
                return Properties.Where(p => p.IsCollection);
            }
        }

        public IEnumerable<MapProperty> ScalarProperties
        {
            get
            {
                return Properties.Where(p => !p.IsCollection);
            }
        }

        public IEnumerable<MapProperty> IdentifierProperties
        {
            get
            {
                return Properties.Where(p => p.IsIdentifier);
            }
        }

        /// <summary>
        /// All properties including inherited ones.
        /// </summary>
        public IEnumerable<MapProperty> InheritedProperties
        {
            get
            {
                var bt = this.Mapping.Classes.SingleOrDefault(c => c.ClassName == this.BaseType);
                if (bt != null)
                {
                    foreach (var item in bt.InheritedProperties)
                        yield return item;
                }

                foreach (var item in this.Properties)
                    yield return item;
            }
        }

        public bool IsSourceTypeAbstract
        {
            get { return Session.TypeManager.IsAbstract(this.SourceType); }
        }

        public string GetQualifiedEntitySetName()
        {
            return Session.TypeManager.GetEntitySetNameOf(this.SourceType) ?? String.Format("type:{0}", this.SourceType);
        }

        public IEnumerable<MapClass> GetAllSubtypes(bool concreteTypesOnly)
        {
            Stack<MapClass> input = new Stack<MapClass>(new MapClass[] { this });
            Queue<MapClass> output = new Queue<MapClass>();

            // Navigate while class hierarchy:
            while (input.Count > 0)
            {
                MapClass c = input.Pop();
                output.Enqueue(c);
                foreach (var item in c.GetDirectSubtypes(false))
                    input.Push(item);
            }

            // Remove first item which is input class itself:
            output.Dequeue();

            // Return all or concrete types only:
            foreach (var item in output)
                if ((concreteTypesOnly == false) || (item.IsAbstract == false))
                    yield return item;
        }


        public IEnumerable<MapClass> GetDirectSubtypes(bool concreteTypesOnly)
        {
            foreach (var item in this.Mapping.Classes)
            {
                if ((item.BaseType == this.ClassName)
                    && ((concreteTypesOnly == false) || (item.IsAbstract == false)))
                    yield return item;
            }
        }

        internal void Validate()
        {
            foreach (var item in this.KnownSubtypes)
                if (this.Mapping.GetClass(item) == null)
                    throw new InvalidOperationException(String.Format("Kown subtype {1} of class {0} not found.", this.ClassName, item));

            // Validate properties:
            foreach (var item in this.Properties)
                item.Validate();
        }
    }
}
