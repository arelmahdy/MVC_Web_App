using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Web_App.Data;
using MVC_Web_App.Models;

namespace MVC_Web_App.Controllers
{
    public class BillingAddressesController : Controller
    {
        private readonly ApplicationDB _context;

        public BillingAddressesController(ApplicationDB context)
        {
            _context = context;
        }

        // GET: BillingAddresses
        public async Task<IActionResult> Index()
        {
            var applicationDB = _context.BillingAddresses.Include(b => b.appUser);
            return View(await applicationDB.ToListAsync());
        }

        // GET: BillingAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingAddress = await _context.BillingAddresses
                .Include(b => b.appUser)
                .FirstOrDefaultAsync(m => m.id == id);
            if (billingAddress == null)
            {
                return NotFound();
            }

            return View(billingAddress);
        }

        // GET: BillingAddresses/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "Id");
            return View();
        }

        // POST: BillingAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,firstName,lastName,UserName,Email,Address,Country,Zip,UserId")] BillingAddress billingAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billingAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "Id", billingAddress.UserId);
            return View(billingAddress);
        }

        // GET: BillingAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingAddress = await _context.BillingAddresses.FindAsync(id);
            if (billingAddress == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "Id", billingAddress.UserId);
            return View(billingAddress);
        }

        // POST: BillingAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,firstName,lastName,UserName,Email,Address,Country,Zip,UserId")] BillingAddress billingAddress)
        {
            if (id != billingAddress.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billingAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillingAddressExists(billingAddress.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "Id", billingAddress.UserId);
            return View(billingAddress);
        }

        // GET: BillingAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingAddress = await _context.BillingAddresses
                .Include(b => b.appUser)
                .FirstOrDefaultAsync(m => m.id == id);
            if (billingAddress == null)
            {
                return NotFound();
            }

            return View(billingAddress);
        }

        // POST: BillingAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billingAddress = await _context.BillingAddresses.FindAsync(id);
            _context.BillingAddresses.Remove(billingAddress);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillingAddressExists(int id)
        {
            return _context.BillingAddresses.Any(e => e.id == id);
        }
    }
}
