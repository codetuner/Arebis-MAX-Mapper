using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Max.Domain.Mapping;
using Max.Domain.Mapping.Implementation;
using Sample.Contracts;

namespace Sample.WcfService.Mapping
{
    public static partial class GeneratedReverseMapperExtensions
    {
        #region ReverseMapper extionsions for Contact

        public static List<Sample.Business.Contact> MapFromContactCollection(this ReverseMapper mapper, IEnumerable<Contact> source)
        {
            List<Sample.Business.Contact> result = new List<Sample.Business.Contact>();
            foreach(var item in source)
                result.Add(mapper.MapFromContact(item));
            return result;
        }
        
        public static Sample.Business.Contact MapFromContact(this ReverseMapper mapper, Contact source)
        {
            if (source == null)
                return null;
            else
                return mapper.MapFromContact(source, null);
        }
        
        public static Sample.Business.Contact MapFromContact(this ReverseMapper mapper, Contact source, Sample.Business.Contact target)
        {
            // Null maps to null:
            if (source == null) return null;

            // Check if object already mapped (as in circular reference scenarios):
            object mappedTarget = mapper.GetMappedTarget(source);
            if (Object.ReferenceEquals(mappedTarget, null) == false)
                return (Sample.Business.Contact)mappedTarget;

            // Retrieve target object:
            if (target == null)
                target = mapper.TryGetTarget<Sample.Business.Contact>(source);
            if ((target == null) && (mapper.CanCreate(typeof(Sample.Business.Contact))))
                mapper.RegisterAsNewObject(target = new Sample.Business.Contact());
            if (target == null)
                throw new Max.Domain.Mapping.MappingException(String.Format("Cannot map {0} to an existing instance.", source.GetType().Name));

            // Register mapping:
            mapper.RegisterMapping(source, target);

            // Perform mapping:
            if (mapper.CanUpdate(target))
                mapper.UpdateContact(source, target);

            // Return target:
            return target;
        }

        internal static void UpdateContact(this ReverseMapper mapper, Contact source, Sample.Business.Contact target)
        {
            // Verify null args:
            if (Object.ReferenceEquals(source, null))
                return;
            else if (Object.ReferenceEquals(target, null))
                throw new Max.Domain.Mapping.MappingException("No target provided to map Contact on.");
       
            // Map source to target:
            mapper.UpdateBaseObject(source, target);
            if (target.ContactID != source.Id)
                target.ContactID = source.Id;
            if (target.Title != source.Title)
                target.Title = (String.IsNullOrWhiteSpace(source.Title)) ? null : source.Title.Trim();
            if (target.FirstName != source.FirstName)
                target.FirstName = (String.IsNullOrWhiteSpace(source.FirstName)) ? null : source.FirstName.Trim();
            if (target.MiddleName != source.MiddleName)
                target.MiddleName = (String.IsNullOrWhiteSpace(source.MiddleName)) ? null : source.MiddleName.Trim();
            if (target.LastName != source.LastName)
                target.LastName = (String.IsNullOrWhiteSpace(source.LastName)) ? null : source.LastName.Trim();
            if (target.Suffix != source.Suffix)
                target.Suffix = (String.IsNullOrWhiteSpace(source.Suffix)) ? null : source.Suffix.Trim();
            if (target.EmailAddress != source.EmailAddress)
                target.EmailAddress = (String.IsNullOrWhiteSpace(source.EmailAddress)) ? null : source.EmailAddress.Trim();
            if (target.Phone != source.Phone)
                target.Phone = (String.IsNullOrWhiteSpace(source.Phone)) ? null : source.Phone.Trim();

            // Call partial AfterUpdate method:
            AfterUpdateContact(mapper, source, target);
        }

        static partial void AfterUpdateContact(this ReverseMapper mapper, Contact source, Sample.Business.Contact target);

        #endregion

        #region ReverseMapper extionsions for Employee

        public static List<Sample.Business.Employee> MapFromEmployeeCollection(this ReverseMapper mapper, IEnumerable<Employee> source)
        {
            List<Sample.Business.Employee> result = new List<Sample.Business.Employee>();
            foreach(var item in source)
                result.Add(mapper.MapFromEmployee(item));
            return result;
        }
        
        public static Sample.Business.Employee MapFromEmployee(this ReverseMapper mapper, Employee source)
        {
            if (source == null)
                return null;
            else if (source is SalesPerson)
                return mapper.MapFromSalesPerson((SalesPerson)source, null);
            else
                return mapper.MapFromEmployee(source, null);
        }
        
        public static Sample.Business.Employee MapFromEmployee(this ReverseMapper mapper, Employee source, Sample.Business.Employee target)
        {
            // Null maps to null:
            if (source == null) return null;

            // Check if object already mapped (as in circular reference scenarios):
            object mappedTarget = mapper.GetMappedTarget(source);
            if (Object.ReferenceEquals(mappedTarget, null) == false)
                return (Sample.Business.Employee)mappedTarget;

            // Retrieve target object:
            if (target == null)
                target = mapper.TryGetTarget<Sample.Business.Employee>(source);
            if (target == null)
                throw new Max.Domain.Mapping.MappingException(String.Format("Cannot map {0} to an existing instance.", source.GetType().Name));

            // Register mapping:
            mapper.RegisterMapping(source, target);

            // Perform mapping:
            if (mapper.CanUpdate(target))
                mapper.UpdateEmployee(source, target);

            // Return target:
            return target;
        }

        internal static void UpdateEmployee(this ReverseMapper mapper, Employee source, Sample.Business.Employee target)
        {
            // Verify null args:
            if (Object.ReferenceEquals(source, null))
                return;
            else if (Object.ReferenceEquals(target, null))
                throw new Max.Domain.Mapping.MappingException("No target provided to map Employee on.");
       
            // Map source to target:
            mapper.UpdateSystemObject(source, target);
            if (target.EmployeeID != source.Id)
                target.EmployeeID = source.Id;
            if (target.Gender != source.Gender)
                target.Gender = (String.IsNullOrWhiteSpace(source.Gender)) ? null : source.Gender.Trim();
            if (target.BirthDate != source.BirthDate)
                target.BirthDate = source.BirthDate;
            if (target.HireDate != source.HireDate)
                target.HireDate = source.HireDate;
            if (target.ManagerID != source.ManagerId)
                target.ManagerID = source.ManagerId;

            // Call partial AfterUpdate method:
            AfterUpdateEmployee(mapper, source, target);
        }

        static partial void AfterUpdateEmployee(this ReverseMapper mapper, Employee source, Sample.Business.Employee target);

        #endregion

        #region ReverseMapper extionsions for SalesPerson

        public static List<Sample.Business.SalesPerson> MapFromSalesPersonCollection(this ReverseMapper mapper, IEnumerable<SalesPerson> source)
        {
            List<Sample.Business.SalesPerson> result = new List<Sample.Business.SalesPerson>();
            foreach(var item in source)
                result.Add(mapper.MapFromSalesPerson(item));
            return result;
        }
        
        public static Sample.Business.SalesPerson MapFromSalesPerson(this ReverseMapper mapper, SalesPerson source)
        {
            if (source == null)
                return null;
            else
                return mapper.MapFromSalesPerson(source, null);
        }
        
        public static Sample.Business.SalesPerson MapFromSalesPerson(this ReverseMapper mapper, SalesPerson source, Sample.Business.SalesPerson target)
        {
            // Null maps to null:
            if (source == null) return null;

            // Check if object already mapped (as in circular reference scenarios):
            object mappedTarget = mapper.GetMappedTarget(source);
            if (Object.ReferenceEquals(mappedTarget, null) == false)
                return (Sample.Business.SalesPerson)mappedTarget;

            // Retrieve target object:
            if (target == null)
                target = mapper.TryGetTarget<Sample.Business.SalesPerson>(source);
            if (target == null)
                throw new Max.Domain.Mapping.MappingException(String.Format("Cannot map {0} to an existing instance.", source.GetType().Name));

            // Register mapping:
            mapper.RegisterMapping(source, target);

            // Perform mapping:
            if (mapper.CanUpdate(target))
                mapper.UpdateSalesPerson(source, target);

            // Return target:
            return target;
        }

        internal static void UpdateSalesPerson(this ReverseMapper mapper, SalesPerson source, Sample.Business.SalesPerson target)
        {
            // Verify null args:
            if (Object.ReferenceEquals(source, null))
                return;
            else if (Object.ReferenceEquals(target, null))
                throw new Max.Domain.Mapping.MappingException("No target provided to map SalesPerson on.");
       
            // Map source to target:
            mapper.UpdateEmployee(source, target);
            if (target.SalesQuota != source.SalesQuota)
                target.SalesQuota = source.SalesQuota;
            if (target.Bonus != source.Bonus)
                target.Bonus = source.Bonus;
            if (target.CommissionPct != source.CommissionPct)
                target.CommissionPct = source.CommissionPct;
            if (target.SalesYTD != source.SalesYTD)
                target.SalesYTD = source.SalesYTD;

            // Call partial AfterUpdate method:
            AfterUpdateSalesPerson(mapper, source, target);
        }

        static partial void AfterUpdateSalesPerson(this ReverseMapper mapper, SalesPerson source, Sample.Business.SalesPerson target);

        #endregion

    }
}