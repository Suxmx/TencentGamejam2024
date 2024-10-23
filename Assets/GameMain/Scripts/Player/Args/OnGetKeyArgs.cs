using Framework.Develop;
using UnityEngine;

namespace Tencent
{
    public class OnGetKeyArgs : GameEventArgs
    {
        public static int EventId = typeof(OnGetKeyArgs).GetHashCode();
        public override int Id => EventId;

        public string Key;
        public Sprite KeySprite;
        
        public static OnGetKeyArgs Create(string key,Sprite sprite)
        {
            var e = new OnGetKeyArgs();
            e.Key = key;
            e.KeySprite = sprite;
            return e;
        }
    }
}