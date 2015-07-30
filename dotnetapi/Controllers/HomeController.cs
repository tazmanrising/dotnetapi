using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dotnetapi.Data.Models;
using dotnetapi.Data;
using PagedList;

namespace dotnetapi.Controllers
{
    public class HomeController : Controller
    {

        private DOCContext db = new DOCContext();

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var tips = from s in db.tblTips 
                       .Where(d => d.@group == 2)   // supervisor = 2  , coordinator = 5
                       select s;
                           
            if (!String.IsNullOrEmpty(searchString))
            {
                tips = tips.Where(s => s.title.Contains(searchString)
                                       || s.body.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    tips = tips.OrderByDescending(s => s.title);
                    break;
                case "Date":
                    tips = tips.OrderBy(s => s.created);
                    break;
                case "date_desc":
                    tips = tips.OrderByDescending(s => s.created);
                    break;
                default:  // Name ascending 
                    tips = tips.OrderBy(s => s.updated);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(tips.ToPagedList(pageNumber, pageSize));
            //return View(tips.ToPagedList(1, pageSize));


        }


        public ActionResult News()
        {




            return View(); 

        }


    }
}
