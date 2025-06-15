CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE Users (
    Id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    Username TEXT UNIQUE NOT NULL,
    Email TEXT UNIQUE NOT NULL,
    PasswordHash TEXT NOT NULL,
    DisplayName TEXT UNIQUE NOT NULL,
    CreatedAt TIMESTAMP DEFAULT now(),
    IsActive BOOLEAN DEFAULT true
);

CREATE TABLE Categories (
    Id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    Name TEXT NOT NULL,
    Description TEXT
);

CREATE TABLE ExchangeRates (
    Id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    FromCurrency TEXT NOT NULL,
    ToCurrency TEXT NOT NULL,
    Rate NUMERIC(18,8) NOT NULL,
    EffectiveDate DATE NOT NULL
);

CREATE TABLE Transactions (
    Id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    Timestamp TIMESTAMP,
    Type TEXT CHECK (Type IN ('user-to-user','external-in','external-out','fee','exchange')),
    Galleons INT DEFAULT 0,
    Sickles INT DEFAULT 0,
    Knuts INT DEFAULT 0,
    DkkAmount NUMERIC(10,2),
    Description TEXT,
    FromUserId UUID REFERENCES Users(Id),
    ToUserId UUID REFERENCES Users(Id),
    ExternalParty TEXT,
    CategoryId UUID REFERENCES Categories(Id)
);

CREATE TABLE RecurringTransactions (
    Id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    FromUserId UUID NOT NULL REFERENCES Users(Id),
    ToUserId UUID REFERENCES Users(Id),
    ExternalParty TEXT,
    Description TEXT,
    AmountKnuts INT NOT NULL,
    Currency TEXT NOT NULL,
    Frequency TEXT NOT NULL,
    NextOccurrence DATE NOT NULL,
    IsActive BOOLEAN DEFAULT true,
    CreatedAt TIMESTAMP DEFAULT now()
);

CREATE TABLE Balances (
    Id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    UserId UUID NOT NULL REFERENCES Users(Id),
    Currency TEXT NOT NULL,
    Amount NUMERIC(18,4) NOT NULL,
    UpdatedAt TIMESTAMP NOT NULL DEFAULT now()
);

CREATE TABLE AuditLogs (
    Id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    UserId UUID REFERENCES Users(Id),
    Action TEXT,
    Entity TEXT,
    EntityId UUID,
    Timestamp TIMESTAMP,
    Metadata JSONB
);

CREATE TABLE BalanceSnapshots (
    Id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    UserId UUID NOT NULL REFERENCES Users(Id),
    Currency TEXT,
    Amount NUMERIC,
    RecordedAt TIMESTAMP
);
