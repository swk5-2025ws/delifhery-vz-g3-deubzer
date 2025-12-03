
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = N'delifhery_db')
BEGIN
	PRINT 'Database delifhery_db already exists';
END
ELSE
BEGIN
	EXEC('CREATE DATABASE [delifhery_db]');
	PRINT 'Database delifhery_db created';
	
	EXEC('
		USE [delifhery_db];
		CREATE TABLE [dbo].[Customer](
			[customer_id] UNIQUEIDENTIFIER NOT NULL 
				PRIMARY KEY DEFAULT NEWID(),
			[identity_provider_user_id] VARCHAR(255) NOT NULL,
			[username] VARCHAR(30) NOT NULL,
			[created_at] DATETIME NOT NULL DEFAULT(GETDATE())
		);
	');
	PRINT 'Table Customer created';

	EXEC('
		USE [delifhery_db];
		CREATE TABLE [dbo].[Address](
			[address_id] [int] IDENTITY(1,1) PRIMARY KEY,
			[name] [varchar](100) NOT NULL,
			[street] [varchar](100) NOT NULL,
			[house_number] [varchar](10) NOT NULL,
			[postal_code] [varchar](20) NOT NULL,
			[city] [varchar](50) NOT NULL 
		);
	
	');
	PRINT 'Table Address created';

	EXEC('
		USE[delifhery_db];
		CREATE TABLE [dbo].[ContactMethod](
			[contact_id] [int] IDENTITY(1,1) PRIMARY KEY,
			[customer_id] UNIQUEIDENTIFIER  NOT NULL,
			[type] [varchar](30) NOT NULL,
			[value] [varchar](100) NOT NULL,
			[is_verified] [bit] DEFAULT(0),

			CONSTRAINT FK_ContactMethod_Customer FOREIGN KEY([customer_id]) REFERENCES [dbo].[Customer]([customer_id]) ON DELETE CASCADE
		);
	');
	PRINT 'Table CustomerContant created';

	EXEC('
		USE [delifhery_db];
		CREATE TABLE [dbo].[Shipment](
			[shipment_id] [int] IDENTITY(1,1) PRIMARY KEY,
			[sender_customer_id] UNIQUEIDENTIFIER  NOT NULL,
			[sender_address_id] [int] NOT NULL,
			[recipient_address_id] [int] NOT NULL,
			[tracking_number] [varchar](50) NOT NULL UNIQUE,
			[weight_kg] [float] NULL,
			[height_cm] [float] NULL,
			[width_cm] [float] NULL,
			[length_cm] [float] NULL,
			[current_status] [varchar](50) NOT NULL,
			[created_at] [datetime] NOT NULL DEFAULT(GETDATE()),

			CONSTRAINT FK_Shipment_SenderCustomer FOREIGN KEY ([sender_customer_id]) REFERENCES [dbo].[Customer]([customer_id]) ON DELETE CASCADE,
			CONSTRAINT FK_Shipment_SenderAddress FOREIGN KEY ([sender_address_id]) REFERENCES [dbo].[Address]([address_id]),
			CONSTRAINT FK_Shipment_RecipientAddress FOREIGN KEY ([recipient_address_id]) REFERENCES [dbo].[Address]([address_id])

		);
	');
	PRINT 'Table Shipment created';

	EXEC('
		USE [delifhery_db];
		CREATE TABLE [dbo].[TrackingEvent](
			[tracking_event_id] [int] IDENTITY(1,1) PRIMARY KEY,
			[shipment_id] [int] NOT NULL,
			[status] [varchar](50) NOT NULL,
			[location] [varchar](100),
			[note] [varchar](255),
			[occurred_at] [datetime] NOT NULL,

			CONSTRAINT FK_TrackingEvent_Shipment FOREIGN KEY ([shipment_id]) REFERENCES [dbo].[Shipment]([shipment_id]) ON DELETE CASCADE
		);
	');
	PRINT 'Table TrackingEvent  created';

	EXEC('
		USE [delifhery_db];
		CREATE TABLE [dbo].[ShipmentPrice](
			[price_id] [int] IDENTITY(1,1) PRIMARY KEY,
			[shipment_id] [int] NOT NULL,
			[amount] [float] NOT NULL,
			[currency] [varchar] (10) NOT NULL,
			[calculated_at] [datetime] NOT NULL DEFAULT GETDATE(),

			CONSTRAINT FK_ShipmentPrice_Shipment FOREIGN KEY ([shipment_id]) REFERENCES [dbo].[Shipment]([shipment_id]) ON DELETE CASCADE
		);
	');
	PRINT 'Table ShipmentPrice created';

	EXEC('
		USE [delifhery_db];
		CREATE TABLE [dbo].[Payment](
			[payment_id] [int] IDENTITY(1,1) PRIMARY KEY,
			[shipment_id] [int] NOT NULL,
			[external_payment_id] [varchar](100),
			[amount] [float],
			[currency] [varchar](10),
			[status] [varchar](30),
			[callback_url] [varchar](255),
			[redirect_url] [varchar](255),
			[created_at] [datetime],
			[completed_at] [datetime],

			CONSTRAINT FK_Payment_Shipment FOREIGN KEY ([shipment_id]) REFERENCES [dbo].[Shipment]([shipment_id]) ON DELETE CASCADE
		);
	');
	PRINT 'Table Payment created';
	EXEC('
		USE [delifhery_db];
		CREATE TABLE [dbo].[Carrier](
			[carrier_id] [int] IDENTITY(1,1) PRIMARY KEY,
			[api_key] [varchar](255),
			[name] [varchar](100) NOT NULL,
			[is_active] [bit] DEFAULT(0)
		);
	');
	PRINT 'Table Carrier created';

	EXEC('
		USE [delifhery_db];
		CREATE TABLE [dbo].[NotificationSubscription](
			[notification_id] [int] IDENTITY(1,1) PRIMARY KEY,
			[shipment_id] [int] NOT NULL,
			[customer_id] UNIQUEIDENTIFIER  NOT NULL,
			[created_at] [datetime] NOT NULL DEFAULT GETDATE(),

			CONSTRAINT FK_NotificationSubscription_Shipment FOREIGN KEY ([shipment_id]) REFERENCES [dbo].[Shipment]([shipment_id]) ON DELETE CASCADE,
			CONSTRAINT FK_NotificationSubscription_Customer FOREIGN KEY ([customer_id]) REFERENCES [dbo].[Customer]([customer_id])
		);
	');
	PRINT 'Table NotificationSubscription created';

END