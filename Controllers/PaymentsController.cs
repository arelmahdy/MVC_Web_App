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
    public class PaymentsController : Controller
    {
        private readonly ApplicationDB _context;

        public PaymentsController(ApplicationDB context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var applicationDB = _context.Payments.Include(p => p.Cart).Include(p => p.appUser).Include(p => p.billingAddress);
            return View(await applicationDB.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Cart)
                .Include(p => p.appUser)
                .Include(p => p.billingAddress)
                .FirstOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["cartId"] = new SelectList(_context.Carts, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "Id");
            ViewData["billingId"] = new SelectList(_context.BillingAddresses, "id", "Address");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,cardType,cardName,cardNumber,expiration,cvv,cartId,billingId,UserId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["cartId"] = new SelectList(_context.Carts, "Id", "Id", payment.cartId);
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "Id", payment.UserId);
            ViewData["billingId"] = new SelectList(_context.BillingAddresses, "id", "Address", payment.billingId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["cartId"] = new SelectList(_context.Carts, "Id", "Id", payment.cartId);
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "Id", payment.UserId);
            ViewData["billingId"] = new SelectList(_context.BillingAddresses, "id", "Address", payment.billingId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,cardType,cardName,cardNumber,expiration,cvv,cartId,billingId,UserId")] Payment payment)
        {
            if (id != payment.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.id))
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
            ViewData["cartId"] = new SelectList(_context.Carts, "Id", "Id", payment.cartId);
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "Id", payment.UserId);
            ViewData["billingId"] = new SelectList(_context.BillingAddresses, "id", "Address", payment.billingId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Cart)
                .Include(p => p.appUser)
                .Include(p => p.billingAddress)
                .FirstOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.id == id);
        }
    }
}
