using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;

namespace DentalApp.Tests.Integration
{
    public class PatientIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PatientIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllPatients_ShouldReturnSuccess()
        {
            // 1️⃣ Login and get token
            var token = await LoginAndGetTokenAsync();

            // 2️⃣ Add Authorization header
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 3️⃣ Call secured endpoint
            var response = await _client.GetAsync("/api/patientapi");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task<string> LoginAndGetTokenAsync()
        {
            var loginPayload = new
            {
                email = "admin@example.com",
                password = "admin"
            };

            var content = new StringContent(JsonSerializer.Serialize(loginPayload), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/auth/login", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("token").GetString(); 
        }
    }
}
