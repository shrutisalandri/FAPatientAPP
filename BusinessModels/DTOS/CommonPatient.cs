using System;

namespace BusinessModels.DTOS
{
    public class CommonPatient
    {

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string ResidentialAddress { get; set; }

        public string ResidentialSuburb { get; set; }

        public string ResidentialPostCode { get; set; }

        public string ResidentialState { get; set; }

        public bool PostAddressSameAsResidentialAddress { get; set; }

        public string PostalAddress { get; set; }

        public string PostalSuburb { get; set; }

        public string PostalPostCode { get; set; }

        public string PostalState { get; set; }

        public string HealthFundName { get; set; }

        public string HealthFundMemberNumber { get; set; }

        public string HeatlhFundRefreenceNumber { get; set; }

        public string MedicareMemberNumber { get; set; }

        public bool HasHealthFund { get; set; }

        public string MeidcareReferenceNumber { get; set; }

        public DateTime? MedicareExpiryDate { get; set; }

        public string DVAPensionNumber { get; set; }

    }
}
