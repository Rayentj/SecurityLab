using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DentalApp.Domain.Model
{
    [Table("Appointments")]
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public DateTime DateTime { get; set; }

        [ForeignKey("Surgery")]
        public int SurgeryId { get; set; }
        public Surgery Surgery { get; set; }

        [ForeignKey("Dentist")]
        public int DentistId { get; set; }
        public Dentist Dentist { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }


        public string Status { get; set; } = "Pending"; // or use an enum

    }
}