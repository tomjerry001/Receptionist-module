﻿using ClinicalManagementSystem_Team5.Models;

public interface IDoctorRepository
{
    Task<IEnumerable<Specialization>> GetSpecializationsAsync();
    Task<IEnumerable<Doctor>> GetDoctorsBySpecializationAsync(int specializationId);

    Task<(int TokenNumber, decimal TotalAmount)> BookAppointmentAsync(int patientId, int doctorId, DateTime dateAndTime);
}
