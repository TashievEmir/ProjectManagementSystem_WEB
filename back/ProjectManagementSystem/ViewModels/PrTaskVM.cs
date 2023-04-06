using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ProjectManagementSystem.Entity;
using System;

namespace ProjectManagementSystem.ViewModels
{
    [AutoMap(typeof(PrTask), ReverseMap = true)]
    public class PrTaskVM
    {
        [SourceMember(nameof(PrTask.Name))]
        public string Name { get; set; }

        [SourceMember(nameof(PrTask.Status))]
        public int Status { get; set; }

        [SourceMember(nameof(PrTask.StartDate))]
        public DateTime StartDate { get; set; }

        [SourceMember(nameof(PrTask.EndDate))]
        public DateTime EndDate { get; set; }

        [SourceMember(nameof(PrTask.Manager))]
        public int Manager { get; set; }

        [SourceMember(nameof(PrTask.UserId))]
        public int UserId { get; set; }

        [SourceMember(nameof(PrTask.ProjectId))]
        public int ProjectId { get; set; }
    }
}
