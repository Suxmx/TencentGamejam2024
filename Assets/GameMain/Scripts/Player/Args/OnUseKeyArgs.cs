using Framework.Develop;

namespace Tencent
{
    public class OnUseKeyArgs : GameEventArgs
    {
        public static int EventId = typeof(OnUseKeyArgs).GetHashCode();
        public override int Id => EventId;
        public string Key;
        public bool AllClear = false;

        public static OnUseKeyArgs Create(string key, bool clear = false)
        {
            var e = new OnUseKeyArgs();
            e.Key = key;
            e.AllClear = clear;
            return e;
        }
    }
}