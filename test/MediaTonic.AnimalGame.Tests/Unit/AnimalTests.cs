using MediaTonic.AnimalGame.API.Models;
using MediaTonic.AnimalGame.Tests.Attributes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MediaTonic.AnimalGame.Tests.Unit
{
    public class AnimalTests
    {
        public AnimalTests()
        {

        }

        [Fact]
        public void Create_Animal_Reutrn_Animal()
        {
            var user = User.CreateUser("user 1", "user1");

            var animal = Animal.CreateAnimal(API.Enums.AnimalType.Cat, "Robbie", user);

            Assert.NotNull(animal);

            Assert.IsType<Animal>(animal);

            Assert.Equal("Robbie", animal.Name);

            Assert.Equal(50, animal.CurrentHungriness());

            Assert.NotNull(animal.User);

            //Check Id is created for animal
            Assert.NotEqual(System.Guid.Empty, animal.Id);

            //Check at least an animal is in the animal list
            Assert.NotEmpty(Animal.Animals);
        }

        [Fact]
        public void Animal_Dog_Feed_Returns_CorrectFeed()
        {
            var user2 = User.CreateUser("user 2", "user2");

            var animal = Animal.CreateAnimal(API.Enums.AnimalType.Dog, "Charles", user2);

            Assert.NotNull(animal);
            Assert.IsType<Animal>(animal);
            Assert.Equal(50, animal.CurrentHungriness());
            animal.FeedAnimal();
            Assert.Equal(44, animal.CurrentHungriness());
        }

        [Fact]
        public void Animal_Rabbit_Feed_Returns_CorrectFeed()
        {
            var user3 = User.CreateUser("user 3", "user3");

            var animal = Animal.CreateAnimal(API.Enums.AnimalType.Rabbit, "Daisy", user3);

            Assert.NotNull(animal);
            Assert.IsType<Animal>(animal);
            Assert.Equal(50, animal.CurrentHungriness());
            animal.FeedAnimal();
            Assert.Equal(40, animal.CurrentHungriness());
        }

        [Fact]
        public void Animal_Dog_Petting_Returns_CorrectHappinessRating()
        {
            var user4 = User.CreateUser("user 4", "user4");

            var animal = Animal.CreateAnimal(API.Enums.AnimalType.Dog, "Fluffy", user4);

            Assert.NotNull(animal);
            Assert.IsType<Animal>(animal);
            Assert.Equal(50, animal.CurrentHappiness());
            animal.PetAnimal();
            Assert.Equal(54, animal.CurrentHappiness());
        }

        [Fact]
        public void Animal_Tortoise_Pet_Returns_CorrectHappinessRating()
        {
            var user5 = User.CreateUser("user 5", "user5");

            var animal = Animal.CreateAnimal(API.Enums.AnimalType.Tortoise, "George", user5);

            Assert.NotNull(animal);
            Assert.IsType<Animal>(animal);
            Assert.Equal(50, animal.CurrentHappiness());
            animal.PetAnimal();
            Assert.Equal(51, animal.CurrentHappiness());
        }

        [Fact]
        public void Animal_Dog_Pet_DecreasesAfter65_Secs()
        {
            var user6 = User.CreateUser("user 6", "user6");

            var animal = Animal.CreateAnimal(API.Enums.AnimalType.Dog, "Neil", user6);


            Assert.NotNull(animal);
            Assert.IsType<Animal>(animal);
            Assert.Equal(50, animal.CurrentHappiness());
            System.Threading.Thread.Sleep(65000);


            Assert.True(animal.CurrentHappiness() < 50);
        }

        [Fact]
        public void Animal_Dog_Pet_Hunger_Increases_After65_Secs()
        {
            var user7 = User.CreateUser("user 7", "user7");

            var animal = Animal.CreateAnimal(API.Enums.AnimalType.Dog, "Terry", user7);


            Assert.NotNull(animal);
            Assert.IsType<Animal>(animal);
            Assert.Equal(50, animal.CurrentHungriness());
            System.Threading.Thread.Sleep(65000);

            Assert.True(animal.CurrentHungriness() > 50);
        }


        [Fact, TestPriority(10)]
        public void Create_Animal_NullUser_Returns_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => Animal.CreateAnimal(API.Enums.AnimalType.Dog, "Duffy", null));
        }

        [Fact, TestPriority(10)]
        public void Create_Animal_EmptyUserName_Returns_Exception()
        {
            var user = User.CreateUser("Michael Cheese", "MC18938344");
            Assert.IsType<User>(user);

            Assert.Throws<ArgumentException>(() => Animal.CreateAnimal(API.Enums.AnimalType.Dog, string.Empty, user));
        }
    }
}
