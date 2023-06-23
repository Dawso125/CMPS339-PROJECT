USE CMPS_339_AmusementPark;

IF NOT EXISTS
	(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Parks]')AND type in (N'U'))

		BEGIN
			CREATE TABLE [dbo].[Parks]
			(
				Id INT PRIMARY KEY IDENTITY(1,1),
				[Name] VARCHAR(30) NOT NULL

			);
			PRINT('Parks table created')
		END
ELSE
	BEGIN
		PRINT('Park table exists.')
	END
IF NOT EXISTS
	(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Attractions]')AND type in (N'U'))

		BEGIN 
			CREATE TABLE [dbo].[Attractions]
			(
				Id INT PRIMARY KEY IDENTITY(1,1),
				ParkId INT,
				CONSTRAINT FK_Attractions_Parks 
					FOREIGN KEY (ParkId) REFERENCES Parks(Id) ON DELETE CASCADE
			);
		PRINT('Attractions table created.')
		END
ELSE
	BEGIN
		PRINT('Attractions table exists.')
	END
IF NOT EXISTS
	(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AttractionDetails]')AND type in (N'U'))

		BEGIN 
			CREATE TABLE [dbo].[AttractionDetails]
			(
				Id INT PRIMARY KEY IDENTITY(1,1),
				AttractionId INT,
				AttractionName varchar(30) NOT NULL,
				Capacity INT NOT NULL,
				OpenTime TIME NOT NULL,
				CloseTime TIME NOT NULL,
				MinimumAge INT,
				MinimumHeight INT,
				TicketPrice DECIMAL NOT NULL,
				CONSTRAINT FK_AttractionDetails_Attractions 
					FOREIGN KEY (AttractionId) REFERENCES Attractions(Id) ON DELETE CASCADE
			);
		PRINT('AttractionDetails table created.')
		END
ELSE
	BEGIN
		PRINT('AttractionDetails table exists.')
	END
IF NOT EXISTS
	(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tickets]')AND type in (N'U'))

		BEGIN 
			CREATE TABLE [dbo].[Tickets]
			(
				Id INT PRIMARY KEY IDENTITY(1,1),
				AttractionId INT,
				CONSTRAINT FK_Tickets_Attractions 
					FOREIGN KEY (AttractionId) REFERENCES Attractions(Id) ON DELETE CASCADE
			);
		PRINT('Tickets table created.')
		END
ELSE
	BEGIN
		PRINT('Tickets table exists.')
	END
IF NOT EXISTS
	(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TicketDetails]')AND type in (N'U'))

		BEGIN 
			CREATE TABLE [dbo].[TicketDetails]
			(
				Id INT PRIMARY KEY IDENTITY(1,1),
				TicketId INT,
				CONSTRAINT FK_TicketDetails_Tickets 
					FOREIGN KEY (TicketId) REFERENCES Tickets(Id) ON DELETE CASCADE
			);
		PRINT('TicketDetails table created.')
		END
ELSE
	BEGIN
		PRINT('TicketDetails table exists.')
	END
IF NOT EXISTS
	(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]')AND type in (N'U'))

		BEGIN 
			CREATE TABLE [dbo].[Users]
			(
				Id INT PRIMARY KEY IDENTITY(1,1),
				Username varchar not null,
				Passwords varchar not null,
				IsActive BIT,
			);
		PRINT('Users table created.')
		END
ELSE
	BEGIN
		PRINT('Users table exists.')
	END

IF NOT EXISTS
	(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserDetails]')AND type in (N'U'))
		BEGIN 
			CREATE TABLE [dbo].[UserDetails](
				Id INT PRIMARY KEY IDENTITY(1,1),
				Email VARCHAR not null,
				PhoneNumber VARCHAR(20) CONSTRAINT PhoneNumber_Format CHECK (PhoneNumber LIKE 'XXX-XXX-XXXX'),
				[Address] VARCHAR NOT NULL,
				UserId INT, 
					CONSTRAINT FK_UserDetails_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
				);
				PRINT('UserDetails Table is created.');
		END
ELSE 
	BEGIN 
		PRINT('UserDetails Table exists');
	END

IF NOT EXISTS
	(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Guests]')AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[Guests](
			Id INT PRIMARY KEY IDENTITY(1,1),
			UserId INT, 
				CONSTRAINT FK_Guests_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
			FirstName VARCHAR NOT NULL,
			LastName VARCHAR NOT NULL,
			MiddleInitial VARCHAR(2) NOT NULL,
			DateOfBirth DATE NOT NULL
			);
			PRINT('Guests table created.');
		END
ELSE
	BEGIN
		PRINT('Guests table exists');
	END

IF NOT EXISTS
	(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TicketGuests]')AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[TicketGuests](
			TicketId INT,
			GuestId INT,
			PRIMARY KEY (TicketId, GuestId),
			CONSTRAINT FK_TicketGuests_Tickets FOREIGN KEY (TicketId) REFERENCES Tickets(Id),
			CONSTRAINT FK_TicketGuests_Guests FOREIGN KEY (GuestId) REFERENCES Guests(Id)
			);
			PRINT ('TicketGuests table created.');
		END

ELSE 
	BEGIN
		PRINT('TicketGuests table exists')
	END
		

