using DataServices.Interfaces;
using System;
using System.Collections.Generic;
using BusinessModels.Abstracts;
using BusinessModels.DTOS;
using BusinessModels;
using Shared.Core;
using System.Data.Odbc;

namespace DataServices
{
    public class OptomateRepository : IRepository
    {
        readonly string _connString;
        readonly int _retries = 0;
        readonly int _commandTimeout = 0;
        public const string OptomatePremier = "optomatepremier";
        public const string SetPassWordQuery = "SET PASSWORDS ADD '6!6@6#5$3%9' ; ";
        public const string DBTimeFormat = "hh:mm tt";
        public const string OptomatePremierTabName = "Optomate Premier";

        public const int InsertIntoAppNexusMaxAttempts = 3;

        public const string SelectAppointmentNextId = @"select COALESCE(LastAppNum,0)+1 FROM GenerateNumbers";

        public const string SelectAppointmentNexusCount = @"SELECT count(ID) from AppNexus WHERE ID = ?nextId?";

        public const string SetNextAppointmentNextId = @"update GenerateNumbers SET LastAppNum = ?nextId?";



        public const string InsertPatientWithBranches = @"INSERT INTO CLIENT
                                                           (NUMBER,GIVEN,SURNAME,ADDRESS1,SUBURB,STATE,POSTCODE,PHONE_AH,PHONE_MOB,EMAIL,BIRTHDATE,BRANCH) 
                                                            VALUES(?Number?, ?Given?, ?Surname?, ?Address1?, ?Suburb?, ?State?, ?Postcode?, ?Phone_Ah?, ?Phone_Mob?, ?Email?, ?BirthDate?, ?Branch? );";

        public const string InsertPatientSingleBranch = @"INSERT INTO CLIENT
                                                           (NUMBER,GIVEN,SURNAME,ADDRESS1,SUBURB,STATE,POSTCODE,PHONE_AH,PHONE_MOB,EMAIL,BIRTHDATE) 
                                                              VALUES(?Number?, ?Given?, ?Surname?, ?Address1?, ?Suburb?, ?State?, ?Postcode?, ?Phone_Ah?, ?Phone_Mob?, ?Email?, ?BirthDate? );";



        public const string SelectBookingsBetweenDates = @"select ID,""start"",""finish"",caption,location,message,clientnum,branch,resourceId,smsconfirm,sentsms,apptype,syncId 
                                                           from ""APPNEXUS"" where cast(""start"" as date) >=CAST(?start? AS DATE) and cast(""finish"" as date) <=CAST(?finish? AS DATE) and resourceid= ?resourceId?";

        public const string SelectBookingsBetweenDatesForMulti = @"select ID,""start"",""finish"",caption,location,message,clientnum,branch,resourceId,smsconfirm,sentsms,apptype,syncId 
                                                           from ""APPNEXUS"" where cast(""start"" as date) >=CAST(?start? AS DATE) and cast(""finish"" as date) <=CAST(?finish? AS DATE) and resourceid= ?resourceId? and branch =?branchCode?";


        public const string SelectPatientByNameAndDob = @"select Number,Title,Given,Surname,Address1,Suburb,State,Postcode,Phone_Ah,Phone_Mob,Email,BirthDate,Comment,Optom from CLIENT where Given =?firstName? and Surname=?lastname? and BirthDate= CAST(?dob? AS DATE) ";

        public const string SelectClientNextId = @"SELECT COALESCE(LASTPATNUM,0)+1 FROM GenerateNumbers ";

        public const string SetClientNextId = @"UPDATE GenerateNumbers SET LASTPATNUM = ?nextClientId?";

        public const string SelectClientCount = @"SELECT count(number) from Client WHERE number = ?nextClientId?;";


        int ExecConn(string sql, object sqlParams = null)
        {
            // Nexus expects timeout in milliseconds, whereas .net command expects seconds
            var sqlWithTimeout = _commandTimeout != 0 ? $"#t {_commandTimeout * 1000}; {sql}" : sql;
            using (var conn = new OdbcConnection(_connString))
            {
                conn.OpenRobust();

                return conn.ExecuteRobust(sqlWithTimeout, sqlParams);
            }
        }

        IEnumerable<T> QueryConn<T>(string sql, object sqlParams = null) where T : class
        {
            // Nexus expects timeout in milliseconds, whereas .net command expects seconds
            var sqlWithTimeout = _commandTimeout != 0 ? $"#t {_commandTimeout * 1000}; {sql}" : sql;
            using (var conn = new OdbcConnection(_connString))
            {

                if (_retries > 0) { conn.OpenRobust(retries: _retries); } else { conn.OpenRobust(); };

                return _retries > 0 ? conn.QueryRobust<T>(sqlWithTimeout, sqlParams, retries: _retries) : conn.QueryRobust<T>(sqlWithTimeout, sqlParams);
            }
        }

        public CommonAppointment GetAppointment(int appointmentId)
        {
            throw new NotImplementedException();
        }

        public List<CommonAppointment> GetAppointments()
        {
            var sql = SelectBookingsBetweenDates;
            //var sqlParams = new
            //{
            //    start = DateUtils.ToYYYY_MM_DD(startTime),
            //    finish = DateUtils.ToYYYY_MM_DD(endTime),
            //    resourceId = resourceCode,
            //    waitseen = _config.CancelStatusID

            //});

            //if (_config.AllowBookingOverCancelledBooking) ////Get appointments excluding cancelled states.
            //{
            //    var sqlToGetCancelledStateAppoitnments = "  AND coalesce(waitseen,0) <> ?waitseen?";
            //    sql = $"{sql}{sqlToGetCancelledStateAppoitnments}";

            //    sqlParams.AddDynamicParams(new { waitseen = _config.CancelStatusID });
            //    return QueryConn<OptomateBooking>(sql, sqlParams);
            //}
            //else ////Get all appointments.
            //{
            //    return QueryConn<OptomateBooking>(sql, sqlParams);
            //}

            return null;
        }

        public CommonPatient GetPatient(int patientId)
        {
            //var sqlParams = new DynamicParameters(
            //  new
            //  {
            //      patientId = patientId,

            //  });
            //return QueryConn<OptomatePatient>($"{SetPassWordQuery} {SelectPatientByNameAndDob}", sqlParams);

            return null;

        }

        public List<CommonPatient> GetPatients()
        {
            throw new NotImplementedException();
        }

        public int InsertPatient(CommonPatient patient, string locationCode)
        {
            OptomatePatient OptomatePatientObj = new OptomatePatient();
            //bool UseBranches = false;
            //bool success = false;
            //int nextId = 0;
            //int rowsrfected = 0;
            //for (int i = 1; i <= Constants.InsertIntoAppNexusMaxAttempts; i++)
            //{
            //    if (!success)
            //    {
            //        nextId = GetClientNextId();
            //        if (nextId <= 0)
            //        {
            //            continue;
            //        }
            //        int numOfRecordsWithId = GetClientCount(nextId);
            //        while (numOfRecordsWithId > 0)
            //        {
            //            nextId = nextId + 1;
            //            numOfRecordsWithId = GetClientCount(nextId);
            //        }
            //        var nextIdSet = SetClientNextId(nextId);
            //        if (!nextIdSet)
            //        {
            //            continue;
            //        }

            //        string enterPrise = SelectEnterPrise();
            //        if (!string.IsNullOrEmpty(enterPrise) && enterPrise.ToUpper() == "TRUE")
            //        {
            //            UseBranches = true;
            //        }
            //        if (UseBranches)
            //        {
            //            OptomatePatientObj = new Models.OptomatePatient
            //            {
            //                Number = nextId,
            //                Given = patient.Firstname,
            //                Surname = patient.Surname,
            //                Address1 = patient.Address1,
            //                Suburb = patient.City,
            //                State = patient.State,
            //                Postcode = patient.Postcode,
            //                Phone_Ah = patient.HomePhone,
            //                Phone_Mob = patient.Phone,
            //                Email = patient.Email,
            //                BirthDate = patient.DOB,
            //                Branch = "" //TODO: Need to confirm on this.
            //            };

            //            rowsrfected = ExecConn($"{Constants.SetPassWordQuery} {Constants.InsertPatientWithBranches}", OptomatePatientObj);
            //        }
            //        else
            //        {
            //            OptomatePatientObj = new Models.OptomatePatient
            //            {
            //                Number = nextId,
            //                Given = patient.Firstname,
            //                Surname = patient.Surname,
            //                Address1 = patient.Address1,
            //                Suburb = patient.City,
            //                State = patient.State,
            //                Postcode = patient.Postcode,
            //                Phone_Ah = patient.Phone,
            //                Phone_Mob = patient.HomePhone,
            //                Email = patient.Email,
            //                BirthDate = patient.DOB,
            //                Comment = patient.BookingNotes,
            //            };

            //            rowsrfected = ExecConn($"{Constants.SetPassWordQuery} {Constants.InsertPatientSingleBranch}", OptomatePatientObj);
            //        }
            //        if (rowsrfected > 0)
            //            success = true;
            //    }
            //}

            return OptomatePatientObj.Number;

        }

        public bool UpdatePatient(CommonPatient patient)
        {
            throw new NotImplementedException();
        }

    }
}
