using DataServices.Interfaces;
using System;
using System.Collections.Generic;
using BusinessModels.DTOS;
using BusinessModels;
using Shared.Core;
using System.Data.Odbc;
using Dapper;
using System.Linq;

namespace DataServices
{
    public class OptomateRepository : IRepository
    {
        readonly string _connString;
        readonly int _retries = 0;
        readonly int _commandTimeout = 0;

        public const string SetPassWordQuery = "SET PASSWORDS ADD '6!6@6#5$3%9' ;";


        public const int InsertIntoAppNexusMaxAttempts = 3;

        public const string SelectAppointmentNextId = @"select COALESCE(LastAppNum,0)+1 FROM GenerateNumbers";

        public const string SelectAppointmentNexusCount = @"SELECT count(ID) from AppNexus WHERE ID = ?nextId?";

        public const string SetNextAppointmentNextId = @"update GenerateNumbers SET LastAppNum = ?nextId?";




        public const string InsertPatientQuery = @"INSERT INTO CLIENT (NUMBER,GIVEN,SURNAME,ADDRESS1,SUBURB,STATE,POSTCODE,PHONE_AH,PHONE_MOB,EMAIL,BIRTHDATE) 
                                                   VALUES(?Number?, ?Given?, ?Surname?, ?Address1?, ?Suburb?, ?State?, ?Postcode?, ?Phone_Ah?, ?Phone_Mob?, ?Email?, ?BirthDate? );";



        public const string GetBookingsQuery = @"SELECT ID,""start"",""finish"",caption,location,message,clientnum,branch,resourceId,smsconfirm,sentsms,apptype,syncId 
                                                 from ""APPNEXUS"" where cast(""start"" as date) >=CAST(?start? AS DATE) and cast(""finish"" as date) <=CAST(?finish? AS DATE)";
        public const string GetBookingQuery = @"SELECT ID,""start"",""finish"",caption,location,message,clientnum,branch,resourceId,smsconfirm,sentsms,apptype,syncId 
                                                 from ""APPNEXUS"" where ?bookingId?";


        public const string GetPatientQuery = @"SELECT Number,Title,Given,Surname,Address1,Suburb,State,Postcode,Phone_Ah,Phone_Mob,Email,BirthDate,Comment,Optom from CLIENT 
                                                WHERE Given =?patientId?";

        public const string GetPatientsQuery = @"select Number,Title,Given,Surname,Address1,Suburb,State,Postcode,Phone_Ah,Phone_Mob,Email,BirthDate,Comment,Optom from CLIENT 
           ";

        public const string SelectClientNextId = @"SELECT COALESCE(LASTPATNUM,0)+1 FROM GenerateNumbers ";

        public const string SetClientNextIdQuery = @"UPDATE GenerateNumbers SET LASTPATNUM = ?nextClientId?";

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

        public OptomateRepository(string ConnectionString)
        {
            _connString = ConnectionString;
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
            var sqlParams = new
            {
                appointmentId = appointmentId
            };

            return ToCommonAppointment(QueryConn<OptomateAppointment>(GetBookingQuery, sqlParams).FirstOrDefault());
        }

        public List<CommonAppointment> GetAppointments(DateTime startDate, DateTime endDate)
        {

            var sqlParams = new
            {
                start = DateUtils.ToYYYY_MM_DD(startDate),
                finish = DateUtils.ToYYYY_MM_DD(endDate)
            };

            return ToCommonAppointments(QueryConn<OptomateAppointment>(GetBookingsQuery, sqlParams));

        }

        public CommonPatient GetPatient(int patientId)
        {
            var sqlParams = new DynamicParameters(new { patientId = patientId });
            return ToCommonPatient(QueryConn<OptomatePatient>($"{SetPassWordQuery} {GetPatientQuery}",
                sqlParams).FirstOrDefault());
        }

        public List<CommonPatient> GetPatients()
        {
            return ToCommonPatients(QueryConn<OptomatePatient>($"{SetPassWordQuery} {GetPatientsQuery}"));
        }

        public int InsertPatient(CommonPatient patient, string locationCode)
        {

            bool success = false;
            int nextId = 0;
            int rowsrfected = 0;
            for (int i = 1; i <= InsertIntoAppNexusMaxAttempts; i++)
            {
                if (!success)
                {
                    nextId = GetClientNextId();
                    if (nextId <= 0)
                    {
                        continue;
                    }
                    int numOfRecordsWithId = GetClientCount(nextId);
                    while (numOfRecordsWithId > 0)
                    {
                        nextId = nextId + 1;
                        numOfRecordsWithId = GetClientCount(nextId);
                    }
                    var nextIdSet = SetClientNextId(nextId);
                    if (!nextIdSet)
                    {
                        continue;
                    }

                    OptomatePatient newPatient = new OptomatePatient()
                    {
                        Number = nextId,
                        Given = patient.FirstName,
                        Surname = patient.LastName,
                        Address1 = patient.ResidentialAddress,
                        Suburb = patient.ResidentialSuburb,
                        State = patient.ResidentialState,
                        Postcode = patient.ResidentialPostCode,
                        Phone_Ah = patient.Phone,
                        Phone_Mob = patient.Mobile,
                        Email = patient.Email,
                        BirthDate = patient.DateOfBirth,

                    };

                    rowsrfected = ExecConn($"{SetPassWordQuery} {InsertPatientQuery}", patient);

                    if (rowsrfected > 0)
                        success = true;
                }
            }

            //ToDo : Implement patient id value return
            return 0;

        }

        public bool UpdatePatient(CommonPatient patient)
        {
            throw new NotImplementedException();
        }

        private int GetAppointmentNextId()
        {
            try
            {
                int result = 0;
                string maxCode = QueryConn<string>(SelectAppointmentNextId)?.FirstOrDefault();
                Int32.TryParse(maxCode, out result);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int GetAppointmentNexusCount(int Id)
        {
            try
            {
                var sqlParams = new { nextId = Id };

                int result = 0;
                string maxCode = QueryConn<string>(SelectAppointmentNexusCount, sqlParams)?.FirstOrDefault();
                Int32.TryParse(maxCode, out result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int GetClientNextId()
        {
            int result = 0;
            string maxCode = QueryConn<string>(SelectClientNextId)?.FirstOrDefault();
            Int32.TryParse(maxCode, out result);
            return result;

        }

        private int GetClientCount(int nextClientId)
        {
            var sqlParams = new { nextClientId = nextClientId };

            int result = 0;
            string maxCode = QueryConn<string>($"{SetPassWordQuery} {SelectClientCount}", sqlParams)?.FirstOrDefault();
            Int32.TryParse(maxCode, out result);
            return result;
        }

        private bool SetClientNextId(int nextClientId)
        {
            var sqlParams = new { nextClientId = nextClientId };

            return ExecConn(SetClientNextIdQuery, sqlParams) > 0;

        }

        private bool SetAppointmentNextId(int Id)
        {
            var sqlParams = new { nextId = Id };

            return ExecConn(SetNextAppointmentNextId, sqlParams) > 0;

        }




        private List<CommonPatient> ToCommonPatients(IEnumerable<OptomatePatient> optomateTouchPatients)
        {
            List<CommonPatient> commonPatients = new List<CommonPatient>();

            foreach (OptomatePatient op in optomateTouchPatients)
            {
                commonPatients.Add(ToCommonPatient(op));
            }

            return commonPatients;
        }

        private List<CommonAppointment> ToCommonAppointments(IEnumerable<OptomateAppointment> optomateTouchAppointments)
        {
            List<CommonAppointment> commonAppointments = new List<CommonAppointment>();

            foreach (OptomateAppointment ota in optomateTouchAppointments)
            {
                commonAppointments.Add(ToCommonAppointment(ota));
            }

            return commonAppointments;
        }

        private CommonPatient ToCommonPatient(OptomatePatient optomatePatient)
        {
            return new CommonPatient()
            {
                Title = optomatePatient.Title,
                FirstName = optomatePatient.Given,
                LastName = optomatePatient.Surname,
                DateOfBirth = optomatePatient.BirthDate,
                //Gender = optomatePatient.,
                Mobile = optomatePatient.Mobile,
                Email = optomatePatient.Email,
                //Phone = optomatePatient.Phone,
                ResidentialAddress = optomatePatient.Address1,
                ResidentialSuburb = optomatePatient.Suburb,
                ResidentialPostCode = optomatePatient.State,
                ResidentialState = optomatePatient.Postcode,

                //PostAddressSameAsResidentialAddress = optomatePatient.Title,
                //PostalAddress = optomatePatient.Title,
                //PostalSuburb = optomatePatient.Title,
                //PostalPostCode = optomatePatient.Title,
                //PostalState = optomatePatient.Title,

                //HealthFundName = optomatePatient.HealthFundName,
                //HealthFundMemberNumber = optomatePatient.HealthFundMemberNumber,
                //HeatlhFundRefreenceNumber = optomatePatient.HeatlhFundRefreenceNumber,
                //MedicareMemberNumber = optomatePatient.MedicareMemberNumber,

                //HasHealthFund = optomatePatient.HasHealthFund,
                //MeidcareReferenceNumber = optomatePatient.MeidcareReferenceNumber,
                //MedicareExpiryDate = optomatePatient.MedicareExpiryDate,
                //DVAPensionNumber = optomatePatient.DVAPensionNumber
            };

        }

        private CommonAppointment ToCommonAppointment(OptomateAppointment optomateAppointment)
        {
            return new CommonAppointment()
            {
                StartDate = optomateAppointment.Start,
                ID = optomateAppointment.Id,
                EndDate = optomateAppointment.Finish,
                BranchIdentifier = optomateAppointment.Branch,
                //UserIdentifier = optomateAppointment.UserIdentifier,
                AppointmentType = optomateAppointment.Appttype,
                PatientId = optomateAppointment.ClientNum,
                //Duration = optomateAppointment.Duration,
            };
        }

    }
}
