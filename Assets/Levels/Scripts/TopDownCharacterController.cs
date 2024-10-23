using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // �ƶ��ٶ�
    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ��ȡˮƽ�ʹ�ֱ���� (WASD ���ͷ��)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // ����һ������������ƶ�����
        movement = new Vector3(moveX, 0f, moveZ).normalized;

        //// �����ƶ�����
        //if (movement != Vector3.zero)
        //{
        //    Quaternion newRotation = Quaternion.LookRotation(movement);
        //    rb.MoveRotation(newRotation);
        //}

        // ͨ�������ƶ���ɫ
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}
