using BusinessModels.Abstracts;
using BusinessModels.DTOS;
using System;
using System.Collections.Generic;

namespace BusinessServices.Interfaces
{
    public interface IPatientService
    {
        List<CommonPatient> GetPatients();

        CommonPatient GetPatient(int PatientId);

        int InsertPatient(CommonPatient patient, string locationCode);

        bool UpdatePatient(CommonPatient patient);

        List<CommonAppointment> GetAppointments(DateTime startDate, DateTime endDate);

        CommonAppointment GetAppointment(int appointmentId);
    }
}
