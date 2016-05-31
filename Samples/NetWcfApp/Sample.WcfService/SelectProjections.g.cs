using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Max.Domain.Mapping;
using Sample.Contracts;

namespace Sample.WcfService.Mapping
{
    public static partial class SelectProjectionsExtensions
    {
        #region EmployeeItem select projections

        /// <summary>
		/// Projects the results into EmployeeItem objects.
        /// </summary>
        public static IQueryable<EmployeeItem> SelectAsEmployeeItemCollection(this IQueryable<Sample.Business.Employee> query)
		{
			return query
				.Select(source => new EmployeeItem() {
					Id = source.EmployeeID,
					Title = source.Contact.Title,
					FirstName = source.Contact.FirstName,
					MiddleName = source.Contact.MiddleName,
					LastName = source.Contact.LastName,
					Gender = source.Gender,
					BirthDate = source.BirthDate,
					ManagerId = source.ManagerID,
					ManagerFirstName = source.Manager.Contact.FirstName,
					ManagerMiddleName = source.Manager.Contact.MiddleName,
					ManagerLastName = source.Manager.Contact.LastName,
				});
        }

        #endregion

        #region Manager select projections

		// Cannot generate projections for types having collection properties.

        #endregion

        #region Subordinate select projections

        /// <summary>
		/// Projects the results into Subordinate objects.
        /// </summary>
        public static IQueryable<Subordinate> SelectAsSubordinateCollection(this IQueryable<Sample.Business.Employee> query)
		{
			return query
				.Select(source => new Subordinate() {
					Id = source.EmployeeID,
					Contact = new Contact() {
						Id = source.Contact.ContactID,
						Title = source.Contact.Title,
						FirstName = source.Contact.FirstName,
						MiddleName = source.Contact.MiddleName,
						LastName = source.Contact.LastName,
						Suffix = source.Contact.Suffix,
						EmailAddress = source.Contact.EmailAddress,
						Phone = source.Contact.Phone,
					},
					Gender = source.Gender,
					BirthDate = source.BirthDate,
					HireDate = source.HireDate,
				});
        }

        #endregion

    }
}
