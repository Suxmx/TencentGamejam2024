using Tencent;

namespace GameMain
{
    public class CollectStar : CollectableObjBase
    {
        public string StarName;
        public override void OnCollected(Player player)
        {
            base.OnCollected(player);
            player.OnCollectStar(StarName);
        }
    }
}