USE BaseballData;

-- Sorting(����)�� ������.
-- ������ ��� ������ ��!
-- �Ϲ����� �ð����⵵�� O(NlogN) ���� �� �ι� �ϴ°� �׷��� �������� �ʴ�.

-- ��, DB�� �����Ͱ� ���ϰ� ���� ������ �����ؾ� �Ѵ�.
-- �뷮�� Ŀ�� ���� �޸𸮷� Ŀ���� �ȵǴ� ��Ȳ�� �´�.
-- �޸𸮰� �����ϱ� ������ ��ũ���� ã�ư���.
-- �޸� ���ٰ� ��ũ ������ �ӵ� ���̴� �� �ʹ��̻� ���� ������ �����ؾ� �Ѵ�.
-- Sorting�� ���� �Ͼ���� �ľ��ϰ� �־�� �Ѵ�.

-- Sorting�� �Ͼ ��
-- 1) SORT MERGE JOIN
	-- ����) �˰��� Ư�� �� MERGE �ϱ� ���� SORT�� �ؾ� ��
-- 2) ORDER BY
-- 3) GROUP BY
-- 4) DISTINCT
-- 5) UNION
-- 6) RANKING WINDOWS FUNCTION
-- 7) MIN, MAX
	-- ����) �׽�Ʈ �ϸ鼭 �˾ƺ����� ����

-- 1) ����
-- 2) ORDER BY
-- ���̺� ��ȸ
SELECT *
FROM players
ORDER BY college;
-- ����) ORDER BY ������ ������ �ؾ��ϱ� ������ SORT�� �Ѵ�.

-- INDEX�� Ȱ���� ORDER BY
SELECT *
FROM batting
ORDER BY playerID, yearID;
-- batting ���̺��� index�� Ȱ���� ��� Sorting�� ������� ���� �� �� �ִ�.

-- 3) GROUP BY
SELECT college, COUNT(college)
FROM players
WHERE college LIKE 'C%'
GROUP BY college;
-- ����) ���踦 �ϱ� ���� SORT�� �Ѵ�.

-- INDEX�� Ȱ���� GROUP BY
SELECT playerID, COUNT(playerID)
FROM players
WHERE playerID LIKE 'C%'
GROUP BY playerID;
-- ���������� index�� Ȱ���߱� ������ Sorting�� ����� ���� �� �� �ִ�.

-- 4) DISTINCT
SELECT DISTINCT college
FROM players
WHERE college LIKE 'C%';
-- ����) �ߺ��� �����ϱ� ���� SORT�� �Ѵ�

-- 5) UNION
SELECT college
FROM players
WHERE college LIKE 'B%'
UNION
SELECT college
FROM players
WHERE college LIKE 'C%';
-- ����) �ߺ��� �����ϱ� ���� SORT�� �Ѵ�.

-- 4, 5�� ���� ��� 2,3 ���� ���� �ٸ��� ������ ����ϱ� ���� '������ �ʿ��Ѱ�?'�� �ٽ� �����غ��� ����ϵ��� ����.

-- 6) ���� ������ �Լ�
SELECT ROW_NUMBER() OVER (ORDER BY college)
FROM players;
-- ����) ���踦 �ϱ� ���� SORT�� �Ѵ�.

-- INDEX�� �� Ȱ���ϸ�, Sorting�� ���� ���� �ʾƵ� ��

-- ������ ���
-- Sorting(����)�� ������.
-- ������ ��� ������ ��!
-- �Ϲ����� �ð����⵵�� O(NlogN) ���� �� �ι� �ϴ°� �׷��� �������� �ʴ�.

-- ��, DB�� �����Ͱ� ���ϰ� ���� ������ �����ؾ� �Ѵ�.
-- �뷮�� Ŀ�� ���� �޸𸮷� Ŀ���� �ȵǴ� ��Ȳ�� �´�.
-- �޸𸮰� �����ϱ� ������ ��ũ���� ã�ư���.
-- �޸� ���ٰ� ��ũ ������ �ӵ� ���̴� �� �ʹ��̻� ���� ������ �����ؾ� �Ѵ�.