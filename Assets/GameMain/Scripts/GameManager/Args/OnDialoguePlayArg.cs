using Framework.Develop;

namespace Framework.Args
{
    public class OnDialoguePlayArg : GameEventArgs
    {
        public static int EventId = typeof(OnDialoguePlayArg).GetHashCode();
        public override int Id => EventId;

        public bool Start;
        
        public static OnDialoguePlayArg Create(bool start)
        {
            var e = new OnDialoguePlayArg();
            e.Start = start;
            return e;
        }
    }
}