using System.Collections.Generic;
using BusinessModels.Abstracts;
using DataServices.Interfaces;
using BusinessModels.DTOS;
using System;
using BusinessServices.Interfaces;

namespace BusinessServices
{
    public class PatientService : IPatientService
    {
        private IRepository _repo;

        public PatientService(IRepository repo)
        {
            _repo = repo;
        }

        public CommonAppointment GetAppointment(int appointmentId)
        {
            return _repo.GetAppointment(appointmentId);
        }

        public List<CommonAppointment> GetAppointments(DateTime startDate, DateTime endDate)
        {
            return _repo.GetAppointments(startDate, endDate);
        }

        public CommonPatient GetPatient(int patientId)
        {
            return _repo.GetPatient(patientId);
        }

        public List<CommonPatient> GetPatients()
        {
            return _repo.GetPatients();
        }

        public List<CommonPatient> SearchPatients(int patientId, string firstName, string lastName)
        {
            return _repo.SearchPatients(patientId, firstName, lastName);
        }


        public int InsertPatient(CommonPatient patient)
        {
            return _repo.InsertPatient(patient);
        }

        public bool UpdatePatient(CommonPatient patient)
        {
            return _repo.UpdatePatient(patient);
        }
    }
}
