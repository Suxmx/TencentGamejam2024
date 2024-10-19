using Framework.Develop;
using Services;
using Services.Asset;
using Services.ObjectPools;
using Services.SceneManagement;

namespace Framework
{
    public partial class GameEntry
    {
        public static ISceneController Scene;

        public static IAssetLoader Resource;

        public static IUIManager UI;

        public static IClassEventSystem NewEvent;

        public static IProcedureManager Procedure;

        public static ISaveManager Save;

        public static IAudioManager Audio;
        
        private void GetBuiltInServices()
        {
            Scene = ServiceLocator.Get<ISceneController>();
            Resource = ServiceLocator.Get<IAssetLoader>();
            UI = ServiceLocator.Get<IUIManager>();
            NewEvent = ServiceLocator.Get<IClassEventSystem>();
            Procedure = ServiceLocator.Get<IProcedureManager>();
            Save = ServiceLocator.Get<ISaveManager>();
            Audio = ServiceLocator.Get<IAudioManager>();

        }
    }
}