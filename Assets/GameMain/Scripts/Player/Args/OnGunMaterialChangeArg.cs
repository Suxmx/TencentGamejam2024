using Framework.Develop;
using GameMain;

namespace Tencent
{
    public class OnGunMaterialChangeArg : GameEventArgs
    {
        public static int EventId = typeof(OnGunMaterialChangeArg).GetHashCode();
        public override int Id => EventId;

        public EMaterial Material;
        public static OnGunMaterialChangeArg Create(EMaterial material)
        {
            var e = new OnGunMaterialChangeArg();
            e.Material = material;
            return e;
        }
    }
}