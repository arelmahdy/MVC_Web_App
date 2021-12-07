using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Web_App.Data;
using MVC_Web_App.Models;

namespace MVC_Web_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppUsersController : Controller
    {
        private readonly ApplicationDB _context;
        public static string message { get; set; }
        public static string successmsg { get; set; }
        public static string pass { get; set; }
        public static string user { get; set; }
        public static string email { get; set; }
        public AppUsersController(ApplicationDB context)
        {
            _context = context;
        }

        // GET: AppUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppUser.ToListAsync());
        }

        // GET: AppUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUsers = await _context.AppUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUsers == null)
            {
                return NotFound();
            }

            return View(appUsers);
        }

        // GET: AppUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,PasswordConfirm,Email,EmilConfirm,Phone")] AppUsers appUsers)
        {
            message = string.Empty;
            successmsg = string.Empty;

            if (ModelState.IsValid)
            {
                appUsers.Id = Guid.NewGuid().ToString();
                string input = appUsers.Password;
                if (!string.IsNullOrEmpty(input))
                {

                    DataTable dt = new DataTable();

                    Users users = new Users();
                    string username = appUsers.UserName;
                    string email = appUsers.Email;
                    dt = users.chekUserNameExist(username);

                    if (dt.Rows.Count < 1)
                    {
                        if (!IsEmailAdressExist(email))
                        {
                            appUsers.Password = AppHash.HashPassword(input);
                            appUsers.PasswordConfirm = AppHash.HashPassword(input);
                            _context.Add(appUsers);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            message = " البريد الالكترونى المدخل (" + appUsers.Email + ")  مستعمل ";
                            return View();
                        }
                    }
                    else
                    {
                        message = "اسم المستخدم المدخل (" + appUsers.UserName + ")  مستعمل ";
                        return View();
                    }

                }

            }
            return View(appUsers);
        }
        public bool IsEmailAdressExist(string email)
        {
            return _context.AppUser.Any(a => a.Email == email);
        }
        // GET: AppUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUsers = await _context.AppUser.FindAsync(id);
            if (appUsers == null)
            {
                return NotFound();
            }
            pass = appUsers.Password;
            ViewBag.pass = pass;
            user = appUsers.UserName;
            email = appUsers.Email;
            return View(appUsers);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Password,PasswordConfirm,Email,EmilConfirm,Phone")] AppUsers appUsers)
        {
            if (id != appUsers.Id)
            {
                return NotFound();
            }

                message = string.Empty;
                successmsg = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    string input = appUsers.Password;
                    if (input != pass)
                    {
                        appUsers.Password = AppHash.HashPassword(input);
                        appUsers.PasswordConfirm = AppHash.HashPassword(input);
                    }

                    DataTable dt = new DataTable();
                    Users users = new Users();

                    if (user != appUsers.UserName)
                    {
                        dt = users.chekUserNameExist(appUsers.UserName);
                        if (dt.Rows.Count > 1)
                        {
                            message = "اسم المستخدم المدخل (" + appUsers.UserName + ")  مستعمل ";
                            return View();
                        }
                    }
                    if (email != appUsers.Email)
                    {
                        if (IsEmailAdressExist(email))
                        {
                            message = " البريد الالكترونى المدخل (" + appUsers.Email + ")  مستعمل ";
                            return View();
                        }
                    }

                    _context.Update(appUsers);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUsersExists(appUsers.Id))
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
            return View(appUsers);
        }

        // GET: AppUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUsers = await _context.AppUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUsers == null)
            {
                return NotFound();
            }

            return View(appUsers);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var appUsers = await _context.AppUser.FindAsync(id);
            _context.AppUser.Remove(appUsers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUsersExists(string id)
        {
            return _context.AppUser.Any(e => e.Id == id);
        }
    }
}
