using DAL.Core.Models;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Core
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IRepository<Employee> EmployeeRepository { get; }
        IRepository<Project> ProjectRepository { get; }
        IRepository<Building> BuildingRepository { get; }
        IRepository<ProjectionData> ProjectionDataRepository { get; }
        IRepository<Report> ReportRepository { get; }

        /// <summary>
        /// Commits all changes
        /// </summary>
        Task Commit();
        /// <summary>
        /// Discards all changes that has not been commited
        /// </summary>
        void RejectChanges();
        void Dispose();
    }
}
