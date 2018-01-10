using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using datatable.Models;
using System.Linq.Dynamic;

namespace datatable.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult getEmployeeData()
        //{
        //    using (Database1Entities db = new Database1Entities())
        //    {
        //        var data =
        //        db.Employees.OrderBy(a => a.EmployeeName).ToList();
        //        return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //    }
        //}

        public JsonResult getEmployeeList(string sortColumnName = "EmployeeName", string sortOrder = "asc", int pageSize = 3, int currentPage = 1, string searchText = "")
        {
            List<Employee> List = new List<Employee>();
            int totalPage = 0;
            int totalRecord = 0;

            using (Database1Entities dc = new Database1Entities())
            {
                var emp = dc.Employees.Select(a => a);   
                //Search
                if (!string.IsNullOrEmpty(searchText)) 
                {
                    emp = emp.Where(a => a.EmployeeName.Contains(searchText) || a.PHONE_NUMBER.ToString().Contains(searchText) || a.EMAIL.Contains(searchText) || a.CITY.Contains(searchText) || a.COUNTRY.Contains(searchText));
                }
                totalRecord = emp.Count();
                if (pageSize > 0)
                {
                    totalPage = totalRecord / pageSize + ((totalRecord % pageSize) > 0 ? 1 : 0);
                    List = emp.OrderBy(sortColumnName + " " + sortOrder).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                }
                else
                {
                    List = emp.ToList();
                }
            }

            return new JsonResult
            {
                //Data = new { List = List, totalPage = totalPage, sortColumnName = sortColumnName, sortOrder = sortOrder, currentPage = currentPage},
                Data = new { List = List, totalPage = totalPage, sortColumnName = sortColumnName, sortOrder = sortOrder, currentPage = currentPage, pageSize = pageSize, searchText = searchText },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}