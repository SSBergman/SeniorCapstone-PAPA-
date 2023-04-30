--Create database PAPA_DB1
--go

use PAPA_DB1
go

--Drop section to empty all data and reset identities
drop table dbo.CompletedClass
drop table dbo.ListedClass
drop table dbo.Requirement
drop table dbo.Semester
drop table dbo.Student
drop table dbo.Degree
drop table dbo.Class
go


--Create section
create table dbo.Degree(
ID_Num int identity(1,1),
Degree_ID varchar(6) constraint DegreePK Primary Key ,
Degree_Name varchar(50) Not Null,
Version_Year char(4) Not Null default year(getdate())
);
go

--drop table Degree
--go

create table dbo.Class(
ID_Num int identity(1,1),
Class_ID varchar(6) constraint ClassPK Primary Key,
Course_Code varchar(6) not null,
Class_Name varchar(50) not null,
Category varchar(4) not null,
InFall bit default 'FALSE' not null,
InSpring bit default 'FALSE' not null,
InSummer bit default 'FALSE' not null,
IsOffered bit default 'TRUE' not null
);
go

--drop table Class
--go

create table dbo.Requirement(
ID_Num int identity(1,1),
Req_ID varchar(6) constraint ReqPK Primary Key,
Degree_ID varchar(6) not null constraint ReqDegFK foreign key references Degree(Degree_ID),
Class_ID varchar(6) not null constraint ReqClaFK foreign key references Class(Class_ID)
);
go

--drop table Requirement
--go


--Tentative, gonna need more columns and encryption later
create table dbo.Student(
ID_Num int identity(1,1),
Student_ID varchar(6) not null constraint StudentPK Primary Key,
Degree_ID varchar(6) null constraint StuDegFK foreign key references Degree(Degree_ID),
Email varchar(30) not null,
Student_Password varchar(20) not null,
Grad_Year char(4) null default year(getdate())
)

--drop table Student
--go

create table dbo.Semester(
ID_Num int identity(1,1),
Semester_ID varchar(6) not null constraint SemesterPK Primary Key,
Student_ID varchar(6) null constraint SemStuFK foreign key references Student(Student_ID),
Season varchar(6) not null,
constraint chk_Season check (Season in ('Fall','Spring','Summer')),
Sem_Year char(4) not null
)
go

--drop table Semester
--go

create table dbo.ListedClass(
ID_Num int identity(1,1),
List_ID varchar(6) not null constraint ListPK Primary Key,
Semester_ID varchar(6) null constraint ListSemFK foreign key references Semester(Semester_ID),
Class_ID varchar(6) null constraint ListClassFK foreign key references Class(Class_ID),
)
go

--drop table ListedClass
--go

create table dbo.CompletedClass(
ID_Num int identity(1,1),
Complete_ID varchar(6) not null constraint CompPK Primary Key,
Student_ID varchar(6) null constraint CompStuFK foreign key references Student(Student_ID),
Class_ID varchar(6) null constraint CompClassFK foreign key references Class(Class_ID),
)
go

--drop table CompletedClass
--go