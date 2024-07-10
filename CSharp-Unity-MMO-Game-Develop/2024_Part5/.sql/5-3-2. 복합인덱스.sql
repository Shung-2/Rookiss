use Northwind;

-- 주문 상세 정보를 살펴보자
SELECT *
FROM [Order Details]
ORDER BY OrderID;

-- CREATE INTEX를 사용하지 않고
-- 임시 테스트 테이블을 만들고 데이터를 복사해보자.
SELECT *
INTO TestOrderDetails
FROM [Order Details];

-- 조회
SELECT *
FROM TestOrderDetails;

-- 복합 인덱스 추가
CREATE INDEX Index_TestOrderDetails
ON TestOrderDetails(OrderID, ProductID);
-- 세트로 묶어서 인덱스 생성!

-- 인덱스 정보 살펴보기
EXEC sp_helpindex 'TestOrderDetails';

-- 총 4가지로 조회한다.
-- OrderID, ProductID로 조회할 때,
-- ProductID, OrderID로 조회할 때,
-- OrderID 조회할 때,
-- ProductID 조회할 때,

-- 인덱스 적용 테스트 1 > GOOD
SELECT *
FROM TestOrderDetails
WHERE OrderID = 10248 AND ProductID = 11;

-- 인덱스 적용 테스트 2 > GOOD
SELECT *
FROM TestOrderDetails
WHERE ProductID = 11 AND OrderID = 10248;

-- 인덱스 적용 테스트 3 > GOOD
SELECT *
FROM TestOrderDetails
WHERE OrderID = 10248;

-- 인덱스 적용 테스트 4 > BAD
SELECT *
FROM TestOrderDetails
WHERE ProductID = 11;

-- 결과값은 다음과 같다
-- INDEX SCAN (=INDEX FULL SCAN) > BAD - 풀스캔을 다 때리고 있으므로 나쁜 상황
-- INDEX SEEK > GOOD - 인덱스를 정상적으로 활용이 되는 상태

-- 인덱스 정보를 살펴보자
DBCC IND('Northwind', 'TestOrderDetails', 2);

--				992
-- 936	960	961	962	963	964
DBCC PAGE('Northwind', 1, 936, 3);

-- 따라서 인덱스(A, B)를 사용하겠다고 명시한 상태라면, 인덱스(A)의 정보는 작성하지 않아도 무방하다.
-- 하지만 인덱스 B로도 검색이 필요하다면 > 인덱스 B는 별도로 걸어줘야 한다.

------------------------------------------------------------------------------------------------

-- 인덱스는 데이터가 추가/갱신/삭제 유지되어야 함
-- 데이터 50개를 강제로 넣어보자.

DECLARE @i INT = 0;
WHILE @i < 50
BEGIN
	INSERT INTO TestOrderDetails
	VALUES (10248, 100 + @i, 10, 1, 0);
	SET @i = @i +1;
END;

-- INDEX 정보 조회
DBCC IND('Northwind', 'TestOrderDetails', 2);

--				992
-- 936	[993]	960	961	962	963	964
DBCC PAGE('Northwind', 1, 936, 3);
DBCC PAGE('Northwind', 1, 993, 3);
-- 936에 있던 데이터가 너무 많아서 넘치면 분리해서 페이지를 쪼개서 관리하는 것을 볼 수 있다.
-- 결론 : 페이지 여유 공간이 없다면 > 페이지 분할(SPLIT) 발생

-- 가공 테스트
SELECT LastName
INTO TestEmployees
FROM Employees;

-- 조회
SELECT * FROM TestEmployees;

-- LastName에 인덱스 추가
CREATE INDEX Index_TestEmployees
ON TestEmployees(LastName);

-- INDEX SCAN > BAD
SELECT *
FROM TestEmployees
WHERE SUBSTRING(LastName, 1, 2) = 'Bu';

-- LastName을 순정으로 쓰는 것이 아닌, SUBSTRING과 같은 함수를 쓸 경우
-- 인덱스를 걸어주었다고 해서 100% 확률로 무조건 찾아줄 수 있다는 보장은 없다.
-- 키를 가공할 때에는 굉장히 조심해야 한다.

-- INDEX SEEK를 하려면?
SELECT *
FROM TestEmployees
WHERE LastName LIKE 'Bu%';

-- 오늘의 결론
-- 복합 인덱스(A, B)를 사용할 때 순서를 주의해야 한다 (A->B 순서로 검색한다.)
-- 인덱스 사용 시, 데이터가 추가로 인해 페이지 여유 공간이 없으면 SPLIT을 통해 새로운 페이지르 만든다.
-- 키를 가공할 때에는 주의하자.