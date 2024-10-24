using Tencent;
using UnityEngine;

namespace GameMain
{
    public class CollectChip : CollectableObjBase
    {
        [SerializeField] private EMaterial MaterialType;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void OnCollected(Player player)
        {
            base.OnCollected(player);
            player.GetMaterialBullet(MaterialType);
        }
    }
}