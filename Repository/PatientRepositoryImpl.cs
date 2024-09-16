using ClinicalManagementSystem_Team5.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ClinicalManagementSystem_Team5.Repository
{
    public class PatientRepositoryImpl : IPatientRepository
    {
        private readonly string connectionString;

        public PatientRepositoryImpl(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConnectionMvcwin");
        }

        public void AddPatient(Patient patient)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddPatient", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Adding input parameters as required by the stored procedure
                    cmd.Parameters.AddWithValue("@PatientName", patient.PatientName);
                    cmd.Parameters.AddWithValue("@DOB", patient.DOB);
                    cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup);
                    cmd.Parameters.AddWithValue("@PhoneNumber", patient.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", patient.Address);
                    cmd.Parameters.AddWithValue("@Email", patient.Email);

                    // Add an output parameter for PatientID (output parameter doesn't require a value)
                    SqlParameter outputIdParam = new SqlParameter("@PatientID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputIdParam);

                    try
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();

                        // Retrieve the output parameter value (PatientID)
                        patient.PatientId = Convert.ToInt32(outputIdParam.Value);  // Ensure correct casting
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }



        public void DeletePatient(int? patientId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeletePatient", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientId", patientId);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_GetAllPatients", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        Patient patient = new Patient();
                        patient.PatientId = Convert.ToInt32(dr["PatientId"].ToString());
                        patient.PatientName = dr["PatientName"].ToString();
                        patient.DOB = Convert.ToDateTime(dr["DOB"]);
                        patient.Gender = dr["Gender"].ToString();
                        patient.BloodGroup = dr["BloodGroup"].ToString();
                        patient.PhoneNumber = dr["PhoneNumber"].ToString();
                        patient.Address = dr["Address"].ToString();
                        patient.Email = dr["Email"].ToString();

                        patients.Add(patient);
                    }
                    connection.Close();
                }
                return patients;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Patient GetPatientById(int? patientId)
        {
            Patient patient = new Patient();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetPatientById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("PatientId", patientId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    patient.PatientId = Convert.ToInt32(dr["PatientId"].ToString());
                    patient.PatientName = dr["PatientName"].ToString();
                    patient.DOB = Convert.ToDateTime(dr["DOB"]);
                    patient.Gender = dr["Gender"].ToString();
                    patient.BloodGroup = dr["BloodGroup"].ToString();
                    patient.PhoneNumber = dr["PhoneNumber"].ToString();
                    patient.Address = dr["Address"].ToString();
                    patient.Email = dr["Email"].ToString();
                }
                con.Close();
            }
            return patient;
        }

        public async Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm)
        {
            var patients = new List<Patient>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("sp_SearchPatients", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var patient = new Patient
                            {
                                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                                PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                                DOB = reader.GetDateTime(reader.GetOrdinal("DOB")),
                                Gender = reader.GetString(reader.GetOrdinal("Gender")),
                                BloodGroup = reader.GetString(reader.GetOrdinal("BloodGroup")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Email = reader.GetString(reader.GetOrdinal("Email"))
                            };
                            patients.Add(patient);
                        }
                    }
                }
            }

            return patients;
        }

    }
}