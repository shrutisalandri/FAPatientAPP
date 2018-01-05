using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unity;
using DataServices.Interfaces;
using DataServices;

namespace WebAPI.Controllers
{

    public class BaseController : Controller
    {
        protected IUnityContainer myContainer = new UnityContainer();

        public BaseController()
        {
            RegisterTypes();
        }

        public void RegisterTypes()
        {
            myContainer.RegisterType(typeof(IRepository), typeof(OptomateRepository), "Optomate");
            myContainer.RegisterType(typeof(IRepository), typeof(OptomateTouchRepository), "OptomateTouch");
        }
    }
}