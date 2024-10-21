using Framework.Develop;

namespace Tencent.Args
{
    public class OnPressurePlateStateChangeArg : GameEventArgs
    {
        public static int EventId = typeof(OnPressurePlateStateChangeArg).GetHashCode();
        public override int Id => EventId;

        public string TriggerKey;
        public bool Enable;
        public static OnPressurePlateStateChangeArg Create(string triggerKey,bool enable)
        {
            var e = new OnPressurePlateStateChangeArg();
            e.TriggerKey = triggerKey;
            e.Enable = enable;
            return e;
        }
    }
}