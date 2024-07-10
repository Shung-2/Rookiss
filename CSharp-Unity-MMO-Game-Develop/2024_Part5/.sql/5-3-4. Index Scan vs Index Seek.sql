USE Northwind;

-- 인덱스 접근 방식 (Access)
-- Index Scan vs Index Seek

-- 테이블을 생성합니다.
CREATE TABLE TestAccess
(
	id INT NOT NULL,
	name NCHAR(50) NOT NULL,
	dummy NCHAR(1000) NULL
);
GO

-- 클러스터드 인덱스를 생성합니다.
CREATE CLUSTERED INDEX TestAccess_CI
ON TestAccess(id);
GO

-- 논클러스트드 인덱스를 생성합니다.
CREATE NONCLUSTERED INDEX TestAccess_NCI
ON TestAccess(name);
GO

-- 테스트 데이터를 넣습니다.
DECLARE @i INT;
SET @i = 1;

WHILE (@i <= 500)
BEGIN
	INSERT INTO TestAccess
	VALUES (@i, 'Name' + CONVERT(VARCHAR, @i), 'Hello World' + CONVERT(VARCHAR, @i));
	SET @i = @i + 1;
END

-- 인덱스 정보 확인
EXEC sp_helpindex 'TestAccess';

-- 인덱스 번호 확인
SELECT index_id, name
FROM sys.indexes
WHERE object_id = object_id('TestAccess');

-- 테이블 조회
DBCC IND('Northwind', 'TestAccess', 1);
DBCC IND('Northwind', 'TestAccess', 2);

-- 클러스트가 어떤 식으로 생성되었는지를 살펴보자.
-- CLUSTERED(1) : id
--			8097
-- 944 945 946 ~ 8103 (167)

-- CLUSTERED(2) : name
--			936
-- 937 938 ~ 939 (13)

-- 논리적 읽기 → 실제 데이터를 찾기 위해 읽은 페이지 수
SET STATISTICS TIME ON;
SET STATISTICS IO ON;

-- 실행계획 분석
-- INDEX SCAN → Leaf Page를 순차적으로 검색
SELECT *
FROM TestAccess;

-- INDEX SEEK → 
-- 논리적 읽기 횟수가 2밖에 안됌!
SELECT *
FROM TestAccess
WHERE id = 104;

-- 논클러스트에서 실행
-- INDEX SEEK + KEY LOOKUP
-- 논리적 읽기 횟수 4, KEY LOOKUP(?)도 생김

SELECT *
FROM TestAccess
WHERE name = 'name5';

-- 논리적 읽기 4가 어디서 왔을까?
-- 논클러스트이기 떄문에 클러스터드 인덱스의 키값만 가지고 있다.
-- 따라서 Name5에 해당하는 id 값만 가지고 있는다.
-- 따라서 id값을 바탕으로 클러스트 인덱스로 가서 다시 찾게 된다.
-- 따라서 논 클러스트에서 1, 2 클러스트에서 3, 4의 스텝을 밟게 된다.

-- INDEX SCAN + KEY LOOKUP
-- 경우에 따라 INDEX SCAN이 나쁜것은 아닌데, 이 상황이 바로 그 상황이다.
SELECT TOP 5 *
FROM TestAccess
ORDER BY name;

-- 따라서 INDEX SCAN이 떴다고해서 무조건 나쁜것은 아니다.
-- 그렇다면 INDEX SCAN임에도 불구하고 왜 논리적 읽기가 13밖에 안될까?
-- 해답은 바로 TOP와 ORDER BY에 있다.
-- NAME은 논클러스트 인덱스이므로 논클러스트 테이블의 LEAF만 순회하면 되고 애당초 TOP5만 출력하기 때문에 오래걸리지 않는다.
-- N * 2 + @의 논리적 읽기 설계의 값을 가진다.

-- 따라서 무조건 INDEX SEEK 떠서 좋은 것이 아니고, INDEX SCAN이 떠서 나쁜 것도 아니다.
-- 그렇다면 '무엇이 좋냐?'라고 물으면 이는 케이스 바이 케이스가 된다.