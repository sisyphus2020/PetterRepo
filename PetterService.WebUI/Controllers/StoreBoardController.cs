using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetterService.WebUI.Controllers
{
    public class StoreBoardController : Controller
    {
        // GET: StoreBoard
        public ActionResult Index()
        {
            return View();
        }

        // GET: StoreBoard/Notice
        public ActionResult Notice()
        {
            return View();
        }

        // GET: StoreBoard/Event
        public ActionResult Event()
        {
            return View();
        }

        // GET: StoreBoard/Gallery
        public ActionResult Gallery()
        {
            return View();
        }

        // GET: StoreBoard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StoreBoard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoreBoard/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        // GET: StoreBoard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StoreBoard/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: StoreBoard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StoreBoard/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
