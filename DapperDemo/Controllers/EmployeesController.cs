using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DapperDemo.Data;
using DapperDemo.Models;
using DapperDemo.Repository;

namespace DapperDemo.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ICompanyRepository _compRepo;
        private readonly IEmployeeRepository _empRepo;
        private readonly IBonusRepository _bonRepo;

        public EmployeesController(ICompanyRepository compRepo, IEmployeeRepository empRepo, IBonusRepository bonRepository)
        {
            _compRepo = compRepo;
            _empRepo = empRepo;
            _bonRepo = bonRepository;
        }


        public async Task<IActionResult> Index(int companyId =0)
        {
            //List<Employee> employees = _empRepo.GetAll();
            //foreach (var obj in employees)
            //{
            //    obj.Company = _compRepo.Find(obj.CompanyId);
            //}
            List<Employee> employees = _bonRepo.GetEmployeesWithCompany(companyId);
            return View(employees);
        }

        


        public IActionResult Create()
        {
            IEnumerable<SelectListItem> companyList = _compRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CompanyId.ToString()
            });
            ViewBag.CompanyList = companyList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Employee employee)
        {
            if (ModelState.IsValid)
            {
               await _empRepo.AddAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

  
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _empRepo.Find(id.GetValueOrDefault());
            if (employee == null)
            {
                return NotFound();
            }

            IEnumerable<SelectListItem> companyList = _compRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CompanyId.ToString()
            });
            ViewBag.CompanyList = companyList;
            return View(employee);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _empRepo.Update(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

             _empRepo.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));
        }

    }
}
