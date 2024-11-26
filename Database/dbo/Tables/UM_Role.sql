CREATE TABLE [dbo].[UM_Role] (
    [RoleId]      INT           IDENTITY (1, 1) NOT NULL,
    [Code]        VARCHAR (20)  NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (200) NULL,
    [IsActive]    BIT           CONSTRAINT [DF_UM_Role_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate] DATETIME      NOT NULL,
    [CreatedBy]   INT           NOT NULL,
    CONSTRAINT [PK_UM_Role] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);




GO
-- Create trigger to capture changes in UM_Role table
CREATE TRIGGER trg_UM_Role_Changes
ON dbo.UM_Role
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Variables to store relevant information
    DECLARE @ChangeID INT;
    DECLARE @Operation VARCHAR(10);
    DECLARE @UserID VARCHAR(128);

    -- Determine operation type
    IF EXISTS (SELECT 1 FROM inserted)
    BEGIN
        IF EXISTS (SELECT 1 FROM deleted)
        BEGIN
            SET @Operation = 'UPDATE'; -- Update operation
        END
        ELSE
        BEGIN
            SET @Operation = 'INSERT'; -- Insert operation
        END
    END
    ELSE
    BEGIN
        SET @Operation = 'DELETE'; -- Delete operation
    END

    -- Assuming you have a way to get UserID in your application
    SET @UserID = SUSER_NAME(); -- Replace with actual logic to get UserID
	
    -- Insert into CT_UM_Role with basic information
    INSERT INTO CT_UM_Role (Timestamp, Operation, UserID)
    VALUES (GETDATE(), @Operation, @UserID);

    -- Get the ChangeID of the inserted row
    SET @ChangeID = SCOPE_IDENTITY();

    -- Insert into CT_UM_Role_Details based on operation type
    IF @Operation = 'UPDATE'
    BEGIN
        INSERT INTO CT_UM_Role_Details (ChangeID, FieldName, OldValue, NewValue)
        SELECT @ChangeID, 'Code', deleted.Code, inserted.Code
        FROM inserted
        JOIN deleted ON inserted.Code = deleted.Code
        WHERE inserted.Code <> deleted.Code;

        INSERT INTO CT_UM_Role_Details (ChangeID, FieldName, OldValue, NewValue)
        SELECT @ChangeID, 'Name', deleted.Name, inserted.Name
        FROM inserted
        JOIN deleted ON inserted.Code = deleted.Code
        WHERE inserted.Name <> deleted.Name;

        INSERT INTO CT_UM_Role_Details (ChangeID, FieldName, OldValue, NewValue)
        SELECT @ChangeID, 'Description', deleted.Description, inserted.Description
        FROM inserted
        JOIN deleted ON inserted.Code = deleted.Code
        WHERE ISNULL(inserted.Description, '') <> ISNULL(deleted.Description, '');
    END
    ELSE IF @Operation = 'DELETE'
    BEGIN
        INSERT INTO CT_UM_Role_Details (ChangeID, FieldName, OldValue, NewValue)
        SELECT @ChangeID, 'Code', deleted.Code, NULL
        FROM deleted;

        INSERT INTO CT_UM_Role_Details (ChangeID, FieldName, OldValue, NewValue)
        SELECT @ChangeID, 'Name', deleted.Name, NULL
        FROM deleted;

        INSERT INTO CT_UM_Role_Details (ChangeID, FieldName, OldValue, NewValue)
        SELECT @ChangeID, 'Description', deleted.Description, NULL
        FROM deleted;
    END
    ELSE IF @Operation = 'INSERT'
    BEGIN
        INSERT INTO CT_UM_Role_Details (ChangeID, FieldName, OldValue, NewValue)
        SELECT @ChangeID, 'Code', NULL, inserted.Code
        FROM inserted;

        INSERT INTO CT_UM_Role_Details (ChangeID, FieldName, OldValue, NewValue)
        SELECT @ChangeID, 'Name', NULL, inserted.Name
        FROM inserted;

        INSERT INTO CT_UM_Role_Details (ChangeID, FieldName, OldValue, NewValue)
        SELECT @ChangeID, 'Description', NULL, inserted.Description
        FROM inserted;
    END
END;