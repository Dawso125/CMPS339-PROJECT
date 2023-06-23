IF EXISTS (SELECT * FROM sys.databases WHERE name = 'CMPS_339_AmusementPark')
BEGIN
   
    PRINT ('Database already exists.');
END
ELSE
BEGIN
   
    CREATE DATABASE CMPS_339_AmusementPark;
    PRINT ('Database Created.');
END