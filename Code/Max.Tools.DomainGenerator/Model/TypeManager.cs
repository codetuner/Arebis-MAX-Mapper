using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;

namespace Max.Tools.DomainGenerator.Model
{
    public class TypeManager
    {
        private Dictionary<string, object> typeInformation = new Dictionary<string, object>();
        private DomainGeneratorSession domainGeneratorSession;

        public TypeManager(DomainGeneratorSession domainGeneratorSession)
        {
            this.domainGeneratorSession = domainGeneratorSession;
            this.EdmManager = new EdmManager();
        }

        public EdmManager EdmManager { get; private set; }

        public System.Reflection.Assembly RegisterAssembly(string asmPath)
        {
            var asm = System.Reflection.Assembly.LoadFrom(asmPath);
            RegisterAssembly(asm);
            return asm;
        }

        public void RegisterAssembly(System.Reflection.Assembly asm)
        {
            foreach (var type in asm.GetExportedTypes())
                typeInformation[ToString(type)] = type;

            this.EdmManager.RegisterAssembly(asm);
        }

        public void RegisterProject(EnvDTE.Project project)
        {
            CollectProjectItems(project.ProjectItems);
            
            this.EdmManager.RegisterProject(project);
        }

        private void CollectProjectItems(EnvDTE.ProjectItems projectItems)
        {
            foreach (EnvDTE.ProjectItem item in projectItems)
            {
                if (item.FileCodeModel != null)
                    CollectTypes(item.FileCodeModel.CodeElements);

                if (item.ProjectItems != null)
                    CollectProjectItems(item.ProjectItems);
            }
        }

        private void CollectTypes(EnvDTE.CodeElements elements)
        {
            foreach (var element in elements)
            {
                if (element is EnvDTE.CodeNamespace)
                {
                    CollectTypes(((EnvDTE.CodeNamespace)element).Members);
                }
                else if (element is EnvDTE.CodeType)
                {
                    string typeName = ((EnvDTE.CodeType)element).FullName;
                    object codeTypeList;
                    if (typeInformation.TryGetValue(typeName, out codeTypeList))
                    {
                        ((List<EnvDTE.CodeType>)codeTypeList).Add((EnvDTE.CodeType)element);
                    }
                    else
                    {
                        codeTypeList = new List<EnvDTE.CodeType>();
                        ((List<EnvDTE.CodeType>)codeTypeList).Add((EnvDTE.CodeType)element);
                        typeInformation[typeName] = codeTypeList;
                    }
                }
            }
        }

        /// <summary>
        /// Whether the given property exists.
        /// </summary>
        public bool HasProperty(string ownerType, string propertyName)
        {
            if (typeInformation[ownerType] is Type)
            {
                return (((Type)typeInformation[ownerType]).GetProperty(propertyName) != null);
            }
            else if (typeInformation[ownerType] is List<EnvDTE.CodeType>)
            {
                foreach (EnvDTE.CodeType typedef in (List<EnvDTE.CodeType>)typeInformation[ownerType])
                {
                    foreach (EnvDTE.CodeElement member in typedef.Members)
                    {
                        if (member.Name == propertyName && member.Kind == EnvDTE.vsCMElement.vsCMElementProperty)
                        {
                            return true;
                        }
                    }
                }

                // Recursively call on basetype:
                return HasProperty(GetBaseTypeOf(ownerType), propertyName);
            }
            else
            {
                throw new ArgumentException("Unknown type.");
            }
        }

        /// <summary>
        /// Returns the type of the given property.
        /// </summary>
        public string GetPropertyType(string ownerType, string propertyName)
        {
            Console.WriteLine("TypeManager: GetPropertyType(\"{0}\", \"{1}\")", ownerType, propertyName);

            if (typeInformation[ownerType] is Type)
            {
                return ToString(((Type)typeInformation[ownerType]).GetProperty(propertyName).PropertyType);
            }
            else if (typeInformation[ownerType] is List<EnvDTE.CodeType>)
            {
                foreach (EnvDTE.CodeType typedef in (List<EnvDTE.CodeType>)typeInformation[ownerType])
                {
                    foreach (EnvDTE.CodeElement member in typedef.Members)
                    {
                        if (member.Name == propertyName && member.Kind == EnvDTE.vsCMElement.vsCMElementProperty)
                        {
                            var prop = (EnvDTE.CodeProperty)member;
                            if (prop.Type.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefArray)
                                return prop.Type.ElementType.AsFullName + "[]";
                            else
                                return prop.Type.AsFullName;
                        }
                    }
                }

                // Recursively call on basetype:
                return GetPropertyType(GetBaseTypeOf(ownerType), propertyName);
            }

            throw new ArgumentException("No such property found.");
        }

        /// <summary>
        /// Whether the given type is a collection type.
        /// </summary>
        public bool IsCollectionType(string type)
        {
            foreach (var ctregex in this.domainGeneratorSession.CollectionTypeExpressions)
            {
                if (ctregex.IsMatch(type))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the member type for the given collection type.
        /// </summary>
        public string GetCollectionItemType(string type)
        {
            foreach (var ctregex in this.domainGeneratorSession.CollectionTypeExpressions)
            {
                if (ctregex.IsMatch(type))
                    return ctregex.Match(type).Groups["itemType"].Value;
            }

            return null;
        }

        /// <summary>
        /// Whethet the given property has a setter method.
        /// </summary>
        public bool HasSetter(string ownerType, string propertyName)
        {
            if (typeInformation[ownerType] is Type)
            {
                return ((Type)typeInformation[ownerType]).GetProperty(propertyName).CanWrite;
            }
            else if (typeInformation[ownerType] is List<EnvDTE.CodeType>)
            {
                foreach (EnvDTE.CodeType typedef in (List<EnvDTE.CodeType>)typeInformation[ownerType])
                {
                    foreach (EnvDTE.CodeElement member in typedef.Members)
                    {
                        if (member.Name == propertyName && member.Kind == EnvDTE.vsCMElement.vsCMElementProperty)
                        {
                            var prop = (EnvDTE.CodeProperty)member;
                            return (prop.Setter != null);
                        }
                    }
                }

                // Recursively call on basetype:
                return HasSetter(GetBaseTypeOf(ownerType), propertyName);
            }

            throw new ArgumentException("No such property found.");
        }

        // Whether the given type is an Enumeration type.
        public bool IsEnumeration(string type)
        {
            if (typeInformation[type] is Type)
            {
                return ((Type)typeInformation[type]).IsEnum;
            }
            else if (typeInformation[type] is List<EnvDTE.CodeType>)
            {
                foreach (EnvDTE.CodeType typedef in (List<EnvDTE.CodeType>)typeInformation[type])
                {
                    if (typedef.Kind == EnvDTE.vsCMElement.vsCMElementEnum)
                        return true;
                }

                return false;
            }
            else
            {
                throw new ArgumentException("Unknown type.");
            }
        }

        /// <summary>
        /// Gets the base type of the given type.
        /// </summary>
        public string GetBaseTypeOf(string type)
        {
            if (typeInformation[type] is Type)
            {
                return ToString(((Type)typeInformation[type]).BaseType);
            }
            else if (typeInformation[type] is List<EnvDTE.CodeType>)
            {
                foreach (EnvDTE.CodeType typedef in (List<EnvDTE.CodeType>)typeInformation[type])
                {
                    foreach (EnvDTE.CodeElement item in typedef.Bases)
                        return item.FullName;
                }

                return null;
            }
            else
            {
                throw new ArgumentException("Unknown type.");
            }
        }

        /// <summary>
        /// Gets the EntitySetName of the given type.
        /// </summary>
        public string GetEntitySetNameOf(string type)
        {
            Debug.WriteLine(String.Format("MAX:TypeManager: GetEntitySetNameOf(\"{0}\")", type ?? "<null>"));
            if (type == null)
            {
                throw new ArgumentNullException("Could not find QualifiedEntitySetName for type <null>.");
            }
            else if (type == "System.Object")
            {
                throw new ArgumentException("Could not find QualifiedEntitySetName for given type. Ensure a valid entity type is passed and it's assembly is registered in the mapping generation definition file.");
            }
            else if (!typeInformation.ContainsKey(type))
            {
                throw new ArgumentException("No type information available for \"" + type + "\" that could help find its QualifiedEntitySetName. Ensure a valid entity type is passed and it's assembly is registered in the mapping generation definition file.");
            }
            else if (typeInformation[type] is Type)
            {
                Debug.WriteLine(String.Format("MAX:TypeManager: GetEntitySetNameOf(\"{0}\") : Is Type", type ?? "<null>"));
                return EdmManager.GetQualifiedEntitySetName(EdmManager.GetEntityTypeNameOf((Type)typeInformation[type]))
                    ?? GetEntitySetNameOf(GetBaseTypeOf(type));
            }
            else if (typeInformation[type] is List<EnvDTE.CodeType>)
            {
                Debug.WriteLine(String.Format("MAX:TypeManager: GetEntitySetNameOf(\"{0}\") : Is CodeType list", type ?? "<null>"));
                foreach (EnvDTE.CodeType typedef in (List<EnvDTE.CodeType>)typeInformation[type])
                {
                    Debug.WriteLine(String.Format("MAX:TypeManager: GetEntitySetNameOf(\"{0}\") : Is CodeType list / {1}", type ?? "<null>", typedef.Name));
                    foreach (EnvDTE.CodeAttribute attr in typedef.Attributes)
                    {
                        if (attr.FullName == "System.Data.Objects.DataClasses.EdmEntityTypeAttribute")
                        {
                            Debug.WriteLine(String.Format("MAX:TypeManager: GetEntitySetNameOf(\"{0}\") : Is CodeType list / {1} / Has EdmEntityTypeAttribute", type ?? "<null>", typedef.Name));
                            var values = new CodeAttributeValuesDictionary(attr);
                            return EdmManager.GetQualifiedEntitySetName(values["NamespaceName"] + "." + values["Name"])
                                ?? GetEntitySetNameOf(GetBaseTypeOf(type));
                        }
                    }
                }
                return GetEntitySetNameOf(GetBaseTypeOf(type));
            }
            else
            {
                throw new ArgumentException("Unknown type.");
            }
        }

        /// <summary>
        /// Lists the value names of an Enumeration type.
        /// </summary>
        public IEnumerable<string> GetEnumerationNames(string type)
        {
            if (typeInformation[type] is Type)
            {
                foreach(var item in ((Type)typeInformation[type]).GetFields(BindingFlags.Public | BindingFlags.Static).Select(f => f.Name))
                    yield return item;
            }
            else if (typeInformation[type] is List<EnvDTE.CodeType>)
            {
                foreach (EnvDTE.CodeType typedef in (List<EnvDTE.CodeType>)typeInformation[type])
                {
                    foreach (EnvDTE.CodeElement member in typedef.Members)
                    {
                        if (member.Kind == EnvDTE.vsCMElement.vsCMElementVariable)
                            yield return member.Name;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Unknown type.");
            }
        }

        /// <summary>
        /// Lists the values of an Enumeration type.
        /// </summary>
        public IEnumerable<int> GetEnumerationValues(string type)
        {
            if (typeInformation[type] is Type)
            {
                foreach (var item in ((Type)typeInformation[type]).GetFields(BindingFlags.Public | BindingFlags.Static).Select(f => (int)f.GetRawConstantValue()))
                    yield return item;
            }
            else if (typeInformation[type] is List<EnvDTE.CodeType>)
            {
                foreach (EnvDTE.CodeType typedef in (List<EnvDTE.CodeType>)typeInformation[type])
                {
                    foreach (EnvDTE.CodeElement member in typedef.Members)
                    {
                        if (member.Kind == EnvDTE.vsCMElement.vsCMElementVariable)
                        {
                            yield return Convert.ToInt32(((EnvDTE.CodeVariable)member).InitExpression);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("Unknown type.");
            }
        }

        /// <summary>
        /// Whether the given type is abstract.
        /// </summary>
        public bool IsAbstract(string type)
        {
            if (typeInformation[type] is Type)
            {
                return ((Type)typeInformation[type]).IsAbstract;
            }
            else if (typeInformation[type] is List<EnvDTE.CodeType>)
            {
                foreach (EnvDTE.CodeType typedef in (List<EnvDTE.CodeType>)typeInformation[type])
                {
                    if (typedef.Kind == EnvDTE.vsCMElement.vsCMElementClass)
                    {
                        if (((EnvDTE.CodeClass)typedef).IsAbstract)
                            return true;
                    }
                }

                return false;
            }
            else
            {
                throw new ArgumentException("Unknown type.");
            }
        }

        /// <summary>
        /// Returns the type name in a form valid for C# code.
        /// </summary>
        public static string ToString(Type type)
        {
            StringBuilder sb = new StringBuilder();
            if (type.IsGenericType)
            {
                sb.Append(type.GetGenericTypeDefinition().FullName.Substring(0, type.GetGenericTypeDefinition().FullName.IndexOf("`")));
                sb.Append('<');
                string sep = "";
                foreach (var tp in type.GetGenericArguments())
                {
                    sb.Append(sep);
                    sb.Append(ToString(tp));
                    sep = ",";
                }
                sb.Append('>');
            }
            else
            {
                sb.Append(type.FullName);
            }
            return sb.ToString();
        }
    }
}
