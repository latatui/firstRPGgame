using UnityEngine;

namespace WarriorAnimsFREE
{
    public class WarriorInput : MonoBehaviour
    {
        private WarriorController warriorController;

        private void Awake()
        {
            warriorController = GetComponent<WarriorController>();
        }

        private void Update()
        {
            if (warriorController.canAction)
            {
                HandleAttack();
                HandleJump();
            }
        }

        private void HandleAttack()
        {
            if (warriorController.MaintainingGround() && warriorController.canAction)
            {
                if (Input.GetMouseButtonDown(0)) // 좌클릭 입력 감지
                {
                    warriorController.Attack1();
                }
            }
        }

        private void HandleJump()
        {
            if (warriorController.canJump && warriorController.canAction)
            {
                if (warriorController.MaintainingGround() && Input.GetKeyDown(KeyCode.Space)) // 스페이스바 입력 감지
                {
                    warriorController.inputJump = true;
                }
            }
        }
    }
}
