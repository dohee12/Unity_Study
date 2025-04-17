using Unity.VisualScripting;
using UnityEngine;

// Goal : "Ray"를 활용해 조준점을 향해 '총알 없이' 총을 발사
// - Ray의 위치(origin), 방향(direction) = 총구(FirePos)
// - Ray Distance (사거리)
// - RaycastHit (충돌물체에 대한 정보 = Ray라는 닿은 물제 정보)
// - 사용자 입력
public class RayGun : MonoBehaviour
{
    // Ray 발사 위치 (=총구)
    public Transform firePos;
    // Ray Distance (사거리)
    public float range = 100.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 사용자 입력에 따라 발사
        if(Input.GetButtonDown("Fire1")) {
            Fire();
        }
    }

    // 총(=Ray) 발사하기 
    void Fire() {
        // 1. Ray를 생성 (위치, 방향)
        Ray ray = new Ray(firePos.position, firePos.forward);
        // 2. RayCastHit을 만듦 (= 부딪혔을 때 정보를 담을 그릇)
        RaycastHit hitInfo;
        // 3. RayCast! (= Ray 발사)
        // - RayCast(Ray 발사정보, 충돌정보, 사거리)
        bool isHit = Physics.Raycast(ray, out hitInfo, range);
        // 3-1. 충돌했을 때!
        if(isHit) {
            // 충돌한 물체가 Enemy인 경우에만
            if(hitInfo.collider.CompareTag("Enemy")) {
                // 충돌한 물체를 '파괴'
                Destroy(hitInfo.collider.gameObject);
            }
        }
        // 3-2. 충돌하지 않은 경우!
        else {
            
        }
    }
}
