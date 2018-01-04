using System.Collections.Generic;
using BusinessModels.Abstracts;
using DataServices.Interfaces;
using BusinessModels.DTOS;

namespace BusinessServices.Interfaces
{
    public class PatientService : IService
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

        public List<CommonAppointment> GetAppointments()
        {
            return _repo.GetAppointments();
        }

        public CommonPatient GetPatient(int patientId)
        {
            return _repo.GetPatient(patientId);
        }

        public List<CommonPatient> GetPatients()
        {
            return _repo.GetPatients();
        }

        public int InsertPatient(CommonPatient patient, string locationCode)
        {
            return _repo.InsertPatient(patient, locationCode);
        }

        public bool UpdatePatient(CommonPatient patient)
        {
            return _repo.UpdatePatient(patient);
        }
    }
}
