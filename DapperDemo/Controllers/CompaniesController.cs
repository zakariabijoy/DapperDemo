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

        public EmployeesController(ICompanyRepository compRepo, IEmployeeRepository empRepo)
        {
            _compRepo = compRepo;
            _empRepo = empRepo; 
        }


        public async Task<IActionResult> Index()
        {
            return View( _empRepo.GetAll());
        }

        


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Employee employee)
        {
            if (ModelState.IsValid)
            {
                _empRepo.Add(employee);
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
