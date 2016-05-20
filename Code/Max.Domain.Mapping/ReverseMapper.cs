using System;
using System.Collections.Generic;
using System.Globalization;

namespace Max.Domain.Mapping
{
    /// <summary>
    /// Mapper to map back from contract types.
    /// </summary>
    public class ReverseMapper
    {
        private IObjectSource _objectSource;
        private HashSet<Type> _typesToUpdate;
        private HashSet<Type> _typesToCreate;
        private bool _asNewVersion;
        private List<object> _mappedSources;
        private List<object> _mappedTargets;

        /// <summary>
        /// Creates a new ReverseMapper session.
        /// </summary>
        public ReverseMapper(IObjectSource objectSource)
        {
            // Construct instance:
            this._objectSource = objectSource;
            this._typesToUpdate = new HashSet<Type>();
            this._typesToCreate = new HashSet<Type>();
            this._mappedSources = new List<object>();
            this._mappedTargets = new List<object>();
            this._asNewVersion = false;
        }

        /// <summary>
        /// Whether the object graph must be forced to create new objects even for existing ones
        /// (provided the types are creatable).
        /// </summary>
        public bool IsAsNewVersion
        {
            get
            {
                return this._asNewVersion;
            }
        }

        /// <summary>
        /// Allow creating and updating instances of the given type in the current mapper session.
        /// </summary>
        /// <typeparam name="TTypeToSynchronize">The target (Entity) type to allow synchronization for.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public ReverseMapper AllowCreatingAndUpdating<TTypeToSynchronize>()
        {
            this.AllowCreating<TTypeToSynchronize>();
            this.AllowUpdating<TTypeToSynchronize>();
            return this;
        }

        /// <summary>
        /// Allow creating instances of the given type in the current mapper session.
        /// </summary>
        /// <typeparam name="TTypeToSynchronize">The target (Entity) type to allow synchronization for.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public ReverseMapper AllowCreating<TTypeToSynchronize>()
        {
            Type t = typeof(TTypeToSynchronize);

            if (t.IsAbstract)
                throw new MappingException(String.Format(CultureInfo.CurrentCulture, "Cannot allow creation of abstract class {0}.", t.ToString()));

            this._typesToCreate.Add(t);

            return this;
        }

        /// <summary>
        /// Allow updating instances of the given type in the current mapper session.
        /// </summary>
        /// <typeparam name="TTypeToSynchronize">The target (Entity) type to allow synchronization for.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public ReverseMapper AllowUpdating<TTypeToSynchronize>()
        {
            Type t = typeof(TTypeToSynchronize);
            while (t != null)
            {
                if (!this._typesToUpdate.Add(t))
                    break;
                t = t.BaseType;
            }

            return this;
        }

        /// <summary>
        /// Whether the mapper creates new versions automatically
        /// for all creatable entities.
        /// </summary>
        public ReverseMapper AsNewVersion(bool enabled)
        {
            this._asNewVersion = enabled;

            return this;
        }

        public void RegisterAsNewObject(object obj)
        {
            if (this._objectSource != null)
                this._objectSource.RegisterAsNewObject(obj);
        }

        public void RegisterAsDeletedObject(object obj)
        {
            if (this._objectSource != null)
                this._objectSource.RegisterAsDeletedObject(obj);
        }

        /// <summary>
        /// Whether the given source object matches the given target object, cq. whether they represent the same entity.
        /// </summary>
        public bool IsMatch(object source, object target)
        {
            if (this._objectSource != null)
                return this._objectSource.IsMatch(source, target);
            else
                return false;
        }

        /// <summary>
        /// Whether the current mapping session allows updating objects of the given type (to be implemented in basetype mapping).
        /// </summary>
        public bool CanUpdate(object target)
        {
            var targetType = (this._objectSource == null) ? target.GetType() : this._objectSource.GetObjectType(target.GetType());
            return (this._typesToUpdate.Contains(targetType));
        }

        /// <summary>
        /// Whether the current mapping session allows creating objects of the given type.
        /// </summary>
        public bool CanCreate(Type targetType)
        {
            return (this._typesToCreate.Contains(targetType));
        }

        public virtual TStoreObject CreateObject<TStoreObject>(object source) where TStoreObject : class, new()
        {
            if (this._objectSource != null)
                return this._objectSource.CreateObject<TStoreObject>(source);
            else
                return default(TStoreObject);
        }

        /// <summary>
        /// Tries to get the (unmapped) target instance matching the given source instance.
        /// Returns the instance if found, null if not found. Is allowed to return a new,
        /// blank instance.
        /// </summary>
        /// <typeparam name="TStoreObject">The type of target object expected.</typeparam>
        /// <param name="source">The source object to search the target for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public virtual TStoreObject TryGetTarget<TStoreObject>(object source) where TStoreObject : class
        {
            if (_asNewVersion && CanCreate(typeof(TStoreObject)))
            {
                // No current instance maps, as a new version is to be created:
                return default(TStoreObject);
            }
            else
            {
                // Try retrieving the mapping instance:
                if (this._objectSource != null)
                    return this._objectSource.TryGetStoreObject<TStoreObject>(source);
                else
                    return default(TStoreObject);
            }
        }

        /// <summary>
        /// Registers a resolved source/target pair.
        /// </summary>
        public void RegisterMapping(object source, object target)
        {
            this._mappedSources.Add(source);
            this._mappedTargets.Add(target);
        }

        /// <summary>
        /// Gets the pre-registered target for a given source.
        /// Returns null if no target was yet registered for the given source.
        /// </summary>
        public object GetMappedTarget(object source)
        {
            for (int i = 0; i < this._mappedSources.Count; i++)
            {
                if (Object.ReferenceEquals(this._mappedSources[i], source))
                    return this._mappedTargets[i];
            }
            return null;
        }

        /// <summary>
        /// Base type mapper method.
        /// </summary>
        public virtual void UpdateSystemObject(object source, object target)
        {
            if (this._objectSource != null)
                this._objectSource.OnUpdateSystemObject(source, target);
        }
    }
}
