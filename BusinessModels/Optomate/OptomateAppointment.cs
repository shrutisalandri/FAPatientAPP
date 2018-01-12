using BusinessModels.Abstracts;
using System;

namespace BusinessModels
{
    public class OptomateAppointment : Appointment
    {
        
        public int Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime Finish { get; set; }

        public string Branch { get; set; }

        public string Optom { get; set; }

        public string Apptype { get; set; }

        public int ClientNum { get; set; }

        public string Given { set; get; }

        public string Surname { set; get; }

        public DateTime? BirthDate { set; get; }

        public string Title { set; get; }

        public string Sex { get; set; }
    }
}
