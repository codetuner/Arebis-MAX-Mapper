-- ===================================================================================================== --
-- This script installs a system stored procedure named 'sp_devtools_TableToCsClass' into the [master]   --
-- database of the server it is run on. This stored procedure can then be called from within any (other) --
-- database on that server(instance) to obtain a sample code and mapping version of a table given it's   --
-- schema name and table name. I.e:                                                                      --
--   USE [AdventureWorks]                                                                                --
--   EXEC sp_devtools_TableToCsClass 'HumanResources', 'Employee'                                        --
-- ===================================================================================================== --

-- Connect to [master] database:
USE [master]
SET NOcOUNT ON
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO

-- Create dummy [sp_devtools_TableToCsClass] stored procedure if it does not yet exist:
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_devtools_TableToCsClass]') AND type in (N'P', N'PC'))
   EXECUTE ('CREATE PROCEDURE [dbo].[sp_devtools_TableToCsClass] AS SELECT 0')
GO

-- Alter [sp_devtools_TableToCsClass] stored procedure:
ALTER PROCEDURE sp_devtools_TableToCsClass
	@schema nvarchar(128),
	@table nvarchar(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @str nvarchar(2000);

	PRINT ''
	PRINT 'Sample entity class definition:'
	PRINT ''

	DECLARE db_cursor CURSOR FOR 
    SELECT
	  '        ' + 
	  CASE DATA_TYPE
	  WHEN 'bit' THEN 'public virtual bool' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'int' THEN 'public virtual int' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'smallint' THEN 'public virtual short' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'tinyint' THEN 'public virtual byte' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'bigint' THEN 'public virtual long' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'float' THEN 'public virtual double' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'real' THEN 'public virtual single' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'decimal' THEN 'public virtual decimal' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'geography' THEN 'public virtual DbGeography' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'geometry' THEN 'public virtual DbGeometry' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'money' THEN '[Column(TypeName = "money")] public virtual decimal' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'varchar' THEN CASE IS_NULLABLE WHEN 'YES' THEN '' ELSE '[Required] ' END + 'public virtual string'
	  WHEN 'nvarchar' THEN CASE IS_NULLABLE WHEN 'YES' THEN '' ELSE '[Required] ' END + 'public virtual string'
	  WHEN 'char' THEN CASE CHARACTER_MAXIMUM_LENGTH WHEN 1 THEN '[Column(TypeName = "char")] public virtual char' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END ELSE CASE IS_NULLABLE WHEN 'YES' THEN '' ELSE '[Required] ' END + 'public virtual string' END
	  WHEN 'nchar' THEN CASE CHARACTER_MAXIMUM_LENGTH WHEN 1 THEN 'public virtual char' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END ELSE CASE IS_NULLABLE WHEN 'YES' THEN '' ELSE '[Required] ' END + 'public virtual string' END
	  WHEN 'date' THEN '[Column(TypeName = "date")] public virtual DateTime' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'datetime' THEN '[Column(TypeName = "datetime")] public virtual DateTime' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'datetime2' THEN '[Column(TypeName = "datetime2")] public virtual DateTime' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'uniqueidentifier' THEN 'public virtual Guid' + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END
	  WHEN 'varbinary' THEN 'public virtual byte[]'
	  ELSE 'public virtual (' + DATA_TYPE + CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END + ')'
	  END
	+ ' ' + COLUMN_NAME + ' { get; set; }'
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE TABLE_SCHEMA LIKE @schema AND TABLE_NAME LIKE @table
	ORDER BY ORDINAL_POSITION;

	PRINT '    [Table("' + @table + '", Schema = "' + @schema + '")]';
	PRINT '    public class ' + @table;
	PRINT '    {';
	OPEN db_cursor   
	FETCH NEXT FROM db_cursor INTO @str
	WHILE @@FETCH_STATUS = 0   
	BEGIN
		PRINT @str
		FETCH NEXT FROM db_cursor INTO @str
	END
	CLOSE db_cursor
	PRINT '    }';

	DEALLOCATE db_cursor

	PRINT ''
	PRINT 'Sample mapping declaration:'
	PRINT ''

	DECLARE db_cursor CURSOR FOR 
	SELECT
	  '        <map property="' + COLUMN_NAME + '"></map>'
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE TABLE_SCHEMA LIKE @schema AND TABLE_NAME LIKE @table
    ORDER BY ORDINAL_POSITION;

	PRINT '    <type name="' + @table + '" source="Domain.' + @table + '">';
	OPEN db_cursor   
	FETCH NEXT FROM db_cursor INTO @str
	WHILE @@FETCH_STATUS = 0   
	BEGIN
		PRINT @str
		FETCH NEXT FROM db_cursor INTO @str
	END
	CLOSE db_cursor
	PRINT '    </type>';

	DEALLOCATE db_cursor

END
GO

-- Mark stored procedure as system:
EXEC sys.sp_MS_marksystemobject sp_devtools_TableToCsClass
GO
