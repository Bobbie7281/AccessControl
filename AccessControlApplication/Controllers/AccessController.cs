using AccessControlApplication.Data;
using AccessControlApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlApplication.Controllers
{
    public class AccessController : Controller
    {
        public ApplicationDbContext _db;
        Register info = new();
        public AccessController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult ManageDatabase()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Download()
        { 
            int userId= int.Parse(Request.Form["Id"].ToString());

            Register? getData = _db.UserDetails.Find(userId);

            return View("Edit",getData);//to create another view for dowloaded data. from this view details can be edited and saved in the database.
        }
        public IActionResult Edit()
        {
            List<Register> usersList = new();
            //return View(usersList);
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost, ActionName("Edit Details")]
        public IActionResult Edit(Register obj)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete User")]
        public IActionResult Delete(Register obj)
        {
            bool matchFound = false;
            obj.Id = int.Parse(Request.Form["Id"].ToString());

            string successfullText = "User with Id " + obj.Id + " was successfully deleted from the database!";
            string unsuccessfullText = "User with Id " + obj.Id + " was not found in the database!";

            var users = _db.UserDetails.ToList();

            foreach (var user in users)
            {
                if (user.Id == obj.Id)
                {
                    _db.UserDetails.Remove(user);
                    _db.SaveChanges();
                    matchFound = true;
                    break;
                }
            }
            if (!matchFound)
            {
                TempData["Unsuccessfull Deletion"] = unsuccessfullText;
            }
            else
            {
                TempData["Successfull Deletion"] = successfullText;
            }

            return RedirectToAction("ManageDatabase", "Access");
        }
        [HttpPost]
        public IActionResult Register(Register obj)
        {
            bool userExist = false;

            if (ModelState.IsValid)
            {
                var usersList = _db.UserDetails.ToList();

                if (usersList.Count == 0)
                {
                    _db.UserDetails.Add(obj);
                    _db.SaveChanges();
                    TempData["Successfull"] = "Data Saved Successfully!!";
                }
                else
                {
                    foreach (var user in usersList)
                    {
                        if (user.IdCardNum == obj.IdCardNum)
                        {
                            TempData["Unsuccessfull"] = "User with Id Card Number " + obj.IdCardNum + " already exists. Data not saved in database!!";
                            userExist = true;
                            break;
                        }
                    }
                    if (!userExist)
                    {
                        _db.UserDetails.Add(obj);
                        _db.SaveChanges();
                        TempData["Successfull"] = "Data saved successfully in the database";
                    }
                }
            }
            else
            {
                return View();
            }
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(Register obj)
        {
            bool login = false;

            if (!ModelState.IsValid)
            {
                var users = _db.UserDetails.ToList();

                foreach (var user in users)
                {
                    if (user.Id == obj.Id && user.IdCardNum == obj.IdCardNum)
                    {
                        login = true;
                        TempData["Successfull Login"] = "Login Successfull";
                        return RedirectToAction("Index", "Home");
                    }
                }
                if (!login)
                {
                    TempData["Unsuccessfull Login"] = "User Id or Password are incorrect!! Log In Unsuccessfull.";
                }
            }

            return View();
        }
    }
}
