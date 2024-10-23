namespace Framework.Develop
{
    public class OnLoaderAnimStartArg : GameEventArgs
    {
        public static int EventId = typeof(OnLoaderAnimStartArg).GetHashCode();
        public override int Id => EventId;

        public static OnLoaderAnimStartArg Create()
        {
            var e = new OnLoaderAnimStartArg();
            return e;
        }
    }
}