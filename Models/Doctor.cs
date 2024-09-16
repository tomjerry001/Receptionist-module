using ClinicalManagementSystem_Team5.Models;

namespace ClinicalManagementSystem_Team5.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public int ConsultationFee { get; set; }
    }

}