using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 获取水平和垂直输入 (WASD 或箭头键)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // 创建一个基于输入的移动向量
        movement = new Vector3(moveX, 0f, moveZ).normalized;

        //// 面向移动方向
        //if (movement != Vector3.zero)
        //{
        //    Quaternion newRotation = Quaternion.LookRotation(movement);
        //    rb.MoveRotation(newRotation);
        //}

        // 通过刚体移动角色
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}
