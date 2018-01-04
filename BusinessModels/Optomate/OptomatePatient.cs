using BusinessModels.Abstracts;
using System;

namespace BusinessModels
{
    public class OptomatePatient: Patient
    {
        public int Number { set; get; }   //PatientId
        public string Given { set; get; }  /// FirstName
        public string Surname { set; get; }  /// LastName
        public string Mobile { set; get; }
        public string Email { set; get; }
        public DateTime? BirthDate { set; get; }
        public string Title { set; get; }
        public string Address1 { set; get; }
        public string Suburb { set; get; }   //city
        public string State { set; get; }
        public string Postcode { set; get; }
        public string Phone_Ah { set; get; }
        public string Phone_Mob { set; get; }
        public string Comment { set; get; }
        public string Optom { set; get; }
        public string Branch { set; get; }
    }
}
