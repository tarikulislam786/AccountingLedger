# How to run AccountingLedger App ?
1) git clone https://github.com/tarikulislam786/AccountingLedger.git
2) Open appsettings.json file from the AccountingLedger.WebApi project and then write your own sql sever name, username or password if it is SQL server authentication. If it is windows authentication then you dont need to write username/password, only put your server name.
![image](https://github.com/user-attachments/assets/df8115f3-9bab-48de-8606-aadc40667c18)

3) Then you run this below sql scripts under your database so that you get all the necessary tables and stored procedures ready. There are three tables and five stored procedures.
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

4) Run the api project keeping the startup project as AccountingLedger.WebApi
5) Then open the Postman and create chart of accounts using the post verb of http://localhost:5058/api/accounts endppints
![image](https://github.com/user-attachments/assets/a0dc0bfa-f9d7-4210-9eef-cc896d709276)
After adding some accounts you can check all the inserted accounts using this Get verbe of http://localhost:5058/api/accounts endpoints
![image](https://github.com/user-attachments/assets/186ba31b-8852-4957-9ac0-d6679917a1a0)

6) Then create JournaEntry using the post verb of http://localhost:5058/api/journalentries endpoint 
![image](https://github.com/user-attachments/assets/727fe209-6751-485c-b0a4-aa7a324f8f01)

While creating journal entry, you may check the fluent validation result as well. As the given snapshot demonstrates that the description field was empty and the debit and credit amount mismatch were calculated so it restricts to create journal entry.
![image](https://github.com/user-attachments/assets/59b829fd-f4f1-472c-baf4-3c5c522e6c40)

7) And finally, we can observe the trial balance accessing through Get verb of this endpoint http://localhost:5058/api/trialbalance 

![image](https://github.com/user-attachments/assets/17a7cfec-dadf-4d64-8dbe-6b41fbd27009)

That's all.

You may check the recorded video regarding this backend implementation if it requires some more clarification
https://mega.nz/file/2BJjmbZC#oGCE9ptIa4Kvnaqu6lp6e1rKin7AjMwqKvRxCCuumh4

For more information
Whatsapp: +8801926228731
mail: tarikulislam.cse@gmail.com
 


