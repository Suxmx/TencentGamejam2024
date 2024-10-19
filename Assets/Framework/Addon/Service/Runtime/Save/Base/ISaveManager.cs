namespace Framework
{
    public interface ISaveManager: IService
    {
        void SaveData(ESaveGroup group, string identifier, SaveData data);
        void LoadAllData();
        void LoadGroupData(ESaveGroup group,string suffix="");
        void SaveGroupToDisk(ESaveGroup group,string suffix="");
        T GetData<T>(ESaveGroup group, string identifier) where T: SaveData;
    }
}