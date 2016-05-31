using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using Sample.Contracts;

namespace Sample.Tests
{
    [TestClass]
    public class SampleServiceTests
    {
        [TestMethod]
        public void ListTopManagersTest()
        { 
            using (var factory = new ChannelFactory<ISampleService>("SampleServiceEndpoint"))
            {
                var channel = factory.CreateChannel();

                var result = channel.ListTopManagers();

                Assert.AreEqual(109, result.First().Id);
            }
        }

        [TestMethod]
        public void ListAllSalesPersonsTest()
        {
            using (var factory = new ChannelFactory<ISampleService>("SampleServiceEndpoint"))
            {
                var channel = factory.CreateChannel();

                var result = channel.ListAllSalesPersons();

                foreach (var item in result)
                    Console.WriteLine("{0} - {1} {2}", item.Id, item.Contact.FirstName, item.Contact.LastName);

                Assert.AreEqual(17, result.Count());
            }
        }

        [TestMethod]
        public void GetEmployeeSalesPersonTest()
        {
            using (var factory = new ChannelFactory<ISampleService>("SampleServiceEndpoint"))
            {
                var channel = factory.CreateChannel();

                var result = channel.GetEmployee(282);

                Assert.AreEqual(282, result.Id);
                Assert.IsInstanceOfType(result, typeof(SalesPerson));
            }
        }
    }
}
