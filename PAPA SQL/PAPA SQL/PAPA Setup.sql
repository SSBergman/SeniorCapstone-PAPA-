Create database PAPA_DB1
go

use PAPA_DB1
go

create table dbo.Degree(
ID_Num int identity(1,1),
Degree_ID varchar(6) constraint DegreePK Primary Key ,
Degree_Name varchar(50) Not Null
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
Grad_Year char(4) null
)

--drop table Student
--go


