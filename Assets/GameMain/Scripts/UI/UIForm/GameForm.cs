using System;
using Framework;
using Framework.Develop;
using Tencent;
using UnityEngine;

namespace GameMain
{
    public partial class GameForm : UGuiForm
    {
        public override void OnInit()
        {
            base.OnInit();
            GetBindComponents(gameObject);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            GameEntry.NewEvent.Subscribe(OnGunMaterialChangeArg.EventId, OnMaterialChange);
        }

        public override void OnClose()
        {
            base.OnClose();
            GameEntry.NewEvent.Unsubscribe(OnGunMaterialChangeArg.EventId, OnMaterialChange);
        }

        private void OnMaterialChange(object sender, GameEventArgs arg)
        {
            var e = (OnGunMaterialChangeArg)arg;
            m_tmp_curMaterial.text = $"当前材质：{e.Material}";
        }
    }
}