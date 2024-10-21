using System;
using Framework;
using Framework.Develop;
using Services.Asset;
using UnityEngine;

namespace GameMain
{
    [DefaultExecutionOrder(-1000)]
    public class GameTester : MonoBehaviour
    {
        private void Awake()
        {
            if (GameEntry.Resource is null)
            {
                GameEntry.Resource = GetComponent<AssetLoader>();
            }

            if (GameEntry.NewEvent is null)
            {
                GameEntry.NewEvent = GetComponent<ClassEventSystem>();
            }
            
            AGameManager.Instance.OnEnter();
        }
    }
}