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
        [SerializeField]private GameObject _keyIconPrefab;
        
        private Dictionary<string, GameObject> _keyDict = new();
        
        public override void OnInit()
        {
            base.OnInit();
            GetBindComponents(gameObject);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            GameEntry.Event.Subscribe(OnGunMaterialChangeArg.EventId, OnMaterialChange);
            GameEntry.Event.Subscribe(OnGetKeyArgs.EventId,OnGetKey);
            GameEntry.Event.Subscribe(OnUseKeyArgs.EventId,OnUseKey);
        }

        public override void OnClose()
        {
            base.OnClose();
            GameEntry.Event.Unsubscribe(OnGunMaterialChangeArg.EventId, OnMaterialChange);
            GameEntry.Event.Unsubscribe(OnGetKeyArgs.EventId,OnGetKey);
            GameEntry.Event.Unsubscribe(OnUseKeyArgs.EventId,OnUseKey);
        }

        private void OnGetKey(object sender, GameEventArgs arg)
        {
            var e = (OnGetKeyArgs)arg;
            var keyobj = Instantiate(_keyIconPrefab, m_trans_keys, true);
            if (e.KeySprite is not null)
            {
                keyobj.GetComponent<Image>().sprite = e.KeySprite;
            }
            _keyDict.Add(e.Key,keyobj);
        }

        private void OnUseKey(object sender, GameEventArgs arg)
        {
            var e = (OnUseKeyArgs)arg;
            Destroy(_keyDict[e.Key]);
        }
        
        private void OnMaterialChange(object sender, GameEventArgs arg)
        {
            var e = (OnGunMaterialChangeArg)arg;
            m_tmp_curMaterial.text = $"当前材质：{e.Material}";
        }
    }
}