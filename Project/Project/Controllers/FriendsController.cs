using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class FriendsController : Controller
    {
        private SMSonlineEntities db = new SMSonlineEntities();
        String sql_con = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=SMSonline;Integrated Security=True";
        // GET: Friends
        public ActionResult Index()
        {
            //var user = db.Friends.Include(f => f.User).Include(f => f.User1);
            var user = db.Users.ToList();
            return View(user);
        }

        SqlConnection con;

        [HttpPost]
        public ActionResult Like(User obj,Friend fr, int? id, string YourRadioButton)
        {
            bool emp = true;

            con = new SqlConnection(sql_con);

            String sql = "insert into Emoji values(@Sender, @Receiver, @Status)";

            YourRadioButton = Request.Form["YourRadioButton"];
            
            if(YourRadioButton == "A")
            {
                emp = true;
            } else if(YourRadioButton == "B")
            {
                emp = false;
            }
            User user = db.Users.Find(id);
            Friend friend = db.Friends.Find(id);

            int userId = db.Friends.Find(id).User.User_Id;

            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@Sender", userId);
            command.Parameters.AddWithValue("@Receiver", id);
            command.Parameters.AddWithValue("@Status", emp);

            con.Open();
            command.ExecuteNonQuery();
            con.Close();

            return View();
        }


        [HttpPost]
        public String Like2()
        {
            int abc = 0;
            return "abc";
        }

        [HttpPost]
        public String Like3(string a)
        {
            string b = a;
            int abc = 0;
            return "abc";
        }

        [HttpPost]
        public String Like4(int idLiked,int Status)
        {
            //string b = a;
            //int abc = 0;

            //bool emp = true;

            con = new SqlConnection(sql_con);

            String sql = "insert into Emoji values(@Sender, @Receiver, @Status)";

            //User user = db.Users.Find(id);
            //Friend friend = db.Friends.Find(id);

            //int userId = db.Friends.Find(id).User.User_Id;
            var a = Session["UserID"];

            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@Sender",a);
            command.Parameters.AddWithValue("@Receiver", idLiked);
            command.Parameters.AddWithValue("@Status", Status);

            con.Open();
            command.ExecuteNonQuery();
            con.Close();

            return "abc";
        }
        //public ActionResult Dislike(Emoji obj)
        //{
        //    con = new SqlConnection(sql_con);

        //    String sql = "insert into Emoji values(@Sender, @Receiver, @Status)";

        //    SqlCommand command = new SqlCommand(sql, con);
        //    command.Parameters.AddWithValue("@Sender", obj.User.UserName);
        //    command.Parameters.AddWithValue("@Receiver", obj.User1.UserName);
        //    command.Parameters.AddWithValue("@Status", true);

        //    con.Open();
        //    command.ExecuteNonQuery();
        //    con.Close();

        //    return View();
        //}

        // GET: Friends/Details/5
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



        // GET: Friends/Create
        public ActionResult Create()
        {
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "UserName");
            ViewBag.UserFriend_Id = new SelectList(db.Users, "User_Id", "UserName");
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Friend_Id,FriendName,UserFriend_Id,User_Id")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Friends.Add(friend);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "UserName", friend.User_Id);
            ViewBag.UserFriend_Id = new SelectList(db.Users, "User_Id", "UserName", friend.UserFriend_Id);
            return View(friend);
        }

        // GET: Friends/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "UserName", friend.User_Id);
            ViewBag.UserFriend_Id = new SelectList(db.Users, "User_Id", "UserName", friend.UserFriend_Id);
            return View(friend);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Friend_Id,FriendName,UserFriend_Id,User_Id")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friend).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "UserName", friend.User_Id);
            ViewBag.UserFriend_Id = new SelectList(db.Users, "User_Id", "UserName", friend.UserFriend_Id);
            return View(friend);
        }

        // GET: Friends/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Friend friend = db.Friends.Find(id);
            db.Friends.Remove(friend);
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
    }
}
