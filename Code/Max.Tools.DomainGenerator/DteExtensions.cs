using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using System.IO;

namespace Max.Tools.DomainGenerator
{
    public static class DteExtensions
    {
        private const string PhysicalFileProjectItemKind = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";

        /// <summary>
        /// Gets the project containing the given file.
        /// </summary>
        public static Project GetProjectContaining(this DTE dte, string file)
        {
            return dte.Solution.FindProjectItem(file).ContainingProject;
        }

        /// <summary>
        /// Name of the physical file.
        /// </summary>
        public static string GetFileName(this ProjectItem item)
        {
            return (string)item.Properties.Item("FileName").Value;
        }

        /// <summary>
        /// Extension of the physical file (including dot).
        /// </summary>
        public static string GetFileNameExtension(this ProjectItem item)
        {
            return (string)item.Properties.Item("Extension").Value;
        }

        /// <summary>
        /// Full, normalized path of the physical file.
        /// </summary>
        public static string GetFullPath(this ProjectItem item)
        {
            return Path.GetFullPath((string)item.Properties.Item("FullPath").Value);
        }

        /// <summary>
        /// Gets the item path relative to the project item. For linked files, this is the path of the link, not the file.
        /// </summary>
        public static string GetVirtualPath(this ProjectItem item)
        {
            var sb = new StringBuilder();
            while (item != null)
            {
                sb.Insert(0, item.Name);
                sb.Insert(0, "\\");
                item = item.Collection.Parent as ProjectItem;
            }

            return sb.ToString().Substring(1);
        }

        /// <summary>
        /// Whether the item is a linked item.
        /// </summary>
        public static bool IsLinkedItem(this ProjectItem item)
        {
            return Convert.ToBoolean(item.Properties.Item("IsLink").Value);
        }

        /// <summary>
        /// Searches the project for an item with the given path. Returns null of not found.
        /// </summary>
        public static ProjectItem FindProjectItem(this Project project, string itempath)
        {
            // Search for item with the same physical path:
            var itemfullpath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(project.FullName), itempath));
            foreach (var item in project.ListAllPhysicalProjectItems())
                if (itemfullpath.Equals(item.GetFullPath(), StringComparison.InvariantCultureIgnoreCase))
                    return item;

            // Return null if not found:
            return null;
        }

        public static VSLangProj.References GetReferences(this Project project)
        {
            return ((VSLangProj.VSProject)project.Object).References;
        }

        public static IEnumerable<Project> GetReferencedProjects(this Project project)
        {
            foreach (VSLangProj.Reference refr in ((VSLangProj.VSProject)project.Object).References)
            {
                if (refr.SourceProject != null)
                    yield return refr.SourceProject;
            }
        }

        /// <summary>
        /// Full directory path of the project.
        /// </summary>
        public static string GetProjectDir(this Project project)
        {
            return Path.GetDirectoryName(project.FullName);
        }

        /// <summary>
        /// Lists all 'real' projects, not solution folders or solution items.
        /// </summary>
        // http://weblogs.asp.net/soever/archive/2007/02/20/enumerating-projects-in-a-visual-studio-solution.aspx
        public static IEnumerable<Project> ListAllProjects(this EnvDTE.Solution solution)
        {
            Queue<Project> toNav = new Queue<Project>();

            // Collect all root projects:
            foreach (Project item in solution.Projects)
                toNav.Enqueue(item);

            // List projects:
            while (toNav.Count > 0)
            {
                var project = toNav.Dequeue();
                if (project.ConfigurationManager != null)
                {
                    yield return project;
                }
                else
                {
                    if (project.ProjectItems != null)
                        foreach (ProjectItem item in project.ProjectItems)
                            if (item.SubProject != null)
                                toNav.Enqueue(item.SubProject);
                }
            }
        }

        /// <summary>
        /// Lists all project items that represent physical files in the given project.
        /// </summary>
        public static IEnumerable<ProjectItem> ListAllPhysicalProjectItems(this EnvDTE.Project project)
        {
            Queue<ProjectItem> toNav = new Queue<ProjectItem>();
            foreach (EnvDTE.ProjectItem item in project.ProjectItems)
                toNav.Enqueue(item);

            while (toNav.Count > 0)
            {
                var item = toNav.Dequeue();

                if (item.Kind == PhysicalFileProjectItemKind)
                    yield return item;

                foreach (EnvDTE.ProjectItem subitem in item.ProjectItems)
                    toNav.Enqueue(subitem);
            }
        }
    }
}
