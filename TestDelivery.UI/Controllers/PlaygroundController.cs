using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDelivery.UI.ViewModels;

namespace TestDelivery.UI.Controllers
{
    public class PlaygroundController : Controller
    {
        //
        // GET: /Playground/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditableLists()
        {
            return View(new Playground());
        }

        [HttpPost]
        public ActionResult EditableLists(string whatever)
        {
            var p = new Playground();
            this.TryUpdateModel(p);
            return View(p);
        }

    }
}
