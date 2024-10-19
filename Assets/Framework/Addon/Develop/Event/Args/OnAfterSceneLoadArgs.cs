
namespace  Framework.Develop
{
    public class OnAfterSceneLoadArgs : GameEventArgs
    {
        public static int EventId = typeof(OnAfterSceneLoadArgs).GetHashCode();
        public override int Id => EventId;

        public int SceneIndex;
        public static OnAfterSceneLoadArgs Create(int index)
        {
            var e = new OnAfterSceneLoadArgs();
            e.SceneIndex = index;
            return e;
        }
    }
}
