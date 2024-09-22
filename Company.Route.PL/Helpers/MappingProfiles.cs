using AutoMapper;
using Company.Route.DAL.Models;
using Company.Route.PL.ViewModels.Employess;

namespace Company.Route.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // ForMember is to configure the data mapped if they have diff names , but no need here to do that
            CreateMap<EmployeeViewModel ,Employee> ().ReverseMap()/*.ForMember(d=>d.Name , o=>o.MapFrom(s=>s.EmpName))*/  ;
        }

    }
}
