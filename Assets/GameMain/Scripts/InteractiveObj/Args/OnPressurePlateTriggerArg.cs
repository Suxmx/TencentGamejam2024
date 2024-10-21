using Framework.Develop;

namespace Tencent.Args
{
    public class OnPressurePlateTriggerArg : GameEventArgs
    {
        public static int EventId = typeof(OnPressurePlateTriggerArg).GetHashCode();
        public override int Id => EventId;

        public string TriggerKey;
        public static OnPressurePlateTriggerArg Create(string triggerKey)
        {
            var e = new OnPressurePlateTriggerArg();
            e.TriggerKey = triggerKey;
            return e;
        }
    }
}