USE StocksDb;

DECLARE @AdminId nvarchar(450);
SELECT TOP 1 @AdminId = Id FROM AspNetUsers WHERE Email = 'admin@stocksapi.com';

IF @AdminId IS NULL
BEGIN
    -- Insert admin user if not exists
    SET @AdminId = NEWID();
    INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FirstName, LastName, CreatedAt, IsActive)
    VALUES (@AdminId, 'admin@stocksapi.com', 'ADMIN@STOCKSAPI.COM', 'admin@stocksapi.com', 'ADMIN@STOCKSAPI.COM', 1, 'AQAAAAEAACcQAAAAEKWz6tE+M+m4SxvDjb0u6Qz6qAMqvBKX6K83tnvzWqOAgqN5DxxQ+VN5NFh6WihPUA==', 'SECURITYSTAMP', NEWID(), 0, 0, 1, 0, 'Admin', 'User', GETUTCDATE(), 1);
END

-- Update existing records in Stocks
UPDATE Stocks SET UserId = @AdminId WHERE UserId IS NULL OR UserId = '';

-- Update existing records in MonthlyInvestments
UPDATE MonthlyInvestments SET UserId = @AdminId WHERE UserId IS NULL OR UserId = '';
