using Framework.Develop;
using GameMain;

namespace Tencent
{
    public class OnBulletNumChangeArg : GameEventArgs
    {
        public static int EventId = typeof(OnBulletNumChangeArg).GetHashCode();
        public override int Id => EventId;

        public EMaterial MaterialType;
        public int BulletNum;

        public static OnBulletNumChangeArg Create(EMaterial type, int num)
        {
            var e = new OnBulletNumChangeArg();
            e.MaterialType = type;
            e.BulletNum = num;
            return e;
        }
    }
}