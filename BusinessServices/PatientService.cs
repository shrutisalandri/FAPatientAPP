using System.Collections.Generic;
using DataServices.Interfaces;
using BusinessModels.DTOS;
using System;
using BusinessServices.Interfaces;
using Serilog;

namespace BusinessServices
{
    public class PatientService : IPatientService
    {
        private IRepository _repo;
        private ILogger Log;

        public PatientService(IRepository repo, ILogger logger)
        {
            _repo = repo;
            Log = logger;
        }

        public CommonAppointment GetAppointment(int appointmentId)
        {
            try
            {
                return _repo.GetAppointment(appointmentId);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return null;
        }

        public List<CommonAppointment> GetAppointments(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _repo.GetAppointments(startDate, endDate);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return null;
        }

        public CommonPatient GetPatient(int patientId)
        {
            try
            {
                return _repo.GetPatient(patientId);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return null;
        }

        public List<CommonPatient> GetPatients()
        {
            try
            {
                return _repo.GetPatients();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return null;
        }

        public List<CommonPatient> SearchPatients(int patientId, string firstName, string lastName)
        {
            try
            {
                return _repo.SearchPatients(patientId, firstName, lastName);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return null;
        }

        public int InsertPatient(CommonPatient patient)
        {
            try
            {
                return _repo.InsertPatient(patient);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return 0;
        }

        public bool UpdatePatient(CommonPatient patient)
        {
            try
            {
                return _repo.UpdatePatient(patient);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return false;
        }
    }
}
