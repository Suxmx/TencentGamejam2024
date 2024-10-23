using Framework.Develop;

namespace Tencent
{
    public class OnCameraModeChangeArg : GameEventArgs
    {
        public static int EventId = typeof(OnCameraModeChangeArg).GetHashCode();
        public override int Id => EventId;

        public ECameraMode Mode;
        
        public static OnCameraModeChangeArg Create(ECameraMode mode)
        {
            var e = new OnCameraModeChangeArg();
            e.Mode = mode;
            return e;
        }
    }
}