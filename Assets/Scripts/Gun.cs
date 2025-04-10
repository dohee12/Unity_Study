using System.Collections.Generic;
using UnityEngine;

// Goal : 총구가 향하는 방향으로 총알을 발사
// - 총구(FirePos)
// - 총알(Bullet)
// - 사용자 입력에 따라 총알 발사
// Object Pooling을 통해 총알 재사용
//  ㄴ 탄창 원본
//  ㄴ 탄장
//  ㄴ 탄창 크기기
public class Gun : MonoBehaviour
{
    // - 총구(FirePos)
    public Transform firePos;
    // - 총알(Bullet) 공장 (설계도[Prefab] 필요)
    public GameObject bulletFactory;
    // - ObjectPooling: 탄창 원본 (array)
    public GameObject[] bulletPool;
    // - ObjectPooling: 탄창 (list)
    public List<GameObject> bulletList;
    // - ObjectPooling: 탄창 크기
    public int bulletPoolSize = 10;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 잠금 (화면 중앙 고정)
        InitBulletPool(); // 시작 시, 탄창 생성
    }

    // ObjectPooling 탄창 초기화
    void InitBulletPool(){
        // 1. 탄창 원본 초기화
        bulletPool = new GameObject[bulletPoolSize]; // 탄창 크기만큼 배열 생성
        // 2. 탄창 리스트 초기화
        bulletList = new List<GameObject>(); // 리스트 생성
        // 3. 탄창에 총알 채우기
        for (int i = 0; i < bulletPoolSize; i++){
            // 3-1. 탄창 크기만큼 총알 생성
            GameObject bullet = Instantiate(bulletFactory);
            // 3-2. 총알은 비활성화 상태로 변경
            bullet.SetActive(false); // 총알 비활성화
            // 3-3. 생성한 총알을 탄창에 담는다 (pool,list)
            bulletPool[i] = bullet; // 원본 탄창
            bulletList.Add(bulletPool[i]); // 실제 탄창에 착탄
        }
    }

    // Update is called once per frame
    void Update()
    {
        // // 마우스 왼쪽 버튼을 클릭하면
        // if (Input.GetMouseButtonDown(0)){
        if (Input.GetButtonDown("Fire1")){
            // 총알 발사
            //Fire();
            FireUsingObjectPooling();
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

    // ObjectPooling 용 Fire
    void FireUsingObjectPooling() {
        // 1 탄창에서 꺼내 쓰기
        // --bulletList[0] : 탄장의 첫번째 총알
        // 2 위치
        bulletList[0].transform.position = firePos.position; // 총구 위치
        // 3 방향
        bulletList[0].transform.forward = firePos.forward; // 총구 방향
        // 4 총알 활성화 : 비활성화 상태로 넣었기 때문에..
        bulletList[0].SetActive(true); // 총알 활성화
        // 5 탄창에서 방금 사용한 첫 번째 총알 제거
        bulletList.RemoveAt(0); // 탄창에서 방금 사용한 총알 제거
    }
}
