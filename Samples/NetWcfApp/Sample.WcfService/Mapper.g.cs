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
    public static partial class GeneratedMapperExtensions
    {
        #region Mapper extionsions for Contact

        public static Collection<Contact> MapToContactCollection(this Mapper mapper, IEnumerable<Sample.Business.Contact> source)
        {
            Collection<Contact> target = new Collection<Contact>();
            foreach(var item in source) target.Add(mapper.MapToContact(item));
            return target;
        }

        public static Contact MapToContact(this Mapper mapper, Sample.Business.Contact source)
        {
            if (source == null)
                return null;
            else
                return mapper.MapToContact(source, new Contact());
        }
        
        internal static Contact MapToContact(this Mapper mapper, Sample.Business.Contact source, Contact target)
        {
            // Null maps to null:
            if (source == null) return null;

            // Check if object already mapped (as in circular reference scenarios):
            object mappedTarget = mapper.GetMappedTarget(source);
            
            // If so, return mapped instance:
            if (Object.ReferenceEquals(mappedTarget, null) == false)
                return (Contact)mappedTarget;

            // Else, register mapping and map target:
            mapper.RegisterMapping(source, target);
            mapper.UpdateContact(source, target);
            
            // Return mapped target:
            return target;
        }
        
        internal static void UpdateContact(this Mapper mapper, Sample.Business.Contact source, Contact target)
        {
            // Verify null args:
            if (Object.ReferenceEquals(source, null))
                return;
            else if (Object.ReferenceEquals(target, null))
                throw new Max.Domain.Mapping.MappingException("No target provided to map Contact on.");

            // Perform base type mapping:
            mapper.UpdateBaseObject(source, target);

            // Perform mapping of properties:
            target.Id = source.ContactID;
            target.Title = source.Title;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;
            target.EmailAddress = source.EmailAddress;
            target.Phone = source.Phone;

            // Call partial AfterUpdate method:
            AfterUpdateContact(mapper, source, target);
        }

        static partial void AfterUpdateContact(this Mapper mapper, Sample.Business.Contact source, Contact target);

        #endregion

        #region Mapper extionsions for Employee

        public static Collection<Employee> MapToEmployeeCollection(this Mapper mapper, IEnumerable<Sample.Business.Employee> source)
        {
            Collection<Employee> target = new Collection<Employee>();
            foreach(var item in source) target.Add(mapper.MapToEmployee(item));
            return target;
        }

        public static Employee MapToEmployee(this Mapper mapper, Sample.Business.Employee source)
        {
            if (source == null)
                return null;
            else if (source is Sample.Business.SalesPerson)
                return mapper.MapToSalesPerson((Sample.Business.SalesPerson)source);
            else
                return mapper.MapToEmployee(source, new Employee());
        }
        
        internal static Employee MapToEmployee(this Mapper mapper, Sample.Business.Employee source, Employee target)
        {
            // Null maps to null:
            if (source == null) return null;

            // Check if object already mapped (as in circular reference scenarios):
            object mappedTarget = mapper.GetMappedTarget(source);
            
            // If so, return mapped instance:
            if (Object.ReferenceEquals(mappedTarget, null) == false)
                return (Employee)mappedTarget;

            // Else, register mapping and map target:
            mapper.RegisterMapping(source, target);
            mapper.UpdateEmployee(source, target);
            
            // Return mapped target:
            return target;
        }
        
        internal static void UpdateEmployee(this Mapper mapper, Sample.Business.Employee source, Employee target)
        {
            // Verify null args:
            if (Object.ReferenceEquals(source, null))
                return;
            else if (Object.ReferenceEquals(target, null))
                throw new Max.Domain.Mapping.MappingException("No target provided to map Employee on.");

            // Perform base type mapping:
            mapper.UpdateSystemObject(source, target);

            // Perform mapping of properties:
            target.Id = source.EmployeeID;
            target.Contact = mapper.MapToContact(source.Contact);
            target.Gender = source.Gender;
            target.BirthDate = source.BirthDate;
            target.HireDate = source.HireDate;
            target.ManagerId = source.ManagerID;

            // Call partial AfterUpdate method:
            AfterUpdateEmployee(mapper, source, target);
        }

        static partial void AfterUpdateEmployee(this Mapper mapper, Sample.Business.Employee source, Employee target);

        #endregion

        #region Mapper extionsions for EmployeeItem

        public static Collection<EmployeeItem> MapToEmployeeItemCollection(this Mapper mapper, IEnumerable<Sample.Business.Employee> source)
        {
            Collection<EmployeeItem> target = new Collection<EmployeeItem>();
            foreach(var item in source) target.Add(mapper.MapToEmployeeItem(item));
            return target;
        }

        public static EmployeeItem MapToEmployeeItem(this Mapper mapper, Sample.Business.Employee source)
        {
            if (source == null)
                return null;
            else
                return mapper.MapToEmployeeItem(source, new EmployeeItem());
        }
        
        internal static EmployeeItem MapToEmployeeItem(this Mapper mapper, Sample.Business.Employee source, EmployeeItem target)
        {
            // Null maps to null:
            if (source == null) return null;

            // Check if object already mapped (as in circular reference scenarios):
            object mappedTarget = mapper.GetMappedTarget(source);
            
            // If so, return mapped instance:
            if (Object.ReferenceEquals(mappedTarget, null) == false)
                return (EmployeeItem)mappedTarget;

            // Else, register mapping and map target:
            mapper.RegisterMapping(source, target);
            mapper.UpdateEmployeeItem(source, target);
            
            // Return mapped target:
            return target;
        }
        
        internal static void UpdateEmployeeItem(this Mapper mapper, Sample.Business.Employee source, EmployeeItem target)
        {
            // Verify null args:
            if (Object.ReferenceEquals(source, null))
                return;
            else if (Object.ReferenceEquals(target, null))
                throw new Max.Domain.Mapping.MappingException("No target provided to map EmployeeItem on.");

            // Perform base type mapping:
            mapper.UpdateSystemObject(source, target);

            // Perform mapping of properties:
            target.Id = source.EmployeeID;
            target.Gender = source.Gender;
            target.BirthDate = source.BirthDate;
            target.ManagerId = source.ManagerID;
            mapper.MapEmployeeItemTitleProperty(source, target);
            mapper.MapEmployeeItemFirstNameProperty(source, target);
            mapper.MapEmployeeItemMiddleNameProperty(source, target);
            mapper.MapEmployeeItemLastNameProperty(source, target);
            mapper.MapEmployeeItemManagerFirstNameProperty(source, target);
            mapper.MapEmployeeItemManagerMiddleNameProperty(source, target);
            mapper.MapEmployeeItemManagerLastNameProperty(source, target);

            // Call partial AfterUpdate method:
            AfterUpdateEmployeeItem(mapper, source, target);
        }

        static partial void AfterUpdateEmployeeItem(this Mapper mapper, Sample.Business.Employee source, EmployeeItem target);


        static void MapEmployeeItemTitleProperty(this Mapper mapper, Sample.Business.Employee source, EmployeeItem target)
        {
            do
            {
                var item0 = source;
                var item1 = item0.Contact;
                if (item1 == null) break;
                var item2 = item1.Title;
                target.Title = item2;
            } while (false);
        }

        static void MapEmployeeItemFirstNameProperty(this Mapper mapper, Sample.Business.Employee source, EmployeeItem target)
        {
            do
            {
                var item0 = source;
                var item1 = item0.Contact;
                if (item1 == null) break;
                var item2 = item1.FirstName;
                target.FirstName = item2;
            } while (false);
        }

        static void MapEmployeeItemMiddleNameProperty(this Mapper mapper, Sample.Business.Employee source, EmployeeItem target)
        {
            do
            {
                var item0 = source;
                var item1 = item0.Contact;
                if (item1 == null) break;
                var item2 = item1.MiddleName;
                target.MiddleName = item2;
            } while (false);
        }

        static void MapEmployeeItemLastNameProperty(this Mapper mapper, Sample.Business.Employee source, EmployeeItem target)
        {
            do
            {
                var item0 = source;
                var item1 = item0.Contact;
                if (item1 == null) break;
                var item2 = item1.LastName;
                target.LastName = item2;
            } while (false);
        }

        static void MapEmployeeItemManagerFirstNameProperty(this Mapper mapper, Sample.Business.Employee source, EmployeeItem target)
        {
            do
            {
                var item0 = source;
                var item1 = item0.Manager;
                if (item1 == null) break;
                var item2 = item1.Contact;
                if (item2 == null) break;
                var item3 = item2.FirstName;
                target.ManagerFirstName = item3;
            } while (false);
        }

        static void MapEmployeeItemManagerMiddleNameProperty(this Mapper mapper, Sample.Business.Employee source, EmployeeItem target)
        {
            do
            {
                var item0 = source;
                var item1 = item0.Manager;
                if (item1 == null) break;
                var item2 = item1.Contact;
                if (item2 == null) break;
                var item3 = item2.MiddleName;
                target.ManagerMiddleName = item3;
            } while (false);
        }

        static void MapEmployeeItemManagerLastNameProperty(this Mapper mapper, Sample.Business.Employee source, EmployeeItem target)
        {
            do
            {
                var item0 = source;
                var item1 = item0.Manager;
                if (item1 == null) break;
                var item2 = item1.Contact;
                if (item2 == null) break;
                var item3 = item2.LastName;
                target.ManagerLastName = item3;
            } while (false);
        }
        #endregion

        #region Mapper extionsions for Manager

        public static Collection<Manager> MapToManagerCollection(this Mapper mapper, IEnumerable<Sample.Business.Employee> source)
        {
            Collection<Manager> target = new Collection<Manager>();
            foreach(var item in source) target.Add(mapper.MapToManager(item));
            return target;
        }

        public static Manager MapToManager(this Mapper mapper, Sample.Business.Employee source)
        {
            if (source == null)
                return null;
            else
                return mapper.MapToManager(source, new Manager());
        }
        
        internal static Manager MapToManager(this Mapper mapper, Sample.Business.Employee source, Manager target)
        {
            // Null maps to null:
            if (source == null) return null;

            // Check if object already mapped (as in circular reference scenarios):
            object mappedTarget = mapper.GetMappedTarget(source);
            
            // If so, return mapped instance:
            if (Object.ReferenceEquals(mappedTarget, null) == false)
                return (Manager)mappedTarget;

            // Else, register mapping and map target:
            mapper.RegisterMapping(source, target);
            mapper.UpdateManager(source, target);
            
            // Return mapped target:
            return target;
        }
        
        internal static void UpdateManager(this Mapper mapper, Sample.Business.Employee source, Manager target)
        {
            // Verify null args:
            if (Object.ReferenceEquals(source, null))
                return;
            else if (Object.ReferenceEquals(target, null))
                throw new Max.Domain.Mapping.MappingException("No target provided to map Manager on.");

            // Perform base type mapping:
            mapper.UpdateSystemObject(source, target);

            // Perform mapping of properties:
            target.Id = source.EmployeeID;
            target.Contact = mapper.MapToContact(source.Contact);
            target.Gender = source.Gender;
            target.BirthDate = source.BirthDate;
            target.HireDate = source.HireDate;
            foreach(var item in source.Subordinates) target.Subordinates.Add(mapper.MapToSubordinate((Sample.Business.Employee)item));

            // Call partial AfterUpdate method:
            AfterUpdateManager(mapper, source, target);
        }

        static partial void AfterUpdateManager(this Mapper mapper, Sample.Business.Employee source, Manager target);

        #endregion

        #region Mapper extionsions for SalesPerson

        public static Collection<SalesPerson> MapToSalesPersonCollection(this Mapper mapper, IEnumerable<Sample.Business.SalesPerson> source)
        {
            Collection<SalesPerson> target = new Collection<SalesPerson>();
            foreach(var item in source) target.Add(mapper.MapToSalesPerson(item));
            return target;
        }

        public static SalesPerson MapToSalesPerson(this Mapper mapper, Sample.Business.SalesPerson source)
        {
            if (source == null)
                return null;
            else
                return mapper.MapToSalesPerson(source, new SalesPerson());
        }
        
        internal static SalesPerson MapToSalesPerson(this Mapper mapper, Sample.Business.SalesPerson source, SalesPerson target)
        {
            // Null maps to null:
            if (source == null) return null;

            // Check if object already mapped (as in circular reference scenarios):
            object mappedTarget = mapper.GetMappedTarget(source);
            
            // If so, return mapped instance:
            if (Object.ReferenceEquals(mappedTarget, null) == false)
                return (SalesPerson)mappedTarget;

            // Else, register mapping and map target:
            mapper.RegisterMapping(source, target);
            mapper.UpdateSalesPerson(source, target);
            
            // Return mapped target:
            return target;
        }
        
        internal static void UpdateSalesPerson(this Mapper mapper, Sample.Business.SalesPerson source, SalesPerson target)
        {
            // Verify null args:
            if (Object.ReferenceEquals(source, null))
                return;
            else if (Object.ReferenceEquals(target, null))
                throw new Max.Domain.Mapping.MappingException("No target provided to map SalesPerson on.");

            // Perform base type mapping:
            mapper.UpdateEmployee(source, target);

            // Perform mapping of properties:
            target.SalesQuota = source.SalesQuota;
            target.Bonus = source.Bonus;
            target.CommissionPct = source.CommissionPct;
            target.SalesYTD = source.SalesYTD;
            mapper.MapSalesPersonTerritorySalesYTDProperty(source, target);

            // Call partial AfterUpdate method:
            AfterUpdateSalesPerson(mapper, source, target);
        }

        static partial void AfterUpdateSalesPerson(this Mapper mapper, Sample.Business.SalesPerson source, SalesPerson target);


        static void MapSalesPersonTerritorySalesYTDProperty(this Mapper mapper, Sample.Business.SalesPerson source, SalesPerson target)
        {
            do
            {
                var item0 = source;
                var item1 = item0.Territory;
                if (item1 == null) break;
                var item2 = item1.SalesYTD;
                target.TerritorySalesYTD = item2;
            } while (false);
        }
        #endregion

        #region Mapper extionsions for Subordinate

        public static Collection<Subordinate> MapToSubordinateCollection(this Mapper mapper, IEnumerable<Sample.Business.Employee> source)
        {
            Collection<Subordinate> target = new Collection<Subordinate>();
            foreach(var item in source) target.Add(mapper.MapToSubordinate(item));
            return target;
        }

        public static Subordinate MapToSubordinate(this Mapper mapper, Sample.Business.Employee source)
        {
            if (source == null)
                return null;
            else
                return mapper.MapToSubordinate(source, new Subordinate());
        }
        
        internal static Subordinate MapToSubordinate(this Mapper mapper, Sample.Business.Employee source, Subordinate target)
        {
            // Null maps to null:
            if (source == null) return null;

            // Check if object already mapped (as in circular reference scenarios):
            object mappedTarget = mapper.GetMappedTarget(source);
            
            // If so, return mapped instance:
            if (Object.ReferenceEquals(mappedTarget, null) == false)
                return (Subordinate)mappedTarget;

            // Else, register mapping and map target:
            mapper.RegisterMapping(source, target);
            mapper.UpdateSubordinate(source, target);
            
            // Return mapped target:
            return target;
        }
        
        internal static void UpdateSubordinate(this Mapper mapper, Sample.Business.Employee source, Subordinate target)
        {
            // Verify null args:
            if (Object.ReferenceEquals(source, null))
                return;
            else if (Object.ReferenceEquals(target, null))
                throw new Max.Domain.Mapping.MappingException("No target provided to map Subordinate on.");

            // Perform base type mapping:
            mapper.UpdateSystemObject(source, target);

            // Perform mapping of properties:
            target.Id = source.EmployeeID;
            target.Contact = mapper.MapToContact(source.Contact);
            target.Gender = source.Gender;
            target.BirthDate = source.BirthDate;
            target.HireDate = source.HireDate;

            // Call partial AfterUpdate method:
            AfterUpdateSubordinate(mapper, source, target);
        }

        static partial void AfterUpdateSubordinate(this Mapper mapper, Sample.Business.Employee source, Subordinate target);

        #endregion

    }
}