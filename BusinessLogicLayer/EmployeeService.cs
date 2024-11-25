using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }

        // Get all employees
        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _employeeRepository.GetEmployeesAsync();
        }

        // Add a new employee with validation logic
        public async Task AddEmployeeAsync(Employee employee)
        {
           
            var employees = await _employeeRepository.GetEmployeesAsync();

         
            var totalSalaries = employees.Sum(e => e.Salary);
            if (totalSalaries + employee.Salary > 1000000)
                throw new InvalidOperationException("Total salaries cannot exceed $1,000,000.");

            
            if (employee.Salary > 100000)
                throw new InvalidOperationException("An employee's salary cannot exceed $100,000.");

         
            var engineerCount = employees.Count(e => e.Position == "Engineer");
            if (employee.Position == "Engineer" && engineerCount >= 5)
                throw new InvalidOperationException("Cannot add more than 5 employees with the position 'Engineer'.");

           
            var managerCount = employees.Count(e => e.Position == "Manager");
            if (employee.Position == "Manager" && managerCount >= 5)
                throw new InvalidOperationException("Cannot add more than 5 employees with the position 'Manager'.");

           
            var hrCount = employees.Count(e => e.Position == "HR");
            if (employee.Position == "HR" && hrCount >= 1)
                throw new InvalidOperationException("Cannot add more than 1 employee with the position 'HR'.");

    
            await _employeeRepository.AddEmployeeAsync(employee);
        }

       
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }

       
        public async Task GetDetailsAsync(Employee employee)
        {
            await _employeeRepository.GetDetailsAsync(employee);
        }

       
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            
            var employees = await _employeeRepository.GetEmployeesAsync();

          
            var totalSalaries = employees.Where(e => e.Id != employee.Id).Sum(e => e.Salary) + employee.Salary;
            if (totalSalaries > 1_000_000)
                throw new InvalidOperationException("Total salaries cannot exceed $1,000,000.");

          
            if (employee.Salary > 100_000)
                throw new InvalidOperationException("An employee's salary cannot exceed $100,000.");

           
            var engineerCount = employees.Where(e => e.Id != employee.Id).Count(e => e.Position == "Engineer");
            if (employee.Position == "Engineer" && engineerCount >= 5)
                throw new InvalidOperationException("Cannot have more than 5 employees with the position 'Engineer'.");

    
            var managerCount = employees.Where(e => e.Id != employee.Id).Count(e => e.Position == "Manager");
            if (employee.Position == "Manager" && managerCount >= 5)
                throw new InvalidOperationException("Cannot have more than 5 employees with the position 'Manager'.");

            var hrCount = employees.Where(e => e.Id != employee.Id).Count(e => e.Position == "HR");
            if (employee.Position == "HR" && hrCount >= 1)
                throw new InvalidOperationException("Cannot have more than 1 employee with the position 'HR'.");

            
            await _employeeRepository.UpdateEmployeeAsync(employee);
        }
    }
}

