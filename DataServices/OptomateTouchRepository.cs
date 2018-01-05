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

        const string GetAppointmentQuery = @"SELECT APP.[ID], [STARTDATE] as StartDate,
             [ENDDATE] AS EndDate, APP.[BRANCH_IDENTIFIER] AS BranchIdentifier, APP.[USER_IDENTIFIER] AS UserIdentifier,
             [PATIENTID] AS PatientId, [DURATION] AS Duration,[GIVEN] AS FirstName, [SURNAME] AS LastName, [BIRTHDATE] AS BirthDate, [TITLE] AS  Title, [GENDER] AS Gender
             FROM [dbo].[APPOINTMENT] APP WITH(NOLOCK)
			 JOIN [dbo].[PATIENT] PA WITH(NOLOCK) ON PA.ID = APP.PATIENTID
             WHERE [ID] = @AppointmentId";

        const string GetAppointmentsQuery = @"SELECT APP.[ID], [STARTDATE] as StartDate,
             [ENDDATE] AS EndDate, APP.[BRANCH_IDENTIFIER] AS BranchIdentifier, APP.[USER_IDENTIFIER] AS UserIdentifier,
             [PATIENTID] AS PatientId, [DURATION] AS Duration,[GIVEN] AS FirstName, [SURNAME] AS LastName, [BIRTHDATE] AS BirthDate, [TITLE] AS  Title, [GENDER] AS Gender
             FROM [dbo].[APPOINTMENT] APP WITH(NOLOCK)
			 JOIN [dbo].[PATIENT] PA WITH(NOLOCK) ON PA.ID = APP.PATIENTID
            WHERE STARTDATE >= @StartDate AND ENDDATE <= @EndDate";

        const string InsertPatientQuery = @"INSERT INTO [dbo].[PATIENT]
            ([DATE_ADDED], [USER_ADDED], [TIMESTMP], [TITLE], [GIVEN], [MIDDLE], [SURNAME], [BIRTHDATE], [GENDER], [INACTIVE],
             [RESIDENT_ADDRESS], [RESIDENT_SUBURB], [RESIDENT_STATE], [RESIDENT_POSTCODE], [POSTAL_ADDRESS], [POSTAL_SUBURB],
             [POSTAL_STATE], [POSTAL_POSTCODE], [MOBILE_PHONE], [HOME_PHONE],[EMAIL],
             [HEALTHFUND_IDENTIFIER], [MEMBER_NUMBER], [HEALTHFUND_REFNO], [NO_HEALTHFUND], [MEDICARE_NUMBER], [MEDICARE_REFNO],
             [MEDICARE_EXPIRY], [DVA_NUMBER])
            VALUES (GetDate(),'FSTAVAILABLE', GetDate(), @Title, @FirstName, @Middle, @LastName, @BirthDate, @Gender, @InActive, @ResidentAddress,
            @ResidentSuburb, @ResidentState, @ResidentPostCode, @PostalAddress, @PostalSuburb, @PostalState, @PostalPostCode, @Mobile, @Phone,
            @Email, @HealthFundIdentifier, @MemberNumber, @HealthFundRefNo, @NoHealthFund, @MedicareNumber,
            @MedicareRefNo, @MedicareExpiry, @DVANumber);
            SELECT SCOPE_IDENTITY()";

        const string UpdatePatientQuery = @"UPDATE [dbo].[PATIENT]
            SET [USER_ADDED]='FSTAVAILABLE', [TITLE] = @Title, [GIVEN] = @FirstName, [MIDDLE] = @Middle, [SURNAME] = @LastName, 
            [BIRTHDATE] = @BirthDate,[GENDER] = @Gender, [INACTIVE] = @InActive, [RESIDENT_ADDRESS] = @ResidentAddress,
            [RESIDENT_SUBURB] = @ResidentSuburb, [RESIDENT_STATE] = @ResidentState, [RESIDENT_POSTCODE] = @ResidentPostCode,
            [POSTAL_ADDRESS] = @PostalAddress, [POSTAL_SUBURB] = @PostalSuburb,[POSTAL_STATE] = @PostalState, [POSTAL_POSTCODE] = @PostalPostCode,
            [MOBILE_PHONE] = @Mobile, [HOME_PHONE] = @Phone ,[EMAIL] = @Email,
            [HEALTHFUND_IDENTIFIER] = @HealthFundIdentifier, [MEMBER_NUMBER] = @MemberNumber,
            [HEALTHFUND_REFNO] = @HealthFundRefNo, [NO_HEALTHFUND] = @NoHealthFund, [MEDICARE_NUMBER] = @MedicareNumber,
            [MEDICARE_REFNO] = @MedicareRefNo, [MEDICARE_EXPIRY] = @MedicareExpiry, [DVA_NUMBER] = @DVANumber";


        const string GetPatientQuery = @"SELECT [ID], [TITLE] AS Title, [GIVEN] AS FirstName,[MIDDLE] AS Middle, [SURNAME] AS LastName,    
            [BIRTHDATE] AS BirthDate, [GENDER] AS Gender, [INACTIVE] AS InActive, [RESIDENT_ADDRESS] AS ResidentAddress,
            [RESIDENT_SUBURB] AS ResidentSuburb, [RESIDENT_STATE] AS ResidentState, [RESIDENT_POSTCODE] AS ResidentPostCode,
            [POSTAL_ADDRESS] AS PostalAddress, [POSTAL_SUBURB] AS PostalSuburb, [POSTAL_STATE] AS PostalState, [POSTAL_POSTCODE] AS PostalPostCode,
            [MOBILE_PHONE] AS Mobile,[HOME_PHONE] AS Phone, [EMAIL] AS Email,
            [HEALTHFUND_IDENTIFIER] AS HealthFundIdentifier, [MEMBER_NUMBER] AS MemberNumber, [HEALTHFUND_REFNO] AS HealthFundRefNo,
            [NO_HEALTHFUND] AS NoHealthFund, [MEDICARE_NUMBER] AS MedicareNumber, [MEDICARE_REFNO] AS MedicareRefNo,
            [MEDICARE_EXPIRY] AS MedicareExpiry, [DVA_NUMBER] AS DVANumber
            FROM [Optomate].[dbo].[PATIENT] WITH(NOLOCK)
            WHERE [ID] = @PatientId";

        const string GetPatientsQuery = @"SELECT [ID], [TITLE] AS Title, [GIVEN] AS FirstName, [MIDDLE] AS Middle, [SURNAME] AS LastName,    
            [BIRTHDATE] AS BirthDate, [GENDER] AS Gender, [INACTIVE] AS InActive, [RESIDENT_ADDRESS] AS ResidentAddress,
            [RESIDENT_SUBURB] AS ResidentSuburb, [RESIDENT_STATE] AS ResidentState, [RESIDENT_POSTCODE] AS ResidentPostCode,
            [POSTAL_ADDRESS] AS PostalAddress, [POSTAL_SUBURB] AS PostalSuburb, [POSTAL_STATE] AS PostalState, [POSTAL_POSTCODE] AS PostalPostCode,
            [MOBILE_PHONE] AS Mobile, [HOME_PHONE] AS Phone, [EMAIL] AS Email,
            [HEALTHFUND_IDENTIFIER] AS HealthFundIdentifier, [MEMBER_NUMBER] AS MemberNumber, [HEALTHFUND_REFNO] AS HealthFundRefNo,
            [NO_HEALTHFUND] AS NoHealthFund, [MEDICARE_NUMBER] AS MedicareNumber, [MEDICARE_REFNO] AS MedicareRefNo,
            [MEDICARE_EXPIRY] AS MedicareExpiry, [DVA_NUMBER] AS DVANumber
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

        public int InsertPatient(CommonPatient patient)
        {
            var sqlParams = new
            {
                Title = patient.Title,
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
                Phone = patient.Phone,
                HealthFundIdentifier = patient.HealthFundName,
                MemberNumber = patient.HealthFundMemberNumber,
                HealthFundRefNo = patient.HealthFundReferenceNumber,
                NoHealthFund = !patient.HasHealthFund,
                MedicareNumber = patient.MedicareMemberNumber,
                MedicareRefNo = patient.MeidcareReferenceNumber,
                MedicareExpiry = patient.MedicareExpiryDate,
                DVANumber = patient.DVAPensionNumber

            };

            return ExecScalarConn(InsertPatientQuery, sqlParams);
        }

        public bool UpdatePatient(CommonPatient patient)
        {
            var sqlParams = new
            {
                Title = patient.Title,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                BirthDate = patient.DateOfBirth,
                Gender = patient.Gender,
                InActive = patient.InActive,
                ResidentAddress = patient.ResidentialAddress,
                ResidentSuburb = patient.ResidentialSuburb,
                ResidentState = patient.ResidentialState,
                ResidentPostCode = patient.ResidentialPostCode,
                Mobile = patient.Mobile,
                Email = patient.Email,
                Phone = patient.Phone,
                HealthFundIdentifier = patient.HealthFundName,
                MemberNumber = patient.HealthFundMemberNumber,
                HealthFundRefNo = patient.HealthFundReferenceNumber,
                NoHealthFund = !patient.HasHealthFund,
                MedicareNumber = patient.MedicareMemberNumber,
                MedicareRefNo = patient.MeidcareReferenceNumber,
                MedicareExpiry = patient.MedicareExpiryDate,
                DVANumber = patient.DVAPensionNumber,

            };

            return ExecScalarConn(UpdatePatientQuery, sqlParams) > 0;
        }

        public List<CommonAppointment> GetAppointments(DateTime startDate, DateTime endDate)
        {
            var sqlParams = new
            {
                startDate = startDate,
                endDate = endDate
            };

            return ToCommonAppointments(QueryConn<OptomateTouchAppointment>($"{GetAppointmentsQuery}", sqlParams));
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
                Phone = optomatePatient.Phone,
                ResidentialAddress = optomatePatient.ResidentAddress,
                ResidentialSuburb = optomatePatient.ResidentSuburb,
                ResidentialPostCode = optomatePatient.ResidentState,
                ResidentialState = optomatePatient.ResidentPostCode,

                PostalAddress = optomatePatient.PostalAddress,
                PostalSuburb = optomatePatient.PostalSuburb,
                PostalPostCode = optomatePatient.PostalPostCode,
                PostalState = optomatePatient.PostalState,

                HealthFundName = optomatePatient.HealthFundIdentifier,
                HealthFundMemberNumber = optomatePatient.MemberNumber,
                HealthFundReferenceNumber = optomatePatient.HealthFundRefNo,
                MedicareMemberNumber = optomatePatient.MedicareNumber,

                HasHealthFund = !optomatePatient.NoHealthFund,
                MeidcareReferenceNumber = optomatePatient.MeidcareRefNo,
                //MedicareExpiryDate = DateTime.TryParse(optomatePatient.MedicareExpiry,),
                DVAPensionNumber = optomatePatient.DVANumber
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