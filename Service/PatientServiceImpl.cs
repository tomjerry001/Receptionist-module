using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicalManagementSystem_Team5.Models;
using ClinicalManagementSystem_Team5.Repository;

namespace ClinicalManagementSystem_Team5.Service
{
    public class PatientServiceImpl : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientServiceImpl(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task AddPatientAsync(Patient patient)
        {
            await Task.Run(() => _patientRepository.AddPatient(patient)); // Use Task.Run for async operation
        }

        public async Task DeletePatientAsync(int patientId)
        {
            await Task.Run(() => _patientRepository.DeletePatient(patientId)); // Use Task.Run for async operation
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await Task.Run(() => _patientRepository.GetAllPatients()); // Use Task.Run for async operation
        }

        public async Task<Patient> GetPatientByIdAsync(int patientId)
        {
            return await Task.Run(() => _patientRepository.GetPatientById(patientId)); // Use Task.Run for async operation
        }

        public async Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm)
        {
            return await _patientRepository.SearchPatientsAsync(searchTerm);
        }
    }
}
