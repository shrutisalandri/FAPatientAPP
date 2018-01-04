using BusinessModels.Abstracts;
using System;

namespace BusinessModels
{
    public class OptomateTouchAppointment : Appointment
    {
        public int ID { get; set; }

        public DateTime? DateAdded { get; set; }

        public string UserAdded { get; set; }

        public DateTime? DateEdited { get; set; }

        public string UserEdited { get; set; }

        public DateTime? TimeStamp { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string BranchIdentifier { get; set; }

        public string UserIdentifier { get; set; }

        public string AppointmentType { get; set; }

        public int PatientId { get; set; }

        public int Duration { get; set; }
    }
}
