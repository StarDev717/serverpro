using AutoMapper;
using DAL.Core.Models;
using DAL.Resources;
using System.Collections.Generic;
using System.Linq;

namespace ApiServer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resouce with Reverse
            CreateMap<UserRegistration, User>()
                .ReverseMap();

            CreateMap<UsersListResource, User>()
                .ReverseMap();

            CreateMap<BuildingDto, Building>()
                .ReverseMap();

            CreateMap<EmployeeDto, Employee>()
                .ReverseMap();

            //CreateMap<Project, ProjectDto>()
            //    .ForMember(pd => pd.CreatedBy, opt => opt.Ignore())
            //    .AfterMap((p, pd) =>
            //    {
            //        var name = string.Format($"{p.CreatedByUser.FirstName} {p.CreatedByUser.LastName}");
            //        pd.CreatedBy = name;
            //    });

            CreateMap<CreateProjectDto, Project>()
                .ForMember(p => p.Id, pd => pd.Ignore())
                .ForMember(p => p.Buildings, pd => pd.Ignore())
                .ForMember(p => p.Employees, pd => pd.Ignore())
                .ReverseMap();

            CreateMap<ProjectDto, Project>()
                .ForMember(p => p.Id, pd => pd.Ignore())
                .ForMember(p => p.Buildings, pd => pd.Ignore())
                .AfterMap((pd, p) =>
                {
                    var addedBuildings = pd.Buildings.Select(g => g);
                    foreach (var b in addedBuildings)
                    {
                        var building = new Building
                        {
                            ProjectId = pd.Id == 0 ? 0 : (int)pd.Id,
                            Title = b.Title,
                            City = b.City,
                            Address = b.Address,
                            Class = b.Class,
                            Country = b.Country,
                            IsActive = true,
                            State = b.State,
                            Submarket = b.Submarket,
                            Zip = b.Zip
                        };

                        p.Buildings.Add(building);
                    }

                })
                .ReverseMap();

            CreateMap<ProjectionDataDto, ProjectionData>()
                .ForMember(p => p.Id, opt => opt.MapFrom(pd => pd.Id == 0 ? 0 : (int)pd.Id))
                .ForMember(p => p.CreatedByUser, opt => opt.Ignore())
                .ForMember(p => p.LastUpdatedBy, opt => opt.MapFrom(pd => pd.UserId))
                .AfterMap((pd, p) =>
                {
                    if(pd.UserId == null)
                    {
                        p.CreatedBy = pd.UserId;
                    }
                })
                .ReverseMap();

            CreateMap<CreateReportDto, Report>()
                .ForMember(r => r.Id, rd => rd.Ignore())
                .ForMember(r => r.Projections, rd => rd.Ignore())
                .AfterMap((rd, r) =>
                {
                    var addedProjections = rd.Projections.Select(g => g);
                    foreach (var p in addedProjections)
                    {
                        var projectionData = new ProjectionData
                        {
                            BuildingAddress = p.BuildingAddress,
                            BuildingCity = p.BuildingCity,
                            BuildingCountry = p.BuildingCountry,
                            BuildingState = p.BuildingState,
                            BuildingTitle = p.BuildingTitle,
                            BuildingZip = p.BuildingZip,
                            Class = p.Class,
                            Submarket = p.Submarket,
                            Designation = p.Designation,
                            Email = p.Email,
                            EmployeeAddress = p.EmployeeAddress,
                            EmployeeCity = p.EmployeeCity,
                            EmployeeCountry = p.EmployeeCountry,
                            EmployeeState = p.EmployeeState,
                            EmployeeZip = p.EmployeeZip,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            Phone = p.Phone, 
                            BuildingLat = p.BuildingLat,
                            BuildingLong = p.BuildingLong, 
                            EmployeeLat = p.EmployeeLat, 
                            EmployeeLong = p.EmployeeLong,
                            Distance = p.Distance, 
                            Duration = p.Duration
                        };
                        r.Projections.Add(projectionData);
                    }


                });

            CreateMap<Report, ReportDto>()
                .ForMember(rd => rd.Buildings, r => r.Ignore())
                .ForMember(rd => rd.Employees, r => r.Ignore())
                .ForMember(rd => rd.Projections, r => r.Ignore())
                .AfterMap((r, rd) =>
                {
                    var allProjections = r.Projections.Select(g => g);
                    Dictionary<string, List<EmployeeDistanceDto>> dictionary =
                                new Dictionary<string, List<EmployeeDistanceDto>>();
                    var projections = new List<ProjectionDataDto>();
                    var employees = new List<EmployeeDistanceDto>();

                    var reportEmployees = new List<EmployeeDistanceDto>();
                    var reportBuildings = new List<BuildingDistanceDto>();

                    Dictionary<string, bool> employeeDictionary =
                                new Dictionary<string, bool>();

                    foreach (var p in allProjections)
                    {
                        var name = string.Format($"{p.BuildingTitle} - {p.BuildingAddress}");
                        var employeesData = new EmployeeDistanceDto
                        {
                            Address = p.EmployeeAddress,
                            City = p.EmployeeCity,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            Country = p.EmployeeCountry,
                            State = p.EmployeeState,
                            Zip = p.EmployeeZip,
                            Designation = p.Designation,
                            Phone = p.Phone,
                            Email = p.Email,
                            EmployeeLat = p.EmployeeLat,
                            EmployeeLong = p.EmployeeLong,
                            Distance = p.Distance,
                            Duration = p.Duration,
                        };

                        var buildingData = new BuildingDistanceDto
                        {
                            Title = p.BuildingTitle,
                            Address = p.BuildingAddress,
                            City = p.BuildingCity,
                            Country = p.EmployeeCountry,
                            State = p.EmployeeState,
                            Zip = p.EmployeeZip,
                            BuildingLat = p.BuildingLat,
                            BuildingLong = p.BuildingLong
                        };


                        var projectionData = new ProjectionDataDto
                        {
                            Id = p.Id,
                            BuildingTitle = p.BuildingTitle,
                            BuildingAddress = p.BuildingAddress,
                            BuildingCity = p.BuildingCity,
                            BuildingCountry = p.BuildingCountry,
                            BuildingState = p.BuildingState,
                            BuildingZip = p.BuildingZip,
                            BuildingLat = p.BuildingLat,
                            BuildingLong = p.BuildingLong,
                            Class = p.Class,
                            Submarket = p.Submarket,
                            Employees = employees
                        };
                        if (!dictionary.ContainsKey(name))
                        {
                            reportBuildings.Add(buildingData);
                            employees = new List<EmployeeDistanceDto>();
                            employees.Add(employeesData);
                            projectionData.Employees = employees;
                            dictionary.Add(name, employees);
                            projections.Add(projectionData);
                        }
                        else
                        {
                            dictionary[name].Add(employeesData);
                            var found = projections.FirstOrDefault(c => c.BuildingTitle == p.BuildingTitle && c.BuildingAddress == p.BuildingAddress);
                            found.Employees = dictionary[name];
                        }

                        if (!employeeDictionary.ContainsKey(p.EmployeeAddress))
                        {
                            employeeDictionary.Add(p.EmployeeAddress, true);
                            reportEmployees.Add(employeesData);
                        }
                    }

                    rd.Projections = projections;
                    rd.Buildings = reportBuildings;
                    rd.Employees = reportEmployees;
                });

        }
    }
}
