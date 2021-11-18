	

	CREATE SEQUENCE dbo.sqNftProje AS INT START WITH 1 INCREMENT BY 1; 
	CREATE TABLE dbo.NftProje(
		Id				INT NOT NULL,	
		
		GId				UNIQUEIDENTIFIER NOT NULL,
		Durum			BIT NOT NULL,				
		TarihSaat		DATETIME,

		Ad				NVARCHAR(250),

        KullaniciId		INT NOT NULL,

		CONSTRAINT PK_NftProje PRIMARY KEY (Id),
		CONSTRAINT FK_NftProje_KullaniciId FOREIGN KEY (KullaniciId) REFERENCES TemKullanici (Id)
	);
	CREATE INDEX IX_NftProje_GId ON NftProje (GId);
	CREATE INDEX IX_NftProje_Durum ON NftProje (Durum);
	CREATE INDEX IX_NftProje_TarihSaat ON NftProje (TarihSaat);


