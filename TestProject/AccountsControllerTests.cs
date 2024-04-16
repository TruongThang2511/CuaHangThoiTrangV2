using CuaHangThoiTrangV2.Controllers;
using CuaHangThoiTrangV2.Models;
using CuaHangThoiTrangV2.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CuaHangThoiTrangV2
{
    [TestFixture]
    public class Tests
    {
        private dbCHTTContext _dbContext;
        private AccountsController _accountsController;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<dbCHTTContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new dbCHTTContext(options);

            // Khởi tạo AccountsController với DbContext kiểm thử
            _accountsController = new AccountsController(_dbContext);
        }

        [TearDown]
        public void Cleanup()
        {
            // Xóa cơ sở dữ liệu trong bộ nhớ sau mỗi test
            _dbContext.Database.EnsureDeleted();
        }
        [Test]
        public async Task Register_ValidModel_Success()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            {
                FullName = "John Doe",
                Email = "john.doe@example.com",
                Phone = "123456789",
                Password = "password",
                ConfirmPassword = "password"
            };

            // Act
            var result = await _accountsController.Register(registerViewModel) as RedirectToActionResult;

            // Assert
            Assert.Equals("Index", result.ActionName);
            Assert.Equals("Accounts", result.ControllerName);
        }

        [Test]
        public async Task Login_ValidCredentials_Success()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                email = "john.doe@example.com",
                Password = "password"
            };

            // Act
            var result = await _accountsController.Login(loginViewModel) as RedirectToActionResult;

            // Assert
            Assert.Equals("Index", result.ActionName);
            Assert.Equals("Home", result.ControllerName);
        }
        [Test]
        public void TestMethod()
        {
            // Kiểm tra lớp kiểm thử trong một assembly
            Assembly assembly = Assembly.GetExecutingAssembly(); // Hoặc sử dụng Assembly.Load("TênAssembly")
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.GetCustomAttributes(typeof(TestFixtureAttribute), true).Length > 0)
                {
                    Console.WriteLine("Tìm thấy lớp kiểm thử: " + type.Name);
                }
            }
        }

    }
}