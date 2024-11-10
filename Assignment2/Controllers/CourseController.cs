using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using Assignment2.Entities;
using Assignment2.Services;
using Assignment2.Models;
namespace Assignment2.Controllers
{
    

    public class CourseController : BaseController
        {
            private readonly CourseManagementDbContext _context;
            private readonly IEmailService _emailService;

        public CourseController(CourseManagementDbContext context,IEmailService emailService)
            {
           
            _context = context;
            _emailService = emailService;
            }

            // List all courses
            public async Task<IActionResult> Index()
            {
            ViewData["WelcomeMessage"] = GenerateWelcomeMessage();

            var courses = await _context.Courses.Include(c => c.Students).ToListAsync();
                return View(courses);
            }

            // Create a new course
            public IActionResult Create()
            {
            ViewData["WelcomeMessage"] = GenerateWelcomeMessage();
            var course = new Course();
            course.StartDate = DateTime.Now;
                return View(course);
            }

            [HttpPost]
            public IActionResult Create(Course course)
            {
                if (ModelState.IsValid)
                {
                TempData["LastActionMessage"] = "Course added successfully!";
                _context.Add(course);
                     _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(course);
            }

            

            // Edit course
            public IActionResult Edit(int id)
            {
            ViewData["WelcomeMessage"] = GenerateWelcomeMessage();
            var course =  _context.Courses.Find(id);
                if (course == null)
                {
                    return NotFound();
                }
                return View(course);
            }

            [HttpPost]
            public  IActionResult Edit(int id, Course course)
            {
                if (id != course.CourseId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                TempData["LastActionMessage"] = "Course Updated successfully!";
                try
                    {
                        _context.Update(course);
                         _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CourseExists(course.CourseId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(course);
            }

            
            



        public IActionResult Manage(int id)
        {
            ViewData["WelcomeMessage"] = GenerateWelcomeMessage();
            var course = _context.Courses.Include(c => c.Students).FirstOrDefault(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new ManageCourseViewModel
            {
                Course = course,
                Student = new Student() // For creating a new student
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddStudent(ManageCourseViewModel viewModel)
        {
            var student = viewModel.Student;
            ModelState.Remove("Course");
            ModelState.Remove("Student.Course");
            ModelState.Remove("Student.Status");
            if (!ModelState.IsValid)
            {
                // Return the same view with the validation errors
                return RedirectToAction("Manage", new { id = student.CourseId });
            }

            


            
            student.Status = EnrollmentStatus.ConfirmationMessageNotSent;

            _context.Students.Add(student);
            _context.SaveChanges();

            TempData["LastActionMessage"] = "Student added successfully!";
            return RedirectToAction("Manage", new { id = student.CourseId });
        }


        [HttpPost]
        public IActionResult SendConfirmationMessages(int courseId)
        {
            TempData["LastActionMessage"] = "Confirmation Message sent successfully!";
            var course = _context.Courses
                .Include(c => c.Students)
                .FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
            {
                return NotFound();
            }

            foreach (var student in course.Students.Where(s => s.Status == EnrollmentStatus.ConfirmationMessageNotSent))
            {
                student.Status = EnrollmentStatus.ConfirmationMessageSent;
                var subject = $"Enrollment confirmation for \"{course.Name}\" required";
                var confirmUrl = Url.Action("ConfirmEnrollment", "Course", new { courseId, studentId = student.StudentId }, protocol: HttpContext.Request.Scheme);
                var message = $"<h1>Hello {student.Name}:</h1>" +
                    $"<p>Your request to enroll in the course \"{course.Name}\" in room {course.RoomNumber} starting {course.StartDate.ToShortDateString()} with instructor {course.Instructor}.</p>" +
                    $"<p>We are pleased to have you in the course so if you could <a href='{confirmUrl}'>confirm your enrollment</a> as soon as possible that would be appreciated!</p>" +
                    $"<p>Sincerely,</p>" +
                    $"<p>The Course Manager App</p>";
                
                // Logic to send confirmation message (e.g., email) goes here
                _emailService.SendEnrollmentConfirmation(student.Email, message,subject);
            }

            _context.SaveChanges();

            return RedirectToAction("Manage", new { id = courseId });
        }

        private bool CourseExists(int id)
            {
                return _context.Courses.Any(e => e.CourseId == id);
            }

        // Action to display the confirmation response form
        public IActionResult ConfirmEnrollment( int courseId, int studentId)
        {
            ViewData["WelcomeMessage"] = GenerateWelcomeMessage();
            var student = _context.Students.Find(studentId);
            var course = _context.Courses.Find(courseId);

            if (student == null || course == null)
            {
                return NotFound();
            }

            var model = new ConfirmEnrollmentViewModel
            {
                StudentId = studentId,
                CourseId = courseId,
                StudentName = student.Name,
                CourseName = course.Name,
                Room = course.RoomNumber,
                StartDate = course.StartDate,
                Instructor = course.Instructor
            };

            return View(model);
        }
        // Action to handle the form submission
        [HttpPost]
        public IActionResult ConfirmEnrollment(ConfirmEnrollmentViewModel model)
        {
            var student = _context.Students.Find(model.StudentId);
            if (student != null)
            {
                student.Status = model.IsEnrolled ? EnrollmentStatus.EnrollmentConfirmed : EnrollmentStatus.EnrollmentDeclined;
                _context.SaveChanges();
            }

            return RedirectToAction("ThankYou");
        }

        // Action for thank you page
        public IActionResult ThankYou()
        {
            ViewData["WelcomeMessage"] = GenerateWelcomeMessage();
            return View();
        }
    }



    }


