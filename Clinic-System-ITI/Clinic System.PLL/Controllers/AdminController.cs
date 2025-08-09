using Clinic_System.BLL.Service.Abstraction;
using Clinic_System.BLL.ModelVM.DoctorVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Clinic_System.BLL.Service.Implementation;

namespace Clinic_System.PLL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IDashBoardService _dashboardService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        public AdminController(IDashBoardService dashboardService, IDoctorService doctorService, IPatientService patientService, IAppointmentService appointmentService)
        {
            _dashboardService = dashboardService;
            _doctorService = doctorService;
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        public IActionResult Index()
        {
            var dashboardData = _dashboardService.GetDashboardData();
            return View(dashboardData);
        }

        public IActionResult ManageDoctors()
        {
            var doctors = _doctorService.GetAllDoctors();
            return View("ManageDoctors", doctors);
        }

        public IActionResult ManagePatients()
        {
            var patients = _patientService.GetAllPatients();
            return View("ManagePatients", patients);
        }

        public IActionResult ManageAppointments()
        {
            var appointments = _appointmentService.GetAllAppointments();
            return View("ManageAppointments", appointments);
        }

        public IActionResult AdminProfile()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }
    }
}

