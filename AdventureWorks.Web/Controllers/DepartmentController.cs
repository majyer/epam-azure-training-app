using System.Web.Mvc;
using AdventureWorks.Services.HumanResources;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using StackExchange.Redis;
using System.Configuration;
using System;
using System.Diagnostics;
using Newtonsoft.Json;
namespace AdventureWorks.Web.Controllers
{
    public class DepartmentsController : Controller
    {
        //private const string departmentEmployeecache = "DepartmentEmployee";
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

            var stopwatch = new Stopwatch();
            IDatabase cache = lazyConnection.Value.GetDatabase();

            List<DepartmentEmployee> departmentEmployees =null;
            DepartmentService departmentService = new DepartmentService();
            var departmentInfo = departmentService.GetDepartmentInfo(id);
            stopwatch.Start();
            var cacheget = cache.StringGet(id.ToString());
            //var hashes = cache.HashGetAll(id.ToString());
          
            if (!cacheget.IsNull)
            {
                departmentEmployees = JsonConvert.DeserializeObject<List<DepartmentEmployee>>(cacheget);
                //departmentEmployees = new List<DepartmentEmployee>();
                //foreach(DepartmentEmployee hashentry in departmentEmployees)
                //{
                //    departmentEmployees.Add(JsonConvert.DeserializeObject<DepartmentEmployee>(hashentry.Value));
                //}
                ViewBag.Title = "Employees in cache " + departmentInfo.Name + " Department";
            }
            else
            {
                string strUrl = "http://epam-azure-training-app.azurewebsites.net/api/DepartmentApi/" + id.ToString();
               JavaScriptSerializer EmployeeSerializer = new JavaScriptSerializer();
                var EmployeehttpClient = new HttpClient();
                var employees = EmployeehttpClient.GetAsync(strUrl).Result.Content.ReadAsStringAsync().Result;
                departmentEmployees = EmployeeSerializer.Deserialize<List<DepartmentEmployee>>(employees);
                cache.StringSet(id.ToString(), JsonConvert.SerializeObject(departmentEmployees));
                //foreach(DepartmentEmployee de in departmentEmployees)
                //{
                //    cache.StringSet(de.Id.ToString(), JsonConvert.SerializeObject(de));
                //}
                ViewBag.Title = "Employees in url" + departmentInfo.Name + " Department";
            }
            stopwatch.Stop();
            ViewBag.Message = "MS " + stopwatch.ElapsedMilliseconds;
            //lazyConnection.Value.Dispose();
            return View(departmentEmployees);
        }
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
