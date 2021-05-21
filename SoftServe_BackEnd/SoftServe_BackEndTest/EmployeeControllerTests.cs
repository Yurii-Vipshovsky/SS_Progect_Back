// using System.Net;
// using System.Net.Http;
// using System.Threading.Tasks;
// using FluentAssertions;
// using SoftServe_BackEndTest;
// using Xunit;
//
// namespace SoftServe_BackEndTest
// {
//     public class EmployeeControllerTests: IntegrationTest
//     {
//         [Fact]
//         public async Task GetAll_StatusCode_Test()
//         {
//             var response = await TestClient.GetAsync("api/Event");
//             response.StatusCode.Should().Be(HttpStatusCode.OK);
//         }
//         
//         [Fact]
//         public async Task GetAll_Empty()
//         {
//             var response = TestClient.GetAsync("api/Event");
//             var pagedResponse = await GetPageResponse(response);
//             ((int)pagedResponse["totalRecords"]).Should().Be(0);
//             ((string) pagedResponse["message"]).Should().Be("Page is empty");
//         }
//         
//         [Fact]
//         public async Task GetAll_AfterPost()
//         {
//             var response = TestClient.GetAsync("api/Event");
//             var pagedResponse = await GetPageResponse(response);
//         
//             var startCount = ((int) pagedResponse["totalRecords"]);
//             
//             var newEmployee = await CreatePostAsync(GetNewEvent(31));
//         
//             response = TestClient.GetAsync("api/Employees");
//             pagedResponse = await GetPageResponse(response);
//             ((int)pagedResponse["totalRecords"]).Should().Be(startCount+1);
//             var returnedEmployee = pagedResponse["data"][startCount];
//             ((int)returnedEmployee["id"]).Should().Be(newEmployee.Id);
//         }
//         
//         [Fact]
//         public async Task GetAll_Sorted()
//         {
//             var response = TestClient.GetAsync("api/Employees");
//             var pagedResponse = await GetPageResponse(response);
//             
//             var startCount = ((int)pagedResponse["totalRecords"]);
//             
//             await CreatePostAsync(GetNewEvent(1, name: "Arsen"));
//             await CreatePostAsync(GetNewEvent(2, name: "Maria"));
//             await CreatePostAsync(GetNewEvent(3, name: "Bohdan"));
//             
//             response = TestClient.GetAsync("api/Employees?sortBy=FirstName");
//             pagedResponse = await GetPageResponse(response);
//             ((int)pagedResponse["totalRecords"]).Should().Be(startCount+3);
//             var employeeData = (pagedResponse["data"]);
//             employeeData.Should().BeInAscendingOrder(obj => obj["firstName"]);
//         }
//         
//         [Fact]
//         public async Task GetById_AfterPost()
//         {
//             var response = await TestClient.GetAsync("api/Employees/100");
//         
//             response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//         
//             await CreatePostAsync(GetNewEmployee(100));
//             
//             var responseAfterPost = await TestClient.GetAsync("api/Employees/100");
//             
//             responseAfterPost.StatusCode.Should().Be(HttpStatusCode.OK);
//         }
//         
//         [Fact]
//         public async Task DeleteById_AfterPost()
//         {
//             await CreatePostAsync(GetNewEmployee(14));
//             
//             var response = TestClient.GetAsync("api/Employees/14");
//             (await response).StatusCode.Should().Be(HttpStatusCode.OK);
//             var dataResponse = await GetPageResponse(response);
//             
//             ((int)dataResponse["data"]["id"]).Should().Be(14);
//         
//             await TestClient.DeleteAsync("api/Employees/14");
//             response = TestClient.GetAsync("api/Employees/14");
//             (await response).StatusCode.Should().Be(HttpStatusCode.NotFound);
//         }
//         
//         [Fact]
//         public async Task UpdateById()
//         {
//             await CreatePostAsync(GetNewEmployee(21));
//             
//             var response = TestClient.GetAsync("api/Employees/21");
//             (await response).StatusCode.Should().Be(HttpStatusCode.OK);
//             var pageResponse = await GetPageResponse(response);
//             
//            ((int)pageResponse["data"]["id"]).Should().Be(21);
//             await TestClient.PutAsJsonAsync("api/Employees/21", GetNewEmployee(22));
//             
//             var responseForId1 = await TestClient.GetAsync("api/Employees/21");
//             responseForId1.StatusCode.Should().Be(HttpStatusCode.NotFound);
//             var responseForId2 = await TestClient.GetAsync("api/Employees/22");
//             responseForId2.StatusCode.Should().Be(HttpStatusCode.OK);
//         
//         }
//     }
// }