using CmuveeReviews.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmuveeReviews.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string serach,string category = "喜剧片", int pageIndex = 1, int pageSize = 20)
        {
            string where = $"Category='{category}'";
            if (!string.IsNullOrEmpty(serach)) where = $"Name like '%{serach}%' or AliasName like '%{serach}%' or Remark like '%{serach}%' or Description like '%{serach}%'";

            PagerModel<Movies> page = new PagerModel<Movies>(pageSize, pageIndex, 0, null);
            int totalCount = 0;
            X.ORMData.PageHelper pager = new X.ORMData.PageHelper("movies", "*", where, "", "", "SourceId", "DESC", "", pageIndex, pageSize, "Pager_2015");
            List<Movies> moviesList = X.ORMData.ORMBase.GetListPage<Movies>(pager, out totalCount, "DatabaseName");
            page = new PagerModel<Movies>(pageSize, pageIndex, totalCount, null);
            
            ViewBag.MoviesList = moviesList;
            ViewBag.Serach = serach;

            List<Movies> categorylist = Movies.GetList<Movies>("select Category,cast(count(*) as text) as DoubanScore from movies group by Category order by count(*) desc", null).ToList();
            ViewBag.Categorylist = categorylist;
            return View(page);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}