using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

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
            Debug.WriteLine("MAX:EdmManager: Searching project " + project.Name);

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
            // Load document:
            Debug.WriteLine(String.Format("MAX:EdmManager: RegisterEdmx(\"{0}\")", filename));
            XDocument doc = XDocument.Load(filename);

            // Check EDMX version (non-critical):
            try
            {
                if (doc.Root.Attribute("Version").Value == "1.0")
                    throw new InvalidOperationException(String.Format("The EDMX file {0} is still an EF 1.0 document. Please convert to EF 4.0.", filename));
            }
            catch { }

            // Register conceptual models:
            foreach (var conceptualModel in doc.Root.Descendants().Where(d => d.Name.LocalName == "ConceptualModels"))
            {
                RegisterConceptualModel(conceptualModel);
            }
        }

        /// <summary>
        /// Registers a CSDL file.
        /// </summary>
        public void RegisterCsdl(string filename)
        {
            Debug.WriteLine(String.Format("MAX:EdmManager: RegisterCsdl(\"{0}\")", filename));
            this.RegisterConceptualModel(XDocument.Load(filename).Root);
        }

        /// <summary>
        /// Registers concpetual model resources contained in the given assembly.
        /// </summary>
        public void RegisterAssembly(Assembly asm)
        {
            Debug.WriteLine(String.Format("MAX:EdmManager: RegisterAssembly(\"{0}\")", asm.CodeBase));
            foreach (string resourceName in asm.GetManifestResourceNames().Where(n => n.EndsWith(".csdl")))
            {
                RegisterConceptualModel(XDocument.Load(asm.GetManifestResourceStream(resourceName)).Root);
            }
        }

        /// <summary>
        /// Registers a conceptual model or conceptual model schema element.
        /// </summary>
        private void RegisterConceptualModel(XElement conceptualModel)
        {
            // Retrieve EntitySet declarations:
            var entitySetDeclarations
                = conceptualModel
                    .Descendants().Where(d => d.Name.LocalName == "EntityContainer")
                    .Descendants().Where(d => d.Name.LocalName == "EntitySet");

            // Determine and register EntitySet names:
            foreach (var entitySetDeclaration in entitySetDeclarations)
            {
                string qualifiedEntitySetName = entitySetDeclaration.Parent.Attribute("Name").Value + "." + entitySetDeclaration.Attribute("Name").Value;
                RegisterEntitySetName(entitySetDeclaration.Attribute("EntityType").Value, qualifiedEntitySetName);
            }
        }

        /// <summary>
        /// Registers a single EntitySetName.
        /// </summary>
        /// <param name="entityTypeName">The EntityType name (NamespaceName.Name) as defined by the EdmEntityTypeAttribute or the EntityType attribute of the EntitySet element in the conceptual entity model.</param>
        /// <param name="qualifiedEntitySetName">Qualified entity set name (EntityContainerName.EntitySetName).</param>
        public void RegisterEntitySetName(string entityTypeName, string qualifiedEntitySetName)
        {
            Debug.WriteLine(String.Format("MAX:EdmManager: RegisterEntitySetName(\"{0}\", \"{1}\")", entityTypeName ?? "<null>", qualifiedEntitySetName ?? "<null>"));

            entitySetNames[entityTypeName] = qualifiedEntitySetName;
        }

        /// <summary>
        /// Gets the qualified EntitySetName for the given entity typename.
        /// </summary>
        /// <param name="entityTypeName">The entity typename. Note that this is not the .NET type name but the 'EntityType' name as defined in the EF conceptual model.</param>
        /// <returns>The qualified EntitySetName consisting of the entity container name and the entity set name.</returns>
        public string GetQualifiedEntitySetName(string entityTypeName)
        {
            Debug.WriteLine(String.Format("MAX:EdmManager: GetQualifiedEntitySetName(\"{0}\")", entityTypeName ?? "<null>"));

            string result;
            if (entitySetNames.TryGetValue(entityTypeName, out result))
            {
                Debug.WriteLine(String.Format("MAX:EdmManager: GetQualifiedEntitySetName(\"{0}\") => \"{1}\"", entityTypeName ?? "<null>", result ?? "<null>"));
                return result;
            }
            else
            {
                Debug.WriteLine(String.Format("MAX:EdmManager: GetQualifiedEntitySetName(\"{0}\") => null", entityTypeName ?? "<null>"));
                return null;
            }
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
