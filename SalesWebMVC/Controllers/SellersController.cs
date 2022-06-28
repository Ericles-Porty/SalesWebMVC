using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerServices _sellerServices;
        private readonly DepartmentServices _departmentServices;

        public SellersController(SellerServices sellerServices, DepartmentServices departmentServices)
        {
            _sellerServices = sellerServices;
            _departmentServices = departmentServices;
        }

        // GET: Sellers
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _sellerServices.FindAllAsync());
        }

        // GET: Sellers/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var seller = await _sellerServices.FindByIdAsync(id);
            return seller != null ? View(seller) : RedirectToAction(nameof(Error), new ErrorViewModel { Message = "Seller not found!" });
        }

        // GET: Sellers/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentServices.FindAllAsync();
            var viewModel = new SellerFormViewModel() { Departments = departments };
            return View(viewModel);
        }

        // POST: Sellers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,BirthDate,BaseSalary,DepartmentId")] Seller seller)
        {

            if (!ModelState.IsValid)
            {
                var departments = await _departmentServices.FindAllAsync();
                var viewModel = new SellerFormViewModel { Departments = departments, Seller = seller };
                return View(viewModel);
            }
            await _sellerServices.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        // GET: Sellers/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var departments = await _departmentServices.FindAllAsync();
            var seller = await _sellerServices.FindByIdAsync(id);
            var viewModel = new SellerFormViewModel() { Departments = departments, Seller = seller };
            return seller != null ? View(viewModel) : RedirectToAction(nameof(Error), new ErrorViewModel { Message = "Seller not found!" });
        }

        // POST: Sellers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,BirthDate,BaseSalary,DepartmentId")] Seller seller)
        {
            if (id != seller.Id) return RedirectToAction(nameof(Error), new ErrorViewModel { Message = "Seller ID mismatch!" });

            if (ModelState.IsValid)
            {
                try
                {
                    await _sellerServices.UpdateAsync(seller);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!(await _sellerServices.SellerExistsAsync(seller.Id)))
                        return RedirectToAction(nameof(Error), new ErrorViewModel { Message = e.Message });
                    else
                        throw;

                }
                return RedirectToAction(nameof(Index));
            }
            var departments = await _departmentServices.FindAllAsync();
            var viewModel = new SellerFormViewModel() { Departments = departments, Seller = seller };
            return View(viewModel);
        }

        // GET: Sellers/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var seller = await _sellerServices.FindByIdAsync(id);
            return seller != null ? View(seller) : RedirectToAction(nameof(Error), new ErrorViewModel { Message = "Seller not found!" });
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seller = await _sellerServices.FindByIdAsync(id);
            if (seller != null) await _sellerServices.DeleteAsync(seller);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
