using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BusinessServices;
using BusinessServices.Interfaces;
using DataServices.Interfaces;
using Unity;
using Unity.Resolution;
using Microsoft.Extensions.Configuration;
using BusinessModels.DTOS;
using Microsoft.AspNetCore.Cors;
using Serilog;

namespace WebAPI.Controllers
{

    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class HomeController : BaseController
    {
        IPatientService _patientService;
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        ILogger logger;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            logger = new LoggerConfiguration().WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                          .CreateLogger();
        }


        [HttpGet]
        [HttpGet]
        [Route("api/GetPatients")]
        public List<CommonPatient> GetPatients(string PMS)
        {
            try
            {
                if (!string.IsNullOrEmpty(PMS))
                {

                    _connectionString = _configuration.GetValue<string>("ConnectionStrings:" + PMS);
                    _patientService = new PatientService(myContainer.Resolve<IRepository>(PMS, new ResolverOverride[]
                                      {
                                       new ParameterOverride("ConnectionString",_connectionString )
                                      }), logger);

                    return _patientService.GetPatients();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return null;

        }

        [HttpGet]
        [Route("api/SearchPatients")]
        public List<CommonPatient> SearchPatients(string PMS, int patientId, string firstName, string lastName)
        {
            try
            {
                if (!string.IsNullOrEmpty(PMS))
                {

                    _connectionString = _configuration.GetValue<string>("ConnectionStrings:" + PMS);
                    _patientService = new PatientService(myContainer.Resolve<IRepository>(PMS, new ResolverOverride[]
                                      {
                                       new ParameterOverride("ConnectionString",_connectionString )
                                      }), logger);

                    return _patientService.SearchPatients(patientId, firstName, lastName);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return null;

        }

        [HttpGet]
        [Route("api/GetPatient")]
        public CommonPatient GetPatient(string PMS, int Id)
        {

            try
            {
                if (!string.IsNullOrEmpty(PMS))
                {
                    _connectionString = _configuration.GetValue<string>("ConnectionStrings:" + PMS);
                    _patientService = new PatientService(myContainer.Resolve<IRepository>(PMS, new ResolverOverride[]
                                      {
                                       new ParameterOverride("ConnectionString", _connectionString)
                                      }), logger);

                    return _patientService.GetPatient(Id);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return null;

        }

        [HttpGet]
        [Route("api/GetAppointments")]
        public List<CommonAppointment> GetAppointments(string PMS, DateTime startDate, DateTime endDate)
        {

            startDate = DateTime.Today.AddDays(-20);
            endDate = DateTime.Today;

            try
            {
                if (!string.IsNullOrEmpty(PMS))
                {
                    _connectionString = _configuration.GetValue<string>("ConnectionStrings:" + PMS);
                    _patientService = new PatientService(myContainer.Resolve<IRepository>(PMS, new ResolverOverride[]
                                      {
                                       new ParameterOverride("ConnectionString",_connectionString )
                                      }), logger);

                    return _patientService.GetAppointments(startDate, endDate);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return null;

        }

        [HttpGet]
        [Route("api/GetAppointment")]
        public CommonAppointment GetAppointment(string PMS, int Id)
        {

            try
            {
                if (!string.IsNullOrEmpty(PMS))
                {
                    _connectionString = _configuration.GetValue<string>("ConnectionStrings:" + PMS);
                    _patientService = new PatientService(myContainer.Resolve<IRepository>(PMS, new ResolverOverride[]
                                      {
                                       new ParameterOverride("ConnectionString", _connectionString)
                                      }), logger);

                    return _patientService.GetAppointment(Id);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return null;

        }


        [HttpPost]
        [Route("api/InsertPatient")]
        public int InsertPatient(string PMS, CommonPatient patient)
        {
            try
            {

                if (!string.IsNullOrEmpty(PMS))
                {
                    _connectionString = _configuration.GetValue<string>("ConnectionStrings:" + PMS);
                    _patientService = new PatientService(myContainer.Resolve<IRepository>(PMS, new ResolverOverride[]
                                      {
                                       new ParameterOverride("ConnectionString", _connectionString)
                                      }), logger);
                   
                    return _patientService.InsertPatient(patient);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return 0;

        }

        [HttpPost]
        [Route("api/UpdatePatient")]
        public bool UpdatePatient(string PMS, CommonPatient patient)
        {
            try
            {
                if (!string.IsNullOrEmpty(PMS))
                {
                    _connectionString = _configuration.GetValue<string>("ConnectionStrings:" + PMS);
                    _patientService = new PatientService(myContainer.Resolve<IRepository>(PMS, new ResolverOverride[]
                                      {
                                       new ParameterOverride("ConnectionString", _connectionString)
                                      }), logger);

                                      return _patientService.UpdatePatient(patient);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return false;

        }


    }
}
