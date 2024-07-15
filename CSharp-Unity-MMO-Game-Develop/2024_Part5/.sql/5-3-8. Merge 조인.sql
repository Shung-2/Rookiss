USE BaseballData;

-- Merge(병합) 조인 = Sort Merge(정렬 병합) 조인

-- 지난 시간에 확인했던 MERGE JOIN
SELECT *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID;
-- 실행 계획을 보면 클러스터드 인덱스가 뜨고, 이를 소팅해주고, 소팅한 결과를 머지 조인하는 걸 알 수 있다. 그렇다면 원리는 어떻게 되는걸까?
-- 오늘도 마찬가지로 C#을 통해 알아보도록 하자.

-- ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

SET STATISTICS TIME ON;
SET STATISTICS IO ON;
SET STATISTICS PROFILE ON;

-- 논클러스터드
--	 1
-- 2 3 4

-- 클러스터드
--	 1
-- 2 3 4

-- Heap Table [ {Page}, {Page} ]

-- 데이터 조회
SELECT *
FROM players AS p
	INNER JOIN salaries AS s
	ON p.playerID = s.playerID;
-- 실행 계획을 살펴보면 Many-To-Many 방식으로 돌아가고 있는 것을 볼 수 있다.
-- 실행 계획을 살펴보면 salaries를 소팅하고, players를 소팅하고 머지를 하고 있는 것을 알 수 있다.

-- 소팅 비용도 무시 못하는데 왜 소팅을 해야할까?
-- 

-- 그리고 클러스터드 인덱스 스캔이 양쪽으로 뜨고 있는데
-- 이는 리프페이지를 쫘-악 스캔하는 것을 클러스터드 스캔이라고 했다.

-- 머지 조인도 조건이 붙는다. 더 빠르게 동작하기 위해서는 outer가 unique 해야한다.)
-- 위 select 문에서 아쉬운 이유가 many-to-many였기 때문에 이를 one-to-many 조건으로 변경해야 가장 빠른데 그렇지 못한것이다.

-- One-To-Many (outer가 unique해야 함 => 프라이머리키(Pk), unique)
-- 일일히 랜덤 엑세스를 하는 것이 아닌 → 클러스터드 스캔 후 정렬 하는 판단도 있다.

-- 그렇다면 인덱스 스캔이 아닌, 정렬되어 있는 데이터를 사용하면 어떨까?
-- schools과 school player 테이블을 활용해 테스트해보자.

SELECT *
FROM schools AS s
	INNER JOIN schoolsplayers AS p
	ON s.schoolID = p.schoolID;
-- 실행 계획을 보면 정렬을 스킵한 것을 알 수 있다.
-- 근데 논 클러스터드 인덱스를 사용하고 있다!
-- school player의 인덱스를 살펴보면 모든 조건으로 인덱스를 붙인 애가 있어서 그럼!
-- 머지도 many-to-many가 비활성화 된 상태! 더 좋은 상태다.

-- 오늘의 결론
-- Merge → Sort Mearge로 알아두자.
-- 1) 양쪽 집함을 Sort(정렬)하고 Merge(병합)한다.
	-- 이미 정렬된 상태라면 Sort는 생략. 특히 클러스터드로 물리적 정렬된 상태라면 Best이다.
	-- 정렬할 데이터가 너무 많으면 좋은 상황이 아니며, Hash를 이용하는게 더 나을 수 있다.
-- 2) 랜덤 엑세스 위주로 수행되지 않는다.
-- 3) Many-To-Many 즉 다대다보다는 One-to-Many(일대다) 조인에 효과적이다.
	-- PK, UNIQUE가 붙었을 때 보다 효과적이다.