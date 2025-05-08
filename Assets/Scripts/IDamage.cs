// Interface : 피격당할 수 있는지 여부를 나타내는 장치
// 껍데기, 장치, 구조 : 추상화 장치

public interface IDamage
{
    // Damaged : IDamage 인터페이스 사용 시, 반드시 만들어줘야 하는 함수
    // 피격당하면, 임의의 값(=데미지)만큼 HP 감소
    void Damage(int damage);
}