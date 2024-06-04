use BaseballData;

SELECT *
FROM teamshalf;

-- TRAN을 명시하지 않으면, 자동으로 COMMIT이 진행된다.
INSERT INTO teamshalf VALUES(1981, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL);

-- BEGIN TRAN;
-- COMMIT
-- ROLLBACK

-- 트랜잭션은 무엇인가?

-- 거래
-- A의 인벤토리에서 아이템 제거
-- B의 인벤토리에 아이템 추가
-- 거래 수수료에 따른 A의 골드 감소 등..

-- 이 중, B의 인벤토리에 아이템이 추가가 됐는데, 
-- A의 인벤토리에서 아이템 제거가 되지 않을 경우 문제가 생긴다.
-- 따라서 기본적으로 원자성(ALL OR NOTHING)의 형태로 작업이 진행되어야 한다.

-- 위 사태를 해결하기 위한 것이 트랜잭션의 개념이다.


--------------------------------------------------------------------------------

-- 메일 → BEGIN TRAN
--- 메일을 보낼 것인가? → COMMIT
--- 메일을 취소할 것인가? → ROLLBACK

-- 성공/실패 여부에 따라 COMMIT (=COMMIT을 수동으로 하겟다.)
BEGIN TRAN;
	INSERT INTO teamshalf VALUES(1980, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL); 
ROLLBACK;

BEGIN TRAN;
	INSERT INTO teamshalf VALUES(1980, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL); 
COMMIT;

-- 응용
BEGIN TRY
	BEGIN TRAN;
		INSERT INTO teamshalf VALUES(1980, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL); 
		INSERT INTO teamshalf VALUES(1980, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL); 
	COMMIT;
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 -- 현재 활성화된 트랜잭션 수를 반환한다.
		ROLLBACK
END CATCH

-- TRAN 사용할 때 주의할 점
-- TRAN 안에는 꼭!!! 원자적으로 실행될 애들만 넣어서 사용하자.
-- C# List<Player> List<Salary> 원자적으로 수정 -> lock을 잡아서 실행 -> writelock (상호배타적인 락) readlock (공유 락)

BEGIN TRAN;
	INSERT INTO teamshalf VALUES(1985, 123, 456, 789, NULL, NULL, NULL, NULL, NULL, NULL); 
ROLLBACK;