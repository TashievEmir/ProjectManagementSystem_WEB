using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ProjectManagementSystem.Entity;
using System;

namespace ProjectManagementSystem.ViewModels
{
    [AutoMap(typeof(Project), ReverseMap = true)]
    public class ProjectVM
    {
        [SourceMember(nameof(Project.Name))]
        public string Name { get; set; }

        [SourceMember(nameof(Project.Status))]
        public string Status { get; set; }

        [SourceMember(nameof(Project.StartDate))]
        public DateTime StartDate { get; set; }

        [SourceMember(nameof(Project.EndDate))]
        public DateTime EndDate { get; set; }

        [SourceMember(nameof(Project.Manager))]
        public int Manager { get; set; }

        /*[SourceMember(nameof(ProjectUser.UserId))]
        public int UserId { get; set; }

        [SourceMember(nameof(ProjectUser.ProjectId))]
        public int ProjectId { get; set; }*/
    }
}
