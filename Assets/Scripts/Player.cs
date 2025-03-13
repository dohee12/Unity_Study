using UnityEngine;

// 1. Player를 움직입니다.
// - Input Key
// - Direction
// - Move Speed
public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    // Player를 이동
    void Movement()
    {
        // 4. 언제?
        // 3. 어떤 입력 키를 눌렀을 때?
        //  - 앞/뒤 방향
        float v = Input.GetAxis("Vertical");
        //  - 좌/우 방향
        float h = Input.GetAxis("Horizontal");
        // 2. 어느 방향으로?
        Vector3 direction = new Vector3(1, 2, 3);
        // 1. 어느정도의 빠르기로?
        transform.Translate(direction * 5 * Time.deltaTime);
        // Goal : 이동시킨다
    }
}
