﻿using BusinessModels;
using BusinessModels.Abstracts;
using BusinessModels.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Interfaces
{
    public interface IRepository
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
