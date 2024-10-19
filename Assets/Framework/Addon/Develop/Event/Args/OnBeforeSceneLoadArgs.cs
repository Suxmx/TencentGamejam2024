namespace Framework.Develop
{
    public class OnBeforeSceneLoadArgs : GameEventArgs
    {
        public static int EventId = typeof(OnBeforeSceneLoadArgs).GetHashCode();
        public override int Id => EventId;

        public int SceneIndex;
        public static OnBeforeSceneLoadArgs Create(int index)
        {
            var e = new OnBeforeSceneLoadArgs();
            e.SceneIndex = index;
            return e;
        }
    }
}