using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Entity;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

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
        public string StartDate { get; set; }

        [SourceMember(nameof(Project.EndDate))]
        public string EndDate { get; set; }

        [SourceMember(nameof(Project.Manager))]
        public int Manager { get; set; }

        public List<int> Members { get; set; }
    }
}
