using Framework;
using Framework.Develop;
using UnityEngine;

namespace GameMain
{
    public partial class GameForm : UGuiForm
    {
        public override void OnInit()
        {
            base.OnInit();
            GetBindComponents(gameObject);
        }

        public override void OnOpen()
        {
            base.OnOpen();

        }

        public override void OnClose()
        {
            base.OnClose();

        }
    }
}