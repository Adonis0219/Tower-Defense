using System.Collections;

interface IHit
{
    // 총알 피격
    void Hit(float damage);

    // 상대방과의 충돌 시 데미지 함수
    IEnumerator OnCollisionHit(float damage);
}