using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AdventureWorks.Services.HumanResources;

namespace AdventureWorks.Web.Controllers
{
    public class AzureTrainingController : ApiController
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
        [ResponseType(typeof(IEnumerable<Department>))]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, Departments);
        }

        [ResponseType(typeof(Department))]
        public HttpResponseMessage GetById(int id)
        {
            var department = Departments.FirstOrDefault(c => c.Id == id);

            return department == null
                ? Request.CreateErrorResponse(HttpStatusCode.NotFound, "Superhero not found")
                : Request.CreateResponse(HttpStatusCode.OK, department);
        }
        //// GET: api/AzureTraining
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/AzureTraining/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/AzureTraining
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/AzureTraining/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/AzureTraining/5
        //public void Delete(int id)
        //{
        //}
    }
}
