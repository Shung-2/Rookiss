USE BaseballData;

-- 윈도우 함수
-- 행들의 서브 집합을 대상으로, 각 행별로 계산을 해서 스칼라(단일 고정) 값을 출력하는 함수.

-- 느낌상 GROUPING이랑 비슷한가?
-- SUM, COUNT, AVG 집계 함수

SELECT *
FROM salaries
ORDER BY salary DESC;

SELECT playerID, MAX(salary)
FROM salaries
GROUP BY playerID
ORDER BY MAX(salary) DESC;

-- 헤딩하면서 배우는 윈도우 함수
-- ~OVER ([PARTIOTION] [ORDER BY] [ROWS])
-- 3개를 무조건 사용해야 하는 것은 아니다.

-- 전체 데이터를 연봉 순서로 나열하고, 순위를 표기해보자.
SELECT *,
	ROW_NUMBER() OVER (ORDER BY salary DESC) AS ROWNUM, -- 행#번호
	RANK() OVER (ORDER BY salary DESC) AS RNK, -- 랭킹
	DENSE_RANK() OVER (ORDER BY salary DESC) AS DRNK, -- 랭킹
	NTILE(100) OVER (ORDER BY salary DESC) AS PERCENTAGE -- 백분율 (상위 몇 %)
FROM salaries;

-- playerID 별 순위를 따로 하고 싶다면
SELECT *,
	RANK() OVER (PARTITION BY playerID ORDER BY salary DESC)
FROM salaries
ORDER BY playerID;

-- LAG(바로 이전), LEAD(바로 다음)
SELECT *,
	RANK() OVER (PARTITION BY playerID ORDER BY salary DESC),
	LAG(salary) OVER (PARTITION BY playerID ORDER BY salary DESC) AS prevSalary,
	LEAD(salary) OVER (PARTITION BY playerID ORDER BY salary DESC) AS prevSalary
FROM salaries
ORDER BY playerID;

-- FIRST VALUE(첫 값), LAST VALUE(마지막 값)
-- FRAME : FIRST ~ CURRENT
SELECT *,
	RANK() OVER (PARTITION BY playerID ORDER BY salary DESC),
	FIRST_VALUE(salary) OVER (PARTITION BY playerID ORDER BY salary DESC) AS best,
	LAST_VALUE(salary) OVER (PARTITION BY playerID ORDER BY salary DESC) AS worst
FROM salaries
ORDER BY playerID;

-- 위 질의는 worst의 값이 정상적으로 나오지 않는데 worst는 현재 행에서 가장 작은 값을 찾으므로
-- 1행에서는 값이 450~이 되고, 2번 행에서는 275~의 값이 되며
-- 3번 행에서부터 50~이 된 걸 볼 수 있다.
-- 따라서 이를 해결하기 위해서는 row의 값을 수정해줘야 한다.

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

-- UNBOUNDED PRECEDING는 무한으로 앞으로 가도 된다. 뜻
-- UNBOUNDED FOLLOWING는 앞으로 가다. 뜻 즉 처음부터 나까지. 나부터 마지막까지의 의미가 된다.