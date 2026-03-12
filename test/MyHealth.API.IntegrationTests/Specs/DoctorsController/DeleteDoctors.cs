using System.Threading.Tasks;
using Acheve.AspNetCore.TestHost.Security;
using MyHealth.API.Infrastructure;
using MyHealth.Model;
using Xunit;

namespace MyHealth.API.Specs.DoctorsController
{
    public class DeleteDoctors : DatabaseTestBase
    {
        private readonly Doctor _validDoctor = new Doctor
        {
            TenantId = 1,
            Address = ""
        };

        private async Task<int> CreateDoctorAsync()
        {
            var createResponse = await Server.CreateRequest(Api.Post.Doctors)
                .WithIdentity(Identities.Administrator)
                .WithDefaultPostHeaders()
                .WithContent(_validDoctor)
                .PostAsync();

            createResponse.EnsureSuccessStatusCode();
            return await createResponse.Content.ReadAsAsync<int>();
        }

        [Fact]
        public async Task Administrator_Can_Delete_Doctor()
        {
            var doctorId = await CreateDoctorAsync();

            // Act
            var response = await Server.CreateRequest(Api.Delete.Doctor.For(doctorId))
                .ForTenant(1)
                .WithIdentity(Identities.Administrator)
                .SendAsync("DELETE");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Tenant_Can_Delete_Doctor()
        {
            var doctorId = await CreateDoctorAsync();

            // Act
            var response = await Server.CreateRequest(Api.Delete.Doctor.For(doctorId))
                .ForTenant(1)
                .WithIdentity(Identities.Tenant)
                .SendAsync("DELETE");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task User_Can_Delete_Doctor()
        {
            var doctorId = await CreateDoctorAsync();

            // Act
            var response = await Server.CreateRequest(Api.Delete.Doctor.For(doctorId))
                .ForTenant(1)
                .SendAsync("DELETE");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
