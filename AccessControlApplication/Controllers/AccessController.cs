using AccessControlApplication.Data;
using AccessControlApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace AccessControlApplication.Controllers
{
    public class AccessController : Controller
    {
        public ApplicationDbContext _db;
        SearchByCategory category = new();

        public static int userId = 0;
        public static string name = "";
        public static string idCard = "";
        public static string allInfo = "";

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

            currentUser.User = loggedUser;

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

        public IActionResult DisplayUsers()
        {
            CombinedClasses obj = new();
            Register? users = new();
            List<Register> allData = new();
            LoggedUser user = new();

            if (category.GetAllData == true)
            {
                allData = _db.UserDetails.ToList();
            }
            else if(userId !=0 || idCard!="")
            {
                if (userId != 0)
                {
                    users = _db.UserDetails.Find(userId);
                    userId = 0;
                    category.GetAllData = true;
                    category.Search = "";
                }
                else if (idCard != "")
                {
                    users = _db.UserDetails.FirstOrDefault(id => id.IdCardNum == idCard);
                    idCard = "";
                    category.GetAllData = true;
                    category.Search = "";
                }
                if (users != null)
                {
                    allData.Add(users);
                }
                else
                {
                    TempData["Unsuccessfull"] = "No Data found in the database!!";
                }
            }
            else
            {
                if (name != "")
                {
                    var all = _db.UserDetails.ToList();
                    var names = all.Where(n => Regex.IsMatch(input: n.FullName, pattern: name)).ToList();
           
                    if (names != null)
                    {
                        foreach (var item in names)
                        {
                            allData.Add(item);
                        }
                    }
                    else
                    {
                        TempData["Unsuccessfull"] = "No Data found in the database!!";
                    }
                    name = "";
                    category.GetAllData = true;
                    category.Search = "";
                }
            }

            obj.Category = category;
            obj.RegisteredUsers = allData;
            obj.User = user;
            return View(obj);
        }
        public IActionResult SearchById()
        {
            CombinedClasses setCategory = new();

            category.Search = "Id";
            setCategory.Category = category;

            return RedirectToAction("DisplayUsers", "Access");
        }
        public IActionResult SearchByName()
        {
            CombinedClasses setCategory = new();

            category.Search = "Name";
            setCategory.Category = category;

            return RedirectToAction("DisplayUsers", "Access");
        }
        public IActionResult SearchByIdcard()
        {
            CombinedClasses setCategory = new();

            category.Search = "Idcard";
            setCategory.Category = category;

            return RedirectToAction("DisplayUsers", "Access");
        }
        public IActionResult GetAllData()
        {
            CombinedClasses setCategory = new();

            category.Search = "AllData";
            setCategory.Category = category;

            return RedirectToAction("DisplayUsers", "Access");
        }

        [HttpPost]
        public IActionResult GetInfoById()
        {
            CombinedClasses? userData = new();
            category.GetAllData = false;

            try
            {
                if (Request.Form["Id"] == "")
                {
                    category.GetAllData = true;
                    TempData["Unsuccessfull"] = "Search bar is empty. Please enter a valid user Id.";
                }
                else
                {
                    userId = int.Parse(Request.Form["Id"].ToString());
                }
            }
            catch
            {
                category.GetAllData = true;
                TempData["Unsuccessfull"] = "\"" + Request.Form["Id"].ToString() + "\"" + " is not valid User Id. The User Id should contain only numbers.";
            }

            return RedirectToAction("DisplayUsers", "Access");
        }
        [HttpPost]
        public IActionResult GetInfoByName()
        {
            CombinedClasses? userData = new();
            category.GetAllData = false;

            try
            {
                if (Request.Form["Name"] == "")
                {
                    category.GetAllData = true;
                    TempData["Unsuccessfull"] = "Search bar is empty. Please enter a valid Name.";
                }
                else
                {
                    name = Request.Form["Name"].ToString();
                }
            }
            catch { }

            return RedirectToAction("DisplayUsers", "Access");
        }
        [HttpPost]
        public IActionResult GetInfoByIdcard()
        {
            CombinedClasses? userData = new();
            category.GetAllData = false;

            try
            {
                if (Request.Form["IdCard"] == "")
                {
                    category.GetAllData = true;
                    TempData["Unsuccessfull"] = "Search bar is empty. Please enter a valid Id Card number.";
                }
                else
                {
                    idCard = Request.Form["IdCard"].ToString();
                }
            }
            catch { }

            return RedirectToAction("DisplayUsers", "Access");
        }
        [HttpPost]
        public IActionResult GetAllInfo()
        {
            CombinedClasses? userData = new();
            category.GetAllData = false;

            allInfo = Request.Form["AllInfo"].ToString();

            return RedirectToAction("DisplayUsers", "Access");
        }
        [HttpPost]
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
