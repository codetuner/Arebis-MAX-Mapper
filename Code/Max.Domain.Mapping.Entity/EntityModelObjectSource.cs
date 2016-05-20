using System;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Globalization;
using System.Linq;

namespace Max.Domain.Mapping.Entity
{
    /// <summary>
    /// An IObjectSource implementation that uses an ADO.NET Entity Data Model generated ObjectContext.
    /// </summary>
    public class EntityModelObjectSource : IObjectSource
    {
        /// <summary>
        /// Constructs the ObjectSource for the given ObjectContext.
        /// </summary>
        /// <param name="objectContext">The EF ObjectContext to use as backend of this store.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public EntityModelObjectSource(ObjectContext objectContext)
        {
            // Workaround to ensure the assembly containing the entity context metadata is loaded:
            // (see: https://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=3405138&SiteID=1)
            try { objectContext.MetadataWorkspace.LoadFromAssembly(objectContext.GetType().Assembly); }
            catch { }

            // Construct instance:
            this.ObjectContext = objectContext;
            this.IsEntityKeyChecking = true;
        }

        public ObjectContext ObjectContext { get; private set; }

        public bool IsEntityKeyChecking { get; set; }

        public void RegisterAsNewObject(object storeObject)
        {
            if (this.IsEntity(storeObject))
                this.ObjectContext.AddObject(GetEntitySetName(this.ObjectContext, storeObject.GetType()), storeObject);
        }

        public void RegisterAsDeletedObject(object storeObject)
        {
            if (this.IsEntity(storeObject))
                this.ObjectContext.DeleteObject(storeObject);
        }

        public virtual TStoreObject CreateObject<TStoreObject>(object sourceObject) where TStoreObject : class, new()
        {
            return new TStoreObject();
        }

        public virtual Type GetObjectType(Type storeObjectType)
        {
            return storeObjectType;
        }

        public virtual TStoreObject TryGetStoreObject<TStoreObject>(object sourceObject) where TStoreObject : class
        {
            IEntityWithKey entityWithKeySource = sourceObject as IEntityWithKey;
            object result = null;
            if ((entityWithKeySource != null) && (entityWithKeySource.EntityKey != null))
                this.ObjectContext.TryGetObjectByKey(entityWithKeySource.EntityKey, out result);
            return (TStoreObject)result;
        }

        public virtual bool IsMatch(object sourceObject, object storeObject)
        {
            return (((IEntityWithKey)sourceObject).EntityKey == ((IEntityWithKey)storeObject).EntityKey);
        }

        public virtual void OnUpdateSystemObject(object sourceObject, object storeObject)
        {
            // Check EntityKey match:
            if (this.IsEntityKeyChecking)
            {
                IEntityWithKey entityWithKeySource = sourceObject as IEntityWithKey;
                IEntityWithKey entityWithKeyTarget = storeObject as IEntityWithKey;
                if ((entityWithKeySource != null) && (entityWithKeyTarget != null))
                {
                    if ((entityWithKeySource.EntityKey != null) && (entityWithKeyTarget.EntityKey != null) && (!entityWithKeyTarget.EntityKey.IsTemporary) && (entityWithKeyTarget.EntityKey != entityWithKeySource.EntityKey))
                        throw new MappingException(String.Format(CultureInfo.CurrentCulture, "EntityKeys of mapped source and target objects for source of type {0} do not match.", sourceObject.GetType()));
                }
            }
        }

        /// <summary>
        ///  Whether the given storeObject is a real stored object.
        /// </summary>
        public virtual bool IsEntity(object storeObject)
        { 
            return (storeObject is IEntityWithKey);
        }

        /// <summary>
        /// Returns the EntitySetName for the given entity type.
        /// </summary>
        public static string GetEntitySetName(ObjectContext context, Type entityType)
        {
            Type type = entityType;

            while (type != null)
            {
                // Use first EdmEntityTypeAttribute found:
                foreach (EdmEntityTypeAttribute typeattr in type.GetCustomAttributes(typeof(EdmEntityTypeAttribute), false))
                {
                    // Retrieve the entity container for the conceptual model:
                    var container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);

                    // Retrieve the name of the entityset matching the given types EdmEntityTypeAttribute:
                    string entitySetName = (from meta in container.BaseEntitySets
                                            where meta.ElementType.FullName == typeattr.NamespaceName + "." + typeattr.Name
                                            select meta.Name)
                                            .FirstOrDefault();

                    // If match, return the entitySetName:
                    if (entitySetName != null) return entitySetName;
                }

                // If no matching attribute or entitySetName found, try basetype:
                type = type.BaseType;
            }

            // Return null if no entitySetName could be found:
            return null;
        }
    }
}
