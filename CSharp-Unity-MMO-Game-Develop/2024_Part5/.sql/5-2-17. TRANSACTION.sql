use BaseballData;

SELECT *
FROM teamshalf;

-- TRAN�� ������� ������, �ڵ����� COMMIT�� ����ȴ�.
INSERT INTO teamshalf VALUES(1981, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL);

-- BEGIN TRAN;
-- COMMIT
-- ROLLBACK

-- Ʈ������� �����ΰ�?

-- �ŷ�
-- A�� �κ��丮���� ������ ����
-- B�� �κ��丮�� ������ �߰�
-- �ŷ� �����ῡ ���� A�� ��� ���� ��..

-- �� ��, B�� �κ��丮�� �������� �߰��� �ƴµ�, 
-- A�� �κ��丮���� ������ ���Ű� ���� ���� ��� ������ �����.
-- ���� �⺻������ ���ڼ�(ALL OR NOTHING)�� ���·� �۾��� ����Ǿ�� �Ѵ�.

-- �� ���¸� �ذ��ϱ� ���� ���� Ʈ������� �����̴�.


--------------------------------------------------------------------------------

-- ���� �� BEGIN TRAN
--- ������ ���� ���ΰ�? �� COMMIT
--- ������ ����� ���ΰ�? �� ROLLBACK

-- ����/���� ���ο� ���� COMMIT (=COMMIT�� �������� �ϰٴ�.)
BEGIN TRAN;
	INSERT INTO teamshalf VALUES(1980, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL); 
ROLLBACK;

BEGIN TRAN;
	INSERT INTO teamshalf VALUES(1980, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL); 
COMMIT;

-- ����
BEGIN TRY
	BEGIN TRAN;
		INSERT INTO teamshalf VALUES(1980, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL); 
		INSERT INTO teamshalf VALUES(1980, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL); 
	COMMIT;
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 -- ���� Ȱ��ȭ�� Ʈ����� ���� ��ȯ�Ѵ�.
		ROLLBACK
END CATCH

-- TRAN ����� �� ������ ��
-- TRAN �ȿ��� ��!!! ���������� ����� �ֵ鸸 �־ �������.
-- C# List<Player> List<Salary> ���������� ���� -> lock�� ��Ƽ� ���� -> writelock (��ȣ��Ÿ���� ��) readlock (���� ��)

BEGIN TRAN;
	INSERT INTO teamshalf VALUES(1985, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL); 
ROLLBACK;