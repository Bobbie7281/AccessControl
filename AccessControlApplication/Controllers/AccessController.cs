﻿using AccessControlApplication.Data;
using AccessControlApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlApplication.Controllers
{
    public class AccessController : Controller
    {
        public ApplicationDbContext _db;
        static int currentUser;
        public AccessController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult LogIn()
        {
            /*CombinedClasses obj = new();
            LoggedUser user = new();
            user.CurrentUser = 0;

            obj.User = user;*/
            CombinedClasses currentUser = new();
            LoggedUser loggedUser = new();

            currentUser.User= loggedUser;

            return View(currentUser);
        }
        public IActionResult Register()
        {
            CombinedClasses currentUser = new();
            LoggedUser loggedUser = new();

            currentUser.User = loggedUser;
            return View(currentUser);
        }
        public IActionResult ManageDatabase()
        {
            CombinedClasses obj = new();
            LoggedUser user = new();
            obj.User = user;

            return View(obj);
        }
        [HttpPost]
        public IActionResult Download()
        {
            int userId;
            ButtonControls? downloadState = new();
            CombinedClasses? combinedData = new();
            LoggedUser? user = new();
            Register? getData;


            try
            {
                userId = int.Parse(Request.Form["Id"].ToString());
                getData = _db.UserDetails.Find(userId);
                currentUser = userId;
                combinedData.RegisterUser = getData;
                combinedData.ButtonSync = downloadState;
                combinedData.User = user;

            }
            catch
            {
                TempData["Invalid Input"] = "User Id is not valid. Only numbers are accepted!!";
                downloadState.Download = false;
                return RedirectToAction("Edit");
            }

            if (getData == null)
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
            LoggedUser user = new();
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
            details.User = user;

            return View(details);
        }

        [HttpPost]
        public IActionResult EditInfo(CombinedClasses obj)
        {
            CombinedClasses? combinedData = new();

            Register newData = new();
            newData.Id = currentUser;
            newData.IdCardNum = obj.RegisterUser!.IdCardNum;
            newData.FullName = obj.RegisterUser!.FullName;
            newData.Address = obj.RegisterUser!.Address;
            newData.ContactNumber = obj.RegisterUser!.ContactNumber;
            newData.EmailAddress = obj.RegisterUser!.EmailAddress;

            try
            {
                _db.UserDetails.Update(newData);
                _db.SaveChanges();
                TempData["Edit Successfull"] = "Data edited and saved successfully!";
            }
            catch (Exception ex)
            {
                TempData["Edit Unsuccessfull"] = "Data not saved due to the following: /n" + ex.Message;
            }

            return RedirectToAction("Edit", "Access");
        }
        public IActionResult Delete()
        {
            CombinedClasses currentUser = new();
            LoggedUser loggedUser = new();

            currentUser.User = loggedUser;
            return View(currentUser);
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

            return RedirectToAction("Delete", "Access");
        }
        [HttpPost]
        public IActionResult Register(CombinedClasses obj)
        {
            bool userExist = false;
            CombinedClasses currentUser = new();
            LoggedUser loggedUser = new();
            currentUser.User = loggedUser;

            Register newUser = new()
            {
                IdCardNum = obj.RegisterUser!.IdCardNum,
                FullName = obj.RegisterUser!.FullName,
                Address = obj.RegisterUser!.Address,
                ContactNumber = obj.RegisterUser!.ContactNumber,
                EmailAddress = obj.RegisterUser!.EmailAddress,
            };

            if (ModelState.IsValid)
            {
                var usersList = _db.UserDetails.ToList();

                if (usersList.Count == 0)
                {
                    _db.UserDetails.Add(newUser);
                    _db.SaveChanges();
                    TempData["Successfull"] = "Data Saved Successfully!!";
                }
                else
                {
                    foreach (var user in usersList)
                    {
                        if (user.IdCardNum == newUser.IdCardNum)
                        {
                            TempData["Unsuccessfull"] = "User with Id Card Number " + newUser.IdCardNum + " already exists. Data not saved in database!!";
                            userExist = true;
                            break;
                        }
                    }
                    if (!userExist)
                    {
                        _db.UserDetails.Add(newUser);
                        _db.SaveChanges();
                        TempData["Successfull"] = "Data saved successfully in the database";
                    }
                }
            }
            else
            {
                return View(currentUser);
            }
            return View(currentUser);
        }

        [HttpPost]
        public IActionResult LogIn(CombinedClasses obj)
        {
            bool login = false;
            CombinedClasses currentUser = new();
            LoggedUser loggedUser = new();
            currentUser.User = loggedUser;

            if (!ModelState.IsValid)
            {
                var users = _db.UserDetails.ToList();

                foreach (var user in users)
                {
                    if (user.Id == obj.RegisterUser!.Id && user.IdCardNum == obj.RegisterUser!.IdCardNum)
                    {
                        login = true;
                        loggedUser.CurrentUser = obj.RegisterUser!.Id;

                        obj.User = loggedUser;
                        TempData["Successfull Login"] = "Login Successfull";
                        return RedirectToAction("Index", "Home");
                    }
                }
                if (!login)
                {
                    TempData["Unsuccessfull Login"] = "User Id or Password are incorrect!! Log In Unsuccessfull.";
                }
            }

            return View(currentUser);
        }
    }
}
