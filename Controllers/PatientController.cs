using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicalManagementSystem_Team5.Models;
using ClinicalManagementSystem_Team5.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalManagementSystem_Team5.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        public PatientController(IPatientService patientService, IDoctorService doctorService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
        }

        // GET: PatientController
        public async Task<IActionResult> Index(string searchTerm)
        {
            var patients = string.IsNullOrWhiteSpace(searchTerm)
                ? await _patientService.GetAllPatientsAsync()
                : await _patientService.SearchPatientsAsync(searchTerm);

            return View(patients);
        }

        // GET: PatientController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            return View(patient);
        }


        // GET: Patient/Create
        public IActionResult Create()
        {
            return View(); // Load the Create.cshtml view
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            try
            {
                // Add the patient to the database via the service layer.
                await _patientService.AddPatientAsync(patient);

                // Redirect to the Index action (or wherever you'd like after successful creation)
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                // Handle the exception (e.g., log it)
                ModelState.AddModelError("", $"An error occurred while adding the patient: {ex.Message}");
            }

            // Return the same view if an error occurs
            return View(patient);
        }


        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                // Your logic here
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: Patient/BookAppointment/5
        public async Task<IActionResult> BookAppointment(int id)
        {
            // Pass the patient id and the list of specializations to the view
            ViewBag.PatientId = id;
            ViewBag.Specializations = await _doctorService.GetSpecializationsAsync();
            return View();
        }



        // API to get doctors by specialization
        [HttpGet]
        public async Task<JsonResult> GetDoctorsBySpecialization(int specializationId)
        {
            var doctors = await _doctorService.GetDoctorsBySpecializationAsync(specializationId);
            return Json(doctors);
        }

        // GET: PatientController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            return View(patient);
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _patientService.DeletePatientAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAppointment(int patientId, int doctorId, DateTime appointmentDate, string timeSlot)
        {
            DateTime dateAndTime = DateTime.Parse($"{appointmentDate.ToShortDateString()} {timeSlot}");

            var result = await _doctorService.BookAppointmentAsync(patientId, doctorId, dateAndTime);
            int tokenNumber = result.TokenNumber;
            decimal totalAmount = result.TotalAmount;

            ViewBag.TokenNumber = tokenNumber;
            ViewBag.TotalAmount = totalAmount;

            return RedirectToAction(nameof(Index));
        }




    }
}