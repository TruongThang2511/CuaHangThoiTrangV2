﻿using Microsoft.AspNetCore.Mvc;

namespace CuaHangThoiTrangV2.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
