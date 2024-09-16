using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicalManagementSystem_Team5.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; } // Make sure this matches the database column name

        [Required]
        [StringLength(100)] // Added a string length constraint for better database design
        public string PatientName { get; set; } // Make sure this matches the database column name

        [Required]
        public DateTime DOB { get; set; } // Make sure this matches the database column name

        [Required]
        [StringLength(10)] // Added a string length constraint to fit common gender values
        public string Gender { get; set; } // Make sure this matches the database column name

        [Required]
        [StringLength(10)] // Added a string length constraint for blood group values
        public string BloodGroup { get; set; } // Make sure this matches the database column name

        [Required]
        [StringLength(15)] // Added a string length constraint for phone numbers
        public string PhoneNumber { get; set; } // Make sure this matches the database column name

        [Required]
        [StringLength(255)] // Added a string length constraint for addresses
        public string Address { get; set; } // Make sure this matches the database column name

        [Required]
        [EmailAddress] // Added data annotation for email validation
        [StringLength(100)] // Added a string length constraint for email addresses
        public string Email { get; set; } // Make sure this matches the database column name
    }
}
