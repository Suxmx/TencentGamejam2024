using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tencent;
using UnityEngine;

namespace GameMain
{
    public class CollectKey :CollectableObjBase
    {
        [SerializeField, LabelText("钥匙对应Key")] private string _keyString;

        [SerializeField, LabelText("钥匙对应icon")]
        private Sprite _icon;

        public override void OnCollected(Player player)
        {
            base.OnCollected(player);
            var keyInfo = new KeyInfo();
            keyInfo.KeyIcon = _icon;
            keyInfo.KeyString = _keyString;
            player.GetKey(keyInfo);
            Destroy(gameObject);
        }
    }
}