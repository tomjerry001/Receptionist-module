using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicalManagementSystem_Team5.Models;

namespace ClinicalManagementSystem_Team5.Service
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllPatientsAsync();  // Added async method
        Task<Patient> GetPatientByIdAsync(int patientId);  // Added async method
        Task AddPatientAsync(Patient patient);  // Added async method
        Task DeletePatientAsync(int patientId);  // Added async method
        Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm);
    }
}
