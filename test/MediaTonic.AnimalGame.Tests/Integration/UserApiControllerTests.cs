using FluentAssertions;
using MediaTonic.AnimalGame.API.Models;
using MediaTonic.AnimalGame.API.ViewModels;
using MediaTonic.AnimalGame.Tests.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MediaTonic.AnimalGame.Tests.Integration
{
    public class UserApiControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        public UserApiControllerTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task User_GetUserByName_ReturnUserSuccessCode()
        {
            var response = await _fixture.Client.GetAsync("api/User/GetByUserName?userName=mn123");

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var userViewModel = JsonConvert.DeserializeObject<UserViewModel>(responseStrong);

            var userViewModel2 = User.GetUserByUserName("mn123");

            Assert.Equal(userViewModel.Name, userViewModel2.Name);

        }

        [Fact]
        public async Task User_GetUserByName_ReturnBadRequest()
        {
            var response = await _fixture.Client.GetAsync("api/User/GetByUserName?userName=" + string.Empty);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task User_GetUserByName_Return_NotFound()
        {
            var response = await _fixture.Client.GetAsync("api/User/GetByUserName?userName=mn1123");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task User_GetUserByName_Return_BadRequest()
        {
            var response = await _fixture.Client.GetAsync("api/User/GetByUserName?userName=" + string.Empty);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task User_GetUserById_ReturnUserSuccessCode()
        {
            var user = User.GetUserByUserName("mn123");

            var responseApi = await _fixture.Client.GetAsync("api/User/" + user.Id);

            var responseStrong = await responseApi.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, responseApi.StatusCode);

            var userViewModel = JsonConvert.DeserializeObject<UserViewModel>(responseStrong);

            Assert.Equal(user.Name, userViewModel.Name);
        }

        [Fact]
        public async Task User_GetUserByIncorrectId_ReturnNotFound()
        {
            var responseApi = await _fixture.Client.GetAsync("api/User/" + Guid.NewGuid());

            Assert.Equal(System.Net.HttpStatusCode.NotFound, responseApi.StatusCode);
        }


        [Fact]
        public async Task User_Createuser_ReturnCreatedResponseCode()
        {
            var userPostModel = new CreateUserViewModel
            {
                Name = "Terry Smith",
                UserId = "TS123"
            };

            var responseApi = await _fixture.Client.PostAsync("api/User/", new JsonContent(userPostModel));

            responseApi.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.Created, responseApi.StatusCode);

            var responseStrong = await responseApi.Content.ReadAsStringAsync();

            var userViewModel = JsonConvert.DeserializeObject<UserViewModel>(responseStrong);

            Assert.Equal(userViewModel.UserName, userPostModel.UserId);
        }

        [Fact]
        public async Task User_Createuser_ReturnsConflictError()
        {
            var userPostModel = new CreateUserViewModel
            {
                Name = "Michael Nee",
                UserId = "MN123"
            };

            var responseApi = await _fixture.Client.PostAsync("api/User/", new JsonContent(userPostModel));

            Assert.Equal(System.Net.HttpStatusCode.Conflict, responseApi.StatusCode);
        }
    }
}
