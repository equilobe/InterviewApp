using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestDelivery.UI.Controllers
{
#if !DEBUG
    [Authorize]
#endif
  
    public abstract class AdminController : ApplicationController
    {


    }
}
