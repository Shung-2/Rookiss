USE BaseballData;

-- Sorting(정렬)을 줄이자.
-- 정렬은 사실 위험한 놈!
-- 일반적인 시간복잡도는 O(NlogN) 따라서 한 두번 하는건 그렇게 위험하지 않다.

-- 단, DB는 데이터가 어마어마하게 많기 때문에 유의해야 한다.
-- 용량이 커서 가용 메모리로 커버가 안되는 상황이 온다.
-- 메모리가 부족하기 때문에 디스크까지 찾아간다.
-- 메모리 접근과 디스크 접근의 속도 차이는 몇 십배이상 나기 떄문에 유의해야 한다.
-- Sorting이 언제 일어나는지 파악하고 있어야 한다.

-- Sorting이 일어날 때
-- 1) SORT MERGE JOIN
	-- 원인) 알고리즘 특성 상 MERGE 하기 전에 SORT를 해야 함
-- 2) ORDER BY
-- 3) GROUP BY
-- 4) DISTINCT
-- 5) UNION
-- 6) RANKING WINDOWS FUNCTION
-- 7) MIN, MAX
	-- 원인) 테스트 하면서 알아보도록 하자

-- 1) 생략
-- 2) ORDER BY
-- 테이블 조회
SELECT *
FROM players
ORDER BY college;
-- 원인) ORDER BY 순서로 정렬을 해야하기 때문에 SORT를 한다.

-- INDEX를 활용한 ORDER BY
SELECT *
FROM batting
ORDER BY playerID, yearID;
-- batting 테이블의 index를 활용할 경우 Sorting이 사라지는 것을 볼 수 있다.

-- 3) GROUP BY
SELECT college, COUNT(college)
FROM players
WHERE college LIKE 'C%'
GROUP BY college;
-- 원인) 집계를 하기 위해 SORT를 한다.

-- INDEX를 활용한 GROUP BY
SELECT playerID, COUNT(playerID)
FROM players
WHERE playerID LIKE 'C%'
GROUP BY playerID;
-- 마찬가지로 index를 활용했기 때문에 Sorting이 사라진 것을 볼 수 있다.

-- 4) DISTINCT
SELECT DISTINCT college
FROM players
WHERE college LIKE 'C%';
-- 원인) 중복을 제거하기 위해 SORT를 한다

-- 5) UNION
SELECT college
FROM players
WHERE college LIKE 'B%'
UNION
SELECT college
FROM players
WHERE college LIKE 'C%';
-- 원인) 중복을 제거하기 위해 SORT를 한다.

-- 4, 5번 같은 경우 2,3 번과 과가 다르기 떄문에 사용하기 전에 '정말로 필요한가?'를 다시 생각해보고 사용하도록 하자.

-- 6) 순위 윈도우 함수
SELECT ROW_NUMBER() OVER (ORDER BY college)
FROM players;
-- 원인) 집계를 하기 위해 SORT를 한다.

-- INDEX를 잘 활용하면, Sorting을 굳이 하지 않아도 됨

-- 오늘의 결론
-- Sorting(정렬)을 줄이자.
-- 정렬은 사실 위험한 놈!
-- 일반적인 시간복잡도는 O(NlogN) 따라서 한 두번 하는건 그렇게 위험하지 않다.

-- 단, DB는 데이터가 어마어마하게 많기 때문에 유의해야 한다.
-- 용량이 커서 가용 메모리로 커버가 안되는 상황이 온다.
-- 메모리가 부족하기 때문에 디스크까지 찾아간다.
-- 메모리 접근과 디스크 접근의 속도 차이는 몇 십배이상 나기 떄문에 유의해야 한다.