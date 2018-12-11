using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AdventureWorks.Services.HumanResources;
using AutoMapper;

namespace AdventureWorks.Web.Controllers
{
    public class DepartmentApiController : ApiController
    {
        private static readonly List<Department> Departments = new List<Department>
        {
             new Department
                {
                    Id = 4,
                    Name = "Batman",
                    GroupName = "Bruce Wayne"
                },
                new Department
                {
                    Id = 5,
                    Name = "Wolverine",
                    GroupName = "Logan"
                }
        };
        /// <summary>
        /// Get all departments
        /// </summary>
        /// <remarks>
        /// Get a list of all departments
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [ResponseType(typeof(IEnumerable<Department>))]
        public HttpResponseMessage Get()
        {
            DepartmentService departmentService = new DepartmentService();
            var departmentGroups = departmentService.GetDepartments();
            //return Request.CreateResponse(HttpStatusCode.OK, Departments);
            return Request.CreateResponse(HttpStatusCode.OK, departmentGroups);
        }
        /// <summary>
        /// Get department by id
        /// </summary>
        /// <remarks>
        /// Get a department by id
        /// </remarks>
        /// <param name="id">Id of Department</param>
        /// <returns></returns>
        /// <response code="200">Department found</response>
        /// <response code="404">Department not foundd</response>
        [ResponseType(typeof(Department))]
        public HttpResponseMessage GetById(int id)
        {
            var department = Departments.FirstOrDefault(c => c.Id == id);

            return department == null
                ? Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department not found")
                : Request.CreateResponse(HttpStatusCode.OK, department);
        }
        /// <summary>
        /// Add new Department
        /// </summary>
        /// <remarks>
        /// Add a new Department
        /// </remarks>
        /// <param name="postDepartmentModel">Department to add</param>
        /// <returns></returns>
        /// <response code="201">Department created</response>
        [Authorize(Roles = "write")]
        [ResponseType(typeof(Department))]
        public HttpResponseMessage Post(Department postDepartmentModel)
        {
            // Map a PostDepartmentModel object to Department object
            var department = Mapper.Map<Department>(postDepartmentModel);

            department.Id = Departments.Count+1;
            Departments.Add(department);

            return Request.CreateResponse(HttpStatusCode.Created, department);
        }

        /// <summary>
        /// Update an existing department
        /// </summary>
        /// <param name="putDepartmentModel">Department to update</param>
        /// <returns></returns>
        /// <response code="200">Department updated</response>
        /// <response code="404">Department not found</response>
        [Authorize(Roles = "write")]
        //[Authorize]
        [ResponseType(typeof(Department))]
        public HttpResponseMessage Put(Department putDepartmentModel)
        {
            var existingDepartment = Departments.FirstOrDefault(c => c.Id == putDepartmentModel.Id);
            if (existingDepartment == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department not found");

            var department = Mapper.Map<Department>(putDepartmentModel);
            Departments.Remove(existingDepartment);
            Departments.Add(department);

            return Request.CreateResponse(HttpStatusCode.OK, department);
        }

        /// <summary>
        /// Delete a Department
        /// </summary>
        /// <remarks>
        /// Delete a Department
        /// </remarks>
        /// <param name="id">Id of the Department to delete</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(int id)
        {
            var existingDepartment = Departments.FirstOrDefault(c => c.Id == id);

            if (existingDepartment == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department not found");

            Departments.Remove(existingDepartment);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
