using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Max.Tools.DomainGenerator.Model
{
    /// <summary>
    /// Manages EDM (Entity Data Model) related information.
    /// </summary>
    public class EdmManager
    {
        private Dictionary<string, string> entitySetNames = new Dictionary<string, string>();

        /// <summary>
        /// Registers EDMX files contained in the given Visual Studio Project.
        /// </summary>
        public void RegisterProject(EnvDTE.Project project)
        {
            Debug.WriteLine("EDM Searching project " + project.Name);

            // Search in all root-level project items:
            foreach (EnvDTE.ProjectItem item in project.ProjectItems)
                CollectEdmxFromProject(item);
        }

        private void CollectEdmxFromProject(EnvDTE.ProjectItem item)
        {
             // Handle EDMX files:
            if (item.FileCount > 0)
            {
                if (item.FileNames[0].EndsWith(".edmx"))
                {
                    RegisterEdmx(item.FileNames[0]);
                }
                else if (item.FileNames[0].EndsWith(".csdl"))
                {
                    RegisterCsdl(item.FileNames[0]);
                }
            }

            // Recursively search in project folders:
            foreach (EnvDTE.ProjectItem subitem in item.ProjectItems)
                CollectEdmxFromProject(subitem);
        }

        /// <summary>
        /// Registers an EDMX file.
        /// </summary>
        public void RegisterEdmx(string filename)
        {
            Debug.WriteLine("EDM Registering EDMX " + filename);

            Dictionary<String, string> edmnames = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            if (doc.DocumentElement.GetAttribute("Version") == "1.0")
                throw new InvalidOperationException(String.Format("The EDMX file {0} is still an EF 1.0 document. Please convert to EF 4.0.", filename));
            XmlNamespaceManager xmlnsmgr = new XmlNamespaceManager(doc.NameTable);
            xmlnsmgr.AddNamespace("edm", "http://schemas.microsoft.com/ado/2008/09/edm");
            xmlnsmgr.AddNamespace("edmx", "http://schemas.microsoft.com/ado/2008/10/edmx");
            foreach (XmlNode set in doc.SelectNodes("//edmx:ConceptualModels/edm:Schema/edm:EntityContainer/edm:EntitySet", xmlnsmgr))
            {
                string qualifiedEntitySetName = set.ParentNode.Attributes["Name"].Value + "." + set.Attributes["Name"].Value;
                RegisterEntitySetName(set.Attributes["EntityType"].Value, qualifiedEntitySetName);
            }
        }

        /// <summary>
        /// Registers a CSDL file.
        /// </summary>
        public void RegisterCsdl(string filename)
        {
            Debug.WriteLine("EDM Registering CSDL " + filename);

            Dictionary<String, string> edmnames = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNamespaceManager xmlnsmgr = new XmlNamespaceManager(doc.NameTable);
            xmlnsmgr.AddNamespace("edm", "http://schemas.microsoft.com/ado/2008/09/edm");
            xmlnsmgr.AddNamespace("edmx", "http://schemas.microsoft.com/ado/2008/10/edmx");
            foreach (XmlNode set in doc.SelectNodes("//edm:Schema/edm:EntityContainer/edm:EntitySet", xmlnsmgr))
            {
                string qualifiedEntitySetName = set.ParentNode.Attributes["Name"].Value + "." + set.Attributes["Name"].Value;
                RegisterEntitySetName(set.Attributes["EntityType"].Value, qualifiedEntitySetName);
            }
        }

        /// <summary>
        /// Registers concpetual model resources contained in the given assembly.
        /// </summary>
        public void RegisterAssembly(Assembly asm)
        {
            foreach (string resourceName in asm.GetManifestResourceNames().Where(n => n.EndsWith(".csdl")))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(asm.GetManifestResourceStream(resourceName));
                XmlNamespaceManager xmlnsmgr = new XmlNamespaceManager(doc.NameTable);
                if (doc.DocumentElement.GetAttribute("Version") == "1.0")
                    xmlnsmgr.AddNamespace("edm", "http://schemas.microsoft.com/ado/2006/04/edm");
                else
                    xmlnsmgr.AddNamespace("edm", "http://schemas.microsoft.com/ado/2008/09/edm");
                foreach (XmlNode set in doc.SelectNodes("//edm:EntitySet", xmlnsmgr))
                {
                    string qualifiedEntitySetName = set.ParentNode.Attributes["Name"].Value + "." + set.Attributes["Name"].Value;
                    RegisterEntitySetName(set.Attributes["EntityType"].Value, qualifiedEntitySetName);
                }
            }
        }

        /// <summary>
        /// Registers a single EntitySetName.
        /// </summary>
        /// <param name="entityTypeName">The EntityType name (NamespaceName.Name) as defined by the EdmEntityTypeAttribute or the EntityType attribute of the EntitySet element in the conceptual entity model.</param>
        /// <param name="qualifiedEntitySetName">Qualified entity set name (EntityContainerName.EntitySetName).</param>
        public void RegisterEntitySetName(string entityTypeName, string qualifiedEntitySetName)
        {
            entitySetNames[entityTypeName] = qualifiedEntitySetName;
        }

        /// <summary>
        /// Gets the qualified EntitySetName for the given entity typename.
        /// </summary>
        /// <param name="entityTypeName">The entity typename. Note that this is not the .NET type name but the 'EntityType' name as defined in the EF conceptual model.</param>
        /// <returns>The qualified EntitySetName consisting of the entity container name and the entity set name.</returns>
        public string GetQualifiedEntitySetName(string entityTypeName)
        {
            string result;
            if (entitySetNames.TryGetValue(entityTypeName, out result))
                return result;
            else
                return null;
        }

        /// <summary>
        /// Returns the entity typename for the given .NET type.
        /// </summary>
        public string GetEntityTypeNameOf(Type type)
        {
            EdmEntityTypeAttribute attr = (EdmEntityTypeAttribute)type.GetCustomAttributes(typeof(EdmEntityTypeAttribute), true).FirstOrDefault();
            if (attr != null)
                return attr.NamespaceName + "." + attr.Name;
            else
                return null;
        }
    }
}
