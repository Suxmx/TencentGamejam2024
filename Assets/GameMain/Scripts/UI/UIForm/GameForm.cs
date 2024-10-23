using System;
using System.Collections.Generic;
using Framework;
using Framework.Develop;
using Tencent;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public partial class GameForm : UGuiForm
    {
        [SerializeField] private GameObject _keyIconPrefab;

        private Dictionary<string, GameObject> _keyDict = new();
        private ECameraMode _cameraMode;

        public override void OnInit()
        {
            base.OnInit();
            GetBindComponents(gameObject);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(OnGunMaterialChangeArg.EventId, OnMaterialChange);
            GameEntry.Event.Subscribe(OnGetKeyArgs.EventId, OnGetKey);
            GameEntry.Event.Subscribe(OnUseKeyArgs.EventId, OnUseKey);
            GameEntry.Event.Subscribe(OnCameraModeChangeArg.EventId, OnCameraModeChange);
        }

        public override void OnClose()
        {
            base.OnClose();
            GameEntry.Event.Unsubscribe(OnGunMaterialChangeArg.EventId, OnMaterialChange);
            GameEntry.Event.Unsubscribe(OnGetKeyArgs.EventId, OnGetKey);
            GameEntry.Event.Unsubscribe(OnUseKeyArgs.EventId, OnUseKey);
            GameEntry.Event.Unsubscribe(OnCameraModeChangeArg.EventId, OnCameraModeChange);
        }

        private void OnGetKey(object sender, GameEventArgs arg)
        {
            var e = (OnGetKeyArgs)arg;
            var keyobj = Instantiate(_keyIconPrefab, m_trans_keys, true);
            if (e.KeySprite is not null)
            {
                keyobj.GetComponent<Image>().sprite = e.KeySprite;
            }

            _keyDict.Add(e.Key, keyobj);
        }

        private void OnUseKey(object sender, GameEventArgs arg)
        {
            var e = (OnUseKeyArgs)arg;
            Destroy(_keyDict[e.Key]);
        }

        private void OnMaterialChange(object sender, GameEventArgs arg)
        {
            var e = (OnGunMaterialChangeArg)arg;
            Debug.Log("receive event");
            m_tmp_curMaterial.text = $"当前材质：{e.Material}";
        }

        private void OnCameraModeChange(object sender, GameEventArgs arg)
        {
            var e = (OnCameraModeChangeArg)arg;
            _cameraMode = e.Mode;
            if (_cameraMode == ECameraMode.FirstPerson)
            {
                Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (m_rect_cusor.parent as RectTransform),
                    screenCenter,
                    null,
                    out var uiPosition
                );

                // 更新准心的位置
                m_rect_cusor.anchoredPosition = uiPosition;
            }
        }

        private void Update()
        {
            if (_cameraMode == ECameraMode.TopDownShot)
            {
                Vector2 mousePosition = Input.mousePosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (m_rect_cusor.parent as RectTransform),
                    mousePosition,
                    null,
                    out var uiPosition
                );

                // 更新准心的位置
                m_rect_cusor.anchoredPosition = uiPosition;
            }
        }
    }
}