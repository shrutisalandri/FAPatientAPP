using BusinessModels.Abstracts;
using System;

namespace BusinessModels
{
    public class OptomateAppointment : Appointment
    {
        public int Id { get; set; }
        public int ClientNum { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public string Caption { get; set; }
        public string Branch { get; set; }
        public string Resourceid { get; set; }
        public string Appttype { get; set; }
        public string Staff { get; set; }
        public DateTime? Transdate { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string Location { get; set; }
        public bool smsconfirm { get; set; }
        public bool sentsms { get; set; }
        public string syncId { get; set; }
    }
}
