using MediaTonic.AnimalGame.API.Models;
using MediaTonic.AnimalGame.Tests.Attributes;
using MediaTonic.AnimalGame.Tests.xUnit.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MediaTonic.AnimalGame.Tests.Unit
{
    [TestCaseOrderer("TestOrderExamples.TestCaseOrdering.PriorityOrderer", "TestOrderExamples")]
    public class UserTests
    {
        public UserTests()
        {

        }

        [Fact, TestPriority(9999)]
        public void Create_User_Returns_User()
        {
            var user = User.CreateUser("Michael Smith", "MS1987");
            Assert.IsType<User>(user);

            Assert.NotNull(user);
            Assert.Equal("Michael Smith", user.Name);
            Assert.Equal("MS1987", user.UserName);
            Assert.Equal(0, user.AnimalCount);
            Assert.Empty(user.UserAnimals);
            Assert.NotEmpty(User.Users);
        }

        [Fact, TestPriority(2)]
        public void Create_User_AddAnimal()
        {
            var user = User.CreateUser("Michael Cheese", "MC18938344");
            Assert.IsType<User>(user);

            var animal = Animal.CreateAnimal(API.Enums.AnimalType.Dog, "Snowy", user);
            Assert.IsType<Animal>(animal);

            Assert.NotNull(animal.User);

            Assert.Equal(user, animal.User);
        }

        [Fact, TestPriority(3)]
        public void Create_Multiple_Users_SameUser_ReturnsSameUser()
        {
            var user = User.CreateUser("Michael Legs", "lgs89334342");

            var user2 = User.CreateUser("Michael Legs", "lgs89334342");

            Assert.Equal(user, user2);
        }

        [Fact, TestPriority(5)]
        public void Create_User_AddAnimalTwice_ThrowsException()
        {
            var user = User.CreateUser("Michael Cheese", "MC18938344");
            Assert.IsType<User>(user);

            var animal = Animal.CreateAnimal(API.Enums.AnimalType.Dog, "Duffy", user);

            Assert.IsType<Animal>(animal);
        }
    }
}
