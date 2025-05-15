using UnityEngine;

// Goal : 'Ray'를 활용해 조준점을 향해 '총알 없이' 총을 발사
// - Ray의 위치(origin),방향(direction) = 총구(FirePos)
// - Ray Distance (사거리)
// - RaycastHit (충돌물체에 대한 정보=Ray라는 닿은 물체 정보)
// - 사용자 입력
// - bulletEffect(착탄되었을때) / muzzlEffect(발사할때)
// Goal : 연사가 가능한 총
// - 연사 발사 쿨타임
// - 다음 발사 시간
// - 타이머
public class RayGun : MonoBehaviour
{
    // Camera Caching
    public Camera mainCam;
    // Ray 발사 위치(=총구)
    public Transform firePos;
    // Ray Distance (사거리)
    public float range = 100.0f;
    // bulletEffect(충돌한 위치에 생성)
    public ParticleSystem ps_bulletEffect;
    public ParticleSystem ps_muzzleEffect;
    // 연사율
    public float fireRate = 10.0f;
    // 다음 발사 시간
    private float nextTimeToFire;
    public int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // GetComponent<>();
        // GetComponentInChildren<>();
        // GetComponentInParent<>();
        // transform.parent.Find("Main Camera").GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 사용자 입력에 따라 발사
        // Time.time = Unit 내부 시계(Play 버튼 누르면 시작되는)
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire ){
            // 연사율 계산
            nextTimeToFire = Time.time + 1 / fireRate;
            
            Fire();
            PlayMuzzleEffect();
        }
    }

    // 총(=Ray) 발사하기
    void Fire()
    {
        // 1. Ray를 생성(위치,방향)
        // 생성위치 -> 카메라의 위치
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        // 2. RayCastHit을 만듦(=부딪혔을 때 정보를 담을 그릇)
        RaycastHit hitInfo;
        // 3. RayCast!(=Ray 발사)
        // - Raycast(Ray발사정보, 충돌정보, 사거리);
        bool isHit = Physics.Raycast(ray,out hitInfo,range);
        // 3-a. 충돌했을 때!
        if(isHit){
            // 충돌한 물체가 Enemy인 경우에만
            // if(hitInfo.collider.CompareTag("Enemy")){

            // 충돌한 물체가 피격 가능한 물체라면?
            // -> IDamage라고 interface를 사용하는 GameObject/Component라면
            if(hitInfo.collider.GetComponent<IDmage>() != null){
                // A.충돌한 물체를 '파괴'
                // Destroy(hitInfo.collider.gameObject);

                // B.Enemy의 Hp를 감소
                GameObject enemyObj = hitInfo.collider.gameObject;
                // 어디에 있는 HP? > <Enemy>
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                // 어떤 HP?
                // - HP를 -1씩 감소
                enemy.Damaged(damage);
                // - HP가 0이되면 .. 파괴
                if(enemy.HP < 1){
                    Destroy(hitInfo.collider.gameObject);
                }
            }
            // 충돌 이펙트 재생
            PlayBulletEffect(hitInfo);
            // 사운드 재생

        }
        // 3-b. 충돌하지 않은 경우!
        else{

        }
    }

    // 총알 이펙트 재생 (특정 위치에서, 특정 방향으로)
    // - 충돌위치정보
    void PlayBulletEffect(RaycastHit hitInfo){
        // 충돌하면 충돌한 위치에 BulletEffect 를 옮긴다.
        ps_bulletEffect.transform.position = hitInfo.point;
        // bulletEffect 의 방향을 충돌위치의 Normal값(hitPoint의 수직방향)과 일치
        ps_bulletEffect.transform.forward = hitInfo.normal;
        // bullet effect 파티클을 replay
        ps_bulletEffect.Stop();
        ps_bulletEffect.Play();
    }

    void PlayMuzzleEffect(){
        ps_muzzleEffect.Stop();        
        ps_muzzleEffect.Play();        
    }
}
