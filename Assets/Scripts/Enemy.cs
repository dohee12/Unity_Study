using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

// 피격 시, HP 닳는 체력 시스템 구축
// - Enemy HP
// - HP (Get/Set Property)

// 목표 : 다양한 상황(state)에 대응하는 자동인형(FSM:Finite State Machine)
// a.Idle(대기) : 가만히 서 있는 상태
//  ㄴ distance = Player와 나 사이의 거리
//  ㄴ detectRange = 탐지범위
// b.Move(이동) : Target을 쫓아 움직이는 상태
//  ㄴ CharacterController 로 이동
//  ㄴ moveSpeed = 이동속력
//  ㄴ distance = Player와 나 사이의 거리
// c.Damaged(피격) : 누군가에 공격 당한 상태
// d.Attack(공격) : Target(Player)를 공격
// e.Death(죽음) : hp가 0이 되어 죽게된다.

// Enemy 컴포넌틀 Attach, CharacterController가 자동으로 생성
[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour, IDmage
{
    // Enum(열거형) : 어떤 목적에 따른 데이터 집합/타입을 만듦(값에 이름 지정가능)
    public enum State{
        Idle,Move,Damaged,Attack,Death
    }
    // Enemy의 State Enum형 변수
    public State m_state = State.Idle;
    private int hp = 5;

    // Target(=Player)
    private GameObject target;
    // CharacterController
    private CharacterController cc;
    // Player 탐지범위
    public float detectRange = 5.0f;
    // Player 공격범위
    public float attackRange = 2.0f;
    public float moveSpeed = 5.0f;
    
    // get/set property로 Enemy의 hp 관리
    public int HP{
        // GET : HP를 호출하면, enemy.hp 값을 반환
        get{
            return hp;
        }
        // SET : HP를 호출해서 값을 할당하면, 
        // 할당된 값은 'value'안에 담겨서, set 안의
        // 코드를 실행. (=> enemy.hp 에 할당값을 대입)
        set{
            hp = value;
            // 만일 hp 값이 0이 된다면 Enemy(=자신)파괴
            if(hp < 1){
                Destroy(gameObject);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 초기값을 넣어주는, 초기화 목록..
        // 1. target을 tag로 찾아서 넣어준다.
        //   a.target = GameObject.FindGameObjectWithTag("Player");
        //   b.target = GameObject.Find("Player");
        target = PlayerHealth.Instance.gameObject;
        // 2. CharacterConroller 찾아서 넣어준다.
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // FSM 을 적용시켜 각 상태에 대응
        switch(m_state){
            case State.Idle : Idle(); break; 
            case State.Move : Move(); break; 
            // case State.Damaged : Damaged(0); break; 
            case State.Attack : Attack(); break; 
            // case State.Death : Death(); break; 
            default : break;
        }
    }

    // a.Idle : 가만히 있음
    public void Idle(){
        // 특정 상황이 되면(=Player가 나와 일정거리 가까워지면) 
        // - 나 자신(=Enemy)와 Player 사이의 거리
        //  -> A물체의 x,y,z 값에서 B물체의 x,y,z 값을 뺀 후에, 루트 씌우기
        // float distance = (transform.position - target.transform.position).magnitude;
        float distance = Vector3.Distance(transform.position,target.transform.position);
        // - 일정거리(=탐지범위)
        if(distance < detectRange){
            ChangeState(State.Move);
        }        
    }
    // b.Move : Target을 따라간다
    public void Move(){
        // 1-a. Target과 Enemy(=나자신)사이의 벡터 구하기
        //  * Enemy가 Target을 바라보는 방향의 벡터
        //  * A향한 방향 구하기(A위치 - B위치)
        Vector3 lookAtTarget = target.transform.position - transform.position;
        // 1-b. (a)의 벡터의 크기 구하기(Target-Enemy사이의 거리)
        float distance = lookAtTarget.magnitude; // == Vector3.Distance(a,b);
        // 1-c. (a)의 벡터의 방향 구하기(Target방향)
        //  ㄴ정규화(방향만 남기고, 벡터의 크기를 1로 만듦)
        Vector3 direction = lookAtTarget.normalized;
        // 1. Player를 따라다닌다.
        //  ㄴ 이동 : 일정속력으로, 특정한방향(target)을 향해
        cc.Move(direction*moveSpeed*Time.deltaTime);

        // 2-A. Player를 따라잡은 경우(공격범위>target)
        if(attackRange > distance){
            ChangeState(State.Attack);
        }
        // 2-B. Player를 놓친 경우(탐지범위<target)
        if(detectRange < distance){
            ChangeState(State.Idle);
        }
    }
    // c.Damged : 공격 당하면, HP 깎음
    public void Damaged(int damage){
        // Player를 공격
        // - Player HP -1씩 감소
        PlayerHealth.Instance.HP -= damage;
    }
    // d.Attack : Target을 공격함
    public void Attack(){
        // Target-Enemy의 거리
        float distance = Vector3.Distance(transform.position, target.transform.position);
        // Target 공격 + Animation
        

        // Target이 내 공격범위를 벗어나면,
        if(attackRange < distance){
            // Attack > Move
            ChangeState(State.Move);
        }
    }
    // e.Death : HP가 0이되면 죽음
    public void Death(){}

    // State를 변경해주는 함수
    public void ChangeState(State nextState){
        // 현재 상태를 변경
        m_state = nextState;
        // 상태 변경 시, 추가적인 Event 실행
    }


    // Debug 용도로 DetectRange 표기
    void OnDrawGizmos()
    {
        // detectRange 를 구형태로 시각화
        // 1. Gizmo 선의 색상을 설정
        Gizmos.color = Color.cyan;
        // 2. 특정 위치에서 detectRange의 반지름을 가진 구형태의 Gizmo 생성
        // ㄴ 구의 중심위치 = 나자신의 위치(transform.position)
        // ㄴ 구의 반지름 크기 = detectRange
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // attackRange를 구형태로 시각화
        // 1. Gizmo 선의 색상 결정
        Gizmos.color = Color.red;
        // 2. 특정 위치에서 attackRange 반지름을 가진 구 생성
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
