using BusinessModels.Abstracts;
using System;

namespace BusinessModels
{
    public class OptomateTouchPatient: Patient
    {
        public int ID { set; get; }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        public string Mobile { set; get; }

        public string Email { set; get; }

        public DateTime? BirthDate { set; get; }

        public string Title { set; get; }

        //public string Middle { set; get; }

        public string Gender { set; get; }

        public bool InActive { set; get; }

        public string ResidentAddress { set; get; }

        public string ResidentSuburb { set; get; }

        public string ResidentState { set; get; }

        public string ResidentPostCode { set; get; }

        public string PostalAddress { set; get; }

        public string PostalSuburb { set; get; }

        public string PostalState { set; get; }

        public string PostalPostCode { set; get; }

        public string Phone { set; get; }                    

        public string HealthFundIdentifier { get; set; }

        public string MemberNumber { get; set; }

        public string HealthFundRefNo { get; set; }

        public bool NoHealthFund { get; set; }

        public string MedicareNumber { get; set; }

        public string MeidcareRefNo { get; set; }

        public string MedicareExpiry { get; set; }

        public string DVANumber { get; set; }


    }
}
