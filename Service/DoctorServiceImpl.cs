using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ClinicalManagementSystem_Team5.Models;
using ClinicalManagementSystem_Team5.Repository;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ClinicalManagementSystem_Team5.Service
{
    public class DoctorServiceImpl : IDoctorService
    {
        
        private readonly IDoctorRepository _doctorRepository;
     
        public DoctorServiceImpl(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<Specialization>> GetSpecializationsAsync()
        {
            return await _doctorRepository.GetSpecializationsAsync();
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsBySpecializationAsync(int specializationId)
        {
            return await _doctorRepository.GetDoctorsBySpecializationAsync(specializationId);
        }
        public async Task<(int TokenNumber, decimal TotalAmount)> BookAppointmentAsync(int patientId, int doctorId, DateTime dateAndTime)
        {
            return await _doctorRepository.BookAppointmentAsync(patientId, doctorId, dateAndTime);
        }
     

    }
}
