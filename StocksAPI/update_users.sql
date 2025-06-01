SET QUOTED_IDENTIFIER ON;
GO

-- First, ensure we have an admin user
DECLARE @AdminId nvarchar(450);
SELECT TOP 1 @AdminId = Id FROM AspNetUsers WHERE Email = 'admin@stocksapi.com';

IF @AdminId IS NULL
BEGIN
    SET @AdminId = 'admin-' + CAST(NEWID() AS nvarchar(36));
    INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, 
        PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, 
        LockoutEnabled, AccessFailedCount, FirstName, LastName, CreatedAt, IsActive)
    VALUES (@AdminId, 'admin@stocksapi.com', 'ADMIN@STOCKSAPI.COM', 'admin@stocksapi.com', 
        'ADMIN@STOCKSAPI.COM', 1, 
        'AQAAAAEAACcQAAAAEKWz6tE+M+m4SxvDjb0u6Qz6qAMqvBKX6K83tnvzWqOAgqN5DxxQ+VN5NFh6WihPUA==',
        'SECURITYSTAMP', NEWID(), 0, 0, 1, 0, 'Admin', 'User', GETUTCDATE(), 1);

    -- Create Admin role if it doesn't exist
    IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Admin')
    BEGIN
        DECLARE @RoleId nvarchar(450) = NEWID();
        INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
        VALUES (@RoleId, 'Admin', 'ADMIN', NEWID());

        -- Assign admin user to Admin role
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        VALUES (@AdminId, @RoleId);
    END
    ELSE
    BEGIN
        -- Assign admin user to existing Admin role
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        SELECT @AdminId, Id FROM AspNetRoles WHERE Name = 'Admin';
    END
END

-- Update existing records to use the admin user ID
UPDATE Stocks SET UserId = @AdminId WHERE UserId IS NULL OR UserId = '';
UPDATE MonthlyInvestments SET UserId = @AdminId WHERE UserId IS NULL OR UserId = '';
GO
