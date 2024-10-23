using Framework.Develop;

namespace Tencent
{
    public class OnUseKeyArgs : GameEventArgs
    {
        public static int EventId = typeof(OnUseKeyArgs).GetHashCode();
        public override int Id => EventId;
        public string Key;
        

        public static OnUseKeyArgs Create(string key)
        {
            var e = new OnUseKeyArgs();
            e.Key = key;
            return e;
        }
    }
}