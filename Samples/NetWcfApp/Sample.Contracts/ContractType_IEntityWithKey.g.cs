using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Sample.Contracts
{
	#region IEntityWithKey implementation for Contact

	partial class Contact : IEntityWithKey
	{
		private static readonly string QualifiedEntitySetName = "AdventureWorksEntities.Contacts";
		
		[NonSerialized] private bool _entityKeyResolved;
		[NonSerialized] private System.Data.EntityKey _entityKey;

		/// <summary>
		/// Key of the source entity of this contract object.
		/// </summary>
		System.Data.EntityKey IEntityWithKey.EntityKey
		{
			get
			{
				if (!this._entityKeyResolved)
				{
					if (this.Id == default(System.Int32))
						this._entityKey = null;
					else
						this._entityKey = new System.Data.EntityKey(QualifiedEntitySetName, "ContactID", this.Id);
					this._entityKeyResolved = true;
				}
				return this._entityKey;
			}
			set
			{
				throw new NotSupportedException("Setting the EntityKey of a contract object is not supported.");
			}
		}

		partial void OnAnIdentifierPropertyChanged()
		{
			this._entityKeyResolved = false;
		}
	}

	#endregion

	#region IEntityWithKey implementation for Employee

	partial class Employee : IEntityWithKey
	{
		private static readonly string QualifiedEntitySetName = "AdventureWorksEntities.Employees";
		
		[NonSerialized] private bool _entityKeyResolved;
		[NonSerialized] private System.Data.EntityKey _entityKey;

		/// <summary>
		/// Key of the source entity of this contract object.
		/// </summary>
		System.Data.EntityKey IEntityWithKey.EntityKey
		{
			get
			{
				if (!this._entityKeyResolved)
				{
					if (this.Id == default(System.Int32))
						this._entityKey = null;
					else
						this._entityKey = new System.Data.EntityKey(QualifiedEntitySetName, "EmployeeID", this.Id);
					this._entityKeyResolved = true;
				}
				return this._entityKey;
			}
			set
			{
				throw new NotSupportedException("Setting the EntityKey of a contract object is not supported.");
			}
		}

		partial void OnAnIdentifierPropertyChanged()
		{
			this._entityKeyResolved = false;
		}
	}

	#endregion

}