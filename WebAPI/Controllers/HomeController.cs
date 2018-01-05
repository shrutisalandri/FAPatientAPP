using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessServices;
using DataServices;
using BusinessServices.Interfaces;
using DataServices.Interfaces;
using Unity;
using Newtonsoft;
using Newtonsoft.Json;
using Unity.Resolution;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : BaseController
    {
        IPatientService _patientService;
        IConfiguration _configuration;
        string _connectionString = string.Empty;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/Home
        [HttpGet]
        public string GetPatients(string PMS)
        {

            if (!string.IsNullOrEmpty(PMS))
            {
                _connectionString = _configuration.GetValue<string>("ConnectionString :" + PMS);
                _patientService = new PatientService(myContainer.Resolve<IRepository>(PMS, new ResolverOverride[]
                                  {
                                       new ParameterOverride("ConnectionString",_connectionString )
                                  }));

                var patients = _patientService.GetPatients();

                return JsonConvert.SerializeObject(patients);
            }

            return string.Empty;

        }

        [HttpGet]
        public string GetPatient(string PMS, int Id)
        {

            if (!string.IsNullOrEmpty(PMS))
            {
                _connectionString = _configuration.GetValue<string>("ConnectionString :" + PMS);
                _patientService = new PatientService(myContainer.Resolve<IRepository>(PMS, new ResolverOverride[]
                                  {
                                       new ParameterOverride("ConnectionString", _connectionString)
                                  }));

                var patient = _patientService.GetPatient(Id);

                return JsonConvert.SerializeObject(patient);
            }

            return string.Empty;

        }


    }
}
