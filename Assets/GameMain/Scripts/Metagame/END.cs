using System;
using UnityEngine;

namespace GameMain.Scripts.Metagame
{
    public class END : MonoBehaviour
    {
        private void Update()
        {
            if (Input.anyKey)
            {
                Application.Quit();
            }
        }
    }
}