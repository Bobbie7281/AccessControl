using AccessControlApplication.Controllers;
using AccessControlApplication.Data;
using AccessControlApplication.Email;
using AccessControlApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Moq;

namespace AccessControlAppTesting
{
    public class AccessControllerTesting
    {
        private readonly AccessController? _access;
        private readonly HomeController? _homeController;
        private readonly Mock<ApplicationDbContext> _db = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        private readonly Mock<IEmailSender> _emailSender = new Mock<IEmailSender>();
        private readonly Mock<ICombinedClasses> _combinedClasses = new Mock<ICombinedClasses>();
        private readonly Mock<ILoggedUser> _loggedUser = new Mock<ILoggedUser>();
        private readonly Mock<IRegister> _register = new Mock<IRegister>();
        private readonly Mock<ISearchByCategory> _searchByCategory = new Mock<ISearchByCategory>();
        private readonly Mock<IButtonControls> _buttonControls = new Mock<IButtonControls>();
        private readonly Mock<IActionResult> _result = new Mock<IActionResult>();

        public AccessControllerTesting()
        {
            _access = new AccessController(_db.Object, _emailSender.Object, _combinedClasses.Object,
                _loggedUser.Object, _register.Object, _searchByCategory.Object, _buttonControls.Object);

        }

        [Fact]
        public void test()
        {

            var log = _loggedUser;
            var com = _combinedClasses;

            log.Setup(x => x.AdminRights).Returns(true);
            log.Setup(x => x.CurrentUser).Returns(1);

            bool testadmin = log.Object.AdminRights;
            int testuser = log.Object.CurrentUser;

            var controller = new AccessController(_db.Object, _emailSender.Object, _combinedClasses.Object, log.Object, _register.Object, _searchByCategory.Object, _buttonControls.Object);
            var result = controller.LogIn();
            Assert.IsType<ViewResult>(result);
        
        }



    }
}