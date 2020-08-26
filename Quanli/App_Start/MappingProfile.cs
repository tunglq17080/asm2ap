using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Quanli.Models;
using Quanli.Dtos;

namespace Quanli.App_Start
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Subject, SubjectDto>();
            Mapper.CreateMap<SubjectDto, Subject>();
        }
    }
}