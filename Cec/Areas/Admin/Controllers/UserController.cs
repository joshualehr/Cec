﻿using Cec.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cec.Areas.Admin.Controllers
{
    [Authorize(Roles="canAdminister")]
    [SelectedTab("users")]
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }
    }
}