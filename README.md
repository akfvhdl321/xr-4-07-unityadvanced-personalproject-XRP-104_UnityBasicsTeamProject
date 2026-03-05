2D 액션 플랫폼 게임

Unity로 제작한 2D 횡스크롤 액션 플랫폼 게임 프로젝트입니다.

플랫폼 게임의 핵심 요소인 점프 컨트롤, 적 AI, 아이템 시스템, 투사체 시스템을 구현했습니다.

프로젝트 소개

본 프로젝트는 다음과 같은 시스템을 직접 구현하여 제작한 게임입니다.

플레이어 이동 및 점프

상태 머신 기반 플레이어 구조

적 AI (Patrol / Follow)

아이템 시스템

Fireball 투사체 시스템

체크포인트 리스폰

UI 시스템

주요 기능
Player 시스템
플레이어는 다음 기능을 수행할 수 있습니다.

좌우 이동

점프

점프 보정 시스템

Fireball 발사

피격 및 넉백

점프 안정화 시스템
플랫폼 게임에서 점프 감각을 개선하기 위해 두 가지 기능을 구현했습니다.

Coyote Time

플랫폼에서 떨어진 직후에도 일정 시간 동안 점프가 가능합니다.

플랫폼 끝
↓
0.15초 동안 점프 가능
Jump Buffer

착지 직전에 점프 버튼을 눌러도 점프가 실행됩니다.

점프 입력
↓
착지
↓
즉시 점프
Enemy 시스템
적은 두 가지 타입으로 구성됩니다.

Enemy	설명
PatrolEnemy	일정 거리 좌우 순찰
FollowEnemy	플레이어 감지 시 추적
PatrolEnemy
기능

좌우 이동

낭떠러지 감지

플레이어 접촉 시 데미지

적은 Raycast를 사용하여 발 앞쪽의 바닥을 감지합니다.

Enemy
 ↓
Raycast
 ↓
Ground
바닥이 없으면 방향을 반전합니다.

FollowEnemy
FollowEnemy는 플레이어를 감지하면 추적합니다.

플레이어 감지 조건

X 거리

Y 높이

높이를 체크하는 이유

다른 층에 있는 플레이어를 추적하지 않도록 하기 위해서입니다.

전투 시스템
데미지는 인터페이스 기반 구조로 구현했습니다.

IDamagable
public interface IDamagable
{
    TeamType Team { get; }
    void TakeDamage(int damage, Vector2 attackerPosition);
}
이 구조를 사용하면

Player

Enemy

Projectile

모든 객체가 동일한 방식으로 데미지를 처리할 수 있습니다.

Enemy 밟기 시스템
플레이어가 적 위에서 떨어지면 적이 사망합니다.

조건

플레이어가 아래 방향으로 이동 중일 때

플레이어 낙하
↓
Enemy Stomp Trigger
↓
Enemy 사망
Fireball 시스템
Fireball은 Object Pool 패턴을 사용하여 구현했습니다.

Object Pool
Object Pool은 오브젝트를 미리 생성하여 재사용하는 기술입니다.

장점

Instantiate 호출 감소

Garbage Collection 감소

성능 안정성 증가

Fireball 동작

FireballPool
 ↓
Fireball 생성
 ↓
발사
 ↓
충돌
 ↓
Pool 반환
Item 시스템
아이템은 상속 구조로 구현했습니다.

ItemBase
 ├ JumpBoostItem
 └ FireModeItem
JumpBoostItem
효과

점프력 1.5배 증가

일정 시간 유지

FireModeItem
효과

Fireball 공격 사용 가능

체크포인트 시스템
체크포인트에 닿으면 플레이어 리스폰 위치가 저장됩니다.

Checkpoint 충돌
↓
위치 저장
↓
사망
↓
해당 위치 리스폰
UI 시스템
게임 UI는 다음과 같이 구성되어 있습니다.

UI	설명

체력 UI	플레이어 체력 표시
FireMode UI	FireMode 활성화 표시
JumpBoost UI	점프 부스트 시간 표시
GameOver UI	플레이어 사망 화면

사용 기술

기술	설명

Unity	게임 엔진
C#	프로그래밍 언어
Cinemachine	카메라 시스템
Unity Input System	입력 처리
Git / GitHub	버전 관리

시스템 구조
GameManager
 ├ Player System
 ├ Enemy System
 ├ Item System
 ├ Projectile System
 └ UI System
 
조작 방법
키	기능
A / D	이동
Space	점프
F	Fireball
ESC	일시정지
