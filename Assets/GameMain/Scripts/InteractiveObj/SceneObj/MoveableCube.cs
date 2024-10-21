using System;
using UnityEngine;

namespace Tencent
{
    public class MoveableCube : MonoBehaviour
    {
        private Rigidbody _rigid;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
        }

        public void StartMove()
        {
            _rigid.useGravity = false;
        }

        public void EndMove()
        {
            _rigid.linearVelocity = Vector3.zero;
            _rigid.useGravity = true;
        }

        public void SetTargetPosition(Vector3 position)
        {
            var dist = (transform.position - position).magnitude;
            float speed = 0;
            if (dist > 5)
            {
                speed = Mathf.Clamp(dist, 6, 9f);
            }
            else if (dist > 2)
            {
                speed = 3f;
            }
            else if (dist > 1.5f)
            {
                speed = 2f;
            }
            else
            {
                speed = Mathf.Clamp(dist, 0, 1)*2;
            }

            _rigid.linearVelocity = (position - transform.position).normalized * speed;
        }
    }
}