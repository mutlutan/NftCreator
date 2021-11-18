using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp1.Codes
{
    public class MyDatabaseUpdate
    {
        public static List<MoUpdateQuery> GetDatabaseUpdateList(decimal updateId)
        {
            var sqlUpdateList = new List<MoUpdateQuery>() { };

            #region örnekler

            // table not exists
            //IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TemXX')
            //BEGIN
            //  Create Table....
            //END

            //column not exists
            //IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TemXX' AND COLUMN_NAME = 'Ad')
            //BEGIN
            //  Alter Table TemXX Add Ad	NVARCHAR(100);
            //END

            //SEQUENCE not exists
            //IF OBJECT_ID('dbo.sqTemXX', 'SO') IS NULL
            //BEGIN
            //  CREATE SEQUENCE dbo.sqTemXX AS INT START WITH 1 INCREMENT BY 1;
            //END

            //drop squence
            //IF OBJECT_ID('dbo.sqDepartmanGorev', 'SO') IS NOT NULL
            //BEGIN
            //  DROP SEQUENCE dbo.sqDepartmanGorev;
            //END

            //index exists
            //IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('TemKisi') AND name = 'IX_TemKisi_Kod')
            //BEGIN
            //  DROP INDEX IX_TemKisi_Kod ON TemKisi;
            //END

            //FOREIGN key not exists
            //IF NOT EXISTS(SELECT name FROM sys.foreign_keys WHERE name = 'FK_LisIsletme_SaticiId')
            //BEGIN
            //  ALTER TABLE LisIsletme ADD CONSTRAINT FK_LisIsletme_SaticiId FOREIGN KEY(SaticiId) REFERENCES TemKisi(Id);
            //END

            //FOREIGN key exists
            //IF EXISTS(SELECT name FROM sys.foreign_keys WHERE name = 'FK_RobOgrenci_GrupId')
            //BEGIN
            //    ALTER TABLE RobOgrenci DROP CONSTRAINT FK_RobOgrenci_GrupId;
            //END

            // alter column
            //IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RobProjeSayfa' AND COLUMN_NAME = 'Sira' AND DATA_TYPE = 'int')
            //BEGIN
            //    ALTER TABLE RobProjeSayfa ALTER COLUMN Sira DECIMAL(18,4);
            //END

            //DROP COLUMN
            //IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TemKullaniciLisans' AND COLUMN_NAME = 'LisansTurId')
            //BEGIN
            //    ALTER TABLE TemKullaniciLisans DROP COLUMN LisansTurId;
            //END

            // DROP TABLE
            //IF EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TemKisiTur')
            //BEGIN
            //    DROP TABLE dbo.TemKisiTur;
            //END
            #endregion


            


            //// 00.00.2020 00:00 
            //sqlUpdateList.Add(
            //    new MoUpdateQuery
            //    {
            //        Id = 00m,
            //        Description = " ... değişenler.",
            //        Command = $@"

            //        "
            //    }
            //);

            sqlUpdateList = sqlUpdateList.Where(c => c.Id > updateId).ToList();

            return sqlUpdateList;
        }

    }
}
