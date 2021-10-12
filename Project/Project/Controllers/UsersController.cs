using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace Project.Controllers
{
    public class UsersController : Controller
    {
        private SMSonlineEntities db = new SMSonlineEntities();

        // GET: Users
        public ActionResult Index()
        {

            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                ViewBag.error = "Login failed";
                return RedirectToAction("Login");
            }
            //return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_Id,UserName,PassWord,Email,DOB,Gender,WorkStatus,Address")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_Id,UserName,PassWord,Email,DOB,Gender,WorkStatus,Address")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = EncodePassword(password);
                var data = db.Users.Where(s => s.Email.Equals(email) && s.PassWord.Equals(f_password)).ToList();
                //if (data.Count() > 0)
                if (data != null)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().UserName;

                    if (data.FirstOrDefault().Gender == true)
                    {
                        Session["Gender"] = "Male";
                    }
                    else if (data.FirstOrDefault().Gender == false)
                    {
                        Session["Gender"] = "Female";
                    }

                    if (data.FirstOrDefault().WorkStatus == true)
                    {
                        Session["WorkStatus"] = "Students";
                    }
                    else if (data.FirstOrDefault().WorkStatus == false)
                    {
                        Session["WorkStatus"] = "Employees";
                    }

                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["Address"] = data.FirstOrDefault().Address;
                    Session["DOB"] = data.FirstOrDefault().DOB;
                    Session["UserID"] = data.FirstOrDefault().User_Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    // ScriptManager.RegisterClientScriptBlock(this,this.GetType(), "alertMessage", "alert('Record Inserted Successfully')", true);
                    return RedirectToAction("Login");
                }
                
            }
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            { // check xem email đã tồn tại trên đb chưa
                var checkEmail = db.Users.FirstOrDefault(s => s.Email == _user.Email);
                var checkUserName = db.Users.FirstOrDefault(s => s.UserName == _user.UserName);
                if (checkEmail == null && checkUserName == null)
                {
                    _user.PassWord = EncodePassword(_user.PassWord);
                    //_user.Password = _user.Password;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return RedirectToAction("Register");
                }
            }
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        //create md5 string
        public static string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }
    }
}
