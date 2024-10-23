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

        public static IClassEventSystem Event;

        public static IProcedureManager Procedure;

        public static ISaveManager Save;
        
        
        private void GetBuiltInServices()
        {
            Scene = ServiceLocator.Get<ISceneController>();
            Resource = ServiceLocator.Get<IAssetLoader>();
            UI = ServiceLocator.Get<IUIManager>();
            Event = ServiceLocator.Get<IClassEventSystem>();
            Procedure = ServiceLocator.Get<IProcedureManager>();
            Save = ServiceLocator.Get<ISaveManager>();

        }
    }
}