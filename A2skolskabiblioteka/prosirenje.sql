CREATE TABLE Primerak(
	PrimerakID INT NOT NULL PRIMARY KEY,
	KnjigaID INT NOT NULL,
	Povez VARCHAR(50) NOT NULL
	CONSTRAINT FK_Primerak_Knjiga FOREIGN KEY (KnjigaID) REFERENCES Knjiga(KnjigaID),
);

ALTER TABLE Citalac
ADD
DatumClanarine DATE,
CONSTRAINT CHK_DatumClanarine CHECK(DatumClanarine >= GETDATE())