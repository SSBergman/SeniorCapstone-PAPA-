using Microsoft.AspNetCore.Mvc;
using ASP_Test.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_Test.Controllers
{
    public class StudentController : Controller
    {

        private readonly PapaDb1Context PAPACONTEXT;
        public StudentController(PapaDb1Context _PAPACONTEXT)
        {
            this.PAPACONTEXT = _PAPACONTEXT;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var students = await PAPACONTEXT.Students.ToListAsync();
            var students = await PAPACONTEXT.Students.Where(s=>s.StudentId=="U5").ToListAsync();
            return View(students);
        }
        //[HttpGet]
        //public IActionResult Add()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Add(addStudentview addstudentrequest) 
        //{
        //    var student = new Student()
        //    {
        //        StudentId = addstudentrequest.StudentId,
        //        Email = addstudentrequest.Email,
        //        StudentPassword = addstudentrequest.StudentPassword,
        //    };
        //    await PAPACONTEXT.Students.AddAsync(student);
        //    await PAPACONTEXT.SaveChangesAsync();
        //    return RedirectToAction("index");
        //}
        //[HttpGet("NewStudent")]
        //public async Task<IActionResult>NewStudent(string password,string email)
        //{
        //    var result = await PAPACONTEXT.Students.FromSqlRaw($"NewStudent{email},{password}");
        //    return Ok(result);
        //}

        //[HttpPost]
        //public IActionResult Add(Student addStudentRequest)
        //{
        //    var student = new Student()
        //    {
        //        Degree_ID= addStudentRequest.Degree_ID,
        //        Email = addStudentRequest.Email,
        //        ID_Num = addStudentRequest.ID_Num,
        //        Student_Password = addStudentRequest.Student_Password,
        //        Grad_Year= addStudentRequest.Grad_Year,
        //    };
        //    await PAPACONTEXT.Students.AddAsync(student);
        //    await PAPACONTEXT.SaveChangesAsync();
        //    return RedirectToAction("Add");
        //}

    }
}
