﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Basically.Models;
using Basically.Infrastructure;
using LiteDB;

namespace Basically.Controllers
{
    public class SiteController : Controller
    {
        private IConnector db;
        public SiteController(IConnector Connector)
        {
            db = Connector;
        }

        public IActionResult Index()
        {
            //Get list of Sites and return in a viewbag
            //ViewBag.SiteList = db.List<Site>().FindAll();
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                //PW: return ordered list, sliced, chucked and ordered
                IEnumerable<Site> SiteList = db.List<Site>().FindAll();
                int TotalRecords = SiteList.Count();
                var ChunkedSiteList = SiteList.OrderByDynamic(jtSorting).Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = ChunkedSiteList, TotalRecordCount = TotalRecords });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(Site Model) {

            try
            {
                db.Create(Model);
                return Json(new { Result = "OK",  Record = Model});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(int _id)
        {
            try
            {
                //PW: Delete model
                db.Delete<Site>(_id);
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(Site Model)
        {
            try
            {
                //PW: Update model
                db.Update<Site>(Model);
                return Json(new { Result = "OK"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}