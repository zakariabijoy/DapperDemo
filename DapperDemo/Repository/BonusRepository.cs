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
