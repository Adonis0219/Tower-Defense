using System.Collections;

interface IHit
{
    // �Ѿ� �ǰ�
    void Hit(float damage);

    // ������� �浹 �� ������ �Լ�
    IEnumerator OnCollisionHit(float damage);
}