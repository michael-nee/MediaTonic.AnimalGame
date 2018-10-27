using MediaTonic.AnimalGame.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Newtonsoft.Json;
using MediaTonic.AnimalGame.API.ViewModels;
using MediaTonic.AnimalGame.Tests.Extensions;
using System.Net.Http;

namespace MediaTonic.AnimalGame.Tests.Integration
{
    public class AnimalApiControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public AnimalApiControllerTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAnimal_ReturnsAnimal()
        {
            var animal = User.GetUserByUserName("mn123").UserAnimals.FirstOrDefault();

            var response = await _fixture.Client.GetAsync("api/Animal/" + animal.Id);

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var animalViewModel = JsonConvert.DeserializeObject<AnimalViewModel>(responseStrong);

            Assert.Equal(animal.Id, animalViewModel.Id);
            Assert.Equal(animal.Name, animal.Name);
        }

        [Fact]
        public async Task GetAnimal_Wait_1min_ReturnsAnimalWithDifferentHungrinessLevels()
        {
            var animal = User.GetUserByUserName("mn123").UserAnimals.FirstOrDefault();
            System.Threading.Thread.Sleep(65000);

            var response = await _fixture.Client.GetAsync("api/Animal/" + animal.Id);

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var animalViewModel = JsonConvert.DeserializeObject<AnimalViewModel>(responseStrong);

            Assert.True(animalViewModel.CurrentHappiness < 50);
            Assert.True(animalViewModel.CurrentHungriness > 50);
        }

        [Fact]
        public async Task GetAnimal_ReturnsNotFound()
        {
            var response = await _fixture.Client.GetAsync("api/Animal/" + Guid.NewGuid());

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAnimalWithUserId_ReturnsAnimal()
        {
            var userId = "mn123";

            var user = User.GetUserByUserName(userId);
            var animal = user.UserAnimals.FirstOrDefault();

            var response = await _fixture.Client.GetAsync("api/Animal/GetByIdUserId?id=" + animal.Id + "&userId=" + user.Id);

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var animalViewModel = JsonConvert.DeserializeObject<AnimalViewModel>(responseStrong);

            Assert.Equal(animal.Id, animalViewModel.Id);
            Assert.Equal(animal.Name, animal.Name);
        }

        [Fact]
        public async Task GetAnimal_WithIncorrectUserId_ReturnsNotFound()
        {
            var userId = "mn123";

            var user = User.GetUserByUserName(userId);
            var animal = user.UserAnimals.FirstOrDefault();

            var response = await _fixture.Client.GetAsync("api/Animal/GetByIdUserId?id=" + animal.Id + "&userId=" + Guid.NewGuid());

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

            Assert.Equal("\"No user found for given userId\"", await response.Content.ReadAsStringAsync());

        }

        [Fact]
        public async Task GetAnimal_WithIncorrectAnimalId_ReturnsNotFound()
        {
            var userId = "mn123";

            var user = User.GetUserByUserName(userId);

            var response = await _fixture.Client.GetAsync("api/Animal/GetByIdUserId?id=" + Guid.NewGuid() + "&userId=" + user.Id);

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

            Assert.Equal("\"No animal found for given id\"", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task GetAnimalWithUserName_ReturnsAnimal()
        {
            var userId = "mn123";

            var user = User.GetUserByUserName(userId);
            var animal = user.UserAnimals.FirstOrDefault();

            var response = await _fixture.Client.GetAsync("api/Animal/GetByIdUserName?id=" + animal.Id + "&userName=" + userId);

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var animalViewModel = JsonConvert.DeserializeObject<AnimalViewModel>(responseStrong);

            Assert.Equal(animal.Id, animalViewModel.Id);
            Assert.Equal(animal.Name, animal.Name);
        }

        [Fact]
        public async Task GetAnimal_WithIncorrectUserName_ReturnsNotFound()
        {
            var userId = "mn123";

            var user = User.GetUserByUserName(userId);
            var animal = user.UserAnimals.FirstOrDefault();

            var response = await _fixture.Client.GetAsync("api/Animal/GetByIdUserName?id=" + animal.Id + "&userName=" + "mn111111");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

            Assert.Equal("\"No user found for given userName\"", await response.Content.ReadAsStringAsync());

        }

        [Fact]
        public async Task GetAnimalUserName_WithIncorrectAnimalId_ReturnsNotFound()
        {
            var userName = "mn123";

            var response = await _fixture.Client.GetAsync("api/Animal/GetByIdUserName?id=" + Guid.NewGuid() + "&userName=" + userName);

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

            Assert.Equal("\"No animal found for given id\"", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Animal_CreateAnimal_ReturnCreatedResponseCode()
        {
            var userPostModel = new CreateAnimalViewModel
            {
                Name = "Craig",
                UserId = "mn123",
                AnimalType = API.Enums.AnimalType.Dog
            };

            var responseApi = await _fixture.Client.PostAsync("api/Animal/", new JsonContent(userPostModel));

            responseApi.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.Created, responseApi.StatusCode);

            var responseStrong = await responseApi.Content.ReadAsStringAsync();

            var userViewModel = JsonConvert.DeserializeObject<AnimalViewModel>(responseStrong);

            Assert.Equal(userViewModel.Name, userPostModel.Name);
        }

        [Fact]
        public async Task Animal_CreateAnimal_WrongUserName_ReturnNotFound()
        {
            var userPostModel = new CreateAnimalViewModel
            {
                Name = "Craig",
                UserId = "mn12311111",
                AnimalType = API.Enums.AnimalType.Dog
            };

            var responseApi = await _fixture.Client.PostAsync("api/Animal/", new JsonContent(userPostModel));

            Assert.Equal(System.Net.HttpStatusCode.NotFound, responseApi.StatusCode);

            Assert.Equal("\"No user for userId provided\"", await responseApi.Content.ReadAsStringAsync());

        }

        [Fact]
        public async Task Animal_Pet_Return_OkWithHappinesssLevelAbove50()
        {
            var userId = "mn123";

            var user = User.GetUserByUserName(userId);
            var animal = user.UserAnimals.FirstOrDefault();

            var httpContent = new StringContent(animal.Id.ToString(), Encoding.UTF8, "application/json");

            var responseApi = await _fixture.Client.PutAsync("api/Animal/Pet?id=" + animal.Id, new JsonContent(string.Empty));

            Assert.Equal(System.Net.HttpStatusCode.OK, responseApi.StatusCode);

            var responseStrong = await responseApi.Content.ReadAsStringAsync();

            var happiness = decimal.Parse(responseStrong);

            Assert.True(happiness > 50);
        }

        [Fact]
        public async Task Animal_Pet_Wrong_AnimalId_ReturnNotFound()
        {
            var responseApi = await _fixture.Client.PutAsync("api/Animal/Pet?id=" + Guid.NewGuid(), new JsonContent(string.Empty));

            Assert.Equal(System.Net.HttpStatusCode.NotFound, responseApi.StatusCode);
        }

        [Fact]
        public async Task Animal_Feed_Return_Ok_WithHungrinessLevelBelow50()
        {
            var userId = "mn123";

            var user = User.GetUserByUserName(userId);
            var animal = user.UserAnimals.FirstOrDefault();

            var httpContent = new StringContent(animal.Id.ToString(), Encoding.UTF8, "application/json");

            var responseApi = await _fixture.Client.PutAsync("api/Animal/Feed?id=" + animal.Id, new JsonContent(string.Empty));

            Assert.Equal(System.Net.HttpStatusCode.OK, responseApi.StatusCode);

            var responseStrong = await responseApi.Content.ReadAsStringAsync();

            var hungriness = decimal.Parse(responseStrong);

            Assert.True(hungriness < 50);
        }

        [Fact]
        public async Task Animal_Feed_Wrong_AnimalId_ReturnNotFound()
        {
            var responseApi = await _fixture.Client.PutAsync("api/Animal/Feed?id=" + Guid.NewGuid(), new JsonContent(string.Empty));

            Assert.Equal(System.Net.HttpStatusCode.NotFound, responseApi.StatusCode);
        }
    }
}
