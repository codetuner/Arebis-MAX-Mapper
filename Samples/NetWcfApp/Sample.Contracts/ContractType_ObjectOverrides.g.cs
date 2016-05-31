using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Sample.Contracts
{
	#region Object Overrides for Contact

	partial class Contact
	{
		/// <summary>
		/// Compares the object by its identifier.
		/// </summary>
		public override bool Equals(object obj)
		{
			Contact other = (obj as Contact);
			if (Object.ReferenceEquals(other, null))
				return false;
			else if (this.IsNewInstance())
				return Object.ReferenceEquals(this, other);
			else
				return (this.Id == other.Id);
		}

		/// <summary>
		/// Computes a hash code based on the objects identifier.
		/// </summary>
		public override int GetHashCode()
		{
			if (this.IsNewInstance())
				return base.GetHashCode();
			else
				return this.GetType().GetHashCode() ^ this.Id.GetHashCode();
		}

		/// <summary>
		/// Whether this instance is a new instance.
		/// </summary>
		public virtual bool IsNewInstance()
		{
			return (this.Id == default(System.Int32));
		}

		/// <summary>
		/// Returns a shallow copy of this object.
		/// </summary>
		public new Contact MemberwiseClone()
		{
			return (Contact)base.MemberwiseClone();
		}
	}

	#endregion

	#region Object Overrides for Employee

	partial class Employee
	{
		/// <summary>
		/// Compares the object by its identifier.
		/// </summary>
		public override bool Equals(object obj)
		{
			Employee other = (obj as Employee);
			if (Object.ReferenceEquals(other, null))
				return false;
			else if (this.IsNewInstance())
				return Object.ReferenceEquals(this, other);
			else
				return (this.Id == other.Id);
		}

		/// <summary>
		/// Computes a hash code based on the objects identifier.
		/// </summary>
		public override int GetHashCode()
		{
			if (this.IsNewInstance())
				return base.GetHashCode();
			else
				return this.GetType().GetHashCode() ^ this.Id.GetHashCode();
		}

		/// <summary>
		/// Whether this instance is a new instance.
		/// </summary>
		public virtual bool IsNewInstance()
		{
			return (this.Id == default(System.Int32));
		}

		/// <summary>
		/// Returns a shallow copy of this object.
		/// </summary>
		public new Employee MemberwiseClone()
		{
			return (Employee)base.MemberwiseClone();
		}
	}

	#endregion

	#region Object Overrides for EmployeeItem

	partial class EmployeeItem
	{
		/// <summary>
		/// Compares the object by its identifier.
		/// </summary>
		public override bool Equals(object obj)
		{
			EmployeeItem other = (obj as EmployeeItem);
			if (Object.ReferenceEquals(other, null))
				return false;
			else if (this.IsNewInstance())
				return Object.ReferenceEquals(this, other);
			else
				return (this.Id == other.Id);
		}

		/// <summary>
		/// Computes a hash code based on the objects identifier.
		/// </summary>
		public override int GetHashCode()
		{
			if (this.IsNewInstance())
				return base.GetHashCode();
			else
				return this.GetType().GetHashCode() ^ this.Id.GetHashCode();
		}

		/// <summary>
		/// Whether this instance is a new instance.
		/// </summary>
		public virtual bool IsNewInstance()
		{
			return (this.Id == default(System.Int32));
		}

		/// <summary>
		/// Returns a shallow copy of this object.
		/// </summary>
		public new EmployeeItem MemberwiseClone()
		{
			return (EmployeeItem)base.MemberwiseClone();
		}
	}

	#endregion

	#region Object Overrides for Manager

	partial class Manager
	{
		/// <summary>
		/// Compares the object by its identifier.
		/// </summary>
		public override bool Equals(object obj)
		{
			Manager other = (obj as Manager);
			if (Object.ReferenceEquals(other, null))
				return false;
			else if (this.IsNewInstance())
				return Object.ReferenceEquals(this, other);
			else
				return (this.Id == other.Id);
		}

		/// <summary>
		/// Computes a hash code based on the objects identifier.
		/// </summary>
		public override int GetHashCode()
		{
			if (this.IsNewInstance())
				return base.GetHashCode();
			else
				return this.GetType().GetHashCode() ^ this.Id.GetHashCode();
		}

		/// <summary>
		/// Whether this instance is a new instance.
		/// </summary>
		public virtual bool IsNewInstance()
		{
			return (this.Id == default(System.Int32));
		}

		/// <summary>
		/// Returns a shallow copy of this object.
		/// </summary>
		public new Manager MemberwiseClone()
		{
			return (Manager)base.MemberwiseClone();
		}
	}

	#endregion

	#region Object Overrides for SalesPerson

	partial class SalesPerson
	{
		/// <summary>
		/// Returns a shallow copy of this object.
		/// </summary>
		public new SalesPerson MemberwiseClone()
		{
			return (SalesPerson)base.MemberwiseClone();
		}
	}

	#endregion

	#region Object Overrides for Subordinate

	partial class Subordinate
	{
		/// <summary>
		/// Compares the object by its identifier.
		/// </summary>
		public override bool Equals(object obj)
		{
			Subordinate other = (obj as Subordinate);
			if (Object.ReferenceEquals(other, null))
				return false;
			else if (this.IsNewInstance())
				return Object.ReferenceEquals(this, other);
			else
				return (this.Id == other.Id);
		}

		/// <summary>
		/// Computes a hash code based on the objects identifier.
		/// </summary>
		public override int GetHashCode()
		{
			if (this.IsNewInstance())
				return base.GetHashCode();
			else
				return this.GetType().GetHashCode() ^ this.Id.GetHashCode();
		}

		/// <summary>
		/// Whether this instance is a new instance.
		/// </summary>
		public virtual bool IsNewInstance()
		{
			return (this.Id == default(System.Int32));
		}

		/// <summary>
		/// Returns a shallow copy of this object.
		/// </summary>
		public new Subordinate MemberwiseClone()
		{
			return (Subordinate)base.MemberwiseClone();
		}
	}

	#endregion

}