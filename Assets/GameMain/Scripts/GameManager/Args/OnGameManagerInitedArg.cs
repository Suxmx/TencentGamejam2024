using Framework.Develop;

namespace Framework.Args
{
    public class OnGameManagerInitedArg : GameEventArgs
    {
        public static int EventId = typeof(OnGameManagerInitedArg).GetHashCode();
        public override int Id => EventId;
        
        public static OnGameManagerInitedArg Create()
        {
            var e = new OnGameManagerInitedArg();
            return e;
        }
    }
}