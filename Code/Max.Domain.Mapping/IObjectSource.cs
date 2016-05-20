using System;

namespace Max.Domain.Mapping
{
    /// <summary>
    /// An abstraction of an object store where stored objects live.
    /// </summary>
    public interface IObjectSource
    {
        /// <summary>
        /// Creates and returns an instance of the TStoredObject type of stored object to match the given source object.
        /// </summary>
        TStoreObject CreateObject<TStoreObject>(object sourceObject) where TStoreObject : class, new();

        /// <summary>
        /// Returns the store object type of the given object type.
        /// </summary>
        Type GetObjectType(Type storeObjectType);

        /// <summary>
        /// Request registration of a newly created object with its store.
        /// </summary>
        /// <param name="storeObject">A newly created (entity) store object.</param>
        void RegisterAsNewObject(object storeObject);

        /// <summary>
        /// Request registration of the deletion of an object from its store.
        /// </summary>
        /// <param name="storeObject">The (entity) store object to be deleted.</param>
        void RegisterAsDeletedObject(object storeObject);

        /// <summary>
        /// Request to retrieve the (entity) store object that matches the given (contract) source object.
        /// </summary>
        /// <typeparam name="TStoreObject">Type of store object expected.</typeparam>
        /// <param name="sourceObject">The mapped (contract) source object to find the store object for.</param>
        /// <returns>The store object if found, otherwise null.</returns>
        TStoreObject TryGetStoreObject<TStoreObject>(object sourceObject) where TStoreObject : class;

        /// <summary>
        /// Whether both objects match, cq. represent the same entity.
        /// </summary>
        /// <param name="sourceObject">A mapped (contract) source object.</param>
        /// <param name="storeObject">An (entity) store object.</param>
        /// <returns>True if both objects represent the same entity, false otherwise.</returns>
        bool IsMatch(object sourceObject, object storeObject);

        /// <summary>
        /// Invoked whenever an object is mapped.
        /// </summary>
        /// <param name="sourceObject">The mapped (contract) source object.</param>
        /// <param name="storeObject">The mapped (entity) store object.</param>
        void OnUpdateSystemObject(object sourceObject, object storeObject);
    }
}
