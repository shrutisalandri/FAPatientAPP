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

        public string Middle { set; get; }

        public string Gender { set; get; }

        public bool InActive { set; get; }

        public string ResidentAddress { set; get; }

        public string ResidentSuburb { set; get; }

        public string ResidentState { set; get; }

        public string ResidentPostCode { set; get; }

        public string UserIdentifier { set; get; }

        public string BranchIdentifier { set; get; }
    }
}
