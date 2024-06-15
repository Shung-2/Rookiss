USE Northwind;

-- 인덱스 종류
-- Clustered(영한 사전) vs Non-Clustered(색인)

-- Clustered
	-- Leaf Page = Data Page
	-- 데이터는 Clustered Index 키 순서로 정렬이 된다.

-- Non-Clustered ? (사실 Clustered Index 유무에 따라 다르게 동작)
-- 1) Clustered Index가 없는 경우
	-- Clustered Index가 없으면 데이터는 Heap Table이라는 곳에 저장
	-- Heap RID > Heap Table에 접근하여 데이터를 추출한다. 

-- 2) Clustered Index가 있는 경우
	-- Heap Table이 없는 상태. Leaf Table에 실제 데이터가 존재한다.
	-- Clustered Index의 실제 키 값을 들고 있는다.

-- 임시 테스트 테이블을 만들고 데이터를 복사하자.
SELECT * 
INTO TestOrderDetails
FROM [Order Details]; 

-- 데이터 조회
SELECT *
FROM TestOrderDetails;

-- Non-Clustered 인덱스 추가
CREATE INDEX Index_OrderDetails
ON TestOrderDetails(OrderID, ProductID);

-- 인덱스 정보 확인
EXEC sp_helpindex 'TestOrderDetails';

-- 인덱스 번호 찾기
SELECT index_id, name
FROM sys.indexes
WHERE OBJECT_ID = object_id('TestOrderDetails');

-- 조회
-- Page Type 1 → Data Page를 의미한다.
-- Page Type 2 → Index Page를 의미한다.
DBCC IND('Northwind', 'TestOrderDetails', 2);

--				976
-- 920 944 945 946 947 948
-- Heap RID ([페이지 주소(4)][파일ID(2)][슬롯(2)] ROW)
-- Heap Table[ {Page} {Page} {Page} {Page} ]

-- 페이지 조회
DBCC PAGE('Northwind', 1, 1040, 3);

-- Clustered Index 추가
CREATE CLUSTERED INDEX Index_OrderDetails_Clustered2
ON TestOrderDetails(OrderID);

-- 인덱스 조회
DBCC IND('Northwind', 'TestOrderDetails', 1);

--					9200
-- 9160 9168 9169 9170 9171 9172 9173 9174 9175 9176
-- 물리적으로 데이터를 물고있는 영역