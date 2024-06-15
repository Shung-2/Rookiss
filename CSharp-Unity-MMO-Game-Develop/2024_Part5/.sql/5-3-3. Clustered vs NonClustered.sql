USE Northwind;

-- �ε��� ����
-- Clustered(���� ����) vs Non-Clustered(����)

-- Clustered
	-- Leaf Page = Data Page
	-- �����ʹ� Clustered Index Ű ������ ������ �ȴ�.

-- Non-Clustered ? (��� Clustered Index ������ ���� �ٸ��� ����)
-- 1) Clustered Index�� ���� ���
	-- Clustered Index�� ������ �����ʹ� Heap Table�̶�� ���� ����
	-- Heap RID > Heap Table�� �����Ͽ� �����͸� �����Ѵ�. 

-- 2) Clustered Index�� �ִ� ���
	-- Heap Table�� ���� ����. Leaf Table�� ���� �����Ͱ� �����Ѵ�.
	-- Clustered Index�� ���� Ű ���� ��� �ִ´�.

-- �ӽ� �׽�Ʈ ���̺��� ����� �����͸� ��������.
SELECT * 
INTO TestOrderDetails
FROM [Order Details]; 

-- ������ ��ȸ
SELECT *
FROM TestOrderDetails;

-- Non-Clustered �ε��� �߰�
CREATE INDEX Index_OrderDetails
ON TestOrderDetails(OrderID, ProductID);

-- �ε��� ���� Ȯ��
EXEC sp_helpindex 'TestOrderDetails';

-- �ε��� ��ȣ ã��
SELECT index_id, name
FROM sys.indexes
WHERE OBJECT_ID = object_id('TestOrderDetails');

-- ��ȸ
-- Page Type 1 �� Data Page�� �ǹ��Ѵ�.
-- Page Type 2 �� Index Page�� �ǹ��Ѵ�.
DBCC IND('Northwind', 'TestOrderDetails', 2);

--				976
-- 920 944 945 946 947 948
-- Heap RID ([������ �ּ�(4)][����ID(2)][����(2)] ROW)
-- Heap Table[ {Page} {Page} {Page} {Page} ]

-- ������ ��ȸ
DBCC PAGE('Northwind', 1, 1040, 3);

-- Clustered Index �߰�
CREATE CLUSTERED INDEX Index_OrderDetails_Clustered2
ON TestOrderDetails(OrderID);

-- �ε��� ��ȸ
DBCC IND('Northwind', 'TestOrderDetails', 1);

--					9200
-- 9160 9168 9169 9170 9171 9172 9173 9174 9175 9176
-- ���������� �����͸� �����ִ� ����