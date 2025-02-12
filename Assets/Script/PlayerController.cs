using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;

    void Update()
    {
        // 이동 처리
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical) * Time.deltaTime * 5f;
        transform.Translate(movement);

        // 공격 처리 (예시)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        // 공격력에 전직 후 배율을 고려
        float attackDamage = 10f * playerStats.attackMultiplier;
        Debug.Log("공격! 데미지: " + attackDamage);
    }
}
