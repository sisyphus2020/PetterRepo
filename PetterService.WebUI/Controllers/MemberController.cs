using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetterService.WebUI.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult Index()
        {
            return View();
        }

        // GET: Member/Member
        public ActionResult Member()
        {
            return View();
        }

        // GET: Member/Withdrawal
        public ActionResult Withdrawal()
        {
            return View();
        }
    }
}