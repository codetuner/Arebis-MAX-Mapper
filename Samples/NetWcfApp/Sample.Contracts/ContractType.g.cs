using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Sample.Contracts
{
	#region Contact class definition

	/// <summary>
	/// Contact datacontract class.
	/// </summary>
	[DataContract(Namespace = "urn:Sample.Contracts", IsReference = true), Serializable()]
	public partial class Contact : BaseObject 
	{
		/// <summary>
		/// Constructs a new Contact datacontract instance.
		/// </summary>
		public Contact()
			: base()
		{
			this.OnInstanceCreated();
		}
		
		partial void OnInstanceCreated();

		private System.Int32 _id;

		/// <summary>
		/// Id property.
		/// </summary>
		[DataMember()]
		public System.Int32 Id 
		{ 
			get
			{
				return this._id;
			}
			set 
			{
				this._id = value;
				this.OnAnIdentifierPropertyChanged();
				this.OnIdPropertyChanged(value);
				this.OnPropertyChanged("Id", value);
			}
		}

		partial void OnIdPropertyChanged(System.Int32 newValue);

		private System.String _title;

		/// <summary>
		/// Title property.
		/// </summary>
		[DataMember()]
		public System.String Title 
		{ 
			get
			{
				return this._title;
			}
			set 
			{
				this._title = value;
				this.OnTitlePropertyChanged(value);
				this.OnPropertyChanged("Title", value);
			}
		}

		partial void OnTitlePropertyChanged(System.String newValue);

		private System.String _firstName;

		/// <summary>
		/// FirstName property.
		/// </summary>
		[DataMember()]
		public System.String FirstName 
		{ 
			get
			{
				return this._firstName;
			}
			set 
			{
				this._firstName = value;
				this.OnFirstNamePropertyChanged(value);
				this.OnPropertyChanged("FirstName", value);
			}
		}

		partial void OnFirstNamePropertyChanged(System.String newValue);

		private System.String _middleName;

		/// <summary>
		/// MiddleName property.
		/// </summary>
		[DataMember()]
		public System.String MiddleName 
		{ 
			get
			{
				return this._middleName;
			}
			set 
			{
				this._middleName = value;
				this.OnMiddleNamePropertyChanged(value);
				this.OnPropertyChanged("MiddleName", value);
			}
		}

		partial void OnMiddleNamePropertyChanged(System.String newValue);

		private System.String _lastName;

		/// <summary>
		/// LastName property.
		/// </summary>
		[DataMember()]
		public System.String LastName 
		{ 
			get
			{
				return this._lastName;
			}
			set 
			{
				this._lastName = value;
				this.OnLastNamePropertyChanged(value);
				this.OnPropertyChanged("LastName", value);
			}
		}

		partial void OnLastNamePropertyChanged(System.String newValue);

		private System.String _suffix;

		/// <summary>
		/// Suffix property.
		/// </summary>
		[DataMember()]
		public System.String Suffix 
		{ 
			get
			{
				return this._suffix;
			}
			set 
			{
				this._suffix = value;
				this.OnSuffixPropertyChanged(value);
				this.OnPropertyChanged("Suffix", value);
			}
		}

		partial void OnSuffixPropertyChanged(System.String newValue);

		private System.String _emailAddress;

		/// <summary>
		/// EmailAddress property.
		/// </summary>
		[DataMember()]
		public System.String EmailAddress 
		{ 
			get
			{
				return this._emailAddress;
			}
			set 
			{
				this._emailAddress = value;
				this.OnEmailAddressPropertyChanged(value);
				this.OnPropertyChanged("EmailAddress", value);
			}
		}

		partial void OnEmailAddressPropertyChanged(System.String newValue);

		private System.String _phone;

		/// <summary>
		/// Phone property.
		/// </summary>
		[DataMember()]
		public System.String Phone 
		{ 
			get
			{
				return this._phone;
			}
			set 
			{
				this._phone = value;
				this.OnPhonePropertyChanged(value);
				this.OnPropertyChanged("Phone", value);
			}
		}

		partial void OnPhonePropertyChanged(System.String newValue);

		partial void OnPropertyChanged(string propertyName, object newValue);

		// For internal use only:
		partial void OnAnIdentifierPropertyChanged();
	}

	#endregion

	#region Employee class definition

	/// <summary>
	/// Employee datacontract class.
	/// </summary>
	[DataContract(Namespace = "urn:Sample.Contracts", IsReference = true), Serializable()]
	[KnownType(typeof(SalesPerson))]
	public partial class Employee : System.Object 
	{
		/// <summary>
		/// Constructs a new Employee datacontract instance.
		/// </summary>
		public Employee()
			: base()
		{
			this.OnInstanceCreated();
		}
		
		partial void OnInstanceCreated();

		private System.Int32 _id;

		/// <summary>
		/// Id property.
		/// </summary>
		[DataMember()]
		public System.Int32 Id 
		{ 
			get
			{
				return this._id;
			}
			set 
			{
				this._id = value;
				this.OnAnIdentifierPropertyChanged();
				this.OnIdPropertyChanged(value);
				this.OnPropertyChanged("Id", value);
			}
		}

		partial void OnIdPropertyChanged(System.Int32 newValue);

		private Contact _contact;

		/// <summary>
		/// Contact property.
		/// </summary>
		[DataMember()]
		public Contact Contact 
		{ 
			get
			{
				return this._contact;
			}
			set 
			{
				this._contact = value;
				this.OnContactPropertyChanged(value);
				this.OnPropertyChanged("Contact", value);
			}
		}

		partial void OnContactPropertyChanged(Contact newValue);

		private System.String _gender;

		/// <summary>
		/// Gender property.
		/// </summary>
		[DataMember()]
		public System.String Gender 
		{ 
			get
			{
				return this._gender;
			}
			set 
			{
				this._gender = value;
				this.OnGenderPropertyChanged(value);
				this.OnPropertyChanged("Gender", value);
			}
		}

		partial void OnGenderPropertyChanged(System.String newValue);

		private System.DateTime _birthDate;

		/// <summary>
		/// BirthDate property.
		/// </summary>
		[DataMember()]
		public System.DateTime BirthDate 
		{ 
			get
			{
				return this._birthDate;
			}
			set 
			{
				this._birthDate = value;
				this.OnBirthDatePropertyChanged(value);
				this.OnPropertyChanged("BirthDate", value);
			}
		}

		partial void OnBirthDatePropertyChanged(System.DateTime newValue);

		private System.DateTime _hireDate;

		/// <summary>
		/// HireDate property.
		/// </summary>
		[DataMember()]
		public System.DateTime HireDate 
		{ 
			get
			{
				return this._hireDate;
			}
			set 
			{
				this._hireDate = value;
				this.OnHireDatePropertyChanged(value);
				this.OnPropertyChanged("HireDate", value);
			}
		}

		partial void OnHireDatePropertyChanged(System.DateTime newValue);

		private System.Nullable<System.Int32> _managerId;

		/// <summary>
		/// ManagerId property.
		/// </summary>
		[DataMember()]
		public System.Nullable<System.Int32> ManagerId 
		{ 
			get
			{
				return this._managerId;
			}
			set 
			{
				this._managerId = value;
				this.OnManagerIdPropertyChanged(value);
				this.OnPropertyChanged("ManagerId", value);
			}
		}

		partial void OnManagerIdPropertyChanged(System.Nullable<System.Int32> newValue);

		partial void OnPropertyChanged(string propertyName, object newValue);

		// For internal use only:
		partial void OnAnIdentifierPropertyChanged();
	}

	#endregion

	#region EmployeeItem class definition

	/// <summary>
	/// EmployeeItem datacontract class.
	/// </summary>
	[DataContract(Namespace = "urn:Sample.Contracts", IsReference = true), Serializable()]
	public partial class EmployeeItem : System.Object 
	{
		/// <summary>
		/// Constructs a new EmployeeItem datacontract instance.
		/// </summary>
		public EmployeeItem()
			: base()
		{
			this.OnInstanceCreated();
		}
		
		partial void OnInstanceCreated();

		private System.Int32 _id;

		/// <summary>
		/// Id property.
		/// </summary>
		[DataMember()]
		public System.Int32 Id 
		{ 
			get
			{
				return this._id;
			}
			set 
			{
				this._id = value;
				this.OnAnIdentifierPropertyChanged();
				this.OnIdPropertyChanged(value);
				this.OnPropertyChanged("Id", value);
			}
		}

		partial void OnIdPropertyChanged(System.Int32 newValue);

		private System.String _title;

		/// <summary>
		/// Title property.
		/// </summary>
		[DataMember()]
		public System.String Title 
		{ 
			get
			{
				return this._title;
			}
			set 
			{
				this._title = value;
				this.OnTitlePropertyChanged(value);
				this.OnPropertyChanged("Title", value);
			}
		}

		partial void OnTitlePropertyChanged(System.String newValue);

		private System.String _firstName;

		/// <summary>
		/// FirstName property.
		/// </summary>
		[DataMember()]
		public System.String FirstName 
		{ 
			get
			{
				return this._firstName;
			}
			set 
			{
				this._firstName = value;
				this.OnFirstNamePropertyChanged(value);
				this.OnPropertyChanged("FirstName", value);
			}
		}

		partial void OnFirstNamePropertyChanged(System.String newValue);

		private System.String _middleName;

		/// <summary>
		/// MiddleName property.
		/// </summary>
		[DataMember()]
		public System.String MiddleName 
		{ 
			get
			{
				return this._middleName;
			}
			set 
			{
				this._middleName = value;
				this.OnMiddleNamePropertyChanged(value);
				this.OnPropertyChanged("MiddleName", value);
			}
		}

		partial void OnMiddleNamePropertyChanged(System.String newValue);

		private System.String _lastName;

		/// <summary>
		/// LastName property.
		/// </summary>
		[DataMember()]
		public System.String LastName 
		{ 
			get
			{
				return this._lastName;
			}
			set 
			{
				this._lastName = value;
				this.OnLastNamePropertyChanged(value);
				this.OnPropertyChanged("LastName", value);
			}
		}

		partial void OnLastNamePropertyChanged(System.String newValue);

		private System.String _gender;

		/// <summary>
		/// Gender property.
		/// </summary>
		[DataMember()]
		public System.String Gender 
		{ 
			get
			{
				return this._gender;
			}
			set 
			{
				this._gender = value;
				this.OnGenderPropertyChanged(value);
				this.OnPropertyChanged("Gender", value);
			}
		}

		partial void OnGenderPropertyChanged(System.String newValue);

		private System.DateTime _birthDate;

		/// <summary>
		/// BirthDate property.
		/// </summary>
		[DataMember()]
		public System.DateTime BirthDate 
		{ 
			get
			{
				return this._birthDate;
			}
			set 
			{
				this._birthDate = value;
				this.OnBirthDatePropertyChanged(value);
				this.OnPropertyChanged("BirthDate", value);
			}
		}

		partial void OnBirthDatePropertyChanged(System.DateTime newValue);

		private System.Nullable<System.Int32> _managerId;

		/// <summary>
		/// ManagerId property.
		/// </summary>
		[DataMember()]
		public System.Nullable<System.Int32> ManagerId 
		{ 
			get
			{
				return this._managerId;
			}
			set 
			{
				this._managerId = value;
				this.OnManagerIdPropertyChanged(value);
				this.OnPropertyChanged("ManagerId", value);
			}
		}

		partial void OnManagerIdPropertyChanged(System.Nullable<System.Int32> newValue);

		private System.String _managerFirstName;

		/// <summary>
		/// ManagerFirstName property.
		/// </summary>
		[DataMember()]
		public System.String ManagerFirstName 
		{ 
			get
			{
				return this._managerFirstName;
			}
			set 
			{
				this._managerFirstName = value;
				this.OnManagerFirstNamePropertyChanged(value);
				this.OnPropertyChanged("ManagerFirstName", value);
			}
		}

		partial void OnManagerFirstNamePropertyChanged(System.String newValue);

		private System.String _managerMiddleName;

		/// <summary>
		/// ManagerMiddleName property.
		/// </summary>
		[DataMember()]
		public System.String ManagerMiddleName 
		{ 
			get
			{
				return this._managerMiddleName;
			}
			set 
			{
				this._managerMiddleName = value;
				this.OnManagerMiddleNamePropertyChanged(value);
				this.OnPropertyChanged("ManagerMiddleName", value);
			}
		}

		partial void OnManagerMiddleNamePropertyChanged(System.String newValue);

		private System.String _managerLastName;

		/// <summary>
		/// ManagerLastName property.
		/// </summary>
		[DataMember()]
		public System.String ManagerLastName 
		{ 
			get
			{
				return this._managerLastName;
			}
			set 
			{
				this._managerLastName = value;
				this.OnManagerLastNamePropertyChanged(value);
				this.OnPropertyChanged("ManagerLastName", value);
			}
		}

		partial void OnManagerLastNamePropertyChanged(System.String newValue);

		partial void OnPropertyChanged(string propertyName, object newValue);

		// For internal use only:
		partial void OnAnIdentifierPropertyChanged();
	}

	#endregion

	#region Manager class definition

	/// <summary>
	/// Manager datacontract class.
	/// </summary>
	[DataContract(Namespace = "urn:Sample.Contracts", IsReference = true), Serializable()]
	public partial class Manager : System.Object 
	{
		/// <summary>
		/// Constructs a new Manager datacontract instance.
		/// </summary>
		public Manager()
			: base()
		{
			this.Subordinates = new System.Collections.Generic.List<Subordinate>();
			this.OnInstanceCreated();
		}
		
		partial void OnInstanceCreated();

		private System.Int32 _id;

		/// <summary>
		/// Id property.
		/// </summary>
		[DataMember()]
		public System.Int32 Id 
		{ 
			get
			{
				return this._id;
			}
			set 
			{
				this._id = value;
				this.OnAnIdentifierPropertyChanged();
				this.OnIdPropertyChanged(value);
				this.OnPropertyChanged("Id", value);
			}
		}

		partial void OnIdPropertyChanged(System.Int32 newValue);

		private Contact _contact;

		/// <summary>
		/// Contact property.
		/// </summary>
		[DataMember()]
		public Contact Contact 
		{ 
			get
			{
				return this._contact;
			}
			set 
			{
				this._contact = value;
				this.OnContactPropertyChanged(value);
				this.OnPropertyChanged("Contact", value);
			}
		}

		partial void OnContactPropertyChanged(Contact newValue);

		private System.String _gender;

		/// <summary>
		/// Gender property.
		/// </summary>
		[DataMember()]
		public System.String Gender 
		{ 
			get
			{
				return this._gender;
			}
			set 
			{
				this._gender = value;
				this.OnGenderPropertyChanged(value);
				this.OnPropertyChanged("Gender", value);
			}
		}

		partial void OnGenderPropertyChanged(System.String newValue);

		private System.DateTime _birthDate;

		/// <summary>
		/// BirthDate property.
		/// </summary>
		[DataMember()]
		public System.DateTime BirthDate 
		{ 
			get
			{
				return this._birthDate;
			}
			set 
			{
				this._birthDate = value;
				this.OnBirthDatePropertyChanged(value);
				this.OnPropertyChanged("BirthDate", value);
			}
		}

		partial void OnBirthDatePropertyChanged(System.DateTime newValue);

		private System.DateTime _hireDate;

		/// <summary>
		/// HireDate property.
		/// </summary>
		[DataMember()]
		public System.DateTime HireDate 
		{ 
			get
			{
				return this._hireDate;
			}
			set 
			{
				this._hireDate = value;
				this.OnHireDatePropertyChanged(value);
				this.OnPropertyChanged("HireDate", value);
			}
		}

		partial void OnHireDatePropertyChanged(System.DateTime newValue);

		/// <summary>
		/// Subordinates property.
		/// </summary>
		[DataMember()]
		public System.Collections.Generic.List<Subordinate> Subordinates { get; set; }

		partial void OnPropertyChanged(string propertyName, object newValue);

		// For internal use only:
		partial void OnAnIdentifierPropertyChanged();
	}

	#endregion

	#region SalesPerson class definition

	/// <summary>
	/// SalesPerson datacontract class.
	/// </summary>
	[DataContract(Namespace = "urn:Sample.Contracts"), Serializable()]
	public partial class SalesPerson : Employee 
	{
		/// <summary>
		/// Constructs a new SalesPerson datacontract instance.
		/// </summary>
		public SalesPerson()
			: base()
		{
			this.OnInstanceCreated();
		}
		
		partial void OnInstanceCreated();

		private System.Nullable<System.Decimal> _salesQuota;

		/// <summary>
		/// SalesQuota property.
		/// </summary>
		[DataMember()]
		public System.Nullable<System.Decimal> SalesQuota 
		{ 
			get
			{
				return this._salesQuota;
			}
			set 
			{
				this._salesQuota = value;
				this.OnSalesQuotaPropertyChanged(value);
				this.OnPropertyChanged("SalesQuota", value);
			}
		}

		partial void OnSalesQuotaPropertyChanged(System.Nullable<System.Decimal> newValue);

		private System.Decimal _bonus;

		/// <summary>
		/// Bonus property.
		/// </summary>
		[DataMember()]
		public System.Decimal Bonus 
		{ 
			get
			{
				return this._bonus;
			}
			set 
			{
				this._bonus = value;
				this.OnBonusPropertyChanged(value);
				this.OnPropertyChanged("Bonus", value);
			}
		}

		partial void OnBonusPropertyChanged(System.Decimal newValue);

		private System.Decimal _commissionPct;

		/// <summary>
		/// CommissionPct property.
		/// </summary>
		[DataMember()]
		public System.Decimal CommissionPct 
		{ 
			get
			{
				return this._commissionPct;
			}
			set 
			{
				this._commissionPct = value;
				this.OnCommissionPctPropertyChanged(value);
				this.OnPropertyChanged("CommissionPct", value);
			}
		}

		partial void OnCommissionPctPropertyChanged(System.Decimal newValue);

		private System.Decimal _salesYTD;

		/// <summary>
		/// SalesYTD property.
		/// </summary>
		[DataMember()]
		public System.Decimal SalesYTD 
		{ 
			get
			{
				return this._salesYTD;
			}
			set 
			{
				this._salesYTD = value;
				this.OnSalesYTDPropertyChanged(value);
				this.OnPropertyChanged("SalesYTD", value);
			}
		}

		partial void OnSalesYTDPropertyChanged(System.Decimal newValue);

		private decimal? _territorySalesYTD;

		/// <summary>
		/// TerritorySalesYTD property.
		/// </summary>
		[DataMember()]
		public decimal? TerritorySalesYTD 
		{ 
			get
			{
				return this._territorySalesYTD;
			}
			set 
			{
				this._territorySalesYTD = value;
				this.OnTerritorySalesYTDPropertyChanged(value);
				this.OnPropertyChanged("TerritorySalesYTD", value);
			}
		}

		partial void OnTerritorySalesYTDPropertyChanged(decimal? newValue);

		partial void OnPropertyChanged(string propertyName, object newValue);

		// For internal use only:
		partial void OnAnIdentifierPropertyChanged();
	}

	#endregion

	#region Subordinate class definition

	/// <summary>
	/// Subordinate datacontract class.
	/// </summary>
	[DataContract(Namespace = "urn:Sample.Contracts", IsReference = true), Serializable()]
	public partial class Subordinate : System.Object 
	{
		/// <summary>
		/// Constructs a new Subordinate datacontract instance.
		/// </summary>
		public Subordinate()
			: base()
		{
			this.OnInstanceCreated();
		}
		
		partial void OnInstanceCreated();

		private System.Int32 _id;

		/// <summary>
		/// Id property.
		/// </summary>
		[DataMember()]
		public System.Int32 Id 
		{ 
			get
			{
				return this._id;
			}
			set 
			{
				this._id = value;
				this.OnAnIdentifierPropertyChanged();
				this.OnIdPropertyChanged(value);
				this.OnPropertyChanged("Id", value);
			}
		}

		partial void OnIdPropertyChanged(System.Int32 newValue);

		private Contact _contact;

		/// <summary>
		/// Contact property.
		/// </summary>
		[DataMember()]
		public Contact Contact 
		{ 
			get
			{
				return this._contact;
			}
			set 
			{
				this._contact = value;
				this.OnContactPropertyChanged(value);
				this.OnPropertyChanged("Contact", value);
			}
		}

		partial void OnContactPropertyChanged(Contact newValue);

		private System.String _gender;

		/// <summary>
		/// Gender property.
		/// </summary>
		[DataMember()]
		public System.String Gender 
		{ 
			get
			{
				return this._gender;
			}
			set 
			{
				this._gender = value;
				this.OnGenderPropertyChanged(value);
				this.OnPropertyChanged("Gender", value);
			}
		}

		partial void OnGenderPropertyChanged(System.String newValue);

		private System.DateTime _birthDate;

		/// <summary>
		/// BirthDate property.
		/// </summary>
		[DataMember()]
		public System.DateTime BirthDate 
		{ 
			get
			{
				return this._birthDate;
			}
			set 
			{
				this._birthDate = value;
				this.OnBirthDatePropertyChanged(value);
				this.OnPropertyChanged("BirthDate", value);
			}
		}

		partial void OnBirthDatePropertyChanged(System.DateTime newValue);

		private System.DateTime _hireDate;

		/// <summary>
		/// HireDate property.
		/// </summary>
		[DataMember()]
		public System.DateTime HireDate 
		{ 
			get
			{
				return this._hireDate;
			}
			set 
			{
				this._hireDate = value;
				this.OnHireDatePropertyChanged(value);
				this.OnPropertyChanged("HireDate", value);
			}
		}

		partial void OnHireDatePropertyChanged(System.DateTime newValue);

		partial void OnPropertyChanged(string propertyName, object newValue);

		// For internal use only:
		partial void OnAnIdentifierPropertyChanged();
	}

	#endregion

}