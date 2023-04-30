--Sections
--1.Creating entries with unique IDs
--2.Creating items
--3.Editing items
--4.Deleting items
--5.Viewing database

--1
--Creating new entries in database with Unique IDs
--Degree
create procedure dbo.NewDegree
	@Name varchar(50)
as
insert into dbo.Degree(Degree_ID,Degree_Name) values('NEW',@Name)
update Degree 
set Degree_ID=concat('D',ID_Num)
where Degree_ID='NEW'
go
--Class
create procedure dbo.NewClass
	@Code varchar(6),
	@Name varchar(50),
	@Category varchar(4),
	@Fall bit,
	@Spring bit,
	@Summer bit,
	@Offered bit
as
insert into dbo.Class(Class_ID,Course_Code,Class_Name,Category,InFall,InSpring,InSummer,IsOffered) values('NEW',@Code,@Name,@Category,@Fall,@Spring,@Summer,@Offered)
update Class 
set Class_ID=concat('C',ID_Num)
where Class_ID='NEW'
go
--Requirement
create procedure dbo.NewReq
	@DegID varchar(6),
	@ClaID varchar(6)
as
if((select count(*) from Requirement where Degree_ID=@DegID and Class_ID=@ClaID)=0)
begin
insert into dbo.Requirement(Req_ID,Degree_ID,Class_ID) values('NEW',@DegID,@ClaID)
update dbo.Requirement
set Req_ID=CONCAT('R',ID_Num)
where Req_ID='NEW'
end
go
--Student
create procedure dbo.NewStudent
	@Email varchar(30),
	@Password varchar(20)
as
if(@Email like '%@stmartin.edu' and @Password is not null)
begin
	insert into Student(Student_ID,Email,Student_Password) values('NEW',@Email,@Password)
	update Student
	set Student_ID=CONCAT('U',ID_Num)
	where Student_ID='NEW'
end
else
begin
	print 'Email is not from St. Martins or the password is null'
	return
end
go
--Semester
create procedure dbo.NewSem
	@StudentID varchar(6),
	@Season varchar(6),
	@Year char(4)
as
if((select count(*) from Student where Student_ID=@StudentID)=0 or @Season not in ('Fall','Spring','Summer'))
begin
	print 'Invalid parameters'
	return
end
insert into dbo.Semester(Semester_ID,Student_ID,Season,Sem_Year) values('NEW',@StudentID,@Season,@Year)
update dbo.Semester
set Semester_ID=CONCAT('S',ID_Num)
where Semester_ID='NEW'
go
--Listed Class
create procedure dbo.NewList
	@SemesterID varchar(6),
	@ClassID varchar(6)
as
if((select count(*) from ListedClass as L 
	inner join Semester as S on L.Semester_ID=S.Semester_ID 
	inner join Student as U on S.Student_ID=U.Student_ID 
	where Class_ID=@ClassID and U.Student_ID=(select Student_ID from Semester where Semester_ID=@SemesterID))=0)
begin
insert into dbo.ListedClass(List_ID,Semester_ID,Class_ID) values('NEW',@SemesterID,@ClassID)
update dbo.ListedClass
set List_ID=CONCAT('L',ID_Num)
where List_ID='NEW'
end
go
--Completed Class
create procedure dbo.NewComp
	@StudentID varchar(6),
	@ClassID varchar(6)
as
if((select count(*) from CompletedClass where Student_ID=@StudentID and Class_ID=@ClassID)=0)
begin
insert into dbo.CompletedClass(Complete_ID,Student_ID,Class_ID) values('NEW',@StudentID,@ClassID)
update dbo.CompletedClass
set Complete_ID=CONCAT('Q',ID_Num)
where Complete_ID='NEW'
end
go

--2
--Creating Items
--Auto-generate semesters for Student based on Grad Year
create procedure dbo.GenerateSemesters
	@Student varchar(6)
as
declare @GradChar char(4)=(select Grad_Year from Student where Student_ID=@Student)
declare @YearChar char(4)=cast(Year(getdate()) as char)
declare @GradInt int=cast(@GradChar as int)
declare @YearInt int=cast(@YearChar as int)
while @YearInt<=@GradInt
begin
	--Check if semester already exists before generating a new one
	if((select count(*) from Semester where Student_ID=@Student and Season='Spring' and Sem_Year=@YearChar)=0)
	begin
		exec dbo.NewSem @Student,'Spring',@YearChar
	end
	if((select count(*) from Semester where Student_ID=@Student and Season='Summer' and Sem_Year=@YearChar)=0)
	begin
		exec dbo.NewSem @Student,'Summer',@YearChar
	end
	if((select count(*) from Semester where Student_ID=@Student and Season='Fall' and Sem_Year=@YearChar)=0)
	begin
		exec dbo.NewSem @Student,'Fall',@YearChar
	end
	set @YearInt=@YearInt+1
	set @YearChar=CAST(@YearInt as char)
end
go
--List a class in a semester
create procedure dbo.ListClass
	@SemID varchar(6),
	@ClaID varchar(6)
as
exec dbo.NewList @SemID,@ClaID
declare @StuID varchar(6)=(select Student_ID from dbo.Semester where Semester_ID=@SemID)
--If class isn't already listed for completion, list it
if((select count(*) from dbo.CompletedClass where Student_ID=@StuID and Class_ID=@ClaID)=0)
begin
	exec dbo.NewComp @StuID,@ClaID
end
go

--3
--Editing Items
--Changing a class' fields
create procedure dbo.ChangeClass
	@ID varchar(6) =null,
	@Code varchar(6) =null,
	@Name varchar(50) =null,
	@Category varchar(4)=null,
	@Fall bit=null,
	@Spring bit=null,
	@Summer bit=null,
	@Offered bit=null
as
if @ID=null
begin
	print 'An ID is necessary'
	return
end
update Class 
set Course_Code=ISNULL(@Code,Course_Code),
	Class_Name=ISNULL(@Name,Class_Name),
	Category=ISNULL(@Category,Category),
	InFall=ISNULL(@Fall,InFall),
	InSpring=ISNULL(@Spring,InSpring),
	InSummer=ISNULL(@Summer,InSummer),
	IsOffered=ISNULL(@Offered,IsOffered)
where Class_ID=@ID
go
--Changing a Degree's fields
create procedure dbo.ChangeDegree
	@ID varchar(6) =null,
	@Name varchar(50) =null,
	@Year char(4)=null
as
if @ID=null
begin
	print 'An ID is necessary'
	return
end
update Degree 
set Degree_Name=ISNULL(@Name,Degree_Name),
	Version_Year=ISNULL(@Year,Version_Year)
where Degree_ID=@ID
go
--Changing a student's grad year
create procedure dbo.UpdateGrad
	@StuID varchar(6),
	@Year char(4)
as
if(@Year<year(GETDATE()))
begin
	print 'Invalid year. Must be after current year'
	return
end
else if(@Year=(select top 1 Grad_Year from Student where Student_ID=@StuID))
begin
	print 'Year is same as old year'
	return
end
declare @OldYear char(4)=(select top 1 Grad_Year from Student where Student_ID=@StuID)
update Student
set Grad_Year=@Year
where Student_ID=@StuID
if(@OldYear>@Year)
begin
	exec dbo.PruneSemesters @StuID
end
else if(@OldYear<@Year)
begin
	exec dbo.GenerateSemesters @StuID
end
go


--4
--Deleting Items
--Remove a requirement by their ID
create procedure dbo.RemoveReqbyReqID
	@ReqID varchar(6)
as
delete from dbo.Requirement
where Req_ID=@ReqID
go
--Remove Requirement by combo of degree and class IDs
create procedure dbo.RemoveReqbyDegreeIDandClassID
	@DegID varchar(6),
	@ClaID varchar(6)
as
delete from dbo.Requirement
where Degree_ID=@DegID and Class_ID=@ClaID
go
--Delete a degree
create procedure dbo.DeleteDegree
	@DegID varchar(6)
as
	--Need to drop any requirements that reference the degree first
delete from dbo.Requirement where Degree_ID=@DegID
	--If a student is using the degree, going to need to change it to null
update Student
set Degree_ID=null
where Degree_ID=@DegID
	--Now that references aren't an issue, delete degree
delete from dbo.Degree where Degree_ID=@DegID
go
--Unlist a class from Semester by ListID
create procedure dbo.UnlistClassByListID
	@ListID varchar(6)
as
	--Get StudentID for completed class query
declare @StuID varchar(6)=(
	select Student_ID 
	from dbo.Semester 
	where Semester_ID=(
		select Semester_ID 
		from dbo.ListedClass 
		where List_ID=@ListID))
--Delete class from completed classes
delete from dbo.CompletedClass where Student_ID=@StuID and Class_ID=(select Class_ID from dbo.ListedClass where List_ID=@ListID)
--Delete the listing
delete from dbo.ListedClass where List_ID=@ListID
go
--Unlist a Class by Semester and Class IDs
create procedure dbo.UnlistClassBySemesterIDandClassID
	@SemID varchar(6),
	@ClaID varchar(6)
as
	--Get StudentID for completed class query
declare @StuID varchar(6)=(
	select Student_ID 
	from dbo.Semester 
	where Semester_ID=@SemID)
	--Delete class from completed classes
delete from dbo.CompletedClass where Student_ID=@StuID and Class_ID=@ClaID
	--Delete the listing
delete from dbo.ListedClass where Semester_ID=@SemID and Class_ID=@ClaID
go
--Remove completed class by CompID
create procedure dbo.IncompleteClassByCompID
	@CompID varchar(6)
as
declare @StuID varchar(6)=(
	select Student_ID 
	from dbo.CompletedClass 
	where Complete_ID=@CompID)
declare @ClaID varchar(6)=(
	select Class_ID 
	from dbo.CompletedClass 
	where Complete_ID=@CompID)
delete from dbo.ListedClass where List_ID=(
	select List_ID 
	from dbo.ListedClass as c
	where Class_ID=@ClaID 
	and Semester_ID=(
	select Semester_ID 
	from dbo.Semester as s
	where Student_ID=@StuID and s.Semester_ID=c.Semester_ID))
delete from dbo.CompletedClass where Complete_ID=@CompID
go
--Remove completed class by Student and Class IDs
create procedure dbo.IncompleteClassByStudentIDandClassID
	@StuID varchar(6),
	@ClaID varchar(6)
as
delete from dbo.ListedClass where List_ID=(
	select List_ID 
	from dbo.ListedClass as c
	where Class_ID=@ClaID 
	and Semester_ID=(
	select Semester_ID 
	from dbo.Semester as s
	where Student_ID=@StuID and s.Semester_ID=c.Semester_ID))
delete from dbo.CompletedClass where Student_ID=@StuID and Class_ID=@ClaID
go
--Delete a semester
create procedure DeleteSem
	@SemID varchar(6)
as
	--If there are listed classes in the semester, go through the unlisting process
if((select count(*) from dbo.ListedClass where Semester_ID=@SemID)>0)
begin
	declare @ListedID varchar(6)
	while((select count(*) from dbo.ListedClass where Semester_ID=@SemID)>0)
	begin
		set @ListedID=(select top (1) List_ID from dbo.ListedClass where Semester_ID=@SemID)
		exec UnlistClassByListID @ListID=@ListedID
	end
	delete from dbo.Semester where Semester_ID=@SemID
end
else
begin
	delete from dbo.Semester where Semester_ID=@SemID
end
go
--Prune semesters by GradYear
create procedure dbo.PruneSemesters
	@Student varchar(6)
as
declare @GradChar char(4)=(select Grad_Year from Student where Student_ID=@Student)
declare @YearChar char(4)=(select top 1 Sem_Year from Semester where Student_ID=@Student order by Sem_Year desc)
declare @GradInt int=cast(@GradChar as int)
declare @YearInt int=cast(@YearChar as int)
declare @Semester varchar(6)
while @YearInt>@GradInt
begin
	--Check if semester exists to delete it
	if((select count(*) from Semester where Student_ID=@Student and Season='Spring' and Sem_Year=@YearChar)>0)
	begin
		set @Semester=(select top 1 Semester_ID from Semester where Student_ID=@Student and Season='Spring' and Sem_Year=@YearChar)
		exec dbo.DeleteSem @Semester
	end
	if((select count(*) from Semester where Student_ID=@Student and Season='Summer' and Sem_Year=@YearChar)>0)
	begin
		set @Semester=(select top 1 Semester_ID from Semester where Student_ID=@Student and Season='Summer' and Sem_Year=@YearChar)
		exec dbo.DeleteSem @Semester
	end
	if((select count(*) from Semester where Student_ID=@Student and Season='Fall' and Sem_Year=@YearChar)>0)
	begin
		set @Semester=(select top 1 Semester_ID from Semester where Student_ID=@Student and Season='Fall' and Sem_Year=@YearChar)
		exec dbo.DeleteSem @Semester
	end
	set @YearInt=@YearInt-1
	set @YearChar=CAST(@YearInt as char)
end
go
--Remove a class
create procedure dbo.DeleteClass
	@ClassID varchar(6)
as
	--Need to drop any references to the class in ListedClass, Requirement, and CompletedClass
delete from dbo.ListedClass where Class_ID=@ClassID
delete from dbo.Requirement where Class_ID=@ClassID
delete from dbo.CompletedClass where Class_ID=@ClassID
	--Can now delete class with no references pointing to it
delete from dbo.Class where Class_ID=@ClassID
go
--Drop a student
create procedure dbo.DropStudent
	@StuID varchar(6)
as
	--Need to delete all semesters referring to student, this will remove all student's listed classes and some completed classes as well
declare @SemesterID varchar(6)
while((select count(*) from dbo.Semester where Student_ID=@StuID)>0)
begin
set @SemesterID=(select top 1 Semester_ID from dbo.Semester where Student_ID=@StuID)
exec dbo.DeleteSem @SemesterID
end
	--Remove remaining completed classes
delete from dbo.CompletedClass where Student_ID=@StuID
go
--Prune students who graduated
create procedure dbo.PruneGraduatedStudents
as
	--Check if there are students whose graduation year has remained as a previous year (they likely graduated)
if((select count(*) from dbo.Student where Grad_Year<year(getdate()))>0)
begin
	declare @StudentID varchar(6)
	--Go through the students and delete them
	while((select count(*) from dbo.Student where Grad_Year<year(getdate()))>0)
	begin
		set @StudentID=(select top 1 Student_ID from dbo.Student where Grad_Year<year(getdate()))
		exec dbo.DropStudent @StudentID
	end
end
go


--5
--Viewing Database
--Function for readability of seasons
create function dbo.SeasonOffered(@ClaID varchar(6),@Season varchar(6))
returns char(1)
as
begin
	declare @Value char(1)='N'
	if(@Season='Spring' and (select InSpring from dbo.Class where Class_ID=@ClaID)!=0)
	begin
		set @Value='Y'
	end
	else if(@Season='Summer' and (select InSummer from dbo.Class where Class_ID=@ClaID)!=0)
	begin
		set @Value='Y'
	end
	else if(@Season='Fall' and (select InFall from dbo.Class where Class_ID=@ClaID)!=0)
	begin
		set @Value='Y'
	end
	return(@Value)
end
go
--View for displaying classes
create view dbo.ListableClasses
as
select Category,Course_Code, Class_Name, dbo.SeasonOffered(Class_ID,'Spring') as [Spring],dbo.SeasonOffered(Class_ID,'Summer') as [Summer],dbo.SeasonOffered(Class_ID,'Fall') as [Fall] 
from dbo.Class 
where IsOffered!=0
go
--Determine if class is completed or not
create function dbo.IsComplete(@StuID varchar(6),@ClaID varchar(6))
returns char(1)
as
begin
	declare @Value char(1)='N'
	if((select count(*) from dbo.CompletedClass where Student_ID=@StuID and Class_ID=@ClaID)!=0)
	begin
		set @Value='Y'
	end
	return(@Value)
end
go
--View for the Requirement Checklist
create view Req_Checklist
as
select s.Student_ID,c.Category,c.Course_Code,(dbo.IsComplete(s.Student_ID,r.Class_ID))as [Complete] 
from dbo.Student as s 
inner join dbo.Degree as d on s.Degree_ID=d.Degree_ID 
inner join dbo.Requirement as r on d.Degree_ID=r.Degree_ID 
inner join dbo.Class as c on r.Class_ID=c.Class_ID
go
--Create procedure to run this view (just in case)
create procedure dbo.Checklist
	@Student varchar(6)
as
select Course_Code,Complete from Req_Checklist where Student_ID=@Student order by Category,Course_Code
go
--Create a view for the Degree names with their version year for the dropdown
create view dbo.DegreeList
as
select concat(Version_Year,' ',Degree_Name) as [Degree Name] from dbo.Degree
go
--This is more for just checking data, but a procedure to view all tables
create procedure dbo.ViewAll
as
select * from Student order by ID_Num
select * from Semester order by ID_Num
select * from ListedClass order by ID_Num
select * from Degree order by ID_Num
select * from Requirement order by ID_Num
select * from CompletedClass order by ID_Num
select * from Class order by ID_Num
go