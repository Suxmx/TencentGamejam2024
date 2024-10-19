using System;
using System.Collections;
using Framework.Audio;
using Framework.Develop;
using Services.Asset;
using Services.SceneManagement;
using UnityEngine;

namespace Framework
{
    public partial class GameEntry : MonoBehaviour
    {
        private IEnumerator Start()
        {
            GetBuiltInServices();
            GetCustomServices();
            yield break;
        }
    }
}