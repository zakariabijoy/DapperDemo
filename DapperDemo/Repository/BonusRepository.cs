using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDemo.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private IDbConnection db;
        public BonusRepository(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }


        public List<Company> GetAllCompanyWithEmployees()
        {
            var sql = "select c.*, e.* from Employees as e inner join Companies as c on e.CompanyId = c.CompanyId";

            var companyDic = new Dictionary<int, Company>();

            var company = db.Query<Company, Employee, Company>(sql, (c, e) =>
            {
                if(!companyDic.TryGetValue(c.CompanyId, out var currentCompany))
                {
                    currentCompany = c;
                    companyDic.Add(currentCompany.CompanyId, currentCompany);
                }
                currentCompany.Employees.Add(e);
                return currentCompany;
            }, splitOn:"EmployeeId");

            return company.Distinct().ToList();
        }

        public Company GetCompanyWithEmployees(int id)
        {
            var p = new
            {
                CompanyId = id
            };

            var sql = "SELECT * From Companies WHERE CompanyId = @CompanyId;" + "SELECT * From Employees WHERE CompanyId = @CompanyId;";

            Company company;

            using var lists = db.QueryMultiple(sql, p);
            company = lists.Read<Company>().ToList().FirstOrDefault();
            company.Employees = lists.Read<Employee>().ToList();

            return company;
        }

        public List<Employee> GetEmployeesWithCompany(int id)
        {
            var sql = "select e.*, c.* from Employees as e inner join Companies as c on e.CompanyId = c.CompanyId";
            if(id != 0)
            {
                sql += " where e.CompanyId = @Id";
            }

            var employee = db.Query<Employee,Company, Employee>(sql, (emp, com) => {
                emp.Company = com;
                return emp;
            },new {id },splitOn:"CompanyId");

            return employee.ToList();
        }
    }
}
