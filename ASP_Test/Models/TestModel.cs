namespace ASP_Test.Models
{
    public partial class TestModel
    {
        public string CourseCode { get; set; }

        public string SemesterID { get; set; }

        public string SemesterName { get; set; }

        public int index { get; set; }
        public TestModel(string courseCode, string semesterID, string semesterName, int index)
        {
            CourseCode = courseCode;
            SemesterID = semesterID;
            SemesterName = semesterName;
            this.index = index;
        }
    }
}
