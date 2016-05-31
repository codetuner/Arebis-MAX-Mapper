using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Max.Domain.Mapping;
using Max.Domain.Mapping.Entity;
using Sample.Business;
using Sample.WcfService.Mapping;

namespace Sample.WcfService
{
    [ServiceBehavior]
    public class SampleService : Sample.Contracts.ISampleService
    {
        private AdventureWorksEntities context = new AdventureWorksEntities();

        [OperationBehavior()]
        IEnumerable<Contracts.EmployeeItem> Contracts.ISampleService.ListEmployeesByBirthday(DateTime birthDate)
        {
            using (var context = new AdventureWorksEntities())
            {
                //return new Mapper().MapToEmployeeItemCollection(
                //    context.Employees
                //        .Where(e => (e.BirthDate.Day == birthDate.Day) && (e.BirthDate.Month == birthDate.Month))
                //        .OrderBy(e => e.BirthDate).ThenBy(e => e.EmployeeID)
                //);
                return context.Employees
                        .Where(e => (e.BirthDate.Day == birthDate.Day) && (e.BirthDate.Month == birthDate.Month))
                        .OrderBy(e => e.BirthDate).ThenBy(e => e.EmployeeID)
                        .SelectAsEmployeeItemCollection()
                        .ToList();
            }
        }

        [OperationBehavior()]
        IEnumerable<Contracts.Employee> Contracts.ISampleService.ListSubordinates(int managerId)
        {
            using (var context = new AdventureWorksEntities())
            {
                return new Mapper().MapToEmployeeCollection(
                    context.Employees
                        .Where(e => e.ManagerID == managerId)
                        .OrderBy(e => e.EmployeeID)
                );
            }
        }

        [OperationBehavior()]
        IEnumerable<Contracts.Employee> Contracts.ISampleService.ListTopManagers()
        {
            using (var context = new AdventureWorksEntities())
            {
                return new Mapper().MapToEmployeeCollection(
                    context.Employees
                        .Where(e => e.ManagerID == null)
                        .OrderBy(e => e.EmployeeID)
                );
            }
        }


        [OperationBehavior()]
        IEnumerable<Contracts.Manager> Contracts.ISampleService.ListTopManagers2()
        {
            using (var context = new AdventureWorksEntities())
            {
                return
                    context.Employees
                        .Where(e => e.ManagerID == null)
                        .OrderBy(e => e.EmployeeID)
                        .SelectAsEmployeeItemCollectionManual()
                        .ToList();
            }
        }

        [OperationBehavior()]
        IEnumerable<Contracts.SalesPerson> Contracts.ISampleService.ListAllSalesPersons()
        {
            using (var context = new AdventureWorksEntities())
            {
                return new Mapper().MapToSalesPersonCollection(
                    context.Employees.OfType<SalesPerson>()
                        .Include("Contact")
                        .OrderBy(e => e.EmployeeID)
                );
            }
        }

        [OperationBehavior()]
        IEnumerable<Contracts.EmployeeItem> Contracts.ISampleService.ListAllSalesPersonsMapped()
        {
            // Use regular mapping:

            using (var context = new AdventureWorksEntities())
            {
                return new Mapper().MapToEmployeeItemCollection(
                    context.Employees.OfType<SalesPerson>()
                        .Include("Contact")
                        .Include("Manager.Contact")
                        .OrderBy(e => e.EmployeeID)
                );
            }
        }

        [OperationBehavior()]
        IEnumerable<Contracts.EmployeeItem> Contracts.ISampleService.ListAllSalesPersonsMappedMissingIncludes()
        {
            // Use regular mapping:

            using (var context = new AdventureWorksEntities())
            {
                return new Mapper().MapToEmployeeItemCollection(
                    context.Employees.OfType<SalesPerson>()
                        .Include("Contact")
                        //.Include("Manager.Contact")
                        .OrderBy(e => e.EmployeeID)
                );
            }
        }

        [OperationBehavior()]
        IEnumerable<Contracts.EmployeeItem> Contracts.ISampleService.ListAllSalesPersonsProjected()
        {
            // Bypasses entity materialization and mapping by using linq select projection:

            using (var context = new AdventureWorksEntities())
            {
                return
                    context.Employees.OfType<SalesPerson>()
                        .Include("Contact")
                        .Include("Manager.Contact") // Without this include Projections becomes slower !
                        .OrderBy(e => e.EmployeeID)
                        .SelectAsEmployeeItemCollection()
                        .ToList();
            }
        }

        [OperationBehavior()]
        IEnumerable<Contracts.EmployeeItem> Contracts.ISampleService.ListAllSalesPersonsProjectedMissingIncludes()
        {
            // Bypasses entity materialization and mapping by using linq select projection:

            using (var context = new AdventureWorksEntities())
            {
                return
                    context.Employees.OfType<SalesPerson>()
                        .Include("Contact")
                        //.Include("Manager.Contact")
                        .OrderBy(e => e.EmployeeID)
                        .SelectAsEmployeeItemCollection()
                        .ToList();
            }
        }

        [OperationBehavior()]
        IEnumerable<Contracts.SalesPerson> Contracts.ISampleService.ListSalesPersonForTerritory(string territoryName)
        {
            using (var context = new AdventureWorksEntities())
            {
                return new Mapper().MapToSalesPersonCollection(
                    context.Employees.OfType<SalesPerson>()
                        .Where(e => e.Territory.Name == territoryName)
                        .OrderBy(e => e.EmployeeID)
                );
            }
        }

        [OperationBehavior()]
        Contracts.Employee Contracts.ISampleService.GetEmployee(int employeeId)
        {
            using (var context = new AdventureWorksEntities())
            {
                return new Mapper().MapToEmployee(
                    context.Employees
                        .Where(e => e.EmployeeID == employeeId)
                        .SingleOrDefault()
                );
            }
        }

        [OperationBehavior()]
        Contracts.Manager Contracts.ISampleService.GetManager(int employeeId)
        {
            using (var context = new AdventureWorksEntities())
            {
                return new Mapper().MapToManager(
                    context.Employees
                        .Include("Contact")
                        .Include("Subordinates.Contact")
                        .Where(e => e.EmployeeID == employeeId)
                        .SingleOrDefault()
                );
            }
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        Contracts.Employee Contracts.ISampleService.SaveEmployeeWithContact(Contracts.Employee employee)
        {
            using (var context = new AdventureWorksEntities())
            {
                var employeeEntity = new ReverseMapper(new EntityModelObjectSource(context))
                    .AllowCreatingAndUpdating<Employee>()
                    .AllowCreatingAndUpdating<SalesPerson>()
                    .AllowCreatingAndUpdating<Contact>()
                    .MapFromEmployee(employee);

                context.SaveChanges();

                return new Mapper().MapToEmployee(employeeEntity);
            }
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        Contracts.Employee Contracts.ISampleService.SaveEmployeeWithoutContact(Contracts.Employee employee)
        {
            using (var context = new AdventureWorksEntities())
            {
                var employeeEntity = new ReverseMapper(new EntityModelObjectSource(context))
                    .AllowUpdating<Employee>()
                    .MapFromEmployee(employee);

                context.SaveChanges();

                return new Mapper().MapToEmployee(employeeEntity);
            }
        }
    }
}