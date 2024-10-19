using System;
using System.Collections.Generic;

namespace Framework
{
    [Serializable]
    public class SaveGroupData
    {
        public  Dictionary<string, SaveData> Datas=new();

        public void Save(string identifier, SaveData data)
        {
            data.Identifier = identifier;
            Datas[identifier] = data;
        }

        public SaveData Get(string identifier)
        {
            if (!Datas.TryGetValue(identifier, out var data))
                return null;
            return data;
        }

        public static SaveGroupData CreateDefault(ESaveGroup group)
        {
            var data = new SaveGroupData();
            switch (group)
            {
                case ESaveGroup.Settings:
                    var settingData = new SettingSaveData();
                    settingData.Test1 = 10;
                    data.Save("Test",settingData);
                    break;
                case ESaveGroup.GameData:
                    
                    break;
            }

            return data;
        }
    }
}