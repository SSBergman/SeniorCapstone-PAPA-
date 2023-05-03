using ASP_Test.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Routing.Patterns;
using System.Reflection.Metadata.Ecma335;
using System;
using Newtonsoft.Json.Linq;

namespace ASP_Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly PapaDb1Context PAPACONTEXT;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, PapaDb1Context _PAPACONTEXT)
        {
            _logger = logger;
            this.PAPACONTEXT = _PAPACONTEXT;
        }
        public IActionResult Welcome()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult TestingForms()
        {
            DegreeForm values = new DegreeForm();
            values.DegreeName = "";
            values.VersionYear = "";
            //values.IsActive = false;
            return View("TestingForms",values);
        }
        [HttpPost]
        public IActionResult TestingForms(DegreeForm oldValues)
        {
            DegreeForm newValues= new DegreeForm();
            newValues.DegreeName = oldValues.DegreeName;
            newValues.VersionYear = oldValues.VersionYear;
            return View("TestingForms", newValues);
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return View("LogIn");
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(addStudentview loginrequest)
        {
            string HashedPWord = "";
            byte[] PWordBytes = ASCIIEncoding.ASCII.GetBytes(loginrequest.StudentPassword);
            byte[] tmp = new MD5CryptoServiceProvider().ComputeHash(PWordBytes);
            foreach (byte part in tmp)
            {
                HashedPWord = HashedPWord + part.ToString("X2");
            }
            int count = (await PAPACONTEXT.Students.Where(s => s.Email == loginrequest.Email && s.StudentPassword==HashedPWord).ToListAsync()).Count;
            if (count>1||count==0)
            {
                return RedirectToAction("LogIn");
            }
            else
            {
                string UID = (await PAPACONTEXT.Students.Where(s => s.Email == loginrequest.Email && s.StudentPassword == HashedPWord).ToListAsync())[0].StudentId;
                HttpContext.Session.SetString("UID", UID);
                if(UID.CompareTo("ADMIN")==0)
                {
                    return RedirectToAction("AdminDegree");
                }
                else
                {
                    HttpContext.Session.SetInt32("Index", 0);
                    HttpContext.Session.SetInt32("Count", (await PAPACONTEXT.SemCount.Where(s => s.StudentId == UID).ToListAsync())[0].Semesters);
                    return RedirectToAction("Degree");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(addStudentview addstudentrequest)
        {
            if ((await PAPACONTEXT.Students.Where(s => s.Email == addstudentrequest.Email).ToListAsync()).Count > 0)
            {
                return View("SignUp");
            }
            else
            {
                string HashedPWord = "";
                byte[] PWordBytes = ASCIIEncoding.ASCII.GetBytes(addstudentrequest.StudentPassword);
                byte[] tmp = new MD5CryptoServiceProvider().ComputeHash(PWordBytes);
                foreach (byte part in tmp)
                {
                    HashedPWord = HashedPWord + part.ToString("X2");
                }
                System.FormattableString cmd = $"exec dbo.NewStudent {addstudentrequest.Email},{HashedPWord}";
                PAPACONTEXT.Database.ExecuteSql(cmd);
                return RedirectToAction("LogIn");
            }
        }

        public async Task<IActionResult> GoToPlanner()
        {
            HttpContext.Session.SetString("UID", "U1");
            HttpContext.Session.SetInt32("Index", 0);
            string UID = HttpContext.Session.GetString("UID");
            HttpContext.Session.SetInt32("Count", (await PAPACONTEXT.SemCount.Where(s => s.StudentId == UID).ToListAsync())[0].Semesters);
            return RedirectToAction("Degree");
        }
        public async Task<IActionResult> AdminLogIn()
        {
            HttpContext.Session.SetString("UID", "ADMIN");
            return RedirectToAction("LogIn");
        }
        public async Task<IActionResult> NextSem()
        {
            if(String.IsNullOrWhiteSpace(HttpContext.Session.GetInt32("Index").ToString()))
            {
                HttpContext.Session.SetInt32("Index", 0);
            }
            else
            {
                int index = Int32.Parse(HttpContext.Session.GetInt32("Index").ToString());
                index += 1;
                HttpContext.Session.SetInt32("Index", index);
            }

            return RedirectToAction("Degree");
        }
        public async Task<IActionResult> PrevSem()
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetInt32("Index").ToString()))
            {
                HttpContext.Session.SetInt32("Index", 0);
            }
            else
            {
                int index = Int32.Parse(HttpContext.Session.GetInt32("Index").ToString());
                if(index>0)
                {
                    index -= 1;
                }
                HttpContext.Session.SetInt32("Index", index);
            }
            return RedirectToAction("Degree");

        }
        public async Task<IActionResult> Degree()
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID"))||HttpContext.Session.GetString("UID").CompareTo("ADMIN")==0)
            {
                return RedirectToAction("LogIn");
            }
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetInt32("Index").ToString()))
            {
                HttpContext.Session.SetInt32("Index", 0);
            }
            string UID = HttpContext.Session.GetString("UID");
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetInt32("Count").ToString()))
            {
                HttpContext.Session.SetInt32("Count", (await PAPACONTEXT.SemCount.Where(s => s.StudentId == UID).ToListAsync())[0].Semesters);
            }
            SemPlanner newPlanner = new SemPlanner();
            newPlanner.Classes = await PAPACONTEXT.ListableClasses.OrderBy(s=>s.CourseCode).ToListAsync();
            newPlanner.index = Int32.Parse(HttpContext.Session.GetInt32("Index").ToString());
            newPlanner.count = Int32.Parse(HttpContext.Session.GetInt32("Count").ToString());
            newPlanner.Semesters = (await PAPACONTEXT.Semesters.Where(s => s.StudentId == UID).OrderBy(s => s.IdNum).ToListAsync())[newPlanner.index];
            if (newPlanner.index>newPlanner.count-1)
            {
                newPlanner.index = newPlanner.count-1;
                HttpContext.Session.SetInt32("Index", newPlanner.index);
            }
            else if(newPlanner.index<0)
            {
                newPlanner.index = 0;
                HttpContext.Session.SetInt32("Index", newPlanner.index);
            }
            newPlanner.Listed = await PAPACONTEXT.ListedView.Where(s => s.SemesterId == newPlanner.Semesters.SemesterId).ToListAsync();
            newPlanner.Requirements= await PAPACONTEXT.ReqChecklists.Where(s => s.StudentId == UID).OrderBy(s=>s.CourseCode).ToListAsync();
            //MySessionExtensions.Set<SemPlanner>(HttpContext.Session, "DegreeModel", newPlanner);
            return View("Degree",newPlanner);
        }


        public IActionResult Planner()
        {
            return View();
        }


        public IActionResult ListClass(string CID, string SID)
        {
            System.FormattableString cmd = $"exec dbo.ListClass {SID},{CID}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            return RedirectToAction("Degree");
        }

        public IActionResult UnlistClass(string LID)
        {
            System.FormattableString cmd = $"exec dbo.UnlistClassByListID {LID}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            return RedirectToAction("Degree");
        }

        public async Task<IActionResult> Checklist()
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") == 0)
            {
                return RedirectToAction("LogIn");
            }
            string UID = HttpContext.Session.GetString("UID");
            ChecklistModel Check = new ChecklistModel();
            Check.Classes = await PAPACONTEXT.ClassChecklists.Where(s => s.StudentId == UID).OrderBy(s => s.CourseCode).ToListAsync();
            return View("Checklist", Check);
        }
        public IActionResult CompleteClass(string SID, string CID)
        {
            System.FormattableString cmd = $"exec dbo.NewComp {SID},{CID}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            return RedirectToAction("Checklist");
        }
        public IActionResult IncompleteClass(string SID, string CID)
        {
            System.FormattableString cmd = $"exec dbo.IncompleteClassByStudentIDandClassID {SID},{CID}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            return RedirectToAction("Checklist");
        }


        public async Task<IActionResult> Settings()
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") == 0)
            {
                return RedirectToAction("LogIn");
            }
            string UID = HttpContext.Session.GetString("UID");
            int year = System.DateTime.Now.Year;
            Student thisStu = (await PAPACONTEXT.Students.Where(s => s.StudentId == UID).ToListAsync())[0];
            SettingsModel Setting = new SettingsModel();
            Setting.Years = YearsForSettings(year, 6);
            if(thisStu.GradYear!=null)
            {
                Setting.GradYear = thisStu.GradYear;
            }
            else
            {
                Setting.GradYear = "Not Set";
            }
            if(thisStu.DegreeId!=null)
            {
                Setting.CurrentDegree = (await PAPACONTEXT.DegreeLists.Where(s => s.DegreeId == thisStu.DegreeId).ToListAsync())[0].DegreeName;
            }
            else
            {
                Setting.CurrentDegree = "Not Set";
            }
            Setting.Degrees = await PAPACONTEXT.Degrees.OrderBy(s =>s.VersionYear).ThenBy(s => s.DegreeName).ToListAsync();
            return View("Settings", Setting);
        }
    
        public async Task<IActionResult> Summary()
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") == 0)
            {
                return RedirectToAction("LogIn");
            }
            string UID = HttpContext.Session.GetString("UID");
            Student ThisStudent = (await PAPACONTEXT.Students.Where(s => s.StudentId == UID).ToListAsync())[0];
            List<Semester> semesters=await PAPACONTEXT.Semesters.Where(s => s.StudentId == UID).OrderBy(s => s.IdNum).ToListAsync();
            List<ListedView> classes = new List<ListedView>();
            String CurrentDeg;
            if (ThisStudent.DegreeId != null)
            {
                CurrentDeg = (await PAPACONTEXT.DegreeLists.Where(s => s.DegreeId == ThisStudent.DegreeId).ToListAsync())[0].DegreeName;
            }
            else
            {
                CurrentDeg = "Not Set";
            }
            SummaryModel Model = new SummaryModel();
            System.FormattableString currentline;
            currentline=$"Degree: {CurrentDeg}";
            Model.result.Add(currentline.ToString());
            Model.format.Add(1);
            foreach (Semester sem in semesters)
            {
                currentline=$"{sem.Season} {sem.SemYear}";
                Model.result.Add(currentline.ToString());
                Model.format.Add(2);
                classes = await PAPACONTEXT.ListedView.Where(s => s.SemesterId == sem.SemesterId).ToListAsync();
                foreach(ListedView cla in classes)
                {
                    currentline=$"{cla.CourseCode} {cla.ClassName}";
                    Model.result.Add(currentline.ToString());
                    Model.format.Add(3);
                }
            }
            return View("Summary", Model);
        }

        public string[] YearsForSettings(int startYear,int AmountOfYears)
        {
            string[] years = new string[AmountOfYears];
            for (int i =0; i < AmountOfYears; i++)
            {
                years[i] = (startYear + i).ToString();
            }
            return years;
        }

        public IActionResult UpdateDegree(string DID)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") == 0)
            {
                return RedirectToAction("LogIn");
            }
            string UID = HttpContext.Session.GetString("UID");
            System.FormattableString cmd = $"exec dbo.ChangeStuDegree {UID},{DID}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            return RedirectToAction("Settings");
        }
        public async Task<IActionResult> UpdateGradYear(string Year)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") == 0)
            {
                return RedirectToAction("LogIn");
            }
            string UID = HttpContext.Session.GetString("UID");
            System.FormattableString cmd = $"exec dbo.UpdateGrad {UID},{Year}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            HttpContext.Session.SetInt32("Count", (await PAPACONTEXT.SemCount.Where(s => s.StudentId == UID).ToListAsync())[0].Semesters);
            HttpContext.Session.SetInt32("Index", 0);
            return RedirectToAction("Settings");
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View("SignUp");
        }
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
        //    return RedirectToAction("GoToPlanner");
        //}

        public async Task<IActionResult> AdminDegree()
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            AdminDegreeModel Degs = new AdminDegreeModel();
            Degs.Degrees=await PAPACONTEXT.Degrees.OrderBy(s => s.VersionYear).ThenBy(s => s.DegreeName).ToListAsync();
            return View("AdminDegree", Degs);
        }

        [HttpGet]
        public IActionResult CreateDegree()
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            DegreeForm values = new DegreeForm();
            values.DegreeName = "";
            values.VersionYear = "";
            return View("CreateDegrees", values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDegree(DegreeForm Values)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            if(YearViable(Values.VersionYear))
            {
                System.FormattableString cmd = $"exec dbo.NewDegree {Values.DegreeName},{Values.VersionYear}";
                PAPACONTEXT.Database.ExecuteSql(cmd);
                AdminReqModel reqs = new AdminReqModel();
                reqs.DegreeName = Values.DegreeName;
                reqs.VersionYear = Values.VersionYear;
                reqs.DegreeId = (await PAPACONTEXT.Degrees.Where(d => d.DegreeName == Values.DegreeName && d.VersionYear == Values.VersionYear).ToListAsync())[0].DegreeId;
                reqs.Classes = await PAPACONTEXT.RequiredForDegrees.Where(c=>c.DegreeId==reqs.DegreeId).OrderBy(c=>c.CourseCode).ToListAsync();
                return View("CreateReqs", reqs);
            }
            else
            {
                return View("CreateDegrees", Values);
            }
        }
        public bool YearViable(string year)
        {
            if(year.Length!=4)
            {
                return false;
            }
            if(int.TryParse(year,out int numYear))
            {
                //int upperYear = (System.DateTime.Now.Year);
                //int lowerYear = (System.DateTime.Now.Year) - 5;
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditDegree(string DID)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            Degree temp = (await PAPACONTEXT.Degrees.Where(d => d.DegreeId == DID).ToListAsync())[0];
            DegreeForm values = new DegreeForm();
            values.DegreeId = temp.DegreeId;
            values.DegreeName = temp.DegreeName;
            values.VersionYear = temp.VersionYear;
            return View("EditDegrees", values);
        }
        [HttpPost]
        public async Task<IActionResult> EditDegree(DegreeForm Values)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            if (YearViable(Values.VersionYear))
            {
                System.FormattableString cmd = $"exec dbo.ChangeDegree {Values.DegreeId} , {Values.DegreeName} , {Values.VersionYear}";
                PAPACONTEXT.Database.ExecuteSql(cmd);
                AdminReqModel reqs = new AdminReqModel();
                reqs.DegreeName = Values.DegreeName;
                reqs.VersionYear = Values.VersionYear;
                reqs.DegreeId = Values.DegreeId;
                reqs.Classes = await PAPACONTEXT.RequiredForDegrees.Where(c => c.DegreeId == reqs.DegreeId).OrderBy(c => c.CourseCode).ToListAsync();
                return View("CreateReqs", reqs);
            }
            else
            {
                return View("EditDegrees", Values);
            }
        }
        public RedirectToActionResult DeleteDegree(string DID)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            System.FormattableString cmd = $"exec dbo.DeleteDegree {DID}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            return RedirectToAction("AdminDegree");
        }
        public async Task<IActionResult> AddReq(string DID,string CID)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            System.FormattableString cmd = $"exec dbo.NewReq {DID},{CID}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            AdminReqModel reqs = new AdminReqModel();
            reqs.DegreeId = DID;
            reqs.Classes = await PAPACONTEXT.RequiredForDegrees.Where(c => c.DegreeId == reqs.DegreeId).OrderBy(c => c.CourseCode).ToListAsync();
            return View("CreateReqs", reqs);

        }

        public async Task<IActionResult> RemoveReq(string DID, string CID)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            System.FormattableString cmd = $"exec dbo.RemoveReqbyDegreeIDandClassID {DID},{CID}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            AdminReqModel reqs = new AdminReqModel();
            reqs.DegreeId = DID;
            reqs.Classes = await PAPACONTEXT.RequiredForDegrees.Where(c => c.DegreeId == reqs.DegreeId).OrderBy(c => c.CourseCode).ToListAsync();
            return View("CreateReqs", reqs);

        }
        public async Task<IActionResult> AdminClass()
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            AdminClassModel Cla = new AdminClassModel();
            Cla.Classes = await PAPACONTEXT.Classes.OrderBy(s => s.CourseCode).ToListAsync();
            return View("AdminClass", Cla);
        }

        [HttpGet]
        public IActionResult CreateClass()
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            ClassForm values = new ClassForm();
            values.Category = "";
            values.CourseCode = "";
            values.ClassName = "";
            values.Prerequisites = "";
            return View("CreateClasses", values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateClass(ClassForm Values)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            System.FormattableString cmd = $"exec dbo.NewClass {Values.CourseCode},{Values.ClassName},{Values.Category},{Values.Prerequisites},{Values.InFall},{Values.InSpring},{Values.InSummer},{Values.IsOffered}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            return RedirectToAction("AdminClass");
        }

        [HttpGet]
        public async Task<IActionResult> EditClass(string CID)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            Class temp = (await PAPACONTEXT.Classes.Where(d => d.ClassId == CID).ToListAsync())[0];
            ClassForm values = new ClassForm();
            values.ClassId = temp.ClassId;
            values.ClassName = temp.ClassName;
            values.CourseCode = temp.CourseCode;
            values.Category = temp.Category;
            values.Prerequisites = temp.Prerequisites;
            values.InFall = (bool)temp.InFall;
            values.InSpring = (bool)temp.InSpring;
            values.InSummer = (bool)temp.InSummer;
            values.IsOffered = (bool)temp.IsOffered;
            return View("EditClasses", values);
        }
        [HttpPost]
        public async Task<IActionResult> EditClass(ClassForm Values)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            System.FormattableString cmd = $"exec dbo.ChangeClass {Values.ClassId},{Values.CourseCode},{Values.ClassName},{Values.Category},{Values.Prerequisites},{Values.InFall},{Values.InSpring},{Values.InSummer},{Values.IsOffered}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            return RedirectToAction("AdminClass");
        }
        public RedirectToActionResult DeleteClass(string CID)
        {
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("UID")) || HttpContext.Session.GetString("UID").CompareTo("ADMIN") != 0)
            {
                return RedirectToAction("LogIn");
            }
            System.FormattableString cmd = $"exec dbo.DeleteClass {CID}";
            PAPACONTEXT.Database.ExecuteSql(cmd);
            return RedirectToAction("AdminClass");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}