-- Check if PasswordHash column exists, or add it if it's missing
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'px_PasswordHash' OR name = 'PasswordHash')
BEGIN
    ALTER TABLE Users ADD PasswordHash NVARCHAR(100) NULL;
END

-- Update the rows to ensure values are present
UPDATE Users SET PasswordHash = '123' WHERE Username = 'buyer';
UPDATE Users SET PasswordHash = '123' WHERE Username = 'admin';