using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CmuveeReviews.NetCore.Models;

namespace CmuveeReviews.NetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string serach, string category = "喜剧片", int pageIndex = 1, int pageSize = 20)
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

            List<Movies> categorylist = Movies.GetList<Movies>("select Category,cast(count(*) as text) as DoubanScore from movies where category<>'VIP视频秀' and category<>'伦理片' group by Category order by count(*) desc", null).ToList();
            ViewBag.Categorylist = categorylist;
            return View(page);
        }

    }
}
