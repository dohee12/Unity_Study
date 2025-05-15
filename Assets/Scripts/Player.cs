using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

// 1. Player 를 움직입니다.
//  - Input Key
//  - Direction (앞/뒤/좌/우)
//  - Move Speed
// 1-a. CharacterController 를 통해서  Player 움직인다.
//  - Component : CharacterController
//  - 중력 적용
//    ㄴ gravity : 중력 상수값 -9.81
//    ㄴ yVelocity : Player에게 적용되는 중력값
//  - Jump 기능
//    ㄴ JumpPower
//    ㄴ 점프가 가능한 상태인지?(땅을 밟고 있는지 여부)
//    ㄴ n단 점프
//        - jumpCount : 현재 점프 횟수
//        - maxCount  : 최대 점프 횟수
// 2. Player 를 회전시킨다.
//  - 방향 (좌/우, 상/하)
//  - 누적 방향 값 (좌/우, 상/하)
//  - 민감도
public class Player : MonoBehaviour
{
    // Move Speed(이동속력)
    [SerializeField]
    private float moveSpeed = 100.0f;
    // 누적 방향 값 (좌/우, 상/하)
    private float mX,mY;
    // 민감도
    public float sensitivity = 1.0f;
    // Camera (위아래로 회전시키고 싶은 GameObject)
    public GameObject face;
    // 제한 시야각
    public float limitAngle = 80.0f;
    // Character Controller
    private CharacterController cc;
    // 중력 상수값
    private float gravity = -9.81f;
    // Player가 적용되는 중력값
    private float yVelocity;
    // Player 의 점프 파워
    public float jumpPower = 3.0f;
    // Player의 점프 가능 횟수
    private int jumpCount = 0;
    // Plater의 점프 가능 최대 횟수
    public int maxJumpCount = 2;

    // caching
    public Transform playerTransfrom;
    public Transform faceTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Play 시에, cc에 내가 가진 CharacterController 컴포넌트 할당
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement();

        CCMovemnet();

        RotateHorizontal();

        RotateVertical();
    }

    // CharacterController 사용한 Player 이동
    void CCMovemnet(){
        // 1. Input 이동 값 받아오기
        // a. 좌/우 방향
        float h = Input.GetAxis("Horizontal");
        // b. 앞/뒤 방향
        float v = Input.GetAxis("Vertical");
        // 2. 방향 만들기 (받아온 이동 값으로부터)
        Vector3 dir = new Vector3(h,0,v);
        // *문제 : 내가 바라보는 방향을 기준으로 움직이지 않음
        // **해결 : Camera가 바라보는 방향을 기준으로 방향변환
        dir = Camera.main.transform.TransformDirection(dir);

        // --- Player의 점프 ----
        // ** 도전과제 : n단 점프 ** 
        // A.땅에 닿아있는 경우
        if(cc.isGrounded){
            //-> 점프 횟수 초기화(Reset)
            jumpCount = maxJumpCount;
            //-> yVelocity 중력값 초기화
            yVelocity = 0f;
        }
        // B.점프 버튼을 누른 경우,점프 횟수가 0 이상인 경우에만 점프 가능
    if(jumpCount > 0 && Input.GetButtonDown("Jump")){
            //-> 점프 횟수 -1씩
            jumpCount--;
            //-> 점프 파워만큼 Y축 방향으로 점프
            yVelocity = jumpPower;
        }


        // *문제 : Player가 상/하로도 움직임
        // **해결 : 중력적용을 통해서 상/하 움직임 제한
        // 공식 :  F(힘) = 질량(m) * 가속도(a)
        // a. 중력가속도 구하기
        yVelocity += gravity * Time.deltaTime;
        // b. Player의 방향에 적용
        dir.y = yVelocity;

        // 3. Player 움직이기 with CharacterConrollter
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }


    // Player를 상/하 회전
    void RotateVertical(){
        // Goal : Player를 회전
        // 1. Input (마우스 상/하 입력)
        float v = Input.GetAxis("Mouse Y");
        // 2. 입력값 누적 : 
        //  ㄴ 마우스민감도로 속도조절
        //  ㄴ Time.deltaTime 으로 Update 시간차 보정
        mY = mY + v * sensitivity * Time.deltaTime;
        // *문제 : 위아래 회전 시, 360도 회전이 가능
        //  해결 : 위아래 시야각 제한
        mY = Mathf.Clamp(mY, -limitAngle, limitAngle);

        // 3. 특정GameObject(카메라) 회전
        // *문제 : 위아래 회전 방향이 마우스방향과 반대
        //  해결 : -1을 곱해주어서 반대방향이 되도록 설정
        faceTransform.localEulerAngles = new Vector3(-mY,0,0);
    }

    // Player를 좌/우 회전
    void RotateHorizontal(){
        // Goal : Player를 회전
        // 1. Input (마우스 좌/우 입력)
        float h = Input.GetAxis("Mouse X");
        // 2. 입력값 누적,
        //  L: -1, R: +1 * 마우스민감도(회전속력)
        mX = mX + h * sensitivity * Time.deltaTime;
        // 3. 회전시킨다(자기자신).
        playerTransfrom.localEulerAngles = new Vector3(0,mX,0);
        //  a. Player 회전값 가져오기
        //  b. 입력 누적값 반영
        //  c. Player 회전값 갱신(Update)
    }


    #region (Legacy) Player Movement CC컴포넌트 없이 진행
    void Movement(){
        // 4. 언제?
        // 3. 어떤 입력 키를 눌렀을 때?
        //   - 앞/뒤 방향
        float v = Input.GetAxis("Vertical");
        print("v : " + v);
        //   - 좌/우 방향
        float h = Input.GetAxis("Horizontal");
        // 2. 어느 방향으로?
        Vector3 direction = new Vector3(h,0,v);
        // 1. 어느정도의 빠르기로?
        // 속도 : 방향 * 속력
        playerTransfrom.Translate(direction*moveSpeed*Time.deltaTime);
        // Goal : 이동시킨다.
    }
    #endregion
}
