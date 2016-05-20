using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Sample.Contracts
{
    [ServiceContract]
    public interface ISampleService
    {
        [OperationContract]
        IEnumerable<EmployeeItem> ListEmployeesByBirthday(DateTime birthDate);

        [OperationContract]
        IEnumerable<Employee> ListSubordinates(int managerId);

        [OperationContract]
        IEnumerable<Employee> ListTopManagers();

        [OperationContract]
        IEnumerable<Manager> ListTopManagers2();

        [OperationContract]
        IEnumerable<SalesPerson> ListAllSalesPersons();

        [OperationContract]
        IEnumerable<EmployeeItem> ListAllSalesPersonsMapped();

        [OperationContract]
        IEnumerable<EmployeeItem> ListAllSalesPersonsMappedMissingIncludes();

        [OperationContract]
        IEnumerable<EmployeeItem> ListAllSalesPersonsProjected();

        [OperationContract]
        IEnumerable<EmployeeItem> ListAllSalesPersonsProjectedMissingIncludes();

        [OperationContract]
        IEnumerable<SalesPerson> ListSalesPersonForTerritory(string territoryName);

        [OperationContract]
        Employee GetEmployee(int employeeId);

        [OperationContract]
        Manager GetManager(int employeeId);

        [OperationContract]
        Employee SaveEmployeeWithContact(Contracts.Employee employee);

        [OperationContract]
        Employee SaveEmployeeWithoutContact(Contracts.Employee employee);
    }
}
