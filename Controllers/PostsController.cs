using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Web_App.Data;
using MVC_Web_App.Models;
using X.PagedList;

namespace MVC_Web_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostsController : Controller
    {
        private readonly ApplicationDB _context;
        private readonly IWebHostEnvironment host;

        public PostsController(ApplicationDB context, IWebHostEnvironment _host)
        {
            _context = context; 
             host = _host;               
        }

        // GET: Posts
        [HttpGet]
        public async Task<IActionResult> Index(string search,int? page)
        {
            var applicationDB = new object();
            var pageNumber = page ?? 1;
            if (!string.IsNullOrEmpty(search))
            {
                applicationDB = await _context.Posts.Include(e => e.SubCategory).Where(e => e.Author.Contains(search) || e.Title.Contains(search)).ToList().ToPagedListAsync(pageNumber,3);
            }
            else
            {
                applicationDB = await _context.Posts.OrderByDescending(x => x.Id).Include(p => p.SubCategory).ToList().ToPagedListAsync(pageNumber, 3);
            }
         
            return View(applicationDB);
        }

        [HttpPost]
        public IActionResult Index(IEnumerable<string> ID)
        {
            ViewBag.msg = string.Empty;
            try
            {
                List<string> st = ID.ToList();
                if (st.Count > 0)
                {
                    foreach (var id in st)
                    {
                        long deleteId = 0;
                        try
                        {
                            deleteId = long.Parse(id);
                        }
                        catch { }

                        if (deleteId > 0)
                        {
                            var post = _context.Posts.First(m => m.Id == deleteId);
                            if (post != null)
                            {
                                _context.Posts.Remove(post);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.msg = "لا يوجد اي اختيار بالقائمة";
                }
            }
            catch (Exception ex)
            { ViewBag.msg = ex.Message; }

            return RedirectToAction(nameof(Index));
        }
        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["SubId"] = new SelectList(_context.SubCategories, "Id", "SubCatName");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,PostContent,PostImage,Author,PostDate,PostViews,PostLike,LikeUserName,SubId,IsPublish,ProductName,Price,Discount")] Post post, IFormFile img)
        {

            string id = User.FindFirst(ClaimTypes.Name)?.Value;
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.img = string.Empty;
            string newfilename = string.Empty;
            if (img != null && img.Length > 0)
            {
                newfilename = img.FileName;
                if (IsImageValidate(newfilename))
                {
                    string filename = Path.Combine(host.WebRootPath + "/images/Post" + newfilename);
                    await img.CopyToAsync(new FileStream(filename, FileMode.Create));
                }
                else
                {
                    ViewBag.msg = "الملفات المسموح بها : png, jpeg, jpg, gif, bmp";
                    return View();
                }

            }

            try
            {
                post.Author = id;
                post.LikeUserName = "";
                post.PostDate = DateTime.Now;
                post.PostImage = newfilename;
                post.PostLike = 0;
                post.PostViews = 0;

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch { }
            ViewData["SubId"] = new SelectList(_context.SubCategories, "Id", "SubCatName", post.SubId);
            return View(post);
        }
        private bool IsImageValidate(string filename)
        {
            string extension = Path.GetExtension(filename);
            if (extension.Contains(".png"))
                return true;

            if (extension.Contains(".jpeg"))
                return true;

            if (extension.Contains(".jpg"))
                return true;

            if (extension.Contains(".gif"))
                return true;

            if (extension.Contains(".bmp"))
                return true;

            return false;
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["SubId"] = new SelectList(_context.SubCategories, "Id", "SubCatName", post.SubId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Author, DateTime PostDate, int PostViews, int PostLikes, string LikeUserName,string PostImg,
            [Bind("Id,Title,PostContent,PostImage,Author,PostDate,PostViews,PostLike,LikeUserName,SubId,IsPublish,ProductName,Price,Discount")] Post post, IFormFile img)
        {
            if (id != post.Id)
            {
                return NotFound();
            }
            ViewBag.img = string.Empty;
            string newfilename = string.Empty;
            if (img != null && img.Length > 0)
            {
                newfilename = img.FileName;
                if (IsImageValidate(newfilename))
                {
                    string filename = Path.Combine(host.WebRootPath + "/images/Post" , newfilename);
                    FileStream fs = new FileStream(filename, FileMode.Create);
                    await img.CopyToAsync(fs);
                    fs.Close();
                }
                else
                {
                    ViewBag.msg = "الملفات المسموح بها : png, jpeg, jpg, gif, bmp";
                    return View();
                }

            }

            try
                {
                try
                {
                    post.Author = Author;
                    post.LikeUserName = LikeUserName;
                    post.PostDate = PostDate;
                    if (!string.IsNullOrEmpty(newfilename))
                        post.PostImage = newfilename;
                    else
                        post.PostImage = PostImg;
                    post.PostLike = PostLikes;
                    post.PostViews = PostViews;

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch { }
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
             

            ViewData["SubId"] = new SelectList(_context.SubCategories, "Id", "SubCatName", post.SubId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
