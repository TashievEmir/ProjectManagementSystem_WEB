using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ProjectManagementSystem.Entity;
using System;

namespace ProjectManagementSystem.ViewModels
{
    [AutoMap(typeof(PrTask), ReverseMap = true)]
    public class PrTaskVM
    {
        [SourceMember(nameof(PrTask.Id))]
        public int Id { get; set; }

        [SourceMember(nameof(PrTask.Name))]
        public string Name { get; set; }

        [SourceMember(nameof(PrTask.Status))]
        public int Status { get; set; }

        [SourceMember(nameof(PrTask.StartDate))]
        public string StartDate { get; set; }

        [SourceMember(nameof(PrTask.EndDate))]
        public string EndDate { get; set; }

        [SourceMember(nameof(PrTask.Manager))]
        public int Manager { get; set; }

        [SourceMember(nameof(PrTask.UserId))]
        public int UserId { get; set; }

        [SourceMember(nameof(PrTask.ProjectId))]
        public int ProjectId { get; set; }
    }
}
