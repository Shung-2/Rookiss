USE Northwind;

-- ���� �ε��� �÷� ����
-- Index(A, B, C)

-- ��Ŭ�����͵�
--	 1
-- 2 3 4

-- Ŭ�����͵�
--	 1
-- 2 3 4[(56 �޸�) (56 �޸�) (56 �޸�) (56 �޸�) (56 �޸�)...]

-- Heap Table [ {Page}, {Page} ]

-- �ϸ�ũ ���
-- ���� �ð��� �ϸ�ũ ����� ��� �ּ�ȭ�ұ ���ؼ� �н��ߴ�.
-- �׷��� ���� '�ϸ�ũ ����� �ּ�ȭ�ϴ� ���� ����ȭ�� ���ΰ�?'��� ������ �װɷδ� ���� �ƴϴ�. ��� ����� �� �ִ�.
-- �ֳ��ϸ� Leaf Page�� ���� Ž���� ������ �����ϱ� �����̴�.

-- ������� [����, ����]�� ���ؼ� �ε����� �ɾ��� �� (56, �޸�)�� ã�´ٰ� �����Ѵٸ�
-- Ŭ�����͵� �ε����� ���� �������� ã�� ��� 56, �޸��� �� �ϳ��� �����Ѵٴ� ������ ����.
-- ���� 4�� ����������� 56, �޸��� �ƴҶ����� ��� ��ĵ�� ó���ؼ� �����ؾ� �Ѵ�.
-- �׷��Ƿ� �ε����� ����ߴٰ� �ϴ���. ������������ ������ �������� ��ĵ�ؾ� �ϴ� �ʿ伺�� �����.
-- �� �ش����� ���δ� 56~60 �޸��� ã�´ٰ� �� ��� ���̺� ��ĵ�� ������ �� �о����ٴ� ������ �����.
-- ���� �ε����� ������ ������ ū ������ �ִ� ���� ���� �н��� ���̴�.

-- �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

-- �ǽ�
-- ���̺� ����
SELECT *
INTO TestOrders
FROM Orders;

-- ���� ������ ����
DECLARE @i INT = 1;
DECLARE @emp INT;
SELECT @emp = MAX(EmployeeID) FROM Orders;

-- ������ ���� ��ȸ
SELECT *
FROM TestOrders;
-- 830���� ��, ���⿡ 1000���� ������ ���̴�. (830 * 1000)

WHILE (@i < 1000)
BEGIN
	INSERT INTO TestOrders(CustomerID, EmployeeID, OrderDate)
	SELECT CustomerID, @emp + @i, OrderDate
	FROM Orders;
	SET @i = @i + 1;
END

-- �����Ͱ� ��������?
SELECT COUNT(*)
FROM TestOrders;
-- 83����!

-- �ε��� ����
CREATE NONCLUSTERED INDEX idx_emp_ord
ON TestOrders(EmployeeID, OrderDate);

CREATE NONCLUSTERED INDEX idx_ord_emp
ON TestOrders(OrderDate, EmployeeID);

-- ���� ������� �� ������? �̸� �׽�Ʈ �غ���.
SET STATISTICS TIME ON;
SET STATISTICS IO ON;

-- �ε����� ������ �����Ͽ� �� ���� ������.
SELECT *
FROM TestOrders WITH(INDEX(idx_emp_ord))
WHERE EmployeeID = 1 AND OrderDate = CONVERT(DATETIME, '19970101');

SELECT *
FROM TestOrders WITH(INDEX(idx_ord_emp))
WHERE EmployeeID = 1 AND OrderDate = CONVERT(DATETIME, '19970101');
-- ���~ ���� �б�� ���� ��ȹ�� ��� �Ȱ��� ���͹��ȴ�.
-- �� �Ȱ��� ���Դ��� �˾ƺ����� ����.

-- ���� ���캸��
SELECT *
FROM TestOrders
ORDER BY EmployeeID, OrderDate;

SELECT *
FROM TestOrders
ORDER BY OrderDate, EmployeeID;

-- �׷��ٸ� ������ ã�´ٸ� �������� �Ͼ��?
-- ������� �̺�Ʈ�� ���� 7�� 1�Ϻ��� 7�� 8�ϱ��� �����ϰ� ������ �����鿡�� �������� �شٰ� �� ���
-- ������ ��ĵ�ؾ� �Ѵ�. ���� �̴� �ſ� �߿��� ����!
SELECT *
FROM TestOrders WITH(INDEX(idx_emp_ord))
WHERE EmployeeID = 1 AND OrderDate BETWEEN '19970101' AND '19970103';
-- ���� �б� 5

SELECT *
FROM TestOrders WITH(INDEX(idx_ord_emp))
WHERE EmployeeID = 1 AND OrderDate BETWEEN '19970101' AND '19970103';
-- ���� ����� �Ȱ����� ���� �бⰡ 5, 16���� 3�� ���̰� ���� ���� �� �� �ִ�.
-- �׷��� �Ʊ�� �Ȱ��Ҵµ� �� �̹����� �ٸ���?
-- �̸� �˾ƺ��� ���ؼ� �ٽ� �� ���� �˾ƺ��� �ð��� ������.

-- ���� ���캸��
SELECT *
FROM TestOrders
ORDER BY EmployeeID, OrderDate;

SELECT *
FROM TestOrders
ORDER BY OrderDate, EmployeeID;
-- �Ʒ����� OrderDate ������ ������ �Ǿ� �ֱ� ������ 97�� 1�� 1�Ϻ��� 3�ϱ��� ��� ������ Ȯ���ϰ� EmployeeID�� ��ĵ�ؾ� �ϱ� ������ ���� �ɸ���.

-- �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

-- ���
-- [!] Index(a, b, c)�� �����Ǿ��� ��, ���࿡ between�� ����� ��� ������ �ε��� ����� Ȱ������ ���Ѵ�.
-- ���� between�� ����� ��� ���࿡�ٰ� �ɾ��ֵ��� �ε����� �� ��ġ�ؾ� �Ѵ�.
-- �׷��ٸ� between�� ���� �񱳰� �����ϸ� �ε��� ������ �ٲ��ָ� �Ǵ� ���ϱ�? �� �翬�� 'NO'�̴�.
-- �ֳ��ϸ� SQL�� �ۼ��� �� �ϳ��� ���Ǹ����� �¸� ������� ���� ���̴�. ���� SQL ������ �̿��� ���̺��� Ȱ���� ���̱⿡ �ϳ��� ���̽��� ���� �ε����� �߰��ϰ� �����ϴ� ���� ������ �� �ִ�.
-- ���� ��� ���̽��� �� ���� �ٸ� �������� ������ ������ ���� �����ؼ� ���������� ��������� �Ѵ�.

-- ��
-- BETWEEN�� ������ ���� �� �� IN-LIST�� ��ü�ϴ� ���� �������. (��ǻ� ������ =)
SET STATISTICS PROFILE ON;

SELECT *
FROM TestOrders WITH(INDEX(idx_ord_emp))
WHERE EmployeeID = 1 AND OrderDate IN ('19970101', '19970102', '19970103');
-- ���� �бⰡ 16���� 11�� �پ�� ���� �� �� �ִ�.
-- OR���� ���� �����͸� ã�� ���� �� �� �ִ�. ���� 970101�� �ѹ�, 970102�� �ѹ�, 970103���� �ѹ� ��ȸ�Ѵ�.
-- �׷��ٰ��ؼ� BETWEEN�� �׻� IN-LIST�� �ٲٴ°��� ���� �ʴ�. 
-- ������ EMP�� ORD�� ��ȸ�ϴ� SELECT���� ���캸��.

SELECT *
FROM TestOrders WITH(INDEX(idx_emp_ord))
WHERE EmployeeID = 1 AND OrderDate IN ('19970101', '19970102', '19970103');
-- ���� �бⰡ 5�� �ƴ� 11�� �þ���. ���� ������ ������ �� ��������.
-- ���� ������ IN-LIST�� ����ϴ°� ���� ���� �ƴϴ�.
-- '[!] Index(a, b, c)�� �����Ǿ��� ��, ���࿡ between�� ����� ��� ������ �ε��� ����� Ȱ������ ���Ѵ�.'�� ���� ��Ȳ���� ����ؾ� �Ѵ�.

-- ������ ���

-- ���� �÷� �ε����� ���� �� (����, ����) ������ �ε����� ������ �� �� �ִ�.
-- BETWEEN, �ε�ȣ(>, <)�� ���࿡ ����, ������ �ε����� ����� ����Ѵ�.
-- BETWEEN ������ ������ IN-LIST�� ��ü�ϸ� ���� ��쵵 �ִ�. (���࿡ BETWEEN�� �� ���)
-- ���� ������ (=)�̰�, ������ BETWEEN�̶�� �ƹ��� ������ ���� ������ IN-LIST�� �� ���� ���� �ƴϴ�.