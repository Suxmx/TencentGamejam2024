using System;
using Framework;
using Tencent;
using UnityEngine;

namespace GameMain
{
    public class GuideText : MonoBehaviour
    {
        [SerializeField] private string _guideText;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerTrigger>(out var player))
            {
                return;
            }
            Debug.Log("player enter");

            GameEntry.Event.Fire(this, OnShowGuideTextArgs.Create(_guideText, true));
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<PlayerTrigger>(out var player))
            {
                return;
            }
            Debug.Log("player exit");
            GameEntry.Event.Fire(this, OnShowGuideTextArgs.Create("", false));
        }
    }
}