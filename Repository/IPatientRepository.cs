using ClinicalManagementSystem_Team5.Models;

namespace ClinicalManagementSystem_Team5.Repository
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetAllPatients();
        void AddPatient(Patient patient);
        Patient GetPatientById(int? patientId);
        void DeletePatient(int? patientId);
        Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm);
    }
}