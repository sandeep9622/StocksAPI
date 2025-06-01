USE StocksDb;
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;

DECLARE @AdminId nvarchar(450);

-- Check if admin user exists
SELECT TOP 1 @AdminId = Id FROM AspNetUsers WHERE Email = 'admin@stocksapi.com';

-- Create admin if doesn't exist
IF @AdminId IS NULL
BEGIN
    SET @AdminId = 'admin-' + CAST(NEWID() AS nvarchar(36));

    -- Create Admin role if it doesn't exist
    IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Admin')
    BEGIN
        INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
        VALUES (NEWID(), 'Admin', 'ADMIN', NEWID());
    END;

    -- Insert admin user
    INSERT INTO AspNetUsers 
        (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, 
        PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, 
        TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FirstName, LastName, 
        CreatedAt, IsActive)
    VALUES 
        (@AdminId, 'admin@stocksapi.com', 'ADMIN@STOCKSAPI.COM', 'admin@stocksapi.com', 
        'ADMIN@STOCKSAPI.COM', 1, 
        'AQAAAAEAACcQAAAAEKWz6tE+M+m4SxvDjb0u6Qz6qAMqvBKX6K83tnvzWqOAgqN5DxxQ+VN5NFh6WihPUA==',
        NEWID(), NEWID(), 0, 0, 1, 0, 'Admin', 'User', GETUTCDATE(), 1);

    -- Assign admin role to user
    INSERT INTO AspNetUserRoles (UserId, RoleId)
    SELECT @AdminId, Id FROM AspNetRoles WHERE Name = 'Admin';
END;

-- Update existing records
UPDATE Stocks SET UserId = @AdminId WHERE UserId = '';
UPDATE MonthlyInvestments SET UserId = @AdminId WHERE UserId = '';

PRINT 'Admin user ID: ' + @AdminId;
PRINT 'Updates completed successfully.';
