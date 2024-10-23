using System;
using System.Collections;
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
            else
            {
                GetComponent<AssetLoader>().enabled = false;
            }

            if (GameEntry.Event is null)
            {
                GameEntry.Event = GetComponent<ClassEventSystem>();
            }
            else
            {
                GetComponent<ClassEventSystem>().enabled = false;
            }

            if (GameEntry.UI is null)
            {
                GameEntry.UI = GetComponent<UIManager>();
                (GameEntry.UI as UIManager).InitImmediately();
            }
            else
            {
                GetComponent<UIManager>().enabled = false;
                transform.Find("UI").gameObject.SetActive(false);
            }

            
        }

        private void Start()
        {
            AGameManager.Instance.OnEnter();
        }
    }
}