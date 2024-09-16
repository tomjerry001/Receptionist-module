using ClinicalManagementSystem_Team5.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Threading.Tasks;

namespace ClinicalManagementSystem_Team5.Repository
{
    public class DoctorRepositoryImpl : IDoctorRepository
    {
        private readonly string connectionString;

        public DoctorRepositoryImpl(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConnectionMvcwin");
        }

        public async Task<IEnumerable<Specialization>> GetSpecializationsAsync()
        {
            var specializations = new List<Specialization>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SELECT SpecializationId, SpecializationName FROM Specialization", connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            specializations.Add(new Specialization
                            {
                                SpecializationId = reader.GetInt32(0),
                                SpecializationName = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return specializations;
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsBySpecializationAsync(int specializationId)
        {
            var doctors = new List<Doctor>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SELECT DoctorId, DoctorName, ConsultationFee FROM Doctor WHERE SpecializationId = @SpecializationId", connection))
                {
                    command.Parameters.AddWithValue("@SpecializationId", specializationId);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            doctors.Add(new Doctor
                            {
                                DoctorId = reader.GetInt32(0),
                                DoctorName = reader.GetString(1),
                                ConsultationFee = Convert.ToInt32(reader.GetDecimal(2)) // Convert with rounding
                            });

                        }
                    }
                }
            }

            return doctors;
        }



        public async Task<(int TokenNumber, decimal TotalAmount)> BookAppointmentAsync(int patientId, int doctorId, DateTime dateAndTime)
        {
            int tokenNumber;
            decimal totalAmount;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("sp_BookAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PatientId", patientId);
                    command.Parameters.AddWithValue("@DoctorId", doctorId);
                    command.Parameters.AddWithValue("@DateAndTime", dateAndTime); // Pass the date and time

                    command.Parameters.Add("@TokenNumber", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Direction = ParameterDirection.Output;

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    tokenNumber = (int)command.Parameters["@TokenNumber"].Value;
                    totalAmount = (decimal)command.Parameters["@TotalAmount"].Value;
                }
            }

            return (tokenNumber, totalAmount);
        }

    }
}
