using System;
using Framework;
using UnityEngine;

namespace GameMain
{
    public class OpeningSplash : MonoBehaviour
    {
        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.Play("anim");
        }

        public void AnimEnd()
        {
            (GameEntry.Procedure.CurrentProcedure as ProcedureSplash).SplashAnimEnd();
        }
    }
}