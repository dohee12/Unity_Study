using Unity.VisualScripting;
using UnityEngine;

// 피격 시, HP 닳는 체력 시스템 구축
// - Enemy HP
// - HP (Get/Set Property)

public class Enemy : MonoBehaviour, IDmage
{
    public static int Abracadabra = 0;
    private int hp = 5;
    
    // get/set property로 Enemy의 hp 관리
    public int HP{
        // GET : HP를 호출하면, enemy.hp 값을 반환
        get{
            return hp;
        }
        // SET : HP를 호출해서 값을 할당하면, 
        // 할당된 값은 'value'안에 담겨서, set 안의
        // 코드를 실행. (=> enemy.hp 에 할당값을 대입)
        set{
            hp = value;
            // 만일 hp 값이 0이 된다면 Enemy(=자신)파괴
            if(hp < 1){
                Destroy(gameObject);
            }
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

    public void Damaged(int damage){
        // Player를 공격
        // - Player HP -1씩 감소
        PlayerHealth.Instance.HP -= damage;
    }
}
