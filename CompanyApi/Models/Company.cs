using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    [Serializable]
    public class Company
    {
        public Company()
        {
        }

        public Company(string name)
        {
            Name = name;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();

        public override bool Equals(object? obj)
        {
            return Equals(obj as Company);
        }

        public bool Equals(Company anotherCompany)
        {
            if (anotherCompany == null)
            {
                return false;
            }

            return this.Id == anotherCompany.Id && this.Name == anotherCompany.Name 
                                                && Enumerable.SequenceEqual(this.Employees, anotherCompany.Employees);
        }
    }
}
