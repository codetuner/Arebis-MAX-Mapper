using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sample.Contracts;

namespace Sample.WcfService
{
    public static class ManualSelectProjections
    {
        public static IQueryable<Manager> SelectAsEmployeeItemCollectionManual(this IQueryable<Sample.Business.Employee> query)
        {
            return query
                .Select(source => new Manager()
                {
                    Id = source.EmployeeID,
                    Contact = new Contact()
                    {
                        Id = source.Contact.ContactID,
                        Title = source.Contact.Title,
                        FirstName = source.Contact.FirstName,
                        MiddleName = source.Contact.MiddleName,
                        LastName = source.Contact.LastName,
                        Suffix = source.Contact.Suffix,
                        EmailAddress = source.Contact.EmailAddress,
                        Phone = source.Contact.Phone,
                    },
                    BirthDate = source.BirthDate,
                    Gender = source.Gender,
                    Subordinates = source.Subordinates.Select(sub => new Subordinate()
                    {
                        Id = sub.EmployeeID,
                        BirthDate = sub.BirthDate,
                        Contact = new Contact()
                        {
                            Id = sub.Contact.ContactID,
                            Title = sub.Contact.Title,
                            FirstName = sub.Contact.FirstName,
                            MiddleName = sub.Contact.MiddleName,
                            LastName = sub.Contact.LastName,
                            Suffix = sub.Contact.Suffix,
                            EmailAddress = sub.Contact.EmailAddress,
                            Phone = sub.Contact.Phone,
                        },
                        Gender = sub.Gender,
                        HireDate = sub.HireDate,
                    }).ToList(),
                });
        }
    }
}