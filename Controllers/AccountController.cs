using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Web_App.Data;
using MVC_Web_App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC_Web_App.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDB db;
        public static string message { get; set; }
        public static string successmsg { get; set; }

        private readonly IWebHostEnvironment host;
        public static bool IsProfileExist { get; set; }
        public static bool RegisterOpen { get; set; }
        public static long UID { get; set; }
        public static string OldImage { get; set; }
        public AccountController(ApplicationDB _db, IWebHostEnvironment _host)
        {
            host = _host;
            db = _db;

        }
        public IActionResult Register()
        {
            if (db.UserSettings.Count() > 0)
            {
                if (IsRegisterOpen() == "true")
                {
                    RegisterOpen = true;
                }
                else
                {
                    RegisterOpen = false;
                }
            }
            else
            {
                RegisterOpen = true;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,UserName,Password,PasswordConfirm,Email,ConfirmEmail,Phone")] AppUsers AppUser)
        {
            message = string.Empty;
            successmsg = string.Empty;
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(IsEmailConfirm()))
                {
                    if (IsEmailConfirm() == "true")
                    {
                        AppUser.EmailConfirm = true;
                    }
                    else
                    {
                        AppUser.EmailConfirm = false;
                    }
                }
                else
                {
                    AppUser.EmailConfirm = false;
                }
                string input = AppUser.Password;
                if (!string.IsNullOrEmpty(input))
                {
                    if (PassWordMinimumLenght() >= 0 && PassWordMaximumLength() <= 0)
                    {
                        int min = PassWordMinimumLenght();
                        int max = PassWordMaximumLength();
                        if (input.Length < min)
                        {
                            message = "الحد الأدني لعدد احرف الباسوورد " + min + " مقاطع";
                            return View();
                        }
                        if (input.Length > max)
                        {
                            message = "الحد الاقصى لعدد حروف الباسورد " + max + " مقاطع";
                            return View();
                        }
                    }
                    if (!string.IsNullOrEmpty(IsPasswordDigit()))
                    {
                        string IsDigit = IsPasswordDigit();
                        if (IsDigit == "true")
                        {
                            if (!input.Any(char.IsDigit))
                            {
                                message = "يجب ان تحتوى كلمة المرور على رقم واحد على الاقل";
                                return View();
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(IsPasswordUpper()))
                    {
                        string IsUpper = IsPasswordUpper();
                        if (IsUpper == "true")
                        {
                            if (!input.Any(char.IsUpper))
                            {
                                message = "يجب ان تحتوى كلمة المرور على حرف واحد كاببيتال على الاقل";
                                return View();
                            }
                        }
                    }

                    AppUser.Id = Guid.NewGuid().ToString();
                    AppUser.EmailConfirm = false;

                    AppUser.Password = AppHash.HashPassword(input);
                    AppUser.PasswordConfirm = AppHash.HashPassword(input);
                    DataTable dt = new DataTable();
                    string username = AppUser.UserName;
                    Users users = new Users();
                    dt = users.chekUserNameExist(username);
                    string email = AppUser.Email;


                    if (dt.Rows.Count < 1)
                    {
                        if (!IsEmailAdressExist(email))
                        {
                            int usercount = db.AppUser.Count();
                            db.Add(AppUser);
                            string userid = AppUser.Id;
                            await db.SaveChangesAsync();
                            string title = "تأكيد اشتراكك بموقع التجربة";
                            string body = "مرحبا " + username + "<br/>";
                            body += "يرجى الضغط على الرابط ادناه لتأكيد اشتراكك بموقع التجربة" + "<br/>" + "<br/>";
                            body += "https://localhost:44318/Account/AccountValidate?UID=" + userid;
                            if (SendEmail(email, body, title))
                            {
                                if (await InsertEmailConfirm(userid))
                                {
                                    successmsg = "تم انشاء حسابك بنجاح برجاء زيارة بريدك الالكترونى لتفعيل حسابك";
                                    if (!string.IsNullOrEmpty(IsSendEmailAfterRegister()))
                                    {
                                        if (IsSendEmailAfterRegister() == "true")
                                        {
                                            title = "شكرا لتسجيلك معنا بموقع التجربة";
                                            body = "مرحبا " + username + "<br />";
                                            body += "شكرا لتسجيلك معنا بموقع التجربة";
                                            SendEmail(email, body, title);
                                        }
                                    }
                                }
                                else
                                {
                                    message = "خطأ بعملية اضافة الحساب , يرجي المحاولة لاحقا";
                                }
                            }
                            else
                            {
                                if (await InsertEmailConfirm(userid))
                                {
                                    successmsg = "تم انشاء حسابك بنجاح وتعذر ارسال رسالة تفعيل الى بريدك الالكترونى";
                                }
                            }


                            string Roleid = string.Empty;
                            if (usercount <= 0)
                            {

                                AppRole appRole = new AppRole();
                                appRole.Id = Guid.NewGuid().ToString();
                                appRole.RoleName = "Admin";
                                await db.AddAsync(appRole);
                                await db.SaveChangesAsync();
                                Roleid = appRole.Id;


                                appRole.Id = Guid.NewGuid().ToString();
                                appRole.RoleName = "SuperVisor";
                                await db.AddAsync(appRole);
                                await db.SaveChangesAsync();

                                appRole.Id = Guid.NewGuid().ToString();
                                appRole.RoleName = "Member";
                                await db.AddAsync(appRole);
                                await db.SaveChangesAsync();

                                UserRole userRole = new UserRole();
                                userRole.Id = Guid.NewGuid().ToString();
                                userRole.RoleId = Roleid;
                                userRole.UserId = userid;
                                await db.AddAsync(userRole);
                                await db.SaveChangesAsync();
                            }
                            else
                            {
                                Roleid = AppAuthentication.GetRoleId("Member");
                                if (!string.IsNullOrEmpty(Roleid))
                                {
                                    UserRole userRole = new UserRole();
                                    userRole.Id = Guid.NewGuid().ToString();
                                    userRole.RoleId = Roleid;
                                    userRole.UserId = userid;
                                    await db.AddAsync(userRole);
                                    await db.SaveChangesAsync();
                                }
                            }
                            return RedirectToAction(nameof(Register));
                        }
                        else
                        {
                            message = " البريد الالكترونى المدخل (" + email + ")  غير متوفر ";
                            return View();
                        }

                    }
                    else
                    {
                        message = "اسم المستخدم المدخل (" + username + ")  غير متوفر ";
                        return View();
                    }

                }

            }

            return View(Register());
        }
        public bool IsEmailAdressExist(string email)
        {
            return db.AppUser.Any(a => a.Email == email);
        }
        public async Task<bool> InsertEmailConfirm(string userid)
        {

            AppConfrim app = new AppConfrim();
            try
            {
                app.Id = Guid.NewGuid().ToString();
                app.UserId = userid;
                app.DateConfrim = DateTime.Now;
                db.Add(app);
                string id = app.Id;
                await db.SaveChangesAsync();
                return true;
            }
            catch { }
            return false;
        }
        public bool SendEmail(string email, string body, string title)
        {
            try
            {
                MailMessage ms = new MailMessage("smartacode@gmail.com", email);
                ms.Subject = title;
                ms.Body = body;
                ms.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new NetworkCredential()
                {
                    UserName = "smartacode@gmail.com",
                    Password = "omar14502029"
                };

                client.EnableSsl = true;
                client.Send(ms);

                return true;
            }
            catch { }

            return false;
        }
        public async Task<IActionResult> AccountValidate()
        {
            message = string.Empty;
            successmsg = string.Empty;
            bool IsForgetPassword = false;
            string id = HttpContext.Request.Query["UID"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                id = HttpContext.Request.Query["PId"].ToString();
                IsForgetPassword = true;
                if (string.IsNullOrEmpty(id))
                    return NotFound();
            }
            else
            {
                DataTable dt = new DataTable();
                Users users = new Users();
                dt = users.CheckEmailConfirmExist(id);
                if (dt.Rows.Count > 0)
                {
                    try
                    {

                        string appconfirmid = dt.Rows[0][1].ToString();
                        DateTime dateconfirm = DateTime.Parse(dt.Rows[0][2].ToString());
                        string userid = dt.Rows[0][1].ToString();

                        if (dateconfirm.AddHours(24) > DateTime.Now)
                        {
                            if (!IsForgetPassword)
                            {
                                await Task.Run(() => { users.UpdateEmailConfirm(id, true); });
                                users.DeleteEmailConfirm(appconfirmid);
                            }
                            else
                            {
                                string pass = GenetratePassword(10);
                                if (await PasswordReset(userid, pass))
                                {
                                    var user = db.AppUser.FirstOrDefault(x => x.Id == userid);
                                    if (user != null)
                                    {
                                        string title = "كلمة المرور الجديدة بموقع التجربة";
                                        string body = "مرحبا " + user.UserName + "<br/>";
                                        body += "نم توليد كلمة مرور جديدة حسب طلبك بموقع التجربة" + "<br />" + "<br />";
                                        body += "كللمة المرور الجديدة هي :" + "<br />";
                                        body += pass;
                                        if (SendEmail(user.Email, body, title))
                                        {
                                            users.DeleteEmailConfirm(appconfirmid);
                                            TempData["successPass"] = "تم ارسال كلمة المرور الجديدة الي بريدك الالكتروني بنجاح";
                                            RedirectToAction(nameof(Login));

                                        }
                                    }
                                }
                            }


                            if (users.state)
                            {
                                successmsg = "شكرا لتسجيلك معنا لقد اتممت تسجيل اشتراكك" + "\r\n";
                                successmsg += "بامكانك الذهاب لتعديل بياناتك الشخصية او الذهاب الى الصفحة الرئيسية";
                            }
                        }
                        else
                        {
                            message = "عفوا لقد انتهت صلاحية اشتراكك وهى 24 ساعة";
                            users.DeleteEmailConfirm(appconfirmid);
                        }
                    }
                    catch { }

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();

        }
        public string GenetratePassword(int stringLength)
        {
            const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#$%&@<>[]'*+-/=?^_`{|}~.";
            char[] chars = new char[stringLength];
            Random rd = new Random();
            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = AllowedChars[rd.Next(0, AllowedChars.Length)];
            }
            return new string(chars);
        }
        public async Task<bool> PasswordReset(string id, string password)
        {
            var user = await db.AppUser.FirstOrDefaultAsync(e => e.Id == id);
            if (user != null)
            {
                string pass = AppHash.HashPassword(password);
                user.Password = pass;
                user.PasswordConfirm = pass;
                db.Attach(user);
                db.Entry(user).Property(x => x.Password).IsModified = true;
                db.Entry(user).Property(x => x.PasswordConfirm).IsModified = true;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public class LoginModel
        {
            [StringLength(250), Required(ErrorMessage = "اسم المستخدم مطلوب"), Display(Name = "اسم المستخدم")]
            public string UserName { get; set; }

            [StringLength(50), Required(ErrorMessage = "كلمة المرور مطلوبة "), Display(Name = "كلمة المرور "), DataType(DataType.Password)]
            public string Password { get; set; }
            [Display(Name = "تذكرنى")]
            public bool rememberMe { get; set; }
        }
        [BindProperty]
        public LoginModel input { get; set; }

        public IActionResult Login()
        {
            var msg = TempData["successPass"] as string;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool rem)
        {
            if (username == null || password == null)
            {
                return View();
            }
            if (IsLoged(username, password))
            {
                string id = AppAuthentication.GetIdByUserName(username);
                if (!string.IsNullOrEmpty(id))
                {
                    var appuser = await db.AppUser.FindAsync(id);
                    if (appuser != null)
                    {
                        if (appuser.LockOut == false)
                        {
                            appuser.ErrorLogCount = 0;
                            db.AppUser.Attach(appuser);
                            db.Entry(appuser).Property(x => x.ErrorLogCount).IsModified = true;
                            await db.SaveChangesAsync();
                            AddCookies(username, AppAuthentication.GetRoleName(username), password, rem);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            if (await IsLuckOutFinished(id, appuser.LockTime))
                            {
                                AddCookies(username, AppAuthentication.GetRoleName(username), password, rem);
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ViewBag.msg = "تم حظر هذا الحساب مؤقتا يرجي معاودة محاولة تسجيل الدخول بعد انقضاء مدة الحظر";
                                return View();
                            }

                        }

                    }
                }
            }
            else
            {
                if (await LogError(username))
                {
                    ViewBag.msg = "نظرا لمحاولات التسجيل المتكررة والخاطئة تم اغلاق حساب " + username + " لمدة 12 ساعة";
                }
            }
            return View();
        }
        public bool IsLoged(string username, string password)
        {
            DataTable dt = new DataTable();
            string hash = AppHash.HashPassword(password);
            Users cs = new Users();
            dt = cs.CheckLogin(username, hash);
            if (dt.Rows.Count > 0)
            {
                return true;

            }
            return false;
        }
        public async Task<bool> LogError(string username)
        {
            string id = AppAuthentication.GetIdByUserName(username);
            if (!string.IsNullOrEmpty(id))
            {
                var AppUser = await db.AppUser.FindAsync(id);
                if (AppUser != null)
                {
                    AppUser.ErrorLogCount += 1;
                    int count = AppUser.ErrorLogCount;
                    if (AppUser.ErrorLogCount < 5)
                    {
                        db.AppUser.Attach(AppUser);
                        await db.SaveChangesAsync();

                        ViewBag.msg = "بيانات الدخول غير صحيحة !!!" + "\r\n" + "لديك ( " + count + " ) محاولة تسجيل دخول خاطئة من عدد " + "(5) محاولات";
                        return false;
                    }
                    else
                    {
                        db.AppUser.Attach(AppUser);
                        AppUser.ErrorLogCount += 1;
                        AppUser.LockTime = DateTime.Now.AddHours(12);
                        AppUser.LockOut = true;
                        db.Entry(AppUser).Property(x => x.ErrorLogCount).IsModified = true;
                        db.Entry(AppUser).Property(x => x.LockTime).IsModified = true;
                        db.Entry(AppUser).Property(x => x.LockOut).IsModified = true;
                        await db.SaveChangesAsync();
                        return true;
                    }
                }
            }
            return false;

        }
        public async Task<bool> IsLuckOutFinished(string Id, DateTime? lockDate)
        {
            if (Id != null)
            {
                var appuser = await db.AppUser.FindAsync(Id);
                if (appuser != null)
                {
                    if (lockDate != null)
                    {
                        if (DateTime.Now >= lockDate)
                        {
                            db.AppUser.Attach(appuser);
                            appuser.ErrorLogCount = 0;
                            appuser.LockOut = false;
                            appuser.LockTime = null;
                            db.Entry(appuser).Property(x => x.ErrorLogCount).IsModified = true;
                            db.Entry(appuser).Property(x => x.LockOut).IsModified = true;
                            db.Entry(appuser).Property(x => x.LockTime).IsModified = true;
                            await db.SaveChangesAsync();
                            return true;
                        }
                    }
                }

            }
            return false;

        }
        //private void getCookie()
        //{
        //    if (Request.Cookies["UserName"] != null)
        //    {
        //        string userHash = AppHash.Base46ToString(Request.Cookies["UserName"].ToString());
        //        ViewBag.name1 = userHash;
        //    }
        //    if (Request.Cookies["Password"] != null)
        //    {
        //        string passHash = AppHash.Base46ToString(Request.Cookies["Password"].ToString());
        //        ViewBag.password1 = passHash;
        //    }
        //    if (Request.Cookies["rem"] != null)
        //    {
        //        ViewBag.rem1 = Request.Cookies["rem"].ToString();
        //    }
        //}
        //string cookieUserName = "UserName";
        //string cookiepassword = "Password";
        //string cookierem = "rem";
        public async void AddCookies(string username, string rollname, string password, bool remember)
        {

            //if (remember)
            //{
            //    string userHash = AppHash.StringToBase64(username);
            //    string passHash = AppHash.StringToBase64(password);
            //    CookieOptions cookies = new CookieOptions();
            //    cookies.Expires = DateTime.MaxValue;
            //    Response.Cookies.Append(cookieUserName, userHash, cookies);
            //    Response.Cookies.Append(cookiepassword, passHash, cookies);
            //    Response.Cookies.Append(cookierem, remember.ToString(), cookies);
            //}
            //else
            //{
            //    Response.Cookies.Append(cookieUserName, username);
            //    Response.Cookies.Append(cookiepassword, password);
            //    Response.Cookies.Append(cookierem, remember.ToString());
            //}

            var Claim = new List<Claim>{
                new Claim(ClaimTypes.Name,username),
                new Claim(ClaimTypes.NameIdentifier,AppAuthentication.GetIdByUserName(username)),
                new Claim(ClaimTypes.Role,rollname),
                new Claim("Password",password),
                new Claim(ClaimTypes.IsPersistent,remember.ToString())
            };

            var claimIdentity = new ClaimsIdentity(Claim, CookieAuthenticationDefaults.AuthenticationScheme);
            if (remember)
            {
                var authProperty = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = remember,
                    ExpiresUtc = DateTime.UtcNow.AddDays(10)
                };

                await HttpContext.SignInAsync
                    (
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity),
                    authProperty
                );
            }
            else
            {
                var authProperty = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = remember,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync
                    (
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity),
                    authProperty
                );
            }


        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [Authorize]
        public IActionResult Profile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> UserControl()
        {
            message = string.Empty;
            successmsg = string.Empty;
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await db.AppUser.FirstOrDefaultAsync(i => i.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (UserIdProfileExists(id))
            {
                IsProfileExist = true;
            }
            else
            {
                IsProfileExist = false;
            }
            long profileid = GetUserProfileId(id);
            if (profileid > 0)
            {
                UID = profileid;
            }
            else
            {
                UID = 0;
            }
            return View(user);

        }

        // POST: UserProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile([Bind("Id,Country,UserId,DateOfBirth,PersonalWebUrl,UrlImage")] UserProfile userProfile, IFormFile img)
        {
            ViewBag.msg = string.Empty;
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            string newfilename = string.Empty;
            if (img != null && img.Length > 0)
            {
                string fn = img.FileName;
                if (IsImageValidate(fn))
                {
                    string extension = Path.GetExtension(fn);
                    newfilename = Guid.NewGuid().ToString() + extension;
                    string filename = Path.Combine(host.WebRootPath + "/images/user", newfilename);
                    await img.CopyToAsync(new FileStream(filename, FileMode.Create));
                }
                else
                {

                    ViewBag.msg = "الملفات المسموح بها : png, jpeg, jpg, gif, bmp";
                    return View();

                }


            }
            if (string.IsNullOrEmpty(userProfile.UserId) && userProfile.DateOfBirth == null && string.IsNullOrEmpty(userProfile.Country) &&
                string.IsNullOrEmpty(userProfile.UrlImage) && string.IsNullOrEmpty(userProfile.PersonalWebUrl))
            {
                ViewBag.msg = "لم تقم باى اختيار!!!!";
                return View();
            }

            userProfile.UserId = id;
            userProfile.UrlImage = newfilename;
            db.Add(userProfile);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(UserControl));
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

        // GET: UserProfiles/Edit/5
        [Authorize]
        public async Task<IActionResult> EditProfile(long? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var userProfile = await db.userProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            OldImage = userProfile.UrlImage;

            return View(userProfile);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(long id, [Bind("Id,Country,UserId,DateOfBirth,PersonalWebUrl,UrlImage")] UserProfile userProfile, IFormFile img)
        {
            if (id != userProfile.Id)
            {
                return NotFound();
            }

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    string newfilename = string.Empty;
                    if (img != null && img.Length > 0)
                    {
                        string fn = img.FileName;
                        if (IsImageValidate(fn))
                        {
                            string extension = Path.GetExtension(fn);
                            newfilename = Guid.NewGuid().ToString() + extension;
                            string filename = Path.Combine(host.WebRootPath + "/images/user", newfilename);
                            await img.CopyToAsync(new FileStream(filename, FileMode.Create));
                        }
                        else
                        {

                            ViewBag.msg = "الملفات المسموح بها : png, jpeg, jpg, gif, bmp";
                            return View();

                        }
                    }
                    else
                    {
                        newfilename = OldImage;
                    }


                    userProfile.UserId = userId;
                    userProfile.UrlImage = newfilename;
                    db.Update(userProfile);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(UserControl));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileExists(userProfile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(userProfile);
        }
        private bool UserProfileExists(long id)
        {
            return db.userProfiles.Any(e => e.Id == id);
        }
        private bool UserIdProfileExists(string userid)
        {
            return db.userProfiles.Any(e => e.UserId == userid);
        }
        public long GetUserProfileId(string userid)
        {
            try
            {
                long id = db.userProfiles.Where(e => e.UserId == userid).Select(e => e.Id).FirstOrDefault();
                return id;
            }
            catch { }
            return 0;
        }
        public string GetUserPassword(string userid)
        {
            try
            {
                string pass = db.AppUser.Where(e => e.Id == userid).Select(e => e.Password).FirstOrDefault();
                return pass;
            }
            catch { }
            return string.Empty;
        }
        [HttpPost]
        [Authorize]
        public IActionResult VerifyPassword(string pass)
        {
            if (pass == null)
            {
                return RedirectToAction(nameof(UserControl));
            }
            string password = AppHash.HashPassword(pass);
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(id))
            {
                if (password == GetUserPassword(id))
                {
                    message = string.Empty;
                    return RedirectToAction(nameof(ChangePassword), new { UId = password });
                }
                else
                {
                    message = "كلمةالمرور المدخلة غير صحيحة!!!";
                }

            }


            return RedirectToAction(nameof(UserControl));
        }
        [Authorize]
        public IActionResult ChangePassword()
        {
            string uid = HttpContext.Request.Query["UId"].ToString();
            if (string.IsNullOrEmpty(uid))
            {
                return RedirectToAction("Index", "Home");
            }
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Home");
            }
            if (GetUserPassword(id) != uid)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string pass, string passConfirm)
        {
            if (pass == null || passConfirm == null)
            {
                return RedirectToAction(nameof(UserControl));
            }
            string password = AppHash.HashPassword(pass);
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(id))
            {
                if (pass == passConfirm)
                {
                    var user = await db.AppUser.FirstOrDefaultAsync(x => x.Id == id);
                    if (user != null)
                    {
                        user.Password = password;
                        user.PasswordConfirm = password;
                        db.Attach(user);
                        db.Entry(user).Property(x => x.Password).IsModified = true;
                        db.Entry(user).Property(x => x.PasswordConfirm).IsModified = true;
                        await db.SaveChangesAsync();
                        return RedirectToAction(nameof(UserControl));
                    }
                }
                else
                {
                    ViewBag.msg = "كلمتا المرور غير متطابقتين !!!";
                }
            }
            else
            {
                return View("Index", "Home");
            }

            return View();
        }

        public IActionResult ForgetPassword()
        {

            var msg = TempData["successmsg"] as string;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (email == null)
            {
                return View();
            }
            if (IsEmailAdressExist(email))
            {
                var user = await db.AppUser.FirstOrDefaultAsync(e => e.Email == email);
                if (user != null)
                {
                    string title = "استعادة كلمة المرور بموقع التجربة";
                    string body = "مرحبا بك " + user.UserName + "<br />";
                    body += "يرجي الضغط علي الرابط ادناه لتفعيل طلب استعادة كلمة المرور بموقع التجربة" + "<br />" + "<br />";
                    body += "https://localhost:44313/Acount/AccountValidate?PId=" + user.Id;
                    if (SendEmail(email, body, title))
                    {
                        if (await InsertEmailConfirm(user.Id))
                        {
                            ViewBag.msg = "";
                            TempData["successmsg"] = "تم ارسال طلب استعادة كلمةالمرور الى بريدك الالكترونى بنجاح ";
                            TempData.Keep("successmsg");

                            return RedirectToAction(nameof(ForgetPassword));
                        }
                        else
                        {
                            ViewBag.msg = "حدث خطأ اثناء علمية تحديث البيانات !!!";
                        }
                    }
                }
            }
            else
            {
                ViewBag.msg = "البريد الالكتروني غير مستعمل ...";
            }

            return View();
        }
        private string IsEmailConfirm()
        {
            try
            {
                string email = db.UserSettings.Where(e => e.id == 1).Select(e => e.isEmailConfirm).FirstOrDefault().ToString().ToLower();
                return email;
            }
            catch { }


            return string.Empty;
        }
        private string IsRegisterOpen()
        {
            try
            {
                return db.UserSettings.Where(u => u.id == 1).Select(u => u.isRegisterOpen).FirstOrDefault().ToString().ToLower();
            }
            catch { }
            return string.Empty;
        }
        private int PassWordMinimumLenght()
        {
            try
            {
                int i = 0;
                i = db.UserSettings.Where(x => x.id == 1).Select(x => x.MinimumPassLength).FirstOrDefault();
                return i;
            }
            catch { }
            return 0;
        }
        private int PassWordMaximumLength()
        {
            try
            {
                int i = 0;
                i = db.UserSettings.Where(x => x.id == 1).Select(x => x.MaxPassLength).FirstOrDefault();
                return i;
            }
            catch { }
            return 0;
        }
        private string IsPasswordDigit()
        {
            try
            {
                return db.UserSettings.Where(u => u.id == 1).Select(u => u.isDigit).FirstOrDefault().ToString().ToLower();
            }
            catch { }
            return string.Empty;
        }
        private string IsPasswordUpper()
        {
            try
            {
                return db.UserSettings.Where(u => u.id == 1).Select(u => u.isUpper).FirstOrDefault().ToString().ToLower();
            }
            catch { }
            return string.Empty;
        }
        private string IsSendEmailAfterRegister()
        {
            try
            {
                return db.UserSettings.Where(x => x.id == 1).Select(x => x.SendWelcomeMessage).FirstOrDefault().ToString().ToLower();
            }
            catch { }
            return string.Empty;
        }

    }
}

