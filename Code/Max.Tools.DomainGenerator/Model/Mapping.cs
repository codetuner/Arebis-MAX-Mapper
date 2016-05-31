using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace Max.Tools.DomainGenerator.Model
{
    [Serializable]
    public class Mapping
    {
        public Mapping()
        { }

        public Mapping(XmlNode definition, DomainGeneratorSession session)
        {
            this.Session = session;

            // Custom settings:
            this.Settings = new CustomSettings(definition);

            // Set collections:
            this.Classes = new List<MapClass>();
            this.Namespaces = new List<Namespace>();

            // Read attributes:
            this.NamespaceUri = definition.Attributes["namespace"].ValueOr("urn:arebis.be:customers:undefined-namespace");
            this.DefaultBaseClass = definition.Attributes["defaultBaseClass"].ValueOr("System.Object");
            this.DefaultCollectionClass = definition.Attributes["defaultCollectionClass"].ValueOr("System.Collections.Generic.List<{0}>");

            // Add remainder of the mapping:
            this.AppendMapping(definition);
        }

        public void AppendMapping(XmlNode definition)
        {
            // Load references:
            foreach (XmlNode refnode in definition.SelectNodes("reference"))
            {
                if (refnode.Attributes["project"] != null)
                {
                    // Retrieve project:
                    var project = this.Session.Dte.Solution.ListAllProjects().Single(p => p.Name == refnode.Attributes["project"].Value);

                    // Register project with TypeManager:
                    Session.TypeManager.RegisterProject(project);
                }
                else if (refnode.Attributes["path"] != null)
                {
                    // Retrieve/Load assembly:
                    Assembly asm = Assembly.LoadFrom(refnode.Attributes["path"].Value);

                    // Register references with TypeManager:
                    Session.TypeManager.RegisterAssembly(asm);
                }
            }

            // Load namespaces:
            foreach (XmlNode nsnode in definition.SelectNodes("namespace"))
            {
                Namespace ns = new Namespace(nsnode.Attributes["name"].Value, nsnode.Attributes["alias"].ValueOr(null));
                this.Namespaces.Add(ns);
            }

            // Load model types:
            foreach (XmlNode refnode in definition.SelectNodes("type"))
            {
                new MapClass(this, refnode);
            }
        }

        [NonSerialized]
        private DomainGeneratorSession session;

        public DomainGeneratorSession Session
        {
            get { return session; }
            internal set { session = value; }
        }


        public CustomSettings Settings { get; private set; }

        public string NamespaceUri { get; private set; }

        public string DefaultBaseClass { get; private set; }

        public string DefaultCollectionClass { get; private set; }

        public List<MapClass> Classes { get; private set; }

        public List<Namespace> Namespaces { get; private set; }

        public MapClass GetClass(string name)
        {
            foreach (var item in this.Classes)
                if (item.ClassName == name) return item;
            return null;
        }

        internal void Validate()
        {
            foreach (var item in this.Classes)
                item.Validate();
        }
    }
}
