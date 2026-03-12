using System.Collections.Generic;
using System.Threading.Tasks;
using Acheve.AspNetCore.TestHost.Security;
using FluentAssertions;
using MyHealth.API.Infrastructure;
using MyHealth.Model;
using Xunit;

namespace MyHealth.API.Specs.DoctorsController
{
    public class UpdateDoctors : DatabaseTestBase
    {
        private readonly Doctor _validDoctor = new Doctor
        {
            TenantId = 1,
            Address = "",
            Qualifications = "MD, PhD"
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
        public async Task Administrator_Can_Update_Doctor()
        {
            var doctorId = await CreateDoctorAsync();

            var updatedDoctor = new Doctor
            {
                DoctorId = doctorId,
                TenantId = 1,
                Name = "Dr. Updated",
                Address = "123 Main St",
                Qualifications = "MD, FACP",
                Speciality = Specialities.Cardiologist
            };

            // Act
            var response = await Server.CreateRequest(Api.Put.Doctors)
                .ForTenant(1)
                .WithIdentity(Identities.Administrator)
                .WithDefaultPostHeaders()
                .WithContent(updatedDoctor)
                .SendAsync("PUT");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Tenant_Can_Update_Doctor()
        {
            var doctorId = await CreateDoctorAsync();

            var updatedDoctor = new Doctor
            {
                DoctorId = doctorId,
                TenantId = 1,
                Name = "Dr. Tenant Updated",
                Address = "456 Elm St",
                Qualifications = "MD",
                Speciality = Specialities.Orthopedist
            };

            // Act
            var response = await Server.CreateRequest(Api.Put.Doctors)
                .ForTenant(1)
                .WithIdentity(Identities.Tenant)
                .WithDefaultPostHeaders()
                .WithContent(updatedDoctor)
                .SendAsync("PUT");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task User_Can_Update_Doctor()
        {
            var doctorId = await CreateDoctorAsync();

            var updatedDoctor = new Doctor
            {
                DoctorId = doctorId,
                TenantId = 1,
                Name = "Dr. User Updated",
                Address = "789 Oak Ave",
                Qualifications = "MBBS",
                Speciality = Specialities.Neurosurgeon
            };

            // Act
            var response = await Server.CreateRequest(Api.Put.Doctors)
                .ForTenant(1)
                .WithDefaultPostHeaders()
                .WithContent(updatedDoctor)
                .SendAsync("PUT");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
