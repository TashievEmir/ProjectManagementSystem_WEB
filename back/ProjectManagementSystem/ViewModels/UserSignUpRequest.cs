using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ProjectManagementSystem.Entity;

namespace ProjectManagementSystem.ViewModels
{
    [AutoMap(typeof(AppUser), ReverseMap = true)]
    public class UserSignUpRequest
    {
        [SourceMember(nameof(AppUser.Name))]
        public string Name { get; set; }
        [SourceMember(nameof(AppUser.Surname))]
        public string Surname { get; set; }
        [SourceMember(nameof(AppUser.Password))]
        public string Password { get; set; }
        [SourceMember(nameof(AppUser.Role))]
        public short Role { get; set; }
        [SourceMember(nameof(AppUser.Email))]
        public string Email { get; set; }
    }
}
