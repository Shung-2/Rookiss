USE BaseballData;

-- 조인 원리
	-- 1) Nested Loop (NL) 조인
	-- 2) Merge(병합) 조인
	-- 3) Hash(해시) 조인

-- 논클러스터드
--   1
-- 2 3 4 

-- 클러스터드
--   1
-- 2 3 4 

-- ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

-- INNER JOIN을 통한 데이터 조회

SELECT *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID;
-- Merge Join을 통해서 결과 값을 보여준다.

SELECT TOP 5 *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID;
--  Nested Loop를 통해서 결과 값을 보여준다. 

SELECT *
FROM salaries AS s
	INNER JOIN teams AS t
	ON s.teamID = t.teamID;
-- Hash를 통해서 결과 값을 보여준다.

SELECT *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID
	OPTION(LOOP JOIN);
-- 옵션을 통해 강제로 특정 조인을 사용할 수 있도록 할 수 있다.
-- Index Scan은 이중 포문을 돈 것과 같고
-- Index Seek가 뜬 것은 내부 자료 구조를 리스트 형태가 아닌 딕셔너리 형태로 사용했다고 무방하다.


-- 또한 분명히 Inner Join을 통해 salary를 사용했지만, 실행 계획을 보면 player가 먼저 온 것이 아니라 salary로 되어 있으며, 내부가 player가 된 걸 볼 수 있다.
-- 이 경우에도 강제로 옵션을 설정할 수 있다.
SELECT *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID
	OPTION(FORCE ORDER, LOOP JOIN);
-- 클러스터드와 클러스터드가 뜬 걸 볼 수 있다.
-- 즉 C#에서 리스트와 리스트를 쓴것과 일하다고 볼 수 있다.

SELECT TOP 5 *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID;
-- 그렇다면 여기서는 어떻게 NL이 떴을까?
-- 정답은 바로 TOP 5이다. 
-- 이는 최종 결과물의 고정이 되어있을 경우 NL 전략이 굉장히 우수하게 작동하는 것과 동일하다.
-- C#에서 사용한 count >= 5와 동일한 맥락인 셈이다.

-- 오늘의 결론 --
-- NL 특징
-- 먼저 액세스 한 (OUTER) 테이블의 로우를 차례 차례 스캔을 하면서 Inner 테이블에 랜덤 엑세스 한다.
-- INNER 테이블에 인덱스가 없을 경우 노답인 상황..
-- 부분범위 처리에 좋다. (ex. TOP 5)