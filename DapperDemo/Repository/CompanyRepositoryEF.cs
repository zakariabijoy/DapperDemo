using DapperDemo.Data;
using DapperDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDemo.Repository
{
    public class CompanyRepositoryEF : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepositoryEF(ApplicationDbContext context)
        {
            _context = context;
        }
        public Company Add(Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
            return company;
        }

        public Company Find(int id)
        {
            return _context.Companies.Find(id);
        }

        public List<Company> GetAll()
        {
            return _context.Companies.ToList();
        }

        public void Remove(int id)
        {
            var company = _context.Companies.Find(id);
            _context.Companies.Remove(company);
            _context.SaveChanges();
        }

        public Company Update(Company company)
        {
            _context.Companies.Update(company);
            _context.SaveChanges();
            return company;
        }
    }
}
