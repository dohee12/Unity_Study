using UnityEngine;

// Player의 HP 시스템
// - HP (Get/Set Property)
// - Singleton (Design Pattern) :: Only One Instance.
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    private int hp = 10;

    public int HP{
        get{
            return hp;
        }
        set{
            hp = value;
            // hp가 0이되면 GameOver
            if(hp < 1){
                #if UNITY_EDITOR
                // A.Test : Unity Editor를 Stop
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                // B.Production : 프로그램 종료 or 일시정지
                Time.timeScale = 0f;
                #endif
            }
        }
    }

    // Unity 실행 시, 가장 먼저 실행되는 Lifecycle Event함수
    void Awake()
    {
        // Singleton으로 나 자신을 변환
        // 싱글톤으로 만들어진 객체가 없다면(초기상태), 나 자신을 싱글톤 객체로 할당
        if(Instance == null){
            Instance = this;
        }
        // 만일 이미 인스턴스 안의 값이 존재한다면
        else{
            // 이미 존재하는 PlayerHealth가 있다는 의미이므로
            // 나 자신은 필요없음 -> 파괴해도 무방
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
