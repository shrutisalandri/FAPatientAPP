﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModels.DTOS
{
    public class CommonAppointment
    {
        public int ID { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string BranchIdentifier { get; set; }

        public string UserIdentifier { get; set; }

        public string AppointmentType { get; set; }

        public int PatientId { get; set; }

        public int Duration { get; set; }    

        public string FirstName { set; get; }

        public string LastName { set; get; }

        public DateTime? BirthDate { set; get; }

        public string Title { set; get; }

        public string Gender { set; get; }
    }
}
