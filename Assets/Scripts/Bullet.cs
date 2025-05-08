using UnityEngine;

// Goal : 앞으로(z축) 날아가서, 무언가에 충돌
// - 방향 (direction) : z축(forward)
// - 속력 (speed)
// - 충돌 : 실체 (collider) + 물리연산적용(rigidbody)
//  L 어딘가에 부딪히면 파괴(자기자신을 파괴)
//  ㄴ 어디에도 부딪히지 않는 경우, 일정 시간이 지나면 파괴
//     ㄴ LifeTime (=파괴되기까지 걸리는 시간)
//     ㄴ Timer
public class Bullet : MonoBehaviour
{
    // 속력(speed)
    public float speed = 10.0f;
    // Rigidbody
    private Rigidbody rb;
    // LifeTime (=파괴되기까지 걸리는 시간)
    public float lifeTime = 3.0f;
    // Timer
    private float timer = 0.0f;

    // Unity 실행 시 가장 먼저 실행되는 Lifecycle 함수 (onEnable, start보다 먼저저)
    void Awake()
    {
        // bullet(나자신)에게 붙어있는 Rd 값을 할당 (=초기화)
        rb = GetComponent<Rigidbody>();
    }
    // GameObject가 활성화될 때마다 실행되는 Lifecycle 함수
    void OnEnable()
    {
        // (할당된 이후에) 총알이 앞으로 움직인다
        MoveToForward();
    }

    // Update is called once per frame
    void Update()
    {   
        // 2. Timer 작동
        timer += Time.deltaTime;
        // 1. 일정시간(lifeTime)이 지난 경우
        if (timer > lifeTime){
            // Goal. 총알(=자기 자신) 파괴
            DeactiveMySelf();
        }
        
    }
    // 일정 시간이 지나면 파괴(=탄창에 넣기)
    void DeactiveMySelf() {
        // 1. 탄창에 넣기 전에, 상태 Reset
        // + Rigidbody 물리연산 초기화(Reset)
        rb.linearVelocity = Vector3.zero; // 속도 초기화
        rb.angularVelocity = Vector3.zero; // 회전 속도 초기화
        // + Timer 초기화(Reset)
        timer = 0f;

        // 2. 탄창에 다시 집어넣기
        // 2-1. 탄창이 어디??
        GameObject player = GameObject.Find("Player");
        // player.transform.Find("Gun").gameObject;
        // player.transform.GetChild("Gun").GetComponent<Gun>();
        Gun gun = player.GetComponentInChildren<Gun>();
        // 2-2. 탄창에 집어넣기
        gun.DeactiveBullet(gameObject);
    }

    // RB를 활용하여 총알의 Z축방향으로 힘을 가해 날아가게한다.
    // - Rigidbody
    // - 방향 (forward)
    // - 속력(가하는 힘의 크기)
    private void MoveToForward(){
        // Addforce(방향 * 힘, ForceMode);
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    // 어딘가에 부딪히는 경우
    // - collision : 나와 충돌한 녀석의 정보가 들어있다
    void OnCollisionEnter(Collision other) {
        // 만일 부딪힌 GameObject가 Enemy라면
        // 1. 이름이 정확히 Enemy와 일치하는 경우
        bool compareName = other.gameObject.name.Equals("Enemy");
        // 2. 특정 단어를 포함하는 경우
        bool ContainKeyword = other.gameObject.name.Contains("Enemy");
        // 3. 특정 Tag를 가지고 있는 경우
        bool compareTag = other.gameObject.CompareTag("Enemy");
        // 4. 특정 Layer를 가지고 있는 경우
        bool compareLayer = other.gameObject.layer == LayerMask.NameToLayer("Enemy");

        
        if (compareTag){
            // -> 파괴(Enemy)
            Destroy(other.gameObject);
        }

        // +총알 이펙트
        // +파괴 이펙트
        // +효과음음

        // 파괴 (자기 자신- 총알)
        Destroy(gameObject);
    }
}
