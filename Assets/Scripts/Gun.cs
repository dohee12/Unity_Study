using UnityEngine;

// Goal : 총구가 향하는 방향으로 총알을 발사
// - 총구(FirePos)
// - 총알(Bullet)
// - 사용자 입력에 따라 총알 발사

public class Gun : MonoBehaviour
{
    // - 총구(FirePos)
    public Transform firePos;
    // - 총알(Bullet) 공장 (설계도[Prefab] 필요)
    public GameObject bulletFactory;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 왼쪽 버튼을 클릭하면
        if (Input.GetMouseButtonDown(0)){
            // 총알 발사
            Fire();
        }
    }

    // 사용자 입력에 따라 총알 발사 
    // - 총알
    // - 총알의 발사 위치 (=총구의 위치)
    // - 총알 방향 (=총구의 방향)
    // - 총알 생성 (=복제) with 
    private void Fire(){
        // 1. 총알을 생성 (=기존 총알 GameObject 토대로 복제)
        GameObject bulletObj = Instantiate(bulletFactory);
        // 2. 생성한 총알을 총구의 위치로 이동
        bulletObj.transform.position = firePos.position;
        // 3. 생성한 총알이 향하는 방향(Z축|Forward)을 총구가 향하는 방향(Z축|Forward)으로 동일하게
        bulletObj.transform.eulerAngles = firePos.eulerAngles;
        // 4. 총알이 날아간다 (=알아서 날아간다)
    }
}
