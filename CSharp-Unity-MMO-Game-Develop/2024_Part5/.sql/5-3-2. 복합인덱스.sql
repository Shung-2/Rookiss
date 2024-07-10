use Northwind;

-- �ֹ� �� ������ ���캸��
SELECT *
FROM [Order Details]
ORDER BY OrderID;

-- CREATE INTEX�� ������� �ʰ�
-- �ӽ� �׽�Ʈ ���̺��� ����� �����͸� �����غ���.
SELECT *
INTO TestOrderDetails
FROM [Order Details];

-- ��ȸ
SELECT *
FROM TestOrderDetails;

-- ���� �ε��� �߰�
CREATE INDEX Index_TestOrderDetails
ON TestOrderDetails(OrderID, ProductID);
-- ��Ʈ�� ��� �ε��� ����!

-- �ε��� ���� ���캸��
EXEC sp_helpindex 'TestOrderDetails';

-- �� 4������ ��ȸ�Ѵ�.
-- OrderID, ProductID�� ��ȸ�� ��,
-- ProductID, OrderID�� ��ȸ�� ��,
-- OrderID ��ȸ�� ��,
-- ProductID ��ȸ�� ��,

-- �ε��� ���� �׽�Ʈ 1 > GOOD
SELECT *
FROM TestOrderDetails
WHERE OrderID = 10248 AND ProductID = 11;

-- �ε��� ���� �׽�Ʈ 2 > GOOD
SELECT *
FROM TestOrderDetails
WHERE ProductID = 11 AND OrderID = 10248;

-- �ε��� ���� �׽�Ʈ 3 > GOOD
SELECT *
FROM TestOrderDetails
WHERE OrderID = 10248;

-- �ε��� ���� �׽�Ʈ 4 > BAD
SELECT *
FROM TestOrderDetails
WHERE ProductID = 11;

-- ������� ������ ����
-- INDEX SCAN (=INDEX FULL SCAN) > BAD - Ǯ��ĵ�� �� ������ �����Ƿ� ���� ��Ȳ
-- INDEX SEEK > GOOD - �ε����� ���������� Ȱ���� �Ǵ� ����

-- �ε��� ������ ���캸��
DBCC IND('Northwind', 'TestOrderDetails', 2);

--				992
-- 936	960	961	962	963	964
DBCC PAGE('Northwind', 1, 936, 3);

-- ���� �ε���(A, B)�� ����ϰڴٰ� ����� ���¶��, �ε���(A)�� ������ �ۼ����� �ʾƵ� �����ϴ�.
-- ������ �ε��� B�ε� �˻��� �ʿ��ϴٸ� > �ε��� B�� ������ �ɾ���� �Ѵ�.

------------------------------------------------------------------------------------------------

-- �ε����� �����Ͱ� �߰�/����/���� �����Ǿ�� ��
-- ������ 50���� ������ �־��.

DECLARE @i INT = 0;
WHILE @i < 50
BEGIN
	INSERT INTO TestOrderDetails
	VALUES (10248, 100 + @i, 10, 1, 0);
	SET @i = @i +1;
END;

-- INDEX ���� ��ȸ
DBCC IND('Northwind', 'TestOrderDetails', 2);

--				992
-- 936	[993]	960	961	962	963	964
DBCC PAGE('Northwind', 1, 936, 3);
DBCC PAGE('Northwind', 1, 993, 3);
-- 936�� �ִ� �����Ͱ� �ʹ� ���Ƽ� ��ġ�� �и��ؼ� �������� �ɰ��� �����ϴ� ���� �� �� �ִ�.
-- ��� : ������ ���� ������ ���ٸ� > ������ ����(SPLIT) �߻�

-- ���� �׽�Ʈ
SELECT LastName
INTO TestEmployees
FROM Employees;

-- ��ȸ
SELECT * FROM TestEmployees;

-- LastName�� �ε��� �߰�
CREATE INDEX Index_TestEmployees
ON TestEmployees(LastName);

-- INDEX SCAN > BAD
SELECT *
FROM TestEmployees
WHERE SUBSTRING(LastName, 1, 2) = 'Bu';

-- LastName�� �������� ���� ���� �ƴ�, SUBSTRING�� ���� �Լ��� �� ���
-- �ε����� �ɾ��־��ٰ� �ؼ� 100% Ȯ���� ������ ã���� �� �ִٴ� ������ ����.
-- Ű�� ������ ������ ������ �����ؾ� �Ѵ�.

-- INDEX SEEK�� �Ϸ���?
SELECT *
FROM TestEmployees
WHERE LastName LIKE 'Bu%';

-- ������ ���
-- ���� �ε���(A, B)�� ����� �� ������ �����ؾ� �Ѵ� (A->B ������ �˻��Ѵ�.)
-- �ε��� ��� ��, �����Ͱ� �߰��� ���� ������ ���� ������ ������ SPLIT�� ���� ���ο� �������� �����.
-- Ű�� ������ ������ ��������.