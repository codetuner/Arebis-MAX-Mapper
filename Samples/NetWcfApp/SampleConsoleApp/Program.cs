using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Max.Tools.DomainGenerator.Model;
using Max.Tools.DomainGenerator;
using System.IO;
using Microsoft.VisualStudio.TextTemplating;
using Sample.Contracts;

namespace SampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine(typeof(System.Data.Objects.DataClasses.IEntityWithKey).Assembly.FullName);
            Environment.Exit(0);

            {
                var srv = new Sample.WcfService.SampleService() as ISampleService;

                foreach (var item in srv.ListTopManagers())
                {
                    Console.WriteLine("{0}: {1}", item.Id, item.Contact.LastName);
                }
                Console.WriteLine("-");

                foreach (var item in srv.ListTopManagers2())
                {
                    Console.WriteLine("{0}: {1}", item.Id, item.Contact.LastName);
                }
                Console.WriteLine("-");
            }

            //Console.WriteLine(typeof(Max.Tools.DomainGenerator.DomainGeneratorSession).Assembly.FullName);
            //Console.WriteLine(typeof(Max.Domain.Mapping.Mapper).Assembly.FullName);
            //Console.WriteLine(typeof(Max.Domain.Mapping.Entity.EntityDataModelObjectSource).Assembly.FullName);
            
            Environment.Exit(0);


            var dte = (EnvDTE.DTE)Marshal.GetActiveObject("VisualStudio.DTE");
            var pi = dte.Solution.FindProjectItem(@"T:\Projects\Test\201104 MappingGenerator\Sample.WcfService\MapperGenerator.tt");
            var proj = pi.ContainingProject;
            //Console.WriteLine(dte.Solution.FullName);
            //foreach (EnvDTE.Property item in pi.Properties.OfType<EnvDTE.Property>().OrderBy(p => p.Name))
            //{
            //    Console.WriteLine("- {0}={1}", item.Name, item.Value);
            //}

            //var ds = new DomainGeneratorSession(null, new DummyHost());
            //ds.MappingDefinition = @"T:\Projects\Test\201104 MappingGenerator\Sample.Contracts\ContractsMapping.xml";
            //foreach (var cl in ds.Mapping.Classes)
            //{
            //    Console.WriteLine("[] {0}", cl.ClassName);
            //    foreach (var pr in cl.InheritedProperties)
            //    {
            //        Console.WriteLine("   - {0}", pr.Name);
            //    }
            //}

            //foreach (var item in proj.ListAllPhysicalProjectItems())
            //{
            //    Console.WriteLine("+ {0}", item.Name);
            //    Console.WriteLine("  {0}", item.FileNames[0]);
            //}

            //return;


            //var pix = dte.Solution.FindProjectItem(@"T:\Projects\Test\201104 MappingGenerator\Files\Max Empty SubTemplate\__TemplateIcon.ico");
            //if (pix == null)
            //    Console.WriteLine("No project item found");
            //else
            //    Console.WriteLine("Project Item Found: " + pix.GetVirtualPath());

            foreach (EnvDTE.ProjectItem item in proj.ProjectItems)
            {
                if (item.Kind != "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}")
                {
                    Console.WriteLine("# {1}: {0}", item.GetFullPath(), item.Kind);
                    foreach (EnvDTE.ProjectItem item2 in item.ProjectItems)
                    {
                        if (item2.Kind != "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}")
                        {
                            Console.WriteLine("    # {1}: {0}", item2.GetFullPath(), item2.Kind);
                            continue;
                        }
                        //if (!Convert.ToBoolean(item.Properties.Item("IsLink").Value)) continue;
                        Console.WriteLine("    - {1}: {0}", item2.GetVirtualPath(), item2.Kind);
                        Console.WriteLine("      {0}", item2.GetFullPath());
                        //Console.WriteLine("           {0}", (item2.Collection.Parent as EnvDTE.Project).FullName);
                        foreach (EnvDTE.Property prop in item2.Properties)
                            Console.WriteLine("           {0}={1}", prop.Name, prop.Value);
                        Console.WriteLine("           {0}", item2.Properties.Item("CustomTool").Value);
                        foreach (EnvDTE.ProjectItem subitem in item2.ProjectItems)
                            Console.WriteLine("      - {1}: {0}", subitem.GetFullPath(), item2.Kind);
                    }
                    continue;
                }
                //if (!Convert.ToBoolean(item.Properties.Item("IsLink").Value)) continue;
                Console.WriteLine("- {1}: {0}", item.GetVirtualPath(), item.Kind);
                Console.WriteLine("  {0}", item.GetFullPath());
                Console.WriteLine("       {0}", (item.Collection.Parent as EnvDTE.Project).FullName);
                foreach (EnvDTE.Property prop in item.Properties)
                    Console.WriteLine("       {0}={1}", prop.Name, prop.Value);
                Console.WriteLine("       {0}", item.Properties.Item("CustomTool").Value);
                foreach(EnvDTE.ProjectItem subitem in item.ProjectItems)
                    Console.WriteLine("  - {1}: {0}", subitem.GetFullPath(), item.Kind);
            }




        }
    }

    class DummyHost : IServiceProvider, ITextTemplatingEngineHost
    {
        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(EnvDTE.DTE))
                return Marshal.GetActiveObject("VisualStudio.DTE");
            else
                return null;
        }

        #endregion

        #region ITextTemplatingEngineHost Members

        public object GetHostOption(string optionName)
        {
            throw new NotImplementedException();
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            throw new NotImplementedException();
        }

        public void LogErrors(System.CodeDom.Compiler.CompilerErrorCollection errors)
        {
            throw new NotImplementedException();
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            throw new NotImplementedException();
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            throw new NotImplementedException();
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            throw new NotImplementedException();
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            throw new NotImplementedException();
        }

        public string ResolvePath(string path)
        {
            throw new NotImplementedException();
        }

        public void SetFileExtension(string extension)
        {
            throw new NotImplementedException();
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            throw new NotImplementedException();
        }

        public IList<string> StandardAssemblyReferences
        {
            get { throw new NotImplementedException(); }
        }

        public IList<string> StandardImports
        {
            get { throw new NotImplementedException(); }
        }

        public string TemplateFile
        {
            get { return @"T:\Projects\Test\201104 MappingGenerator\Sample.Contracts\ContractsGenerator.tt"; }
        }

        #endregion
    }
}
