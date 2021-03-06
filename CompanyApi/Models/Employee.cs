﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    [Serializable]
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(string name, double salary)
        {
            Id = Guid.NewGuid().ToString("N");
            Name = name;
            Salary = salary;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            var anotherEmployee = (Employee)obj;
            return this.Id == anotherEmployee.Id && this.Name == anotherEmployee.Name &&
                   this.Salary == anotherEmployee.Salary;
        }
    }
}
