using UnityEngine;

// 1. Player를 움직입니다.
// - Input Key
// - Direction (앞/뒤/좌/우)
// - Move Speed
// 2. player를 회전시킨다
// - 방향 (좌/우, 상/하)
// - 누적 방향 값
// - 민감도
public class Player : MonoBehaviour
{
    // Move Speed (이동속력)
    
    private float moveSpeed = 10.0f;
    // 누적 방향 값 (좌/우, 상/하)
    private float mX, mY;
    // - 민감도
    public float sensitivity = 1.0f;
    // Camera (위아래로 회전시키고 싶은 GameObject)
    public GameObject face;
    // 제한 시야각
    public float limitAngle = 80.0f;

    // Rigidbody Component
    public Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() // 한번만 실행행
    {

    }

    // Update is called once per frame
    void Update() // 게임 오프젝트가 죽을 때까지
    {
        Movement();

        RotateHorizontal();

        RotateVertical();

        Jump();
    }

    // Player Jump
    void Jump() {
        // 점프키를 누르면
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Y축 방향으로 점프파워만큼 점프!
            rb.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);
        }
    }

    // player를 상/하 회전
    void RotateVertical() {
        // Goal : Player 회전
        // 1. Input (마우스 상/하 입력)
        float v = Input.GetAxis("Mouse Y");
        // 2. 입력값 누적
        //  ㄴ 마우스 민감도로 속도조절
        //  ㄴ Time.deltaTime으로 Upadte 시간차 보정
        mY = mY + v * sensitivity * Time.deltaTime;
        // * 문제 : 위 아래 회전 시, 360도 회전이 가능
        //   해결 : 위 아래 시야각 제한
        mY = Mathf.Clamp(mY, -limitAngle, limitAngle);
        // 3. 특정GameObject(카메라) 회전
        // * 문제 : 위아래 회전 방향이 마우스방향과 반대
        face.transform.localEulerAngles = new Vector3 (-mY,0,0);
    }


    // player를 좌/우 회전
    void RotateHorizontal() {
        // Goal : Player 회전
        // 1. Input (마우스 입력)
        float h = Input.GetAxis("Mouse X");
        // 2. 입력값 누적, 
        //  L: -1, R: +1 * 마우스 민감도 (회전속력)
        mX = mX + h * sensitivity * Time.deltaTime;
        // 3. 회전시킨다
        transform.localEulerAngles = new Vector3 (0,mX,0);
        //  a. Player 회전값 가져오기
        //  b. 입력 누적값 반영
        //  c. Player 회전값 갱신(Update)
    }

    // Player를 이동
    void Movement()
    {
        // 4. 언제?
        // 3. 어떤 입력 키를 눌렀을 때?
        //  - 앞/뒤 방향
        float v = Input.GetAxis("Vertical");
        print("v : " + v);
        //  - 좌/우 방향
        float h = Input.GetAxis("Horizontal");
        // 2. 어느 방향으로?
        Vector3 direction = new Vector3(1, 2, 3);
        // 1. 어느정도의 빠르기로?
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        // Goal : 이동시킨다
        // 속도 : 방향 * 속력력
    }
}
