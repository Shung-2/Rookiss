USE Northwind;

-- 북마크 룩업
-- 어제 주제
-- Index Scan vs Index Seek
-- Index Scan이 항상 나쁜 것은 아니고
-- Index Seek가 항상 좋은 것은 아니다.
-- 인덱스를 활용하는데 왜 나쁘다는 얘기가 나오고 왜 느릴까?
-- 바로 북마크 룩업 때문이다. 오늘은 이 북마크 룩업에 대해서 알아볼 것이다.

-- ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

-- 논클러스터드와 클러스터드의 차이점에 대해서 계속 알아보고 있다.
-- 논클러스터드
--		1
-- 2 3 4 5 6

-- 클러스터드
--		1
-- 2 3 4 5 6

-- 클러스터드는 데이터가 실제로 leaf Page에 존재하지만
-- 논클러스터드는 데이터를 들고 있는게 아니라 데이터를 찾을 수 있는 열쇠를 들고있는 것이다.
-- 따라서 클러스터드 인덱스가 없을 경우 가정한다면
-- heap table이 생기게 되는데, 이는 일련의 페이지들을 아래와 같이 가지고 있다
-- Heap Table [ {Page}, {Page} ]
-- 이때 논클러스터드의 leaf 에서는 RID를 이용해 힙테이블에 찾아가 데이터를 찾는다.

-- ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

-- 클러스터드의 경우 Index Seek가 느릴 수가 없다.
-- 그러나 논클러스터드의 경우 데이터가 Leaf Page에 없다.
-- 따라서 데이터를 찾기위해 한번 더 타고 가야한다.
	-- 1) RID → Heap Table (북마크 룩업) 개념
	-- 2) Key → 클러스터드 
-- 이번 시간에는 이를 실습하도록 하자.

-- ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

-- TestOrders 테이블을 새로 만들어준다.
SELECT *
INTO TestOrders
FROM Orders;

-- 조회
SELECT *
FROM TestOrders;

-- 인덱스 생성
CREATE NONCLUSTERED INDEX Orders_Index01
ON TestOrders(CustomerID);

-- 인덱스 번호 조회
SELECT index_id, name
FROM sys.indexes
WHERE object_id = object_id('TestOrders');

-- 인덱스 정보 조회
DBCC IND('Northwind', 'TestOrders', 2);

--	   1024
-- 936 944 945 
-- 클러스터드 인덱스를 만들어주지 않았기 때문에 힙 테이블이 분명히 존재한다.
-- Heap Table [ {Page}, {Page} ]

-- 각각의 개념은 얼마 걸렸는지, 페이지 논리적 접근 개수, 실제로 실행된 순서를 나타낸다
SET STATISTICS TIME ON;
SET STATISTICS IO ON;
SET STATISTICS PROFILE ON;

-- 기본 탐색
SELECT *
FROM TestOrders
WHERE CustomerID = 'QUICK';
-- 논리적 읽기 20.
-- CustomerID는 논클러스터드를 걸어놨음에도 불구하고 이를 사용하지 않고 있다.
-- 왜 그럴까?
-- 그냥 테이블을 스캔하는게 더 빠르다고 판단했기 때문이다.
-- 그럼 인덱스를 활용해서 조회할경우 어떤 결과가 나올까?

-- 기본 탐색 (인덱스 강제 사용)
SELECT *
FROM TestOrders WITH(INDEX(Orders_Index01))
WHERE CustomerID = 'QUICK';
-- 논리적 읽기 30.
-- 위 결과로부터 얻을 수 있는 것은 인덱스를 사용하는게 최선이 아님을 명심해야 한다.
-- 그럼 이 논리적 읽기 30은 어디서 나올까?
-- 이는 결과의 SET STATISTICS PROFILE ON; 항목을 보면 자세히 알 수 있다.
-- 논 클러스터드의 1을 한번 순회하고, 그리고 LEAF PAGE를 한번 순회하고, LEAF PAGE에서 RID LOOKUP(=북마크 룩업)을 28번 했기 때문에 느리다.

-- 조건 추가 (룩업을 줄이기 위한 몸부림)
SELECT *
FROM TestOrders WITH(INDEX(Orders_Index01))
WHERE CustomerID = 'QUICK' AND ShipVia = 3;
-- 조건을 추가했기 때문에 논리적 읽기 30은 같으나 SET STATISTICS PROFILE ON 항목의 4번 ROW에서 8개의 행만 읽어들인 것을 알 수 있다.

-- 그렇다면 룩업을 줄일 수 있는 명쾌한 해답이 있을까?
-- 이 모든 것은 케이스 바이 케이스로 답이 다르기 떄문에 명쾌한 해답은 존재하지 않는다.

-- 인덱스 삭제
DROP INDEX TestOrders.Orders_Index01;

-- 복합 인덱스로 처리해볼까? → 룩업을 줄이기 위한 몸부림 ㅠㅠ
CREATE NONCLUSTERED INDEX Orders_Index01
ON TestOrders(CustomerID, ShipVia);
-- 이를 Covered Index 라고 한다. 말 그대로 모든 영역을 다 커버하는 느낌이다.

-- 아까 조건을 재 실행해보자.
SELECT *
FROM TestOrders WITH(INDEX(Orders_Index01))
WHERE CustomerID = 'QUICK' AND ShipVia = 3;
-- 인덱스 시크를 하는 도중에 28개가 아닌 8개만 진행한다!
-- 왜 28개가 아닌 8개로 줄어들었을까?
-- 논 클러스트 Leaf Page에서 Customer ID 만으로 인덱스가 이루어진게 아니라 ShipVia와 함께 세트로 키값을 사용
-- 따라서 힙테이블까지 가서 찾아야하는 것이 아닌, LeafPage 단계에서 20개의 꽝들을 걸러낸다.!
-- 즉 8번 룩업을 시도해서 8번 다 꽝없이 찾는 것이다.

-- Q) 그러면 조건1 AND 조건2 필요하면, 무조건 Index를 걸 때 조건1, 조건 2를 CoverIndex를 통해 처리하면 장땡인가?
-- N) 그럴리가 ^^ 데이터베이스는 그렇게 간단하지가 않다. 꼭 그렇지는 않다.
-- 뭐가 또 문제일까? → 이런식으로 복합 인덱스를 가지고 조회할때는 빠르지만 DML을 사용할 때 작업 부하가 증가한다.
-- DML - Insert, Update, Delete
-- 따라서 데이터의 갱신이 잦지 않으면 상관 없지만, 갱신되는게 활발하다면 이것이 꼭 정답은 아니다.

-- Covered Index가 아닌 다른 방법이 있는데 이것도 알아보자.
-- 먼저 인덱스를 날리고 시작하다.
-- 인덱스 삭제
DROP INDEX TestOrders.Orders_Index01;

-- 룩업을 줄이기 위한 몸부림 2
CREATE NONCLUSTERED INDEX Orders_Index01
ON TestOrders(CustomerID) INCLUDE (ShipVia);
-- INCLUDE를 사용하면 뭐가 다를까?
-- 'CustomerID를 통해 정렬은 하겠지만, ShipVia에 대한 정보는 추가로 들고있겠다.' 라는 뜻이다.

-- 논클러스터드
--		1
-- 2[28(data1(ShipVia=3), data2(ShipVia=2), ... data28] 3 4 5 6

-- 이렇게 하면 무슨 장점이 있을까?
-- 아까 28개의 데이터를 찾았을 때 문제가 되었던 것이 뭐냐면 ShipVia의 값이 3번인 애들은 논클러스트 Leaf Page(=2단계)에서 알 수 없었기 때문에
-- 힙 테이블까지 들고가서 찾아야 했는데, 위와 같이 사용 할 경우 정보는 추가로 들고있기 때문에 Leaf Page 단계에서 ShipVia 조건을 걸러낸 다음 찾기 때문에 실질적으로 필요한 정보만 힙테이블에서 찾을 수 있게 된다.

-- 다시 조회해보자!
SELECT *
FROM TestOrders WITH(INDEX(Orders_Index01))
WHERE CustomerID = 'QUICK' AND ShipVia = 3;
-- Covered Index와 마찬가지로 꽝이 하나도 없는걸 볼 수 있다.

-- 두 조건을 알아보았지만 위와 같은 눈물겨운 노력에도 답이 없다면
-- 애당초 클러스터드 인덱스로 활용을 해서 고려할 수 있다. (제 3의 옵션)
-- 그렇지만 치명적인 단점이 존재하는데. 클러스터드 인덱스는 테이블당 1개만 사용할 수 있다.

-- 결론 --
-- 논클러스터드 인덱스가 악영향을 주는 경우가 무엇이 있을까?
	-- 북마크 룩업이 심각한 부하를 야기할 때!
-- 대안?
	-- 옵션 1) Coverd Index (검색할 모든 컬럼을 포함하겠다)
	-- 옵션 2) Index에다가 Include로 힌트를 남긴다.
	-- 옵션 3) 클러스터드를 고려한다. (단 1번만 사용할 수 있는 인덱스가 한계가 있다.) 아울러 논클러스터드 인덱스에게 까지도 영향을 준다.
