use Northwind;

-- Hash(해시) 조인

-- TestOrders 테이블 생성
SELECT *
INTO TestOrders
FROM Orders;

-- TestCustomers 테이블 생성
SELECT *
INTO TestCustomers
FROM Customers;

-- 데이터 조회
SELECT * FROM TestOrders; -- 830개 데이터
SELECT * FROM TestCustomers; -- 91개 데이터
-- CustomerID로 짝을 맞춰 살펴보면 데이터를 추출할 수 있다.

-- HASH를 통한 데이터 조회
SELECT *
FROM TestOrders AS o
	INNER JOIN TestCustomers AS c
	ON o.CustomerID = c.CustomerID;

-- NL로 동작시키면 어떻게 될까?
SELECT *
FROM TestOrders AS o
	INNER JOIN TestCustomers AS c
	ON o.CustomerID = c.CustomerID
	OPTION (FORCE ORDER, LOOP JOIN);
-- TestOrders가 아우터, TestCustomers가 INNER가 되서 하나하나씩 물어본다.
-- INNER TABLE에 INDEX가 없어서 오래 걸린다.

-- MERGE로 동작시키면 어떻게 될까?
SELECT *
FROM TestOrders AS o
	INNER JOIN TestCustomers AS c
	ON o.CustomerID = c.CustomerID
	OPTION (FORCE ORDER, MERGE JOIN);
-- 양쪽 모두 소팅하고, 다대 다 까지 TRUE로 된 것을 볼 수 있다.

-- HASH를 통한 데이터 조회
SELECT *
FROM TestOrders AS o
	INNER JOIN TestCustomers AS c
	ON o.CustomerID = c.CustomerID;

-- 데이터가 작은 쪽으로 해시 테이블을 만들어 주는게 유리할까?
-- 데이터가 큰 쪽으로 해시 테이블을 만들어 주는 것이 유리할까?
-- 실행계획을 살펴보면 TestCustomers를 이용한 걸 보니 데이터가 작은 쪽으로 해시 테이블을 만들어 준 것을 볼 수 있다.

-- 오늘의 결론
-- 해시 조인의 특징
-- 1) 정렬이 필요하지 않다. → 데이터가 너무 많아서 머지가 부담스러울 때 오늘 배운 Hash가 대안이 될 수 있다.
-- 2) 인덱스 유뮤에 영향을 받지 않는다. (★★★★★)
	-- NL/Merge에 비해 확실한 장점!
	-- HashTable 자체를 만드는 비용을 무시하면 안된다. (수행 빈도가 많은 쿼리를 만들 경우, 결국 인덱스를 추가해줘서 관리를 하는 것이 더 좋다)
-- 3) 랜덤 엑세스 위주로 수행되지 않는다.
-- 4) 데이터가 적은 쪽을 해시 테이블로 만드는 것이 유리하다.