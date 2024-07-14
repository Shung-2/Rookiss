USE BaseballData;

-- ���� ����
	-- 1) Nested Loop (NL) ����
	-- 2) Merge(����) ����
	-- 3) Hash(�ؽ�) ����

-- ��Ŭ�����͵�
--   1
-- 2 3 4 

-- Ŭ�����͵�
--   1
-- 2 3 4 

-- �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

-- INNER JOIN�� ���� ������ ��ȸ

SELECT *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID;
-- Merge Join�� ���ؼ� ��� ���� �����ش�.

SELECT TOP 5 *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID;
--  Nested Loop�� ���ؼ� ��� ���� �����ش�. 

SELECT *
FROM salaries AS s
	INNER JOIN teams AS t
	ON s.teamID = t.teamID;
-- Hash�� ���ؼ� ��� ���� �����ش�.

SELECT *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID
	OPTION(LOOP JOIN);
-- �ɼ��� ���� ������ Ư�� ������ ����� �� �ֵ��� �� �� �ִ�.
-- Index Scan�� ���� ������ �� �Ͱ� ����
-- Index Seek�� �� ���� ���� �ڷ� ������ ����Ʈ ���°� �ƴ� ��ųʸ� ���·� ����ߴٰ� �����ϴ�.


-- ���� �и��� Inner Join�� ���� salary�� ���������, ���� ��ȹ�� ���� player�� ���� �� ���� �ƴ϶� salary�� �Ǿ� ������, ���ΰ� player�� �� �� �� �� �ִ�.
-- �� ��쿡�� ������ �ɼ��� ������ �� �ִ�.
SELECT *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID
	OPTION(FORCE ORDER, LOOP JOIN);
-- Ŭ�����͵�� Ŭ�����͵尡 �� �� �� �� �ִ�.
-- �� C#���� ����Ʈ�� ����Ʈ�� ���Ͱ� ���ϴٰ� �� �� �ִ�.

SELECT TOP 5 *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID;
-- �׷��ٸ� ���⼭�� ��� NL�� ������?
-- ������ �ٷ� TOP 5�̴�. 
-- �̴� ���� ������� ������ �Ǿ����� ��� NL ������ ������ ����ϰ� �۵��ϴ� �Ͱ� �����ϴ�.
-- C#���� ����� count >= 5�� ������ �ƶ��� ���̴�.

-- ������ ��� --
-- NL Ư¡
-- ���� �׼��� �� (OUTER) ���̺��� �ο츦 ���� ���� ��ĵ�� �ϸ鼭 Inner ���̺� ���� ������ �Ѵ�.
-- INNER ���̺� �ε����� ���� ��� ����� ��Ȳ..
-- �κй��� ó���� ����. (ex. TOP 5)