using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessServices;
using DataServices;
using BusinessServices.Interfaces;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : Controller
    {
        IPatientService _patientService;
        public HomeController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/Home
        [HttpGet]
        public IEnumerable<string> GetPatients(string PMS)
        {
            //ToDo: Need to figure out best way for resolving this
            //if (PMS == "Optomate")
            //{
            //    _patientService = new PatientService(new OptomateRepository(""));
            //}
            //else if (PMS == "OptomateTouch")
            //{
            //    _patientService = new PatientService(new OptomateTouchRepository(""));
            //}

            _patientService.GetPatients();

            return new string[] { "value1", "value2" };
        }

        // GET: api/Home/5

    }
}
