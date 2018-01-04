using DataServices.Interfaces;
using Shared.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BusinessModels;
using System.Linq;
using BusinessModels.DTOS;

namespace DataServices
{
    public class OptomateTouchRepository : IRepository
    {
        readonly string _connString;
        readonly int _retries = 0;

        const string GetAppointmentQuery = @"SELECT [ID], [DATE_ADDED] AS DateAdded, [STARTDATE] as StartDate,
             [ENDDATE] AS EndDate, [BRANCH_IDENTIFIER] AS BranchIdentifier, [USER_IDENTIFIER] AS UserIdentifier,
             [PATIENTID] AS PatientId, [DURATION] AS Duration
            FROM [dbo].[APPOINTMENT] WITH(NOLOCK)
            WHERE [ID] = @AppointmentId";

        const string GetAppointmentsQuery = @"SELECT [ID], [DATE_ADDED] AS DateAdded, [STARTDATE] as StartDate,
             [ENDDATE] AS EndDate, [BRANCH_IDENTIFIER] AS BranchIdentifier, [USER_IDENTIFIER] AS UserIdentifier,
             [PATIENTID] AS PatientId, [DURATION] AS Duration
            FROM [dbo].[APPOINTMENT] WITH(NOLOCK)";

        const string InsertPatientQuery = @"INSERT INTO [dbo].[PATIENT]
                  ([DATE_ADDED], [USER_ADDED], [TIMESTMP],[TITLE],[GIVEN],[MIDDLE],[SURNAME],[BIRTHDATE],[GENDER]
                  ,[INACTIVE],[RESIDENT_ADDRESS],[RESIDENT_SUBURB],[RESIDENT_STATE],[RESIDENT_POSTCODE],[MOBILE_PHONE]
                  ,[EMAIL],[USER_IDENTIFIER],[BRANCH_IDENTIFIER])
                  VALUES (GetDate(),'FSTAVAILABLE', GetDate(), @Title, @FirstName, @Middle, @LastName, @BirthDate, @Gender, @InActive, @ResidentAddress,
                  @ResidentSuburb, @ResidentState, @ResidentPostCode, @Mobile, @Email, @UserIdentifier, @BranchIdentifier);
                  SELECT SCOPE_IDENTITY()";

        const string GetPatientQuery = @"SELECT [ID], [TITLE] AS Title, [GIVEN] AS FirstName,[MIDDLE] AS Middle, [SURNAME] AS LastName,    
             [BIRTHDATE] AS BirthDate, [GENDER] AS Gender, [INACTIVE] AS InActive, [RESIDENT_ADDRESS] AS ResidentAddress,
             [RESIDENT_SUBURB] AS ResidentSuburb, [RESIDENT_STATE] AS ResidentState, [RESIDENT_POSTCODE] AS ResidentPostCode,
             [MOBILE_PHONE] AS Mobile, [EMAIL] AS Email, [USER_IDENTIFIER] AS UserIdentifier, [BRANCH_IDENTIFIER] AS BranchIdentifier
            FROM [Optomate].[dbo].[PATIENT] WITH(NOLOCK)
            WHERE [ID] = @PatientId";

        const string GetPatientsQuery = @"SELECT [ID], [TITLE] AS Title, [GIVEN] AS FirstName,[MIDDLE] AS Middle, [SURNAME] AS LastName,    
             [BIRTHDATE] AS BirthDate, [GENDER] AS Gender, [INACTIVE] AS InActive, [RESIDENT_ADDRESS] AS ResidentAddress,
             [RESIDENT_SUBURB] AS ResidentSuburb, [RESIDENT_STATE] AS ResidentState, [RESIDENT_POSTCODE] AS ResidentPostCode,
             [MOBILE_PHONE] AS Mobile, [EMAIL] AS Email, [USER_IDENTIFIER] AS UserIdentifier, [BRANCH_IDENTIFIER] AS BranchIdentifier
            FROM [Optomate].[dbo].[PATIENT] WITH(NOLOCK)";

        public OptomateTouchRepository(string ConnectionString)
        {
            _connString = ConnectionString;
        }

        IEnumerable<T> QueryConn<T>(string sql, object sqlParams = null) where T : class
        {
            using (var conn = new SqlConnection(_connString))
            {
                if (_retries > 0) { conn.OpenRobust(_retries); } else { conn.OpenRobust(); }

                return _retries > 0 ? conn.QueryRobust<T>(sql, sqlParams, retries: _retries) : conn.QueryRobust<T>(sql, sqlParams);
            }
        }
        int ExecConn(string sql, object sqlParams = null)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.OpenRobust();
                return conn.ExecuteRobust(sql, sqlParams);
            }
        }

        int ExecScalarConn(string sql, object sqlParams = null)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.OpenRobust();
                return conn.ExecuteScalarRobust(sql, sqlParams);
            }
        }

        public List<CommonPatient> GetPatients()
        {
            return ToCommonPatients(QueryConn<OptomateTouchPatient>(GetPatientsQuery));
        }

        public CommonPatient GetPatient(int PatientId)
        {
            var sqlParams = new { PatientId = PatientId };
            return ToCommonPatient(QueryConn<OptomateTouchPatient>(GetPatientQuery, sqlParams).FirstOrDefault());
        }

        public int InsertPatient(CommonPatient patient, string locationCode)
        {
            var sqlParams = new
            {
                Title = patient.Gender == "M" ? "Mr" : "Miss",
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                BirthDate = patient.DateOfBirth,
                Gender = patient.Gender,
                InActive = false,
                ResidentAddress = patient.ResidentialAddress,
                ResidentSuburb = patient.ResidentialSuburb,
                ResidentState = patient.ResidentialState,
                ResidentPostCode = patient.ResidentialPostCode,
                Mobile = patient.Mobile,
                Email = patient.Email,
                UserIdentifier = string.Empty,
                BranchIdentifier = locationCode
            };

            return ExecScalarConn(InsertPatientQuery, sqlParams);
        }

        public bool UpdatePatient(CommonPatient patient)
        {
            throw new NotImplementedException();
        }

        public List<CommonAppointment> GetAppointments(DateTime startDate, DateTime endDate)
        {
            //need to startDate, endDate in query

            return ToCommonAppointments(QueryConn<OptomateTouchAppointment>($"{GetAppointmentsQuery}"));
        }

        public CommonAppointment GetAppointment(int appointmentId)
        {
            var sqlParams = new
            {
                AppointmentId = appointmentId
            };

            return ToCommonAppointment(QueryConn<OptomateTouchAppointment>($"{GetAppointmentQuery}", sqlParams).FirstOrDefault());
        }


        private List<CommonPatient> ToCommonPatients(IEnumerable<OptomateTouchPatient> optomateTouchPatients)
        {
            List<CommonPatient> commonPatients = new List<CommonPatient>();

            foreach (OptomateTouchPatient op in optomateTouchPatients)
            {
                commonPatients.Add(ToCommonPatient(op));
            }

            return commonPatients;
        }

        private List<CommonAppointment> ToCommonAppointments(IEnumerable<OptomateTouchAppointment> optomateTouchAppointments)
        {
            List<CommonAppointment> commonAppointments = new List<CommonAppointment>();

            foreach (OptomateTouchAppointment ota in optomateTouchAppointments)
            {
                commonAppointments.Add(ToCommonAppointment(ota));
            }

            return commonAppointments;
        }

        private CommonPatient ToCommonPatient(OptomateTouchPatient optomatePatient)
        {
            return new CommonPatient()
            {
                Title = optomatePatient.Title,
                FirstName = optomatePatient.FirstName,
                LastName = optomatePatient.LastName,
                DateOfBirth = optomatePatient.BirthDate,
                Gender = optomatePatient.Gender,
                Mobile = optomatePatient.Mobile,
                Email = optomatePatient.Email,
                //Phone = optomatePatient.Phone,
                ResidentialAddress = optomatePatient.ResidentAddress,
                ResidentialSuburb = optomatePatient.ResidentSuburb,
                ResidentialPostCode = optomatePatient.ResidentState,
                ResidentialState = optomatePatient.ResidentPostCode,

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

        private CommonAppointment ToCommonAppointment(OptomateTouchAppointment optomateAppointment)
        {
            return new CommonAppointment()
            {
                StartDate = optomateAppointment.StartDate,
                ID = optomateAppointment.ID,           
                EndDate = optomateAppointment.EndDate,
                BranchIdentifier = optomateAppointment.BranchIdentifier,
                UserIdentifier = optomateAppointment.UserIdentifier,
                AppointmentType = optomateAppointment.AppointmentType,
                PatientId = optomateAppointment.PatientId,
                Duration = optomateAppointment.Duration,
            };
        }

    }
}