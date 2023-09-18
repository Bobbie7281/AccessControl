using AccessControlApplication.Controllers;
using AccessControlApplication.Data;
using AccessControlApplication.Email;
using AccessControlApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AccessControlAppTesting
{
    public class AccessControllerTesting
    {
        private readonly AccessController? _access;
        private readonly Mock<ApplicationDbContext>? _db = new Mock<ApplicationDbContext>();
        private readonly Mock<IEmailSender> _emailSender = new Mock<IEmailSender>();
        private readonly Mock<ICombinedClasses> _combinedClasses = new Mock<ICombinedClasses>();
        private readonly Mock<ILoggedUser> _loggedUser = new Mock<ILoggedUser>();
        private readonly Mock<IRegister> _register = new Mock<IRegister>();
        private readonly Mock<ISearchByCategory> _searchByCategory = new Mock<ISearchByCategory>();
        private readonly Mock<IButtonControls> _buttonControls = new Mock<IButtonControls>();

        
        public AccessControllerTesting()
        {
            _access = new AccessController(_db.Object, _emailSender.Object, _combinedClasses.Object,
                _loggedUser.Object, _register.Object, _searchByCategory.Object, _buttonControls.Object);

        }

        [Fact]
        public void test()
        {
            //arrange

            Mock<ISearchByCategory>? test = new Mock<ISearchByCategory>();

            test.Setup(search => search.GetAllData).Returns(true);
            test.Setup(type => type.SearchType).Returns("AllData");


            var controller = new AccessController(_db.Object, _emailSender.Object, _combinedClasses.Object, _loggedUser.Object,
                _register.Object, test.Object, _buttonControls.Object);

            //act

            var result = controller.DisplayUsers();

            //assert

            Assert.NotNull(result);


        }

    }
}