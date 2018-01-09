using BusinessModels.Abstracts;
using System;

namespace BusinessModels
{
    public class OptomatePatient: Patient
    {
        public int Number { set; get; } 

        public string Given { set; get; } 

        public string Surname { set; get; } 

        public string Email { set; get; }

        public DateTime? BirthDate { set; get; }

        public string Title { set; get; }

        public string Address1 { set; get; }

        public string Suburb { set; get; }  

        public string State { set; get; }

        public string Postcode { set; get; }

        public string Phone_Ah { set; get; }

        public string Phone_Mob { set; get; }

        public string Sex { get; set; }

        public string Medicare { get; set; }

        public string MedRef { get; set; }

        public string Expiry { get; set; }

        public string BenefitNum { get; set; }

        public string HealthFund { get; set; }

        public string MemberNum { get; set; }

        public string IsInActive { get; set; }

    }
}
