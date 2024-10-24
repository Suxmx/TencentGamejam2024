using Framework;
using MyTimer;

namespace Tencent
{
    public class DestroyAfterTimeVfx : GameEntityBase
    {
        private TimerOnly _destroyTime = new();

        public override void OnInit()
        {
            base.OnInit();
            _destroyTime.AfterCompelete += _ => UnspawnObj();
        }

        public override void OnShow(object userData)
        {
            base.OnShow(userData);
            float time = (float)userData;
            _destroyTime.Initialize(time);
        }

        public override void OnHide()
        {
            base.OnHide();
            _destroyTime.Paused = true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _destroyTime.Paused = true;
        }
    }
}