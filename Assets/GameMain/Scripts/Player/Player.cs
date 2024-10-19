using System;
using UnityEngine;

namespace Tencent
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        private PlayerInput _input;

        private void Awake()
        {
           InitComponents();
        }

        private void Update()
        {
            _input.UpdateInput();
        }

        #region Init

        private void InitComponents()
        {
            _input = GetComponent<PlayerInput>();
        }
        #endregion
    }
}