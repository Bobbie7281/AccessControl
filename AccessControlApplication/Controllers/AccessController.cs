using AccessControlApplication.Data;
using AccessControlApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccessControlApplication.Controllers
{
    public class AccessController : Controller
    {
        public ApplicationDbContext _db;
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
            int userId;
            ButtonControls? downloadState = new();
            Register? getData;
            CombinedClasses? combinedData = new();
            
            try
            {
                userId = int.Parse(Request.Form["Id"].ToString());
                getData = _db.UserDetails.Find(userId);
                downloadState.Download = true;

                combinedData.RegisterUser = getData;
                combinedData.ButtonSync = downloadState;
                
            }
            catch 
            {
                TempData["Invalid Input"]="User Id is not valid. Only numbers are accepted!!";
                downloadState.Download = false;
                return RedirectToAction("Edit");
            }

            if(getData==null)
            {
                TempData["Non Existing Id"] = "User Id not found in the database!!";
                downloadState.Download = false;
                return RedirectToAction("Edit");
            }
            downloadState.Download = true;
            return View("Edit", combinedData);
        }
        public IActionResult Edit()
        {
            CombinedClasses details = new();
            Register initialInfo = new()
            {
                IdCardNum = "",
                FullName = "",
                Address = "",
                ContactNumber = "",
                EmailAddress = ""
            };
            ButtonControls initialSetUp = new()
            {
                Download = false
            };

            details.RegisterUser = initialInfo;
            details.ButtonSync = initialSetUp;

            return View(details);
        }

        [HttpPost]
        public IActionResult EditInfo()
        {
            return RedirectToAction("Index","Home");
        }
        public IActionResult Delete()
        {
            return View();
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
