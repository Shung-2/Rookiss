USE BaseballData;

-- ������ �Լ�
-- ����� ���� ������ �������, �� �ະ�� ����� �ؼ� ��Į��(���� ����) ���� ����ϴ� �Լ�.

-- ������ GROUPING�̶� ����Ѱ�?
-- SUM, COUNT, AVG ���� �Լ�

SELECT *
FROM salaries
ORDER BY salary DESC;

SELECT playerID, MAX(salary)
FROM salaries
GROUP BY playerID
ORDER BY MAX(salary) DESC;

-- ����ϸ鼭 ���� ������ �Լ�
-- ~OVER ([PARTIOTION] [ORDER BY] [ROWS])
-- 3���� ������ ����ؾ� �ϴ� ���� �ƴϴ�.

-- ��ü �����͸� ���� ������ �����ϰ�, ������ ǥ���غ���.
SELECT *,
	ROW_NUMBER() OVER (ORDER BY salary DESC) AS ROWNUM, -- ��#��ȣ
	RANK() OVER (ORDER BY salary DESC) AS RNK, -- ��ŷ
	DENSE_RANK() OVER (ORDER BY salary DESC) AS DRNK, -- ��ŷ
	NTILE(100) OVER (ORDER BY salary DESC) AS PERCENTAGE -- ����� (���� �� %)
FROM salaries;

-- playerID �� ������ ���� �ϰ� �ʹٸ�
SELECT *,
	RANK() OVER (PARTITION BY playerID ORDER BY salary DESC)
FROM salaries
ORDER BY playerID;

-- LAG(�ٷ� ����), LEAD(�ٷ� ����)
SELECT *,
	RANK() OVER (PARTITION BY playerID ORDER BY salary DESC),
	LAG(salary) OVER (PARTITION BY playerID ORDER BY salary DESC) AS prevSalary,
	LEAD(salary) OVER (PARTITION BY playerID ORDER BY salary DESC) AS prevSalary
FROM salaries
ORDER BY playerID;

-- FIRST VALUE(ù ��), LAST VALUE(������ ��)
-- FRAME : FIRST ~ CURRENT
SELECT *,
	RANK() OVER (PARTITION BY playerID ORDER BY salary DESC),
	FIRST_VALUE(salary) OVER (PARTITION BY playerID ORDER BY salary DESC) AS best,
	LAST_VALUE(salary) OVER (PARTITION BY playerID ORDER BY salary DESC) AS worst
FROM salaries
ORDER BY playerID;

-- �� ���Ǵ� worst�� ���� ���������� ������ �ʴµ� worst�� ���� �࿡�� ���� ���� ���� ã���Ƿ�
-- 1�࿡���� ���� 450~�� �ǰ�, 2�� �࿡���� 275~�� ���� �Ǹ�
-- 3�� �࿡������ 50~�� �� �� �� �� �ִ�.
-- ���� �̸� �ذ��ϱ� ���ؼ��� row�� ���� ��������� �Ѵ�.

SELECT *,
	RANK() OVER (PARTITION BY playerID ORDER BY salary DESC),
	FIRST_VALUE(salary) OVER (PARTITION BY playerID 
								ORDER BY salary DESC
								ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
								) AS best,
	LAST_VALUE(salary) OVER (PARTITION BY playerID
								ORDER BY salary DESC
								ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING
								) AS worst
FROM salaries
ORDER BY playerID;

-- UNBOUNDED PRECEDING�� �������� ������ ���� �ȴ�. ��
-- UNBOUNDED FOLLOWING�� ������ ����. �� �� ó������ ������. ������ ������������ �ǹ̰� �ȴ�.