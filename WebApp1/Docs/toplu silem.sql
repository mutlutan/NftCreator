
-- detaylarının tamamını toplu silme için bir kod
  begin tran;
  declare @OgrenciId int = -1;

  delete from TemOturumLog where KullaniciId in (select Id from TemKullanici Where SahipTur=51 and SahipId in (Select Id from RobOgrenci where Id = @OgrenciId));
  
  delete from TemKullaniciLisans where KullaniciId in (select Id from TemKullanici Where SahipTur=51 and SahipId in (Select Id from RobOgrenci where Id = @OgrenciId));

  delete from TemKullanici Where SahipTur=51 and SahipId in (Select Id from RobOgrenci where Id = @OgrenciId);
    
  delete from RobOgrenci where Id = @OgrenciId;

  rollback tran;