using AccessControlApplication.Data;
using AccessControlApplication.Email;
using AccessControlApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace AccessControlApplication.Controllers
{
    public class AccessController : Controller
    {
        public ApplicationDbContext _db;
        readonly IEmailSender? _emailSender;
        readonly ICombinedClasses? _combinedClasses;
        readonly ILoggedUser? _loggedUser;
        readonly IRegister? _register;
        readonly ISearchByCategory? _searchByCategory;
        readonly IButtonControls? _buttonControls;

        public AccessController(ApplicationDbContext db, IEmailSender emailSender,
            ICombinedClasses combinedClasses, ILoggedUser loggedUser, IRegister register,
            ISearchByCategory searchByCategory, IButtonControls buttonControls)
        {
            _db = db;
            _emailSender = emailSender;
            _combinedClasses = combinedClasses;
            _loggedUser = loggedUser;
            _register = register;
            _searchByCategory = searchByCategory;
            _buttonControls = buttonControls;
        }

        public IActionResult LogIn()
        {
            
            _combinedClasses!.User = (LoggedUser?)_loggedUser;

            return View(_combinedClasses);
        }
        public IActionResult Register()
        {
            _combinedClasses!.User = (LoggedUser?)_loggedUser;

            return View(_combinedClasses);
        }
        public IActionResult ManageDatabase()
        {
            _combinedClasses!.User = (LoggedUser?)_loggedUser;

            return View(_combinedClasses);
        }

        public IActionResult DisplayUsers()
        {
            List<Register> allData = new();
            var resultData = _register;
            var loggedUser = _loggedUser;
            var searchUserId = _searchByCategory!.SearchIdValue;
            var searchUserIdCard = _searchByCategory.SearchIdCardValue;
            var searchUserName = _searchByCategory.SearchNameValue;

            if (_searchByCategory.GetAllData == true)
            {
                allData = _db.UserDetails.ToList();
            }
            else if (searchUserId != 0 || searchUserIdCard != "")
            {
                if (searchUserId != 0)
                {
                    resultData = _db.UserDetails.Find(searchUserId);
                    _searchByCategory.SearchIdValue = 0;
                    _searchByCategory.GetAllData = true;
                    _searchByCategory.SearchType = "";
                }
                else if (searchUserIdCard != "")
                {
                    resultData = _db.UserDetails.FirstOrDefault(id => id.IdCardNum == searchUserIdCard);
                    _searchByCategory.SearchIdCardValue = "";
                    _searchByCategory.GetAllData = true;
                    _searchByCategory.SearchType = "";
                }
                if (resultData != null)
                {
                    allData.Add((Register)resultData);
                }
                else
                {
                    TempData["Unsuccessfull"] = "No Data found in the database!!";
                }
            }
            else
            {
                if (searchUserName != "")
                {
                    var all = _db.UserDetails.ToList();
                    var names = all.Where(n => Regex.IsMatch(input: n.FullName!, pattern: searchUserName)).ToList();
                    
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
                    searchUserName = "";
                    _searchByCategory.GetAllData = true;
                    _searchByCategory.SearchType = "";
                }
            }
            _combinedClasses!.Category = (SearchByCategory)_searchByCategory;
            _combinedClasses.RegisteredUsers = allData;
            _combinedClasses.User = (LoggedUser?)loggedUser;

            return View(_combinedClasses);
        }
        public IActionResult SearchById()
        {
            _searchByCategory!.SearchType = "Id";
            _combinedClasses!.Category = (SearchByCategory)_searchByCategory;

            return RedirectToAction("DisplayUsers", "Access");
        }
        public IActionResult SearchByName()
        {
            _searchByCategory!.SearchType = "Name";

            _combinedClasses!.Category = (SearchByCategory)_searchByCategory;

            return RedirectToAction("DisplayUsers", "Access");
        }
        public IActionResult SearchByIdcard()
        {
            _searchByCategory!.SearchType = "Idcard";
            _combinedClasses!.Category = (SearchByCategory)_searchByCategory;

            return RedirectToAction("DisplayUsers", "Access");
        }
        public IActionResult GetAllData()
        {
            _searchByCategory!.SearchType = "AllData";
            _combinedClasses!.Category = (SearchByCategory)_searchByCategory;

            return RedirectToAction("DisplayUsers", "Access");
        }

        [HttpPost]
        public IActionResult GetInfoById()
        {

            _searchByCategory!.GetAllData = false;

            try
            {
                if (Request.Form["Id"] == "")
                {
                    _searchByCategory.GetAllData = true;
                    TempData["Unsuccessfull"] = "Search bar is empty. Please enter a valid loggedUser Id.";
                }
                else
                {
                    _searchByCategory.SearchIdValue = int.Parse(Request.Form["Id"].ToString());
                }
            }
            catch
            {
                _searchByCategory.GetAllData = true;
                TempData["Unsuccessfull"] = "\"" + Request.Form["Id"].ToString() + "\"" + " is not valid User Id. The User Id should contain only numbers.";
            }

            return RedirectToAction("DisplayUsers", "Access");
        }
        [HttpPost]
        public IActionResult GetInfoByName()
        {

            _searchByCategory!.GetAllData = false;

            try
            {
                if (Request.Form["Name"] == "")
                {
                    _searchByCategory.GetAllData = true;
                    TempData["Unsuccessfull"] = "Search bar is empty. Please enter a valid Name.";
                }
                else
                {
                    _searchByCategory.SearchNameValue = Request.Form["Name"].ToString();
                }
            }
            catch { }

            return RedirectToAction("DisplayUsers", "Access");
        }
        [HttpPost]
        public IActionResult GetInfoByIdcard()
        {
            _searchByCategory!.GetAllData = false;

            try
            {
                if (Request.Form["IdCard"] == "")
                {
                    _searchByCategory.GetAllData = true;
                    TempData["Unsuccessfull"] = "Search bar is empty. Please enter a valid Id Card number.";
                }
                else
                {
                    _searchByCategory.SearchIdCardValue = Request.Form["IdCard"].ToString();
                }
            }
            catch { }

            return RedirectToAction("DisplayUsers", "Access");
        }
        [HttpPost]
        public IActionResult GetAllInfo()
        {
            _searchByCategory!.GetAllData = false;

            return RedirectToAction("DisplayUsers", "Access");
        }
        public IActionResult Edit()
        {
            _register!.Id = 0;
            _register.IdCardNum = "";
            _register.FullName = "";
            _register.Address = "";
            _register.ContactNumber = "";
            _register.EmailAddress = "";

            _buttonControls!.Download = false;
            _combinedClasses!.RegisterUser = (Register)_register;
            _combinedClasses.ButtonSync = (ButtonControls)_buttonControls;
            _combinedClasses.User = (LoggedUser?)_loggedUser;

            return View(_combinedClasses);
        }

        [HttpPost]
        public IActionResult EditInfo(CombinedClasses obj)
        {
            string adminRights = Request.Form["optionsRadios"].ToString();
            obj.RegisterUser!.Administrator = adminRights == "true" ? true : false;

            _register!.Id = obj.RegisterUser!.Id;
            _register.IdCardNum = obj.RegisterUser!.IdCardNum;
            _register.FullName = obj.RegisterUser!.FullName;
            _register.Address = obj.RegisterUser!.Address;
            _register.ContactNumber = obj.RegisterUser!.ContactNumber;
            _register.EmailAddress = obj.RegisterUser!.EmailAddress;
            _register.Administrator = obj.RegisterUser!.Administrator;

            try
            {
                _db.UserDetails.Update((Register)_register);
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
            var getData = _register;

            try
            {
                userId = int.Parse(Request.Form["Id"].ToString());
                getData = _db.UserDetails.Find(userId);
                _combinedClasses!.RegisterUser = getData as Register;
                _combinedClasses.ButtonSync = (ButtonControls?)_buttonControls;
                _combinedClasses.User = (LoggedUser?)_loggedUser;
            }
            catch
            {
                TempData["Invalid Input"] = "User Id is not valid. Only numbers are accepted!!";
                _buttonControls!.Download = false;
                return RedirectToAction("Edit");
            }

            if (getData == null)
            {
                TempData["Non Existing Id"] = "User Id not found in the database!!";
                _buttonControls!.Download = false;
                return RedirectToAction("Edit");
            }
            _buttonControls!.Download = true;
            return View("Edit", _combinedClasses);
        }
        public IActionResult Delete()
        {
            _combinedClasses!.User = (LoggedUser?)_loggedUser;
            return View(_combinedClasses);
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
            string admin = Request.Form["optionsRadios"].ToString();

            var idCardNum = obj.RegisterUser!.IdCardNum;
            var phoneNumber = obj.RegisterUser!.ContactNumber;

            string idCardPatern = @"^\d+[a-zA-Z]$";
            string phonePatern = @"^[0-9]+$";

            bool idCardValid = Regex.IsMatch(input: idCardNum!, idCardPatern);
            bool phoneValid = Regex.IsMatch(input: phoneNumber!, phonePatern);

            admin = admin == "" ? "false" : admin;
            obj.RegisterUser!.Administrator = admin == "true" ? true : false;

            var newUser = _register;

            newUser!.IdCardNum = obj.RegisterUser.IdCardNum;
            newUser.FullName = obj.RegisterUser.FullName;
            newUser.Address = obj.RegisterUser.Address;
            newUser.ContactNumber = obj.RegisterUser.ContactNumber;
            newUser.EmailAddress = obj.RegisterUser.EmailAddress;
            newUser.Administrator = obj.RegisterUser.Administrator;

            _combinedClasses!.User = (LoggedUser?)_loggedUser;


            if (ModelState.IsValid && idCardValid && phoneValid)
            {
                var usersList = _db.UserDetails.ToList();

                if (usersList.Count == 0)
                {
                    _db.UserDetails.Add((Register)newUser);
                    _db.SaveChanges();

                    var savedUser = _db.UserDetails.FirstOrDefault(idCard => idCard.IdCardNum == newUser.IdCardNum);
                    string message = _emailSender!.EmailMessage(savedUser!.FullName!, savedUser.Id.ToString(), savedUser.IdCardNum!);
                    _emailSender.SendMail(newUser.EmailAddress!, message);

                    ModelState.Clear();//Clears the input fields in the form.

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
                        _db.UserDetails.Add((Register)newUser);
                        _db.SaveChanges();
                        var savedUser = _db.UserDetails.FirstOrDefault(idCard => idCard.IdCardNum == newUser.IdCardNum);
                        string message = _emailSender!.EmailMessage(savedUser!.FullName!, savedUser.Id.ToString(), savedUser.IdCardNum!);
                        _emailSender.SendMail(newUser.EmailAddress!, message);

                        ModelState.Clear(); //Clears the input fields in the form.

                        TempData["Successfull"] = "Data saved successfully in the database";
                    }
                }
            }
            else
            {
                if (!idCardValid && phoneValid)
                {
                    TempData["Unsuccessfull"] = "Id card number " + idCardNum + " is not in the correct format. " +
                        "Please make sure that the number contains only numbers with one letter at the end without spacing.";
                }
                else if (idCardValid && !phoneValid)
                {
                    TempData["Unsuccessfull"] = "Contact Number " + phoneNumber + " is not in the correct format. " +
                        "Please make sure that it contains only numbers and without spacing.";
                }
                else
                {
                    TempData["Unsuccessfull"] = "Both Id card number and contact number are not in the correct format!! " +
                        "Id card must contain only numbers with one letter at the end without spacing & phone number must contain only numbers and without spacing.";
                }
                return View(_combinedClasses);
            }
            return View(_combinedClasses);
        }

        [HttpPost]
        public IActionResult LogIn(CombinedClasses obj)
        {
            bool login = false;

            _combinedClasses!.User = (LoggedUser?)_loggedUser;

            if (!ModelState.IsValid)
            {
                var users = _db.UserDetails.ToList();

                foreach (var user in users)
                {
                    if (user.Id == obj.RegisterUser!.Id && user.IdCardNum == obj.RegisterUser!.IdCardNum)
                    {
                        login = true;
                        _loggedUser!.CurrentUser = obj.RegisterUser!.Id;
                        _loggedUser.AdminRights = user.Administrator;
                        _loggedUser.UserCheck = true;
                        _loggedUser.UserName = user.FullName;

                        obj.User = (LoggedUser)_loggedUser;
                        TempData["Successfull Login"] = "Login Successfull";

                        return RedirectToAction("Index", "Home");
                    }
                }
                if (!login)
                {
                    TempData["Unsuccessfull Login"] = "User Id or Password are incorrect!! Log In Unsuccessfull.";
                }
            }

            return View(_combinedClasses);
        }
    }
}
