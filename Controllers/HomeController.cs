using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTSDCES.Models;

namespace NTSDCES.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Info()
        {
            return View();
        }
        public ActionResult Character(int id)
        {
            NTSDCESEntities CharacterData = new NTSDCESEntities();
            Character CharacterDetails = CharacterData.Characters.FirstOrDefault(x => x.CharacterID == id);
            return View(CharacterDetails);
        }
    }
}