﻿using BusinessModels.Abstracts;
using BusinessModels.DTOS;
using System;
using System.Collections.Generic;

namespace BusinessServices.Interfaces
{
    public interface IPatientService
    {
        List<CommonPatient> GetPatients();

        List<CommonPatient> SearchPatients(int patientId, string firstName, string lastName);

        CommonPatient GetPatient(int PatientId);

        int InsertPatient(CommonPatient patient);

        bool UpdatePatient(CommonPatient patient);

        List<CommonAppointment> GetAppointments(DateTime startDate, DateTime endDate);

        CommonAppointment GetAppointment(int appointmentId);
    }
}
