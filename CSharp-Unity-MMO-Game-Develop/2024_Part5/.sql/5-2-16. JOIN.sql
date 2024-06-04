-- JOIN (����)

USE BaseballData;

CREATE TABLE testA
(
	a INTEGER
)

CREATE TABLE testB
(
	b VARCHAR(10)
)

-- A(1, 2, 3)
INSERT INTO testA VALUES(1);
INSERT INTO testA VALUES(2);
INSERT INTO testA VALUES(3);

-- B('A', 'B', 'C')
INSERT INTO testB VALUES('A');
INSERT INTO testB VALUES('B');
INSERT INTO testB VALUES('C');

-- CROSS JOIN (���� ����) N*N�� ���� ������.
-- ��� 1
SELECT *
FROM testA
	CROSS JOIN testB;

-- ��� 2
SELECT *
FROM testA, testB;

-- �����Ͱ� ���� �� ��� �δ��� �Ǵ� �����̹Ƿ� �����ؾ� �Ѵ�.

----------------------------------------------------

USE BaseballData;

SELECT *
FROM players
ORDER BY playerID;

SELECT *
FROM salaries
ORDER BY playerID;

-- INNER JOIN (�� ���� ���̺��� ���η� ���� + ���� ������ ON����)
-- PlayerID�� players, salaries ���ʿ� �� �ְ�, ��ġ�ϴ� �ֵ��� �����Ѵ�.

SELECT *
FROM players AS p
INNER JOIN salaries AS s
ON p.playerID = s.playerID;

-- OUTER JOIN (�ܺ� ����)
-- LEFT / RIGHT 
-- ��� ���ʿ��� �����ϴ� ������ -> ��å�� ���� �ҷ�?

-- LEFT JOIN
-- PlayerID�� ����(Left)�� ������ ������ ǥ��. ������(salaries)�� ������ ������ ������ NULL�� ä��.

SELECT *
FROM players AS p
	LEFT JOIN salaries AS s
	ON p.playerID = s.playerID;

-- RIGHT JOIN
-- PlayerID�� ������(Right)�� ������ ������ ǥ��. ����(players)�� ������ ���� ������ NULL�� ä��.

SELECT *
FROM players AS p
	RIGHT JOIN salaries AS s
	ON p.playerID = s.playerID;
