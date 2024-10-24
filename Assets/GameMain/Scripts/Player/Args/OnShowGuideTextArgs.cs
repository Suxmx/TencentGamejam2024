using Framework.Develop;

namespace Tencent
{
    public class OnShowGuideTextArgs : GameEventArgs
    {
        public static int EventId = typeof(OnShowGuideTextArgs).GetHashCode();
        public override int Id => EventId;

        public string GuideText;
        public bool Show;

        public static OnShowGuideTextArgs Create(string guideTxt,bool show)
        {
            var e = new OnShowGuideTextArgs();
            e.GuideText = guideTxt;
            e.Show = show;
            return e;
        }
    }
}