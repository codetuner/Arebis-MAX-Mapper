using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using System.Xml;
using Max.Tools.DomainGenerator.Model;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace Max.Tools.DomainGenerator
{
    public class DomainGeneratorSession : IDisposable
    {
        private DomainGeneratorSession()
        {
            this.HideSubtemplates = true;

            this.TemplateFile = null;

            this.CollectionTypeExpressions = new List<Regex>();
            this.CollectionTypeExpressions.Add(new Regex(@"^(?'itemType'.+)\[\]$"));
            this.CollectionTypeExpressions.Add(new Regex(@"^.*Collection\<(?'itemType'.+)\>$"));
            this.CollectionTypeExpressions.Add(new Regex(@"^.*List\<(?'itemType'.+)\>$"));
            this.CollectionTypeExpressions.Add(new Regex(@"^.*Set\<(?'itemType'.+)\>$"));
            this.TypeManager = new TypeManager(this);

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(CurrentDomain_TypeResolve);
        }

        public DomainGeneratorSession(EnvDTE.DTE dte)
            : this()
        {
            this.Dte = dte;
        }

        public DomainGeneratorSession(TextTransformation caller, ITextTemplatingEngineHost host)
            : this()
        {
            #region Check arguments
            if (host == null)
                throw new ArgumentNullException("host");
            #endregion

            this.TemplateFile = host.TemplateFile;
            IServiceProvider hostServiceProvider = (IServiceProvider)host;
            this.Dte = (EnvDTE.DTE)hostServiceProvider.GetService(typeof(EnvDTE.DTE));
        }

        public string TemplateFile { get; private set; }

        public string LocalNamespace { get; set; }

        public TypeManager TypeManager { get; private set; }

        /// <summary>
        /// List of regular expressions that match type names that represent collections.
        /// Each regular expression should contains a group (?'itemType'...) that contains the type of a collection member.
        /// </summary>
        public List<Regex> CollectionTypeExpressions { get; private set; }

        public bool HideSubtemplates { get; set; }

        public EnvDTE.DTE Dte { get; private set; }

        public Mapping Mapping { get; private set; }

        /// <summary>
        /// Loads/appends the given mapping file.
        /// </summary>
        public void LoadMapping(string mappingFileName)
        {
            using (new CurrentDirectoryScope(this.TemplateFile))
            {
                mappingFileName = Path.Combine(Environment.CurrentDirectory, mappingFileName);

                if (!File.Exists(mappingFileName))
                {
                    throw new FileNotFoundException(String.Format("Mapping definition file '{0}' not found. Edit it's reference in the '{1}' template file.", mappingFileName, this.TemplateFile));
                }

                using (new CurrentDirectoryScope(mappingFileName))
                {
                    // Load mapping definition document:
                    XmlDocument mappingDoc = new XmlDocument();
                    mappingDoc.Load(mappingFileName);
                    if (this.Mapping == null)
                        this.Mapping = new Mapping(mappingDoc.DocumentElement, this);
                    else
                        this.Mapping.AppendMapping(mappingDoc.DocumentElement);
                    Mapping.Validate();
                }
            }
        }

        public void SetMapping(Mapping mapping)
        {
            if (mapping == null)
            {
                this.Mapping = null;
            }
            else 
            {
                this.Mapping = mapping;
                this.Mapping.Session = this;
                this.Mapping.Validate();
            }
         }

        /// <summary>
        /// Generate all templates with given CustomTool name within the same project as the current template.
        /// </summary>
        public void AutoGenerateLocalTemplates(string customToolName)
        {
            // Get project of current template:
            var masterProject = this.Dte.Solution.FindProjectItem(this.TemplateFile).ContainingProject;

            // Generate all templates with the given toolname:
            foreach (var template in masterProject.ListAllPhysicalProjectItems()
                .Where(pi => (string)pi.Properties.Item("CustomTool").Value == customToolName))
            {
                this.GenerateTemplate(template);
            }
        }

        public virtual void GenerateTemplate(EnvDTE.ProjectItem templateItem)
        {
            var masterProjectItem = this.Dte.Solution.FindProjectItem(this.TemplateFile);
            string fullOutputPath;

            if (templateItem.IsLinkedItem())
                fullOutputPath = Path.Combine(templateItem.ContainingProject.GetProjectDir(), templateItem.GetVirtualPath());
            else
                fullOutputPath = templateItem.GetFullPath();

            fullOutputPath = fullOutputPath.Replace(templateItem.GetFileNameExtension(), ".{ext}");
            this.GenerateTemplateInternal(templateItem.GetFullPath(), fullOutputPath, masterProjectItem, templateItem);
        }

        public virtual void GenerateTemplate(string templatePath)
        {
            var masterProjectItem = this.Dte.Solution.FindProjectItem(this.TemplateFile);
            var masterProject = masterProjectItem.ContainingProject;
            string fullTemplatePath = Path.Combine(Path.GetDirectoryName(masterProjectItem.GetFullPath()), templatePath);
            string fullOutputPath = fullTemplatePath.Replace(Path.GetExtension(templatePath), ".{ext}");
            var templateItem = masterProject.FindProjectItem(fullTemplatePath);
            this.GenerateTemplateInternal(fullTemplatePath, fullOutputPath, masterProjectItem, templateItem);
        }

        public virtual void GenerateTemplate(string templatePath, string outputPath)
        {
            var masterProjectItem = this.Dte.Solution.FindProjectItem(this.TemplateFile);
            var masterProject = masterProjectItem.ContainingProject;
            string fullTemplatePath = Path.Combine(Path.GetDirectoryName(masterProjectItem.GetFullPath()), templatePath);
            string fullOutputPath = Path.Combine(Path.GetDirectoryName(masterProjectItem.GetFullPath()), outputPath);
            var templateItem = masterProject.FindProjectItem(fullTemplatePath);
            this.GenerateTemplateInternal(fullTemplatePath, fullOutputPath, masterProjectItem, templateItem);
        }

        protected internal virtual void GenerateTemplateInternal(string fullTemplatePath, string fullOutputPath, EnvDTE.ProjectItem templateOwner, EnvDTE.ProjectItem outputOwner)
        {
            Debug.WriteLine(String.Format("Generating template \"{0}\"\r\nto \"{1}\".", fullTemplatePath, fullOutputPath));

            #region Auto-hide sub templates

            if ((this.HideSubtemplates) && (templateOwner != null) && (outputOwner != null))
            {
                if (outputOwner.IsLinkedItem())
                {
                    // Ignore linked items.
                }
                else if ((outputOwner.Collection.Parent is EnvDTE.ProjectItem) && ((EnvDTE.ProjectItem)outputOwner.Collection.Parent).GetFullPath().Equals(templateOwner.GetFullPath()))
                {
                    // Item already hidden.
                }
                else
                {
                    // Try to hide item:
                    try
                    {
                        Debug.WriteLine(String.Format("Hiding subtemplate \"{0}\" under \"{1}\".", fullTemplatePath, templateOwner.GetVirtualPath()));
                        templateOwner.ProjectItems.AddFromFile(fullTemplatePath);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(String.Format("Failed to hide subtemplate {0}: {1}", outputOwner.GetFullPath(), ex.Message));
                    }
                }
            }

            #endregion

            #region Generate template

            string output;

            Engine engine = new Engine();
            GenerationHost<Mapping> host = new GenerationHost<Mapping>(fullTemplatePath, this.Mapping);
            host.LocalNamespace = this.LocalNamespace;

            using (new CurrentDirectoryScope(fullTemplatePath))
            using (new CallContextScope("GenerationHost", host))
            {
                output = engine.ProcessTemplate(host.TemplateFileContent, host);
            }

            #endregion

            #region Write output

            if (host.Errors.HasErrors)
            {
                string logfilename = Environment.ExpandEnvironmentVariables(@"%TEMP%\Max.Tools.DomainGenerator.log");

                if (File.Exists(logfilename))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine();
                    sb.AppendFormat("Generation at {0} had {1} error(s):", DateTime.Now, host.Errors.Count);
                    sb.AppendLine();
                    foreach (var err in host.Errors)
                        sb.AppendLine(String.Format("- {0}", err));

                    Debug.Write(sb.ToString());
                    File.AppendAllText(logfilename, sb.ToString());
                }

                throw new ApplicationException(String.Format("Generation of template {0} caused {1} error(s), create a {3} file to have all errors logged; first error: {2}", fullTemplatePath, host.Errors.Count, host.Errors[0], logfilename));
            }
            else
            {
                fullOutputPath = fullOutputPath.Replace(".{ext}", host.FileExtension);

                WriteFile(fullTemplatePath, fullOutputPath, output, host.FileEncoding, templateOwner, outputOwner);
            }

            #endregion
        }

        public virtual void WriteFile(string templatePath, string filename, string content, Encoding encoding, EnvDTE.ProjectItem templateOwner, EnvDTE.ProjectItem outputOwner)
        {
            var fileinfo = new FileInfo(filename);

            if (!fileinfo.Directory.Exists)
                Directory.CreateDirectory(fileinfo.Directory.FullName);

            if (fileinfo.Exists)
            {
                string oldContent = File.ReadAllText(fileinfo.FullName);
                if (content.Trim() == oldContent.Trim())
                {
                    // File has not changed: ignore:
                    return;
                }
                else
                { 
                    // File has changed: checkout & overwrite:
                    CheckoutIfRequired(fileinfo.FullName);
                    Debug.WriteLine(String.Format("Overwriting file {0}", fileinfo.FullName));
                    File.WriteAllText(fileinfo.FullName, content, encoding);
                }
            }
            else
            { 
                // Create new file:
                Debug.WriteLine(String.Format("Writing new file {0}", fileinfo.FullName));
                File.WriteAllText(fileinfo.FullName, content);

                // Add new file to project:
                var masterProject = this.Dte.Solution.FindProjectItem(this.TemplateFile).ContainingProject;
                var parentProjectItems =
                    (outputOwner == null) ?
                        ((templateOwner == null) ?
                            masterProject.ProjectItems :
                            templateOwner.ProjectItems) :
                        ((outputOwner.IsLinkedItem()) ?
                            masterProject.ProjectItems :
                            outputOwner.ProjectItems);
                parentProjectItems.AddFromFile(fileinfo.FullName);
            }
        }

        protected virtual void CheckoutIfRequired(string filename)
        {
            if ((this.Dte.SourceControl != null)
                && (this.Dte.SourceControl.IsItemUnderSCC(filename))
                && (!this.Dte.SourceControl.IsItemCheckedOut(filename)))
                this.Dte.SourceControl.CheckOutItem(filename);
        }

        public virtual void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            AppDomain.CurrentDomain.TypeResolve -= new ResolveEventHandler(CurrentDomain_TypeResolve);
        }

        /// <summary>
        /// Resolve assemblies by looking to the loaded assemblies.
        /// </summary>
        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Debug.WriteLine(String.Format("AppDomain: Resolving asm: " + args.Name));
            string[] nameparts = args.Name.Split(',');
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetName().Name == nameparts[0])
                    return asm;
            }
            return null;
        }

        /// <summary>
        /// Resolve types by looking to the loaded assemblies.
        /// </summary>
        static Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            Debug.WriteLine(String.Format("AppDomain: Resolving typ: " + args.Name));
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (type.ToString() == args.Name)
                        return type.Assembly;
                }
            }
            return null;
        }
    }
}
