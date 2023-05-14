using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ProjectManagementSystem.Entity;

namespace ProjectManagementSystem.ViewModels
{
    [AutoMap(typeof(AppUser), ReverseMap = true)]
    public class AppUserVM
    {
        [SourceMember(nameof(AppUser.Id))]
        public int Id { get; set; }

        [SourceMember(nameof(AppUser.Name))]
        public string Name { get; set; }

        [SourceMember(nameof(AppUser.Surname))]
        public string Surname { get; set; }

        [SourceMember(nameof(AppUser.Role))]
        public short Role { get; set; }

        [SourceMember(nameof(AppUser.Email))]
        public string Email { get; set; }

        [SourceMember(nameof(AppUser.Password))]
        public string Password { get; set; }

    }
}
