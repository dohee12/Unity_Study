using UnityEngine;

// Goal : 앞으로(z축) 날아가서, 무언가에 충돌
// - 방향 (direction)
// - 속력 (speed)
// - 충돌 : 실체 (collider) + 물리연산적용(rigidbody)
public class Bullet : MonoBehaviour
{
    // 속력(speed)
    public float speed = 10.0f;
    // Rigidbody
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // bullet(나자신)에게 붙어있는 Rd 값을 할당 (=초기화)
        rb = gameObject.GetComponent<Rigidbody>();
        // 할당한 이후에 총알이 앞으로 움직인다
        MoveToForward();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // RB를 활용하여 총알의 Z축방향으로 힘을 가해 날아가게한다.
    // - Rigidbody
    // - 방향 (forward)
    // - 속력(가하는 힘의 크기)
    private void MoveToForward(){
        // Addforce(방향 * 힘, ForceMode);
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }
}
