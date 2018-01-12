using DataServices.Interfaces;
using System;
using System.Collections.Generic;
using BusinessModels.DTOS;
using BusinessModels;
using Shared.Core;
using System.Data.Odbc;
using Dapper;
using System.Linq;
using System.Dynamic;

namespace DataServices
{
    public class OptomateRepository : IRepository
    {
        private readonly string _connString;
        private readonly int _retries = 0;
        private readonly int _commandTimeout = 0;

        private const string SetPassWordQuery = "SET PASSWORDS ADD '6!6@6#5$3%9' ;";

        private const string UpdatePatientQuery = @"UPDATE CLIENT
                                                   SET Title=?Title?,GIVEN=?Given?,SURNAME=?Surname?,
                                                   ADDRESS1=?Address1?,SUBURB=?Suburb?,STATE=?State?,POSTCODE=?Postcode?,
                                                   PHONE_AH=?Phone_Ah?,PHONE_MOB=?Phone_Mob?,EMAIL=?Email?,BIRTHDATE=?BirthDate?,
                                                   Sex=?Sex?,Medicare=?Medicare?, MedRef=?MedRef?,Expiry=?Expiry?, 
                                                   HealthFund=?HealthFund?,MemberNum=?MemberNum?, IsInActive=?IsInActive?
                                                   WHERE NUMBER=?PatientId?";

        private const string InsertPatientQuery = @"INSERT INTO CLIENT (NUMBER,Title,GIVEN,SURNAME,ADDRESS1,SUBURB,STATE,POSTCODE,PHONE_AH,
                                                    PHONE_MOB,EMAIL,BIRTHDATE,Sex, Medicare, MedRef, Expiry,  HealthFund,
                                                    MemberNum, IsInActive) 
                                                    VALUES(?Number?,?Title?, ?Given?, ?Surname?, ?Address1?, ?Suburb?, ?State?,
                                                    ?Postcode?, ?Phone_Ah?, ?Phone_Mob?, ?Email?, ?BirthDate?,
                                                    ?Sex?,?Medicare?,?MedRef?,?Expiry?,?HealthFund?,?MemberNum?,?IsInActive?);";


        private const string GetAppointmentsQuery = @"SELECT  Id,""start"",""finish"", apptype, APPNEXUS.Branch, Optom,
                                                     ClientNum, Given, Surname, BirthDate, Title, Sex
                                                     FROM APPNEXUS
                                                     JOIN Client ON APPNEXUS.clientnum=Client.number
                                                     WHERE CAST(""start"" as date) >=CAST(?start? AS DATE) AND 
                                                     CAST(""finish"" as date) <=CAST(?finish? AS DATE)";

        private const string GetAppointmentQuery = @"SELECT  Id,""start"",""finish"", apptype, APPNEXUS.Branch, Optom,
                                                     ClientNum, Given, Surname, BirthDate, Title, Sex
                                                     FROM APPNEXUS
                                                     JOIN Client ON APPNEXUS.clientnum=Client.number
                                                     WHERE ID=?bookingId?";


        private const string GetPatientQuery = @"SELECT Number, Title, Given, Surname, Address1, Suburb, State,
                                                 Postcode, Phone_Ah, Phone_Mob, Email, BirthDate,
                                                 Sex, Medicare, MedRef, Expiry, BenefitNum, HealthFund,
                                                 MemberNum, IsInActive
                                                 FROM CLIENT 
                                                 WHERE NUMBER =?patientId?";

        private const string GetPatientsQuery = @"SELECT Number, Title, Given, Surname, Address1, Suburb, State,
                                                  Postcode, Phone_Ah, Phone_Mob, Email, BirthDate,
                                                  Sex, Medicare, MedRef, Expiry, BenefitNum, HealthFund,
                                                  MemberNum, IsInActive
                                                  FROM CLIENT
                                                  WHERE (IsInActive IS NULL OR IsInActive =false )";

        private const string GetPatientsSearchQuery = @"SELECT Number, Title, Given, Surname, Address1, Suburb, State,
                                                        Postcode, Phone_Ah, Phone_Mob, Email, BirthDate,
                                                        Sex, Medicare, MedRef, Expiry, BenefitNum, HealthFund,
                                                        MemberNum, IsInActive
                                                        FROM CLIENT 
                                                        WHERE (IsInActive IS NULL OR IsInActive = false ) AND";

        private const string SelectClientNextId = @"SELECT COALESCE(LASTPATNUM,0)+1 FROM GenerateNumbers ";

        private const string SetClientNextIdQuery = @"UPDATE GenerateNumbers SET LASTPATNUM = ?nextClientId?";

        private const string SelectClientCount = @"SELECT count(number) from Client WHERE number = ?nextClientId?;";

        public OptomateRepository(string ConnectionString)
        {
            _connString = ConnectionString;
        }

        private int ExecConn(string sql, object sqlParams = null)
        {
            // Nexus expects timeout in milliseconds, whereas .net command expects seconds
            var sqlWithTimeout = _commandTimeout != 0 ? $"#t {_commandTimeout * 1000}; {sql}" : sql;
            using (var conn = new OdbcConnection(_connString))
            {
                conn.OpenRobust();

                return conn.ExecuteRobust(sqlWithTimeout, sqlParams);
            }
        }

        private IEnumerable<T> QueryConn<T>(string sql, object sqlParams = null) where T : class
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
                bookingId = appointmentId
            };

            return ToCommonAppointment(QueryConn<OptomateAppointment>($"{SetPassWordQuery} {GetAppointmentQuery}", sqlParams).FirstOrDefault());
        }

        public List<CommonAppointment> GetAppointments(DateTime startDate, DateTime endDate)
        {

            var sqlParams = new
            {
                start = DateUtils.ToYYYY_MM_DD(startDate),
                finish = DateUtils.ToYYYY_MM_DD(endDate)
            };

            return ToCommonAppointments(QueryConn<OptomateAppointment>($"{SetPassWordQuery} {GetAppointmentsQuery}", sqlParams));

        }

        public CommonPatient GetPatient(int patientId)
        {
            var sqlParams = new DynamicParameters(new { patientId = patientId });

            OptomatePatient patient = QueryConn<OptomatePatient>($"{SetPassWordQuery} {GetPatientQuery}",
                sqlParams).FirstOrDefault();

            return ToCommonPatient(patient);
        }

        public List<CommonPatient> GetPatients()
        {
            return ToCommonPatients(QueryConn<OptomatePatient>($"{SetPassWordQuery} {GetPatientsQuery}"));
        }

        public int InsertPatient(CommonPatient patient)
        {

            int rowsrfected = 0;
            int nextId = GetClientNextId();
            int recordsWithNextId = GetClientCount(nextId);

            while (recordsWithNextId > 0)
            {
                nextId = nextId + 1;
                recordsWithNextId = GetClientCount(nextId);
            }

            bool nextIdSet = SetClientNextId(nextId);

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
                Title = patient.Title,
                Sex = patient.Gender,
                IsInActive = false,
                HealthFund = patient.HealthFundName,
                MemberNum = patient.HealthFundMemberNumber,
                Medicare = patient.MedicareMemberNumber,
                MedRef = patient.MeidcareReferenceNumber,
                Expiry = patient.MedicareExpiryDate?.ToString()
            };

            rowsrfected = ExecConn($"{SetPassWordQuery} {InsertPatientQuery}", newPatient);

            if (rowsrfected > 0)
            {
                return nextId;
            }

            return 0;
        }

        public bool UpdatePatient(CommonPatient patient)
        {
            var sqlParams = new
            {
                PatientId = patient.Id,
                Title = patient.Title,
                Given = patient.FirstName,
                Surname = patient.LastName,
                BirthDate = patient.DateOfBirth,
                Address1 = patient.ResidentialAddress,
                Suburb = patient.ResidentialSuburb,
                State = patient.ResidentialState,
                Postcode = patient.ResidentialPostCode,
                Phone_Mob = patient.Mobile,
                Email = patient.Email,
                Phone_Ah = patient.Phone,
                Sex = patient.Gender,
                IsInActive = patient.InActive,
                HealthFund = patient.HealthFundName,
                MemberNum = patient.HealthFundMemberNumber,
                Medicare = patient.MedicareMemberNumber,
                MedRef = patient.MeidcareReferenceNumber,
                Expiry = patient.MedicareExpiryDate?.ToString(),

                //HealthFundRefNo = patient.HealthFundReferenceNumber,
                //NoHealthFund = !patient.HasHealthFund,
                //DVANumber = patient.DVAPensionNumber,
                //PostalAddress = patient.PostalAddress,
                //PostalSuburb = patient.PostalSuburb,
                //PostalState = patient.PostalState,
                //PostalPostCode = patient.PostalPostCode,

            };

            return ExecConn($"{SetPassWordQuery} {UpdatePatientQuery}", sqlParams) > 0;
        }

        public List<CommonPatient> SearchPatients(int patientId, string firstName, string lastName)
        {
            string query = $"{SetPassWordQuery} {GetPatientsSearchQuery}";

            dynamic sqlParams = new ExpandoObject();

            if (patientId != 0)
            {
                sqlParams.PatientId = patientId;

                query += " (ID= ?PatientId?) AND";

            }
            if (!string.IsNullOrEmpty(firstName))
            {
                sqlParams.FirstName = firstName.Trim();

                query += " (GIVEN= ?FirstName? OR GIVEN LIKE '%?FirstName?%' IGNORE CASE) AND";

            }
            if (!string.IsNullOrEmpty(lastName))
            {
                sqlParams.LastName = lastName;

                query += " (SURNAME= ?LastName? OR SURNAME LIKE '%?LastName?%' IGNORE CASE) AND";

            }

            query = query.Remove(query.Length - 3);

            return ToCommonPatients(QueryConn<OptomateTouchPatient>(query, sqlParams));
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
            if (optomatePatient == null)
            {
                return null;
            }

            return new CommonPatient()
            {
                Id = optomatePatient.Number,
                Title = optomatePatient.Title,
                FirstName = optomatePatient.Given,
                LastName = optomatePatient.Surname,
                DateOfBirth = optomatePatient.BirthDate,
                Gender = optomatePatient.Sex,
                Mobile = optomatePatient.Phone_Mob,
                Email = optomatePatient.Email,
                Phone = optomatePatient.Phone_Ah,
                ResidentialAddress = optomatePatient.Address1,
                ResidentialSuburb = optomatePatient.Suburb,
                ResidentialPostCode = optomatePatient.State,
                ResidentialState = optomatePatient.Postcode,

                HealthFundName = optomatePatient.HealthFund,
                HealthFundMemberNumber = optomatePatient.MemberNum,
                MedicareMemberNumber = optomatePatient.Medicare,
                HasHealthFund = !(string.IsNullOrEmpty(optomatePatient.HealthFund)),
                MeidcareReferenceNumber = optomatePatient.MedRef

                //PostAddressSameAsResidentialAddress = optomatePatient.Title,
                //PostalAddress = optomatePatient.Title,
                //PostalSuburb = optomatePatient.Title,
                //PostalPostCode = optomatePatient.Title,
                //PostalState = optomatePatient.Title,
                //HealthFundReferenceNumber = optomatePatient.,
                //MedicareExpiryDate = optomatePatient.Expiry,
                //DVAPensionNumber = optomatePatient.DVAPensionNumber
            };

        }

        private CommonAppointment ToCommonAppointment(OptomateAppointment optomateAppointment)
        {

            if (optomateAppointment == null)
            {
                return null;
            }

            return new CommonAppointment()
            {
                StartDate = optomateAppointment.Start,
                ID = optomateAppointment.Id,
                EndDate = optomateAppointment.Finish,
                BranchIdentifier = optomateAppointment.Branch,
                UserIdentifier = optomateAppointment.Optom,
                AppointmentType = optomateAppointment.Apptype,
                PatientId = optomateAppointment.ClientNum,
                FirstName = optomateAppointment.Given,
                LastName = optomateAppointment.Surname,
                BirthDate = optomateAppointment.BirthDate,
                Title = optomateAppointment.Title,
                Gender = optomateAppointment.Sex
            };
        }


    }
}
