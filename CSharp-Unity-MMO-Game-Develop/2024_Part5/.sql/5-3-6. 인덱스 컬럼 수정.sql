USE Northwind;

-- 복합 인덱스 컬럼 순서
-- Index(A, B, C)

-- 논클러스터드
--	 1
-- 2 3 4

-- 클러스터드
--	 1
-- 2 3 4[(56 휴먼) (56 휴먼) (56 휴먼) (56 휴먼) (56 휴먼)...]

-- Heap Table [ {Page}, {Page} ]

-- 북마크 룩업
-- 이전 시간에 북마크 룩업을 어떻게 최소화할까에 대해서 학습했다.
-- 그러나 과연 '북마크 룩업을 최소화하는 것이 최적화의 끝인가?'라고 물으면 그걸로는 끝이 아니다. 라고 대답할 수 있다.
-- 왜냐하면 Leaf Page에 대한 탐색은 여전히 존재하기 때문이다.

-- 예를들어 [레벨, 종족]에 대해서 인덱스를 걸었을 때 (56, 휴먼)을 찾는다고 가정한다면
-- 클러스터드 인덱스를 먼저 기준으로 찾을 경우 56, 휴먼이 딱 하나만 존재한다는 보장이 없다.
-- 따라서 4번 리프페이즈에서 56, 휴먼이 아닐때까지 모두 스캔을 처리해서 조사해야 한다.
-- 그러므로 인덱스를 사용했다고 하더라도. 리프페이지에 접근해 페이지를 스캔해야 하는 필요성이 생긴다.
-- 더 극단적인 예로는 56~60 휴먼을 찾는다고 할 경우 테이블 스캔의 범위가 더 넓어진다는 문제가 생긴다.
-- 따라서 인덱스의 순서가 굉장히 큰 영향을 주는 것을 오늘 학습할 것이다.

-- ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

-- 실습
-- 테이블 생성
SELECT *
INTO TestOrders
FROM Orders;

-- 더미 데이터 생성
DECLARE @i INT = 1;
DECLARE @emp INT;
SELECT @emp = MAX(EmployeeID) FROM Orders;

-- 컨닝을 위한 조회
SELECT *
FROM TestOrders;
-- 830개의 행, 여기에 1000개를 곱해줄 것이다. (830 * 1000)

WHILE (@i < 1000)
BEGIN
	INSERT INTO TestOrders(CustomerID, EmployeeID, OrderDate)
	SELECT CustomerID, @emp + @i, OrderDate
	FROM Orders;
	SET @i = @i + 1;
END

-- 데이터가 몇개있을까용?
SELECT COUNT(*)
FROM TestOrders;
-- 83만개!

-- 인덱스 생성
CREATE NONCLUSTERED INDEX idx_emp_ord
ON TestOrders(EmployeeID, OrderDate);

CREATE NONCLUSTERED INDEX idx_ord_emp
ON TestOrders(OrderDate, EmployeeID);

-- 과연 어느쪽이 더 빠를까? 이를 테스트 해보자.
SET STATISTICS TIME ON;
SET STATISTICS IO ON;

-- 인덱스를 강제로 지정하여 두 개를 비교하자.
SELECT *
FROM TestOrders WITH(INDEX(idx_emp_ord))
WHERE EmployeeID = 1 AND OrderDate = CONVERT(DATETIME, '19970101');

SELECT *
FROM TestOrders WITH(INDEX(idx_ord_emp))
WHERE EmployeeID = 1 AND OrderDate = CONVERT(DATETIME, '19970101');
-- 띠용~ 논리적 읽기와 실행 계획이 모두 똑같이 나와버렸다.
-- 왜 똑같이 나왔는지 알아보도록 하자.

-- 직접 살펴보자
SELECT *
FROM TestOrders
ORDER BY EmployeeID, OrderDate;

SELECT *
FROM TestOrders
ORDER BY OrderDate, EmployeeID;

-- 그렇다면 범위로 찾는다면 무슨일이 일어날까?
-- 예를들어 이벤트를 위해 7월 1일부터 7월 8일까지 일주일간 접속한 유저들에게 아이템을 준다고 할 경우
-- 범위로 스캔해야 한다. 따라서 이는 매우 중요한 주제!
SELECT *
FROM TestOrders WITH(INDEX(idx_emp_ord))
WHERE EmployeeID = 1 AND OrderDate BETWEEN '19970101' AND '19970103';
-- 논리적 읽기 5

SELECT *
FROM TestOrders WITH(INDEX(idx_ord_emp))
WHERE EmployeeID = 1 AND OrderDate BETWEEN '19970101' AND '19970103';
-- 실행 결과는 똑같으나 논리적 읽기가 5, 16으로 3배 차이가 나는 것을 알 수 있다.
-- 그러면 아까는 똑같았는데 왜 이번에는 다를까?
-- 이를 알아보기 위해서 다시 또 직접 알아보는 시간을 가지자.

-- 직접 살펴보자
SELECT *
FROM TestOrders
ORDER BY EmployeeID, OrderDate;

SELECT *
FROM TestOrders
ORDER BY OrderDate, EmployeeID;
-- 아래꺼는 OrderDate 순으로 정렬이 되어 있기 때문에 97년 1월 1일부터 3일까지 모든 정보를 확인하고 EmployeeID를 스캔해야 하기 때문에 오래 걸린다.

-- ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

-- 결론
-- [!] Index(a, b, c)로 구성되었을 때, 선행에 between을 사용할 경우 후행은 인덱스 기능을 활용하지 못한다.
-- 따라서 between을 사용할 경우 후행에다가 걸어주도록 인덱스를 잘 배치해야 한다.
-- 그렇다면 between와 같은 비교가 등장하면 인덱스 순서만 바꿔주면 되는 것일까? → 당연히 'NO'이다.
-- 왜냐하면 SQL을 작성할 때 하나의 조건만으로 걔를 사용하진 않을 것이다. 따라서 SQL 구믄을 이용해 테이블을 활용할 것이기에 하나의 케이스만 놓고 인덱스를 추가하고 수정하는 것은 위험할 수 있다.
-- 따라서 모든 케이스를 다 보고 다른 쿼리에도 영향을 줄지를 같이 생각해서 종합적으로 결정지어야 한다.

-- 팁
-- BETWEEN의 범위가 작을 때 → IN-LIST로 대체하는 것을 고려하자. (사실상 여러번 =)
SET STATISTICS PROFILE ON;

SELECT *
FROM TestOrders WITH(INDEX(idx_ord_emp))
WHERE EmployeeID = 1 AND OrderDate IN ('19970101', '19970102', '19970103');
-- 논리적 읽기가 16에서 11로 줄어든 것을 볼 수 있다.
-- OR문을 통해 데이터를 찾는 것을 볼 수 있다. 따라서 970101로 한번, 970102로 한번, 970103으로 한번 조회한다.
-- 그렇다고해서 BETWEEN을 항상 IN-LIST로 바꾸는것은 옳지 않다. 
-- 기존에 EMP와 ORD로 조회하는 SELECT문을 살펴보자.

SELECT *
FROM TestOrders WITH(INDEX(idx_emp_ord))
WHERE EmployeeID = 1 AND OrderDate IN ('19970101', '19970102', '19970103');
-- 논리적 읽기가 5가 아닌 11개 늘었다. 따라서 오히려 성능이 더 떨어졌다.
-- 따라서 무작정 IN-LIST를 사용하는게 좋은 것은 아니다.
-- '[!] Index(a, b, c)로 구성되었을 때, 선행에 between을 사용할 경우 후행은 인덱스 기능을 활용하지 못한다.'와 같은 상황에서 고려해야 한다.

-- 오늘의 결론

-- 복합 컬럼 인덱스를 만들 때 (선행, 후행) 순서가 인덱스의 영향을 줄 수 있다.
-- BETWEEN, 부등호(>, <)가 선행에 들어가면, 후행은 인덱스의 기능을 상실한다.
-- BETWEEN 범위가 적으면 IN-LIST로 대체하면 좋은 경우도 있다. (선행에 BETWEEN이 들어갈 경우)
-- 만약 선행이 (=)이고, 후행이 BETWEEN이라면 아무런 문제가 없기 때문에 IN-LIST가 꼭 좋은 것은 아니다.