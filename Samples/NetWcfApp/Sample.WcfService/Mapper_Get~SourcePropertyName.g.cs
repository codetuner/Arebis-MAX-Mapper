using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Max.Domain.Mapping;
using Sample.Contracts;

namespace Sample.WcfService.Mapping
{
    public static partial class GeneratedMapperExtensions
    {
        #region Contact class implementation

        /// <summary>
        /// Returns the name of the property, or property path, that matches the given mapped property name.
        /// </summary>
        public static string GetContactSourcePropertyName(this Mapper mapper, string mappedPropertyName)
        {
            switch(mappedPropertyName)
            {
                case "Id":
                    return "ContactID";
                case "Title":
                    return "Title";
                case "FirstName":
                    return "FirstName";
                case "MiddleName":
                    return "MiddleName";
                case "LastName":
                    return "LastName";
                case "Suffix":
                    return "Suffix";
                case "EmailAddress":
                    return "EmailAddress";
                case "Phone":
                    return "Phone";
                default:
                    return null;
            }
        }

        #endregion

        #region Employee class implementation

        /// <summary>
        /// Returns the name of the property, or property path, that matches the given mapped property name.
        /// </summary>
        public static string GetEmployeeSourcePropertyName(this Mapper mapper, string mappedPropertyName)
        {
            switch(mappedPropertyName)
            {
                case "Id":
                    return "EmployeeID";
                case "Contact":
                    return "Contact";
                case "Gender":
                    return "Gender";
                case "BirthDate":
                    return "BirthDate";
                case "HireDate":
                    return "HireDate";
                case "ManagerId":
                    return "ManagerID";
                default:
                    return null;
            }
        }

        #endregion

        #region EmployeeItem class implementation

        /// <summary>
        /// Returns the name of the property, or property path, that matches the given mapped property name.
        /// </summary>
        public static string GetEmployeeItemSourcePropertyName(this Mapper mapper, string mappedPropertyName)
        {
            switch(mappedPropertyName)
            {
                case "Id":
                    return "EmployeeID";
                case "Title":
                    return "Contact.Title";
                case "FirstName":
                    return "Contact.FirstName";
                case "MiddleName":
                    return "Contact.MiddleName";
                case "LastName":
                    return "Contact.LastName";
                case "Gender":
                    return "Gender";
                case "BirthDate":
                    return "BirthDate";
                case "ManagerId":
                    return "ManagerID";
                case "ManagerFirstName":
                    return "Manager.Contact.FirstName";
                case "ManagerMiddleName":
                    return "Manager.Contact.MiddleName";
                case "ManagerLastName":
                    return "Manager.Contact.LastName";
                default:
                    return null;
            }
        }

        #endregion

        #region Manager class implementation

        /// <summary>
        /// Returns the name of the property, or property path, that matches the given mapped property name.
        /// </summary>
        public static string GetManagerSourcePropertyName(this Mapper mapper, string mappedPropertyName)
        {
            switch(mappedPropertyName)
            {
                case "Id":
                    return "EmployeeID";
                case "Contact":
                    return "Contact";
                case "Gender":
                    return "Gender";
                case "BirthDate":
                    return "BirthDate";
                case "HireDate":
                    return "HireDate";
                default:
                    return null;
            }
        }

        #endregion

        #region SalesPerson class implementation

        /// <summary>
        /// Returns the name of the property, or property path, that matches the given mapped property name.
        /// </summary>
        public static string GetSalesPersonSourcePropertyName(this Mapper mapper, string mappedPropertyName)
        {
            switch(mappedPropertyName)
            {
                case "SalesQuota":
                    return "SalesQuota";
                case "Bonus":
                    return "Bonus";
                case "CommissionPct":
                    return "CommissionPct";
                case "SalesYTD":
                    return "SalesYTD";
                case "TerritorySalesYTD":
                    return "Territory.SalesYTD";
                default:
                    return null;
            }
        }

        #endregion

        #region Subordinate class implementation

        /// <summary>
        /// Returns the name of the property, or property path, that matches the given mapped property name.
        /// </summary>
        public static string GetSubordinateSourcePropertyName(this Mapper mapper, string mappedPropertyName)
        {
            switch(mappedPropertyName)
            {
                case "Id":
                    return "EmployeeID";
                case "Contact":
                    return "Contact";
                case "Gender":
                    return "Gender";
                case "BirthDate":
                    return "BirthDate";
                case "HireDate":
                    return "HireDate";
                default:
                    return null;
            }
        }

        #endregion

    }
}