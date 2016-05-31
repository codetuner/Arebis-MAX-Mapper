using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using System.CodeDom.Compiler;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Max.Tools.DomainGenerator
{
    /// <summary>
    /// 
    /// </summary>
    /// <see href="http://msdn.microsoft.com/en-us/library/bb126579.aspx"/>
    [Serializable]
    public class GenerationHost : ITextTemplatingEngineHost
    {
        public GenerationHost(string templateFile)
        {
            this.TemplateFile = templateFile;
        }

        public string LocalNamespace { get; set; }

        public string TemplateFile { get; private set; }

        public string TemplateFileContent
        {
            get 
            {
                return File.ReadAllText(this.TemplateFile);
            }
        }

        private string fileExtensionValue = ".g.cs";
        public string FileExtension
        {
            get { return fileExtensionValue; }
        }

        private Encoding fileEncodingValue = Encoding.UTF8;
        public Encoding FileEncoding
        {
            get { return fileEncodingValue; }
        }

        private CompilerErrorCollection errorsValue;
        public CompilerErrorCollection Errors
        {
            get { return errorsValue; }
        }

        /// <summary>
        /// Standard assembly references to include.
        /// </summary>
        public IList<string> StandardAssemblyReferences
        {
            get
            {
                return new string[]
                {
                    // Path of the System assembly:
                    typeof(System.Uri).Assembly.Location,
                    this.GetType().Assembly.Location
                };
            }
        }

        /// <summary>
        /// Standard import/using statements.
        /// </summary>
        public IList<string> StandardImports
        {
            get
            {
                return new string[]
                {
                    "System"
                };
            }
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = System.String.Empty;
            location = System.String.Empty;

            if (File.Exists(requestFileName))
            {
                content = File.ReadAllText(requestFileName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public object GetHostOption(string optionName)
        {
            object returnObject;
            switch (optionName)
            {
                case "CacheAssemblies":
                    returnObject = true;
                    break;
                default:
                    returnObject = null;
                    break;
            }
            return returnObject;
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            if (assemblyReference.Contains(", Version="))
            {
                // Load assembly from the GAC:
                Debug.WriteLine(String.Format("MAX:GenerationHost: Resolving assembly: '{0}' => GAC", assemblyReference));
                return Assembly.Load(new AssemblyName(assemblyReference)).Location;
            }
            else
            {
                // Check within loaded assemblies:
                FileInfo fi = new FileInfo(assemblyReference);
                foreach (var loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (fi.Name == new FileInfo(loadedAssembly.Location).Name)
                    {
                        Debug.WriteLine(String.Format("MAX:GenerationHost: Resolving assembly: '{0}' => LOADED", assemblyReference));
                        return loadedAssembly.Location;
                    }
                }

                // Check file reference:
                string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), assemblyReference);
                if (File.Exists(candidate))
                {
                    Debug.WriteLine(String.Format("MAX:GenerationHost: Resolving assembly: '{0}' => FILE", assemblyReference));
                    return candidate;
                }

                // If we cannot do better, return the original file name:
                Debug.WriteLine(String.Format("MAX:GenerationHost: Resolving assembly: '{0}' => NOT FOUND", assemblyReference));
                return assemblyReference;
            }
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            return Type.GetType(processorName);
        }

        public string ResolvePath(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("the file name cannot be null");
            }

            //If the argument is the fully qualified path of an existing file,
            //then we are done
            //----------------------------------------------------------------
            if (File.Exists(fileName))
            {
                return fileName;
            }

            //Maybe the file is in the same folder as the text template that 
            //called the directive.
            //----------------------------------------------------------------
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            //Look more places.
            //----------------------------------------------------------------
            //More code can go here...

            //If we cannot do better, return the original file name.
            return fileName;
        }


        //If a call to a directive in a text template does not provide a value
        //for a required parameter, the directive processor can try to get it
        //from the host by calling this method.
        //This method can be called 0, 1, or more times.
        //---------------------------------------------------------------------
        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            if (directiveId == null)
            {
                throw new ArgumentNullException("the directiveId cannot be null");
            }
            if (processorName == null)
            {
                throw new ArgumentNullException("the processorName cannot be null");
            }
            if (parameterName == null)
            {
                throw new ArgumentNullException("the parameterName cannot be null");
            }

            //Code to provide "hard-coded" parameter values goes here.
            //This code depends on the directive processors this host will interact with.

            //If we cannot do better, return the empty string.
            return String.Empty;
        }

        public void SetFileExtension(string extension)
        {
            //The parameter extension has a '.' in front of it already.
            //--------------------------------------------------------
            fileExtensionValue = extension;
        }

        public void SetOutputEncoding(System.Text.Encoding encoding, bool fromOutputDirective)
        {
            fileEncodingValue = encoding;
        }

        public void LogErrors(CompilerErrorCollection errors)
        {
            errorsValue = errors;
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            return AppDomain.CurrentDomain;
        }
    }

    [Serializable]
    public class GenerationHost<TModel> : GenerationHost
    {
        public GenerationHost(string templateFileValue, TModel model)
            : base(templateFileValue)
        {
            this.Model = model;
        }

        public TModel Model { get; private set; }
    }
}
