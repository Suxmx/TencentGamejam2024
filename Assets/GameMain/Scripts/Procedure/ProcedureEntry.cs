using Framework;
using JSAM;

namespace GameMain
{
    public class ProcedureEntry : ProcedureBase
    {
        private const string _audioLibrary1 = "Assets/GameMain/Audio/AudioLibrary/DefaultAudioLibrary.asset";
        private const string _audioLibrary2 = "Assets/GameMain/Audio/AudioLibrary/UIAudio Library.asset";

        public override void OnEnter()
        {
            base.OnEnter();
            var L1 = GameEntry.Resource.Load<AudioLibrary>(_audioLibrary1);
            var L2 = GameEntry.Resource.Load<AudioLibrary>(_audioLibrary1);
            // AudioManager.InternalInstance.LoadAudioLibrary(L1);
            // AudioManager.InternalInstance.LoadAudioLibrary(L2);
            Owner.SetValue("NextScene", "Splash");
            ChangeState<ProcedureChangeScene>();
        }
    }
}