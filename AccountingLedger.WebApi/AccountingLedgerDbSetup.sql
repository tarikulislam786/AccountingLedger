-- =========================================================
-- Table: Accounts
-- =========================================================

CREATE TABLE Accounts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Type NVARCHAR(50) NOT NULL
);

-- =========================================================
-- Table: JournalEntries
-- =========================================================
CREATE TABLE JournalEntries (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Date DATETIME NOT NULL,
    Description NVARCHAR(255) NOT NULL
);

-- =========================================================
-- Table: JournalEntryLines
-- =========================================================

CREATE TABLE JournalEntryLines (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    JournalEntryId INT NOT NULL,
    AccountId INT NOT NULL,
    Debit DECIMAL(18,2) NOT NULL,
    Credit DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (JournalEntryId) REFERENCES JournalEntries(Id),
    FOREIGN KEY (AccountId) REFERENCES Accounts(Id)
);


-- =========================================================
-- 6️⃣ Stored Procedure: CreateJournalEntry
-- =========================================================
CREATE TYPE JournalLineType AS TABLE
(
    AccountId INT,
    Debit DECIMAL(18,2),
    Credit DECIMAL(18,2)
);
GO

CREATE PROCEDURE sp_CreateJournalEntry
    @Date DATETIME,
    @Description NVARCHAR(255),
    @Lines JournalLineType READONLY
AS
BEGIN
    DECLARE @JournalEntryId INT;

    INSERT INTO JournalEntries (Date, Description) VALUES (@Date, @Description);
    SET @JournalEntryId = SCOPE_IDENTITY();

    INSERT INTO JournalEntryLines (JournalEntryId, AccountId, Debit, Credit)
    SELECT @JournalEntryId, AccountId, Debit, Credit FROM @Lines;

    SELECT @JournalEntryId AS NewJournalEntryId;
END
GO

-- =========================================================
-- 5️⃣ Stored Procedure: GetJournalEntries
-- =========================================================
CREATE PROCEDURE sp_GetJournalEntries
AS
BEGIN
    SELECT 
        je.Id AS JournalEntryId,
        je.Date,
        je.Description,
        jel.AccountId,
        a.Name AS AccountName,
        jel.Debit,
        jel.Credit
    FROM JournalEntries je
    INNER JOIN JournalEntryLines jel ON je.Id = jel.JournalEntryId
    INNER JOIN Accounts a ON jel.AccountId = a.Id
    ORDER BY je.Date, je.Id;
END
GO

-- =========================================================
-- 6️⃣ Stored Procedure: GetTrialBalance
-- =========================================================
CREATE PROCEDURE sp_GetTrialBalance
AS
BEGIN
    SELECT
        a.Id AS AccountId,
        a.Name AS AccountName,
        a.Type AS AccountType,
        SUM(jel.Debit) - SUM(jel.Credit) AS Balance
    FROM Accounts a
    LEFT JOIN JournalEntryLines jel ON a.Id = jel.AccountId
    GROUP BY a.Id, a.Name, a.Type
    ORDER BY a.Name;
END
GO



-- =========================================================
-- 6️⃣ Stored Procedure: CreateAccount
-- =========================================================
CREATE PROCEDURE sp_CreateAccount
  @Name VARCHAR(100),
  @Type VARCHAR(50)
AS
BEGIN
  INSERT INTO Accounts (Name, Type) VALUES (@Name, @Type);
END

-- =========================================================
-- 6️⃣ Stored Procedure: GetAccounts
-- =========================================================
CREATE PROCEDURE sp_GetAccounts
AS
BEGIN
  SELECT Id, Name, Type FROM Accounts;
END
