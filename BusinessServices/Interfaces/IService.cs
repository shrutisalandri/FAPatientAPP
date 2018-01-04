using BusinessModels.Abstracts;
using BusinessModels.DTOS;
using System.Collections.Generic;

namespace BusinessServices.Interfaces
{
    public interface IService
    {
        List<CommonPatient> GetPatients();

        CommonPatient GetPatient(int PatientId);

        int InsertPatient(CommonPatient patient, string locationCode);

        bool UpdatePatient(CommonPatient patient);

        List<CommonAppointment> GetAppointments();

        CommonAppointment GetAppointment(int appointmentId);
    }
}
