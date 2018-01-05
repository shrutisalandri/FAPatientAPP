export class Patient {
  PatientId: string;
  Title: string;
  FirstName: string;
  LastName: string;
  DOB: Date;
  Gender: string;
  Mobile: string;
  Email: string;
  Phone: string;
  ResidentialAddress: Address;
  PostAddressSameAsResidentialAddress: boolean;
  PostalAddress: Address;
  HasHealthFund: boolean;
  PrivateHealthFund: HealthFund
  MedicareMemberNumber: string;
  MeidcareReferenceNumber: string;
  MedicareExpiryDate: Date;
  DVAPensionNumber: string;
}

export class Address {
  Address: string;
  Suburb: string;
  Postcode: string;
  State: string;
}
export class HealthFund {
  FundName: string;
  FundMemberNumber: string;
  FundRefreenceNumber: string;
}
