	/*Database şema Versiyon */
	CREATE TABLE dbo.TemVersiyon(
		Id			INT NOT NULL, /*Veritabanı şema versiyonu*/

		Tarih		DATETIME NOT NULL,
		Aciklama	NVARCHAR(MAX),  
		Komut		NVARCHAR(MAX),  
		      	
		CONSTRAINT PK_TemVersiyon PRIMARY KEY (Id)
	);
	INSERT TemVersiyon (Id, Tarih, Aciklama, Komut) VALUES (0, getdate(), '', '');

	/*Parametreler - default değerler */
	CREATE TABLE dbo.TemParametre(
		Id					INT NOT NULL,

		/*Birden Fazla Giriş Yapılabilir, giremez*/
		UniqueVisit			BIT NOT NULL,

		/*web sayfası adresi - local : http://localhost:5002  sunucu: https://www.qq.com*/
		HostAddress			NVARCHAR(100) NOT NULL,

		/*Lisans*/
		LisansData			NVARCHAR(MAX), 	/*Lisans class datası*/

		/*Audit yapılacak tablolar*/
		AuditLog			BIT NOT NULL,
		AuditLogTables		NVARCHAR(MAX),
		
		/*(İşletme olarak değiş)Kurum Bilgileri*/
		KurumEnlem			NVARCHAR(15), 
		KurumBoylam			NVARCHAR(15),
		KurumAd				NVARCHAR(100),
		KurumUnvan			NVARCHAR(250),
		KurumAdes			NVARCHAR(250),
		KurumSemtSehir		NVARCHAR(100),
		KurumMail			NVARCHAR(50),
		KurumTelefon1		NVARCHAR(50),
		KurumTelefon2		NVARCHAR(50),
		KurumCep1			NVARCHAR(50),
		KurumCep2			NVARCHAR(50),
		KurumFax1			NVARCHAR(50),
		KurumFax2			NVARCHAR(50),
		
		/*SMTP Mail Account bilgileri (giden maillerde kullanılacak account)*/
		MailHost			NVARCHAR(100),
		MailPort			INT NOT NULL,
		MailEnableSsl		BIT NOT NULL,
		MailUserName		NVARCHAR(100),
		MailPassword		NVARCHAR(100),

		/*Data Backup*/ 
		DataBackupDurum		BIT NOT NULL,
		DataBackupSaat		TIME, /*Hangi saatte çalışacagı*/
		DataBackupTip		INT NOT NULL, /*0:Günlük, 1:Haftalık, 2:Aylık*/
		DataBackupGun		NVARCHAR(15), /*haftalık gönderimde, hangi gün gönderilecek Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday*/
		DataBackupAyGunNo	INT NOT NULL, /*Aylık gönderimde, ayın kaçıncı günü gönderilecek*/
		
		/*File Backup*/
		FileBackupDurum		BIT NOT NULL,
		FileBackupSaat		TIME, /*Hangi saatte çalışacagı*/
		FileBackupTip		INT NOT NULL, /*0:Günlük, 1:Haftalık, 2:Aylık*/
		FileBackupGun		NVARCHAR(15), /*haftalık gönderimde, hangi gün gönderilecek Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday*/
		FileBackupAyGunNo	INT NOT NULL, /*Aylık gönderimde, ayın kaçıncı günü gönderilecek*/

		/*Genel ayarlar*/
		IyzicoBaseUrl		NVARCHAR(50), 
		IyzicoApiKey		NVARCHAR(50),  
		IyzicoSecretKey		NVARCHAR(50), 
         	
		CONSTRAINT PK_TemParametre PRIMARY KEY (Id)
	);
	INSERT INTO TemParametre (Id, UniqueVisit, AuditLog, HostAddress, KurumAd, MailHost, MailPort, MailEnableSsl, MailUserName, MailPassword, DataBackupDurum, DataBackupTip, DataBackupAyGunNo, FileBackupDurum, FileBackupTip, FileBackupAyGunNo ) 
	VALUES (1, 0, 0, N'http://localhost:5002', N'Demo Kurum', N'smtp.live.com', 587, 0, N'mutlutan@outlook.com', N'TwBvADEANgA1ADYAMwAxAA==', 0, 0, 0, 0, 0, 0);

	/*RequestLog*/
	CREATE SEQUENCE dbo.sqTemRequestLog AS INT START WITH 1 INCREMENT BY 1; 
	CREATE TABLE dbo.TemRequestLog(
		Id				INT NOT NULL,

		OperationDate	DATETIME,
		OperationKod	NVARCHAR(50),
		IpAddress		NVARCHAR(50),
		SessionGuid		NVARCHAR(50), /*Session guid*/
		RequestJson		NVARCHAR(MAX),
		ResponseJson	NVARCHAR(MAX),
		
		CONSTRAINT PK_TemRequestLog PRIMARY KEY (Id)
	);
	CREATE INDEX IX_TemRequestLog_OperationDate ON TemRequestLog (OperationDate);
	CREATE INDEX IX_TemRequestLog_OperationKod ON TemRequestLog (OperationKod);
		
	/*Audit*/
	CREATE SEQUENCE dbo.sqTemAuditLog AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.TemAuditLog(
		Id				INT NOT NULL,

		OperationDate	DATETIME,
		UserId			INT NOT NULL,
		IpAddress		VARCHAR(50),
		OperationType	CHAR(1), /*CRUD type*/
		
		TableName		VARCHAR(50),
		PrimaryKeyField	VARCHAR(50), 
		PrimaryKeyValue	NVARCHAR(50),
		
		CurrentValues	NVARCHAR(MAX),
		OriginalValues	NVARCHAR(MAX),
		
	
		CONSTRAINT PK_TemAuditLog PRIMARY KEY (Id)
	);
	CREATE INDEX IX_TemAuditLog_OperationDate ON TemAuditLog (OperationDate);
	CREATE INDEX IX_TemAuditLog_TableName ON TemAuditLog (TableName);
	CREATE INDEX IX_TemAuditLog_UserId ON TemAuditLog (UserId);
	CREATE INDEX IX_TemAuditLog_OperationType ON TemAuditLog (OperationType);
	CREATE INDEX IX_TemAuditLog_PrimaryKeyField ON TemAuditLog (PrimaryKeyField);
	CREATE INDEX IX_TemAuditLog_PrimaryKeyValue ON TemAuditLog (PrimaryKeyValue);

	/*Rol*/
	CREATE SEQUENCE dbo.sqTemRol AS INT START WITH 1101 INCREMENT BY 1; 
	CREATE TABLE dbo.TemRol(
		Id			INT NOT NULL,

		Sira		INT NOT NULL,
		Ad		    NVARCHAR(30) NOT NULL,
		Yetki		NVARCHAR(MAX), 
		
		CONSTRAINT PK_TemRol PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_TemRol_Ad ON TemRol (Ad);
	INSERT INTO TemRol (Id, Sira, Ad) VALUES (0,    -9, N'');
	INSERT INTO TemRol (Id, Sira, Ad) VALUES (1001, -8, N'Admin');
	INSERT INTO TemRol (Id, Sira, Ad) VALUES (Next Value For dbo.sqTemRol, 1, N'Employee');

	/*Para Birim Merkez bankasından güncellenir*/
	CREATE SEQUENCE dbo.sqTemParaBirim AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.TemParaBirim(
		Id				INT NOT NULL,

		Updateable		BIT NOT NULL, /*Updateable:1 ise ve Durum:1 ise otomatik döviz servisinden kurlar güncellenir */
		UpdateDate		DATETIME, 	/*Kur Güncelleme Tarihi*/
		UpdateCode		NVARCHAR(10) NOT NULL, /*Para birimi kodu*/	

		Durum			BIT NOT NULL, 
		Sira			INT NOT NULL,	
		Simge			NVARCHAR(10),
		Kod				NVARCHAR(10), 
		Ad				NVARCHAR(20) NOT NULL, /*Tam para birimi, Üst para birimi (faturada yazıya çevrilirken kullanılacak kısım)*/
		AltBirim		NVARCHAR(20), /*Kesirli para birimi, Alt Para Birimi - Kuruş (faturada yazıya çevrilirken kullanılacak kısım)*/
		
		
		CONSTRAINT PK_TemParaBirim PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_TemParaBirim_Ad ON TemParaBirim (Ad);
	INSERT INTO TemParaBirim (Id, Durum, Updateable, Sira, Simge, UpdateCode, Ad) VALUES (0, 0, 0, -9, N'', N'00', N'');
	INSERT INTO TemParaBirim (Id, Durum, Updateable, Sira, Simge, UpdateCode, Kod, Ad, AltBirim) VALUES (Next Value For dbo.sqTemParaBirim, 1, 0, 1, N'₺', N'TRY', N'TL', N'Türk Lirası', N'Kuruş');
	INSERT INTO TemParaBirim (Id, Durum, Updateable, Sira, Simge, UpdateCode, Kod, Ad, AltBirim) VALUES (Next Value For dbo.sqTemParaBirim, 1, 1, 2, N'$', N'USD', N'USD', N'Dolar', N'Cent');
	INSERT INTO TemParaBirim (Id, Durum, Updateable, Sira, Simge, UpdateCode, Kod, Ad, AltBirim) VALUES (Next Value For dbo.sqTemParaBirim, 1, 1, 3, N'€', N'EUR', N'EUR', N'Euro', N'Cent');
		
	/*Para Kur Merkez bankasından güncellenir*/
	CREATE SEQUENCE dbo.sqTemDovizKurArsiv AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.TemDovizKurArsiv(
		Id				INT NOT NULL,

		ParaBirimId		INT NOT NULL,
		Tarih			DATE NOT NULL,
		TarihSaat		DATE NOT NULL,

		DovizAlis		DECIMAL(18,4) NOT NULL,
		DovizSatis		DECIMAL(18,4) NOT NULL,
		EfektifAlis		DECIMAL(18,4) NOT NULL,
		EfektifSatis	DECIMAL(18,4) NOT NULL,
		
		CONSTRAINT PK_TemDovizKurArsiv PRIMARY KEY (Id),
		CONSTRAINT FK_TemDovizKurArsiv_ParaBirimId FOREIGN KEY (ParaBirimId) REFERENCES TemParaBirim(Id)
	);
	CREATE INDEX IX_TemDovizKurArsiv_ParaBirimId ON TemDovizKurArsiv (ParaBirimId);
	CREATE INDEX IX_TemDovizKurArsiv_Tarih ON TemDovizKurArsiv (Tarih);
	CREATE INDEX IX_TemDovizKurArsiv_TarihSaat ON TemDovizKurArsiv (TarihSaat);
    
    /* Ülke*/
	CREATE SEQUENCE dbo.sqTemUlke AS INT START WITH 1 INCREMENT BY 1;
    CREATE TABLE dbo.TemUlke (
        Id			INT NOT NULL,	

		Sira		INT NOT NULL,
		AlanKod		NVARCHAR(10), /*Ülke Telefon Alan Kod*/
        Kod			NVARCHAR(10) NOT NULL,
        Ad			NVARCHAR(50) NOT NULL,
    
		CONSTRAINT PK_TemUlke PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_TemUlke_Kod ON TemUlke (Kod);
	CREATE UNIQUE INDEX UX_TemUlke_Ad ON TemUlke (Ad);
	INSERT INTO TemUlke (Id, Sira, Kod, Ad) VALUES (0, -9, N'', N'');
	INSERT TemUlke (Id, Sira, AlanKod, Kod, Ad) VALUES (Next Value For dbo.sqTemUlke, 0, N'+90', N'TR', N'TÜRKİYE');

	/* Şehirler*/
	CREATE SEQUENCE dbo.sqTemSehir AS INT START WITH 1 INCREMENT BY 1;
    CREATE TABLE dbo.TemSehir (
        Id			INT NOT NULL,
        UlkeId		INT NOT NULL, 
		
		Sira		INT NOT NULL,
		AlanKod		NVARCHAR(10), /*Sehir Telefon Alan Kod*/
		Kod			NVARCHAR(10),       
        Ad			NVARCHAR(50) NOT NULL,
    
		CONSTRAINT PK_TemSehir PRIMARY KEY (Id),
		CONSTRAINT FK_TemSehir_UlkeId FOREIGN KEY (UlkeId) REFERENCES TemUlke(Id)
	);
	CREATE UNIQUE INDEX UX_TemSehir_UlkeId_Ad ON TemSehir (UlkeId, Ad);
	CREATE INDEX IX_TemSehir_UlkeId ON TemSehir (UlkeId);
	CREATE INDEX IX_TemSehir_Ad ON TemSehir (Ad);
	INSERT INTO TemSehir (Id, UlkeId, Sira, Ad) VALUES (0, 0, -9, N'');

    /* İleçeler */
	CREATE SEQUENCE dbo.sqTemIlce AS INT START WITH 1001 INCREMENT BY 1;
    CREATE TABLE dbo.TemIlce (
        Id			INT NOT NULL,
        SehirId		INT NOT NULL,
		
		Sira		INT NOT NULL,
        Kod			NVARCHAR(10),
        Ad			NVARCHAR(50) NOT NULL,
    
		CONSTRAINT PK_TemIlce PRIMARY KEY (Id),
		CONSTRAINT FK_TemIlce_SehirId FOREIGN KEY (SehirId) REFERENCES TemSehir(Id)
	);
	CREATE UNIQUE INDEX UX_TemIlce_SehirId_Ad ON TemIlce (SehirId, Ad);
	CREATE INDEX IX_TemIlce_SehirId ON TemIlce (SehirId);
	CREATE INDEX IX_TemIlce_Sira ON TemIlce (Sira);
    CREATE INDEX IX_TemIlce_Ad ON TemIlce (Ad);
	INSERT INTO TemIlce (Id, SehirId, Sira, Ad) VALUES (0, 0, -9, N'');
	
	/*Cinsiyet*/
	CREATE TABLE dbo.TemCinsiyet(
		Id			INT NOT NULL,

		Durum		BIT NOT NULL,  
		Sira		INT NOT NULL,
		Ad		    NVARCHAR(20) NOT NULL,
		
		CONSTRAINT PK_TemCinsiyet PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_TemCinsiyet_Ad ON TemCinsiyet (Ad);
	CREATE INDEX IX_TemCinsiyet_Durum ON TemCinsiyet (Durum);
	INSERT INTO TemCinsiyet (Id, Durum, Sira, Ad) VALUES (0, 1, -9, N'');
	INSERT INTO TemCinsiyet (Id, Durum, Sira, Ad) VALUES (1, 1, 1, N'Erkek');
	INSERT INTO TemCinsiyet (Id, Durum, Sira, Ad) VALUES (2, 1, 2, N'Kadın');

	/*Departman - Birim (Organizasyon şeması buradan olusur) */
	CREATE SEQUENCE dbo.sqTemDepartman AS INT START WITH 1 INCREMENT BY 1; 
	CREATE TABLE dbo.TemDepartman(
		Id			INT NOT NULL,
		
		Durum		BIT NOT NULL,  
		UstId		INT NOT NULL,

		Sira		DECIMAL(18,4) NOT NULL,
		Ad		    NVARCHAR(50) NOT NULL,
		
		CONSTRAINT PK_TemDepartman PRIMARY KEY (Id),
		CONSTRAINT FK_TemDepartman_UstId FOREIGN KEY (UstId) REFERENCES TemDepartman(Id)
	);
	CREATE UNIQUE INDEX UX_TemDepartman_Ad ON TemDepartman (Ad);
	CREATE INDEX IX_TemDepartman_Durum ON TemDepartman (Durum);
	INSERT INTO TemDepartman (Id, UstId, Durum, Sira, Ad) VALUES (0, 0, 1, -9, N'');
	INSERT INTO TemDepartman (Id, UstId, Durum, Sira, Ad) VALUES (Next Value For dbo.sqTemDepartman, 0, 1, 1, N'Yönetim');
	INSERT INTO TemDepartman (Id, UstId, Durum, Sira, Ad) VALUES (Next Value For dbo.sqTemDepartman, 0, 1, 2, N'Muhasebe');

	/*Görev*/
	CREATE SEQUENCE dbo.sqTemGorev AS INT START WITH 1 INCREMENT BY 1; 
	CREATE TABLE dbo.TemGorev(
		Id		INT NOT NULL,

		Durum   BIT NOT NULL,  
		Sira	INT NOT NULL,
		Ad		NVARCHAR(50) NOT NULL,
		
		CONSTRAINT PK_TemGorev PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_TemGorev_Ad ON TemGorev (Ad);
	CREATE INDEX IX_TemGorev_Durum ON TemGorev (Durum);
	INSERT INTO TemGorev (Id, Durum, Sira, Ad) VALUES (0, 1, -9, N'');
	INSERT INTO TemGorev (Id, Durum, Sira, Ad) VALUES (Next Value For dbo.sqTemGorev, 1, 1, N'Yönetici');
	INSERT INTO TemGorev (Id, Durum, Sira, Ad) VALUES (Next Value For dbo.sqTemGorev, 1, 2, N'Personel');

	/* EĞİTİM DURUMLARI */
	CREATE TABLE dbo.TemEgitimDurum (
		Id		INT NOT NULL,

		Durum   BIT NOT NULL,  
		Sira	INT NOT NULL,
		Ad		NVARCHAR(100) NOT NULL,

		CONSTRAINT Pk_TemEgitimDurum PRIMARY KEY(Id),
	);
	CREATE UNIQUE INDEX UX_TemEgitimDurum_Ad ON TemEgitimDurum (Ad);
	INSERT INTO TemEgitimDurum (Id, Durum, Sira, Ad) VALUES (0, 1, -9, N'');
	INSERT INTO TemEgitimDurum(Id, Durum, Sira, Ad)	VALUES (1, 1, 1, N'İlkokul');
	INSERT INTO TemEgitimDurum(Id, Durum, Sira, Ad)	VALUES (2, 1, 2, N'Ortaokul');
	INSERT INTO TemEgitimDurum(Id, Durum, Sira, Ad)	VALUES (3, 1, 3, N'Lise');
	INSERT INTO TemEgitimDurum(Id, Durum, Sira, Ad)	VALUES (4, 1, 4, N'Yüksekokul');
	INSERT INTO TemEgitimDurum(Id, Durum, Sira, Ad)	VALUES (5, 1, 5, N'Lisans');
	INSERT INTO TemEgitimDurum(Id, Durum, Sira, Ad)	VALUES (6, 1, 6, N'Yüksek Lisans');
	INSERT INTO TemEgitimDurum(Id, Durum, Sira, Ad)	VALUES (7, 1, 7, N'Doktora');

	/* ŞEHİR VE İLÇE SEÇİMİ ADRES İÇİN KULLANILACAK */
	CREATE SEQUENCE dbo.sqTemAdres AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.TemAdres (
		Id		 INT NOT NULL,

		UstId	 INT NOT NULL,
		Kod		 NVARCHAR(20),
		Ad		 NVARCHAR(100),

		CONSTRAINT Pk_TemAdres PRIMARY KEY (Id),
		CONSTRAINT FK_TemAdres_UstId FOREIGN KEY (UstId) REFERENCES TemAdres(Id)
	);
	INSERT INTO TemAdres (Id, UstId, Kod, Ad) VALUES (0, 0, N'', N'');	

	/* Kullanıcı*/
	CREATE SEQUENCE dbo.sqTemKullanici AS INT START WITH 1 INCREMENT BY 1;
    CREATE TABLE dbo.TemKullanici(
		Id			INT NOT NULL,

		Durum		BIT NOT NULL,
		
		Ad			NVARCHAR(50), /*email adres olabilir, tcno olabilir*/
		Sifre		NVARCHAR(100),	/*Bu alan her update olduğunda, TemKullaniciSifre tablosuna insert edilecek, history için*/
		Rols		NVARCHAR(MAX), /*bu row da silme olduğunda foreign key olmadığından manuel Rol tablosu kontrol edilecek, ve sildirilmeyecek */
		
		Resim		NVARCHAR(MAX),
		AdSoyad		NVARCHAR(50),
		DogumTarihi	DATE, 
	
		InsertUserId	INT,
		UpdateUserId	INT,
		InsertDateTime	DATETIME,
		UpdateDateTime	DATETIME,

		CONSTRAINT PK_TemKullanici PRIMARY KEY (Id)
	);
	CREATE UNIQUE INDEX UX_TemKullanici_Ad ON TemKullanici (Ad);
	INSERT INTO TemKullanici (Id, Durum, Rols, Ad, Sifre) VALUES (0, 0, N'', N'', N'');
	INSERT INTO TemKullanici (Id, Durum, Rols, AdSoyad, Ad, Sifre) VALUES (Next Value For dbo.sqTemKullanici, 1, N'1001', N'admin', N'admin', N'07');
	INSERT INTO TemKullanici (Id, Durum, Rols, AdSoyad, Ad, Sifre) VALUES (Next Value For dbo.sqTemKullanici, 1, N'1101', N'Employee',N'employee', N'07');
	INSERT INTO TemKullanici (Id, Durum, Rols, AdSoyad, Ad, Sifre) VALUES (Next Value For dbo.sqTemKullanici, 1, N'', N'Customer',N'customer', N'07');

	/* Kullanici Şifre history*/
	CREATE SEQUENCE dbo.sqTemKullaniciSifre AS INT START WITH 1 INCREMENT BY 1;
    CREATE TABLE dbo.TemKullaniciSifre(
		Id				INT NOT NULL,
		KullaniciId		INT NOT NULL,
 
		KayitZaman		DATETIME, /*Kayıt tarihi*/
		Sifre			NVARCHAR(100),
		
		CONSTRAINT PK_TemKullaniciSifre PRIMARY KEY (Id),
		CONSTRAINT FK_TemKullaniciSifre_KullaniciId FOREIGN KEY (KullaniciId) REFERENCES TemKullanici (Id)
	);
	CREATE INDEX IX_TemKullaniciSifre_KullaniciId ON TemKullaniciSifre (KullaniciId);


	/* Kullanici Lisans - bu tablodaki kaydın bitis tarihi gelmemiş olan lisanslı sayılır, bu uygunlukta kayıt olmayan demo kullanıcı olur */
	CREATE SEQUENCE dbo.sqTemKullaniciLisans AS INT START WITH 1 INCREMENT BY 1;
    CREATE TABLE dbo.TemKullaniciLisans(
		Id				INT NOT NULL,
		KullaniciId		INT NOT NULL,
 
		Durum			BIT NOT NULL,
		BaslamaTarihi	DATE,		/*Lisans başlama*/
		BitisTarihi		DATE,		/*Lisans bitiş - bitiş tarihi geçen demo olur*/

		InsertUserId	INT,
		UpdateUserId	INT,
		InsertDateTime	DATETIME,
		UpdateDateTime	DATETIME,
		
		CONSTRAINT PK_TemKullaniciLisans PRIMARY KEY (Id),
		CONSTRAINT FK_TemKullaniciLisans_KullaniciId FOREIGN KEY (KullaniciId) REFERENCES TemKullanici (Id)
	);
	CREATE INDEX IX_TemKullaniciLisans_KullaniciId ON TemKullaniciLisans (KullaniciId);

	/* OturumLog*/
	CREATE SEQUENCE dbo.sqTemOturumLog AS INT START WITH 1 INCREMENT BY 1;
    CREATE TABLE dbo.TemOturumLog(
		Id						INT NOT NULL,
		KullaniciId				INT NOT NULL,

		Tarayici				NVARCHAR(250), /*Tarayici ve versiyonu*/
		InternetProtokolAdres	NVARCHAR(50),
		OturumGuid				NVARCHAR(50), /*Session guid*/

		GirisZaman		DATETIME,
		CikisZaman		DATETIME,

		CONSTRAINT PK_TemOturumLog PRIMARY KEY (Id),
		CONSTRAINT FK_TemOturumLog_KullaniciId FOREIGN KEY (KullaniciId) REFERENCES TemKullanici (Id)
	);
	CREATE INDEX IX_TemOturumLog_KullaniciId ON TemOturumLog (KullaniciId);
	CREATE INDEX IX_TemOturumLog_OturumGuid ON TemOturumLog (OturumGuid);
	

	CREATE SEQUENCE dbo.sqTemMailAntet AS INT START WITH 101 INCREMENT BY 1;
	CREATE TABLE dbo.TemMailAntet (
		Id			INT NOT NULL,
		
		Ad			NVARCHAR(50),
		Icerik		NVARCHAR(max), /*html içerik ile editörden yapılandır, replace ile kodlanmış verileri mail merge yap*/

		CONSTRAINT Pk_TemMailAntet PRIMARY KEY (Id)
	);
	INSERT INTO TemMailAntet (Id, Ad, Icerik) VALUES (0, N'Boş', N''); /*Boş antetli mail gerekirse*/

	CREATE TABLE dbo.TemMailSablon (
		Id			INT NOT NULL,
		
		AntetId		INT NOT NULL,

		Kopya		NVARCHAR(500), /*cc*/
		Gizli		NVARCHAR(500), /*bcc*/

		Alan		NVARCHAR(MAX), /*mail merge için gereken alanlar*/

		Konu		NVARCHAR(250),
		Icerik		NVARCHAR(max), /*html içerik ile editörden yapılandır, replace ile kodlanmış verileri mail merge yap*/

		/*dil için bir çözüm */

		CONSTRAINT Pk_TemMailSablon PRIMARY KEY (Id),
		CONSTRAINT FK_TemMailSablon_AntetId FOREIGN KEY (AntetId) REFERENCES TemMailAntet (Id)
	);
	

	CREATE TABLE dbo.TemMailHareketDurum (
		Id			INT NOT NULL,
		
		Ad			NVARCHAR(50),

		--VeriDili	NVARCHAR(max),

		CONSTRAINT Pk_TemMailHareketDurum PRIMARY KEY (Id)
	);
	INSERT INTO TemMailHareketDurum (Id,Ad) VALUES (0, N'Bekliyor'); /*deneme sayısı 1 artır gönderiliyor yap, göndermeyi dene*/
	INSERT INTO TemMailHareketDurum (Id,Ad) VALUES (1, N'Gönderiliyor'); /*deneme sayısı 1 artır gönderiliyor yap, göndermeyi tekrar dene*/
	INSERT INTO TemMailHareketDurum (Id,Ad) VALUES (2, N'Gönderildi'); /*gönderildiğinden emin isen Gönderildi yap*/
	INSERT INTO TemMailHareketDurum (Id,Ad) VALUES (3, N'Hata'); /*Hata olduysa 3 yaz, açıklamaya yaz*/

	CREATE SEQUENCE dbo.sqTemMailHareket AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.TemMailHareket (
		Id				INT NOT NULL,
		
		SablonId		INT NOT NULL,
		KayitZaman		DATETIME, /*Kayıt tarihi*/
		DurumId			INT NOT NULL,
		DenemeSayisi	INT NOT NULL,
		SonDenemeZaman	DATETIME,
		Aciklama		NVARCHAR(max), /*log gibi*/

		Adres			NVARCHAR(500), /*gönderilen adres*/
		Kopya			NVARCHAR(500), /*cc*/
		Gizli			NVARCHAR(500), /*bcc*/
		Konu			NVARCHAR(250),
		Icerik			NVARCHAR(max), /*mail body*/

		CONSTRAINT Pk_TemMailHareket PRIMARY KEY (Id),
		CONSTRAINT FK_TemMailHareket_SablonId FOREIGN KEY (SablonId) REFERENCES TemMailSablon (Id),
		CONSTRAINT FK_TemMailHareket_DurumId FOREIGN KEY (DurumId) REFERENCES TemMailHareketDurum (Id)
	);
	CREATE INDEX IX_TemMailHareket_SablonId ON TemMailHareket (SablonId);
	CREATE INDEX IX_TemMailHareket_DurumId ON TemMailHareket (DurumId);

		
	CREATE SEQUENCE dbo.sqTemMesaj AS INT START WITH 1 INCREMENT BY 1;
	CREATE TABLE dbo.TemMesaj (
		Id				INT NOT NULL,

		UstId			INT NOT NULL,

		GondericiId		INT NOT NULL,
		AliciId			INT NOT NULL,

		Baslik			NVARCHAR(50),
		Icerik			NVARCHAR(max),

		CONSTRAINT Pk_TemMesaj PRIMARY KEY (Id),
		CONSTRAINT FK_TemMesaj_UstId FOREIGN KEY (UstId) REFERENCES TemMesaj(Id),
		CONSTRAINT FK_TemMesaj_GondericiId FOREIGN KEY (GondericiId) REFERENCES TemKullanici (Id),
		CONSTRAINT FK_TemMesaj_AliciId FOREIGN KEY (AliciId) REFERENCES TemKullanici (Id)
	);
