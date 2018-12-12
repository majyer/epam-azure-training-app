using System.Web.Mvc;
using AdventureWorks.Services.HumanResources;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace AdventureWorks.Web.Controllers
{
    public class DepartmentsController : Controller
    {
        // GET: Departments
        public ActionResult Index()
        {
            //DepartmentService departmentService = new DepartmentService();
            //var departmentGroups = departmentService.GetDepartments();

            //return View(departmentGroups);
            HttpContent httpContent = new StringContent("");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var httpClient = new HttpClient();
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            //string strUrl = "http://azure-training-webapps.azurewebsites.net/api/DepartmentApi/";
            string strUrl = "http://epam-azure-training-app.azurewebsites.net/api/DepartmentApi/";
            var departments = httpClient.GetAsync(strUrl).Result.Content.ReadAsStringAsync().Result;
            List<Department> departmentGroups = Serializer.Deserialize<List<Department>>(departments);

            return View(departmentGroups);
        }

        // GET: Departments/Employees/{id}
        public ActionResult Employees(int id)
        {
            //DepartmentService departmentService = new DepartmentService();
            //var departmentEmployees = departmentService.GetDepartmentEmployees(id);
            //var departmentInfo = departmentService.GetDepartmentInfo(id);

            //ViewBag.Title = "Employees in " + departmentInfo.Name + " Department";

            //return View(departmentEmployees);
            string strUrl = "http://azure-training-webapps.azurewebsites.net/api/DepartmentApi/" + id.ToString();

            JavaScriptSerializer EmployeeSerializer = new JavaScriptSerializer();
            var EmployeehttpClient = new HttpClient();
            var employees = EmployeehttpClient.GetAsync(strUrl).Result.Content.ReadAsStringAsync().Result;
            List<DepartmentEmployee> departmentEmployees = EmployeeSerializer.Deserialize<List<DepartmentEmployee>>(employees);

            JavaScriptSerializer InfoSerializer = new JavaScriptSerializer();
            HttpContent InfohttpContent = new StringContent("");
            InfohttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var httpClient = new HttpClient();
            var Info = httpClient.PostAsync(strUrl, InfohttpContent).Result.Content.ReadAsStringAsync().Result;
            //DepartmentInfo departmentInfo = JsonConvert.DeserializeObject(Info).;
            DepartmentInfo departmentInfo = EmployeeSerializer.Deserialize<DepartmentInfo>(Info);



            ViewBag.Title = "Employees in " + departmentInfo.Name + " Department";

            return View(departmentEmployees);
        }
    }
}
