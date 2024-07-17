use Northwind;

-- Hash(�ؽ�) ����

-- TestOrders ���̺� ����
SELECT *
INTO TestOrders
FROM Orders;

-- TestCustomers ���̺� ����
SELECT *
INTO TestCustomers
FROM Customers;

-- ������ ��ȸ
SELECT * FROM TestOrders; -- 830�� ������
SELECT * FROM TestCustomers; -- 91�� ������
-- CustomerID�� ¦�� ���� ���캸�� �����͸� ������ �� �ִ�.

-- HASH�� ���� ������ ��ȸ
SELECT *
FROM TestOrders AS o
	INNER JOIN TestCustomers AS c
	ON o.CustomerID = c.CustomerID;

-- NL�� ���۽�Ű�� ��� �ɱ�?
SELECT *
FROM TestOrders AS o
	INNER JOIN TestCustomers AS c
	ON o.CustomerID = c.CustomerID
	OPTION (FORCE ORDER, LOOP JOIN);
-- TestOrders�� �ƿ���, TestCustomers�� INNER�� �Ǽ� �ϳ��ϳ��� �����.
-- INNER TABLE�� INDEX�� ��� ���� �ɸ���.

-- MERGE�� ���۽�Ű�� ��� �ɱ�?
SELECT *
FROM TestOrders AS o
	INNER JOIN TestCustomers AS c
	ON o.CustomerID = c.CustomerID
	OPTION (FORCE ORDER, MERGE JOIN);
-- ���� ��� �����ϰ�, �ٴ� �� ���� TRUE�� �� ���� �� �� �ִ�.

-- HASH�� ���� ������ ��ȸ
SELECT *
FROM TestOrders AS o
	INNER JOIN TestCustomers AS c
	ON o.CustomerID = c.CustomerID;

-- �����Ͱ� ���� ������ �ؽ� ���̺��� ����� �ִ°� �����ұ�?
-- �����Ͱ� ū ������ �ؽ� ���̺��� ����� �ִ� ���� �����ұ�?
-- �����ȹ�� ���캸�� TestCustomers�� �̿��� �� ���� �����Ͱ� ���� ������ �ؽ� ���̺��� ����� �� ���� �� �� �ִ�.

-- ������ ���
-- �ؽ� ������ Ư¡
-- 1) ������ �ʿ����� �ʴ�. �� �����Ͱ� �ʹ� ���Ƽ� ������ �δ㽺���� �� ���� ��� Hash�� ����� �� �� �ִ�.
-- 2) �ε��� ���¿� ������ ���� �ʴ´�. (�ڡڡڡڡ�)
	-- NL/Merge�� ���� Ȯ���� ����!
	-- HashTable ��ü�� ����� ����� �����ϸ� �ȵȴ�. (���� �󵵰� ���� ������ ���� ���, �ᱹ �ε����� �߰����༭ ������ �ϴ� ���� �� ����)
-- 3) ���� ������ ���ַ� ������� �ʴ´�.
-- 4) �����Ͱ� ���� ���� �ؽ� ���̺�� ����� ���� �����ϴ�.