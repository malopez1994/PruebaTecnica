Create Database PruebaTecnica
go
use PruebaTecnica
go
/* Se crea la tabla, el indice y el procedimiento almacenado de persona que son los usuarios */
Create Table Persona(
Id_Persona int identity(1,1) primary key,
Nombre_Persona varchar(100) not null,
FechaNacimiento_Persona datetime not null,
Genero_Persona char(1) not null
)
go
Create Index ixNombrePersona ON Persona(Nombre_Persona);
go

Create or Alter Procedure SP_CRUD_PERSONA
@Tipo int = 0,
@IdPersona int = 0,
@NombrePersona varchar(100) ='',
@FechaNacimientoPersona datetime,
@Genero char(1) = ''
as
begin
if @Tipo = 1
begin
insert into Persona(Nombre_Persona,FechaNacimiento_Persona,Genero_Persona) values(@NombrePersona,@FechaNacimientoPersona,@Genero)
select SCOPE_IDENTITY();
end
else if  @Tipo = 2
begin
update Persona set Nombre_Persona = @NombrePersona,FechaNacimiento_Persona = @FechaNacimientoPersona, Genero_Persona = @Genero 
where Id_Persona =  @IdPersona
end
else if  @Tipo = 3
begin
delete from Persona where Id_Persona =  @IdPersona
end
else if  @Tipo = 4
begin
select * from Persona where Id_Persona =  @IdPersona
end
else
begin 
select * from Persona
end
end
GO

/* Se crea la tablay el procedimiento almacenado de log de usuarios */
Create Table LogPersona(
Id_Log int identity(1,1) primary key,
Id_Persona int not null,
Metodo varchar(15) not null,
Respuesta_Log varchar(25) not null,
Descripcion_Log varchar(200) not null,
FechaRegistro_Log datetime not null
)
go

Create or Alter Procedure SP_LOG_PERSONA
@Tipo int = 0,
@IdPersona int =0,
@Metodo varchar(15)='',
@Respuesta_Log varchar(25)='',
@Descripcion_Log varchar(200) = ''
as
begin
if @Tipo = 1
begin
insert into LOGPERSONA(Id_Persona,Metodo,Respuesta_Log,Descripcion_Log,FechaRegistro_Log)
values(@IdPersona,@Metodo,@Respuesta_Log,@Descripcion_Log,GETDATE())
end
else
begin 
select * from LOGPERSONA
end
end
GO