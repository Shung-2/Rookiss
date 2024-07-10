USE Northwind;

-- �ε��� ���� ��� (Access)
-- Index Scan vs Index Seek

-- ���̺��� �����մϴ�.
CREATE TABLE TestAccess
(
	id INT NOT NULL,
	name NCHAR(50) NOT NULL,
	dummy NCHAR(1000) NULL
);
GO

-- Ŭ�����͵� �ε����� �����մϴ�.
CREATE CLUSTERED INDEX TestAccess_CI
ON TestAccess(id);
GO

-- ��Ŭ����Ʈ�� �ε����� �����մϴ�.
CREATE NONCLUSTERED INDEX TestAccess_NCI
ON TestAccess(name);
GO

-- �׽�Ʈ �����͸� �ֽ��ϴ�.
DECLARE @i INT;
SET @i = 1;

WHILE (@i <= 500)
BEGIN
	INSERT INTO TestAccess
	VALUES (@i, 'Name' + CONVERT(VARCHAR, @i), 'Hello World' + CONVERT(VARCHAR, @i));
	SET @i = @i + 1;
END

-- �ε��� ���� Ȯ��
EXEC sp_helpindex 'TestAccess';

-- �ε��� ��ȣ Ȯ��
SELECT index_id, name
FROM sys.indexes
WHERE object_id = object_id('TestAccess');

-- ���̺� ��ȸ
DBCC IND('Northwind', 'TestAccess', 1);
DBCC IND('Northwind', 'TestAccess', 2);

-- Ŭ����Ʈ�� � ������ �����Ǿ������� ���캸��.
-- CLUSTERED(1) : id
--			8097
-- 944 945 946 ~ 8103 (167)

-- CLUSTERED(2) : name
--			936
-- 937 938 ~ 939 (13)

-- ���� �б� �� ���� �����͸� ã�� ���� ���� ������ ��
SET STATISTICS TIME ON;
SET STATISTICS IO ON;

-- �����ȹ �м�
-- INDEX SCAN �� Leaf Page�� ���������� �˻�
SELECT *
FROM TestAccess;

-- INDEX SEEK �� 
-- ���� �б� Ƚ���� 2�ۿ� �ȉ�!
SELECT *
FROM TestAccess
WHERE id = 104;

-- ��Ŭ����Ʈ���� ����
-- INDEX SEEK + KEY LOOKUP
-- ���� �б� Ƚ�� 4, KEY LOOKUP(?)�� ����

SELECT *
FROM TestAccess
WHERE name = 'name5';

-- ���� �б� 4�� ��� ������?
-- ��Ŭ����Ʈ�̱� ������ Ŭ�����͵� �ε����� Ű���� ������ �ִ�.
-- ���� Name5�� �ش��ϴ� id ���� ������ �ִ´�.
-- ���� id���� �������� Ŭ����Ʈ �ε����� ���� �ٽ� ã�� �ȴ�.
-- ���� �� Ŭ����Ʈ���� 1, 2 Ŭ����Ʈ���� 3, 4�� ������ ��� �ȴ�.

-- INDEX SCAN + KEY LOOKUP
-- ��쿡 ���� INDEX SCAN�� ���۰��� �ƴѵ�, �� ��Ȳ�� �ٷ� �� ��Ȳ�̴�.
SELECT TOP 5 *
FROM TestAccess
ORDER BY name;

-- ���� INDEX SCAN�� ���ٰ��ؼ� ������ ���۰��� �ƴϴ�.
-- �׷��ٸ� INDEX SCAN�ӿ��� �ұ��ϰ� �� ���� �бⰡ 13�ۿ� �ȵɱ�?
-- �ش��� �ٷ� TOP�� ORDER BY�� �ִ�.
-- NAME�� ��Ŭ����Ʈ �ε����̹Ƿ� ��Ŭ����Ʈ ���̺��� LEAF�� ��ȸ�ϸ� �ǰ� �ִ��� TOP5�� ����ϱ� ������ �����ɸ��� �ʴ´�.
-- N * 2 + @�� ���� �б� ������ ���� ������.

-- ���� ������ INDEX SEEK ���� ���� ���� �ƴϰ�, INDEX SCAN�� ���� ���� �͵� �ƴϴ�.
-- �׷��ٸ� '������ ����?'��� ������ �̴� ���̽� ���� ���̽��� �ȴ�.