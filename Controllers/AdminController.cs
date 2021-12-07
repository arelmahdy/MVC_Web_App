using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Web_App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Web_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDB db;
        public static string message { get; set; }
        public static string successmsg { get; set; }
        public AdminController(ApplicationDB _db)
        {
            db = _db;
        }
        public IActionResult Home()
        {
            return View();
        }
    }
}
