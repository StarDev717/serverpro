using DAL.Core.Models;
using DAL.Persistence;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiContext context;
        private IRepository<User> _userRepository;
        private IRepository<Employee> _employeeRepository;
        private IRepository<Project> _projectRepository;
        private IRepository<Building> _buildingRepository;
        private IRepository<ProjectionData> _projectionDataRepository;
        private IRepository<Report> _reportRepository;

        public UnitOfWork(ApiContext context)
        {
            this.context = context;
        }

        public IRepository<User> UserRepository
        {
            get
            {

                if (_userRepository == null)
                    _userRepository = new GenericRepository<User>(context);
                return _userRepository;
            }
        }

        public IRepository<Employee> EmployeeRepository
        {
            get
            {

                if (_employeeRepository == null)
                    _employeeRepository = new GenericRepository<Employee>(context);
                return _employeeRepository;
            }
        }

        public IRepository<Project> ProjectRepository
        {
            get
            {

                if (_projectRepository == null)
                    _projectRepository = new GenericRepository<Project>(context);
                return _projectRepository;
            }
        }

        public IRepository<Building> BuildingRepository
        {
            get
            {

                if (_buildingRepository == null)
                    _buildingRepository = new GenericRepository<Building>(context);
                return _buildingRepository;
            }
        }

        
        public IRepository<ProjectionData> ProjectionDataRepository
        {
            get
            {

                if (_projectionDataRepository == null)
                    _projectionDataRepository = new GenericRepository<ProjectionData>(context);
                return _projectionDataRepository;
            }
        }

        
        public IRepository<Report> ReportRepository
        {
            get
            {

                if (_reportRepository == null)
                    _reportRepository = new GenericRepository<Report>(context);
                return _reportRepository;
            }
        }

        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void RejectChanges()
        {
            foreach (var entry in context.ChangeTracker.Entries()
              .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}
