use PAPA_DB1
go

alter table dbo.Degree
add ID_Num int not null identity(1,1)
go

alter table dbo.Degree
alter column Degree_ID varchar(6) default concat('A',cast(ID_Num as varchar))
go

create table dbo.Degree(
ID_Num int identity(1,1),
Degree_ID varchar(6) constraint DegreePK Primary Key ,
Degree_Name varchar(50) Not Null
);
go

create procedure dbo.NewDegree
	@Name varchar(50)
as
insert into dbo.Degree(Degree_ID,Degree_Name) values(substring(@Name,1,2),@Name)
update Degree 
set Degree_ID=concat('D',cast((select ID_Num from Degree where Degree_ID=substring(@Name,1,2)) as varchar))
where Degree_ID=substring(@Name,1,2)
go

alter procedure dbo.NewDegree
	@Name varchar(50)
as
insert into dbo.Degree(Degree_ID,Degree_Name) values(substring(@Name,1,2),@Name)
update Degree 
set Degree_ID=concat('D',cast((select ID_Num from Degree where Degree_ID=substring(@Name,1,2)) as varchar))
where Degree_ID=substring(@Name,1,2)
go

--insert into dbo.Degree(Degree_ID,Degree_Name) values('D1','Example Degree')

exec dbo.NewDegree 'Example Degree'
go

select * from Degree
go

exec dbo.NewDegree 'Bachelor''s for Computer Science'
go

update Degree
set Degree_ID='D2'
where Degree_ID='Ba'

create procedure dbo.NewClass
	@Code varchar(6),
	@Name varchar(50),
	@Category varchar(4),
	@Fall bit,
	@Spring bit,
	@Summer bit,
	@Offered bit
as
insert into dbo.Class(Class_ID,Course_Code,Class_Name,Category,InFall,InSpring,InSummer,IsOffered) values(substring(@Name,1,2),@Code,@Name,@Category,@Fall,@Spring,@Summer,@Offered)
update Class 
set Class_ID=concat('C',cast((select ID_Num from Class where Class_ID=substring(@Name,1,2)) as varchar))
where Class_ID=substring(@Name,1,2)
go

exec dbo.NewClass 'CSC340', 'Intro to Algorithms','CSC',0,0,0,1
go

--See InClassPractice6 for if statements in procedures
alter procedure dbo.ChangeClass
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

exec ChangeClass @ID='C2',@Fall=1

select * from Class
select * from Degree

create procedure dbo.NewReq
	@DegID varchar(6),
	@ClaID varchar(6)
as
insert into dbo.Requirement(Req_ID,Degree_ID,Class_ID) values('DC',@DegID,@ClaID)
update dbo.Requirement
set Req_ID=CONCAT('R',ID_Num)
where Req_ID='DC'
go

exec dbo.NewReq @DegID='D4', @ClaID='C2'
go

select * from dbo.Requirement as R inner join dbo.Degree as D on R.Degree_ID=D.Degree_ID inner join dbo.Class as C on R.Class_ID=C.Class_ID

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

exec NewStudent 'Spencer.Bergman@stmartin.edu','ExamplePassword'
go


