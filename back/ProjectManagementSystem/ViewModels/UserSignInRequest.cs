using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ProjectManagementSystem.Entity;

namespace ProjectManagementSystem.ViewModels
{
    [AutoMap(typeof(AppUser), ReverseMap = true)]
    public class UserSignInRequest
    {
        [SourceMember(nameof(AppUser.Password))]
        public string Password { get; set; }
        [SourceMember(nameof(AppUser.Email))]
        public string Email { get; set; }
    }
}
