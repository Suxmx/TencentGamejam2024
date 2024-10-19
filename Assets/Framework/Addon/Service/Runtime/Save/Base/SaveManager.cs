using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Services;
using Services.Save;
using UnityEngine;

namespace Framework
{
    public class SaveManager : Service, ISaveManager
    {
        private Dictionary<ESaveGroup, SaveGroupData> _groupDatas = new();

        protected override void Awake()
        {
            base.Awake();
            //既然枚举里面有了，那么这个Group一定是要被用到的
            foreach (ESaveGroup group in Enum.GetValues(typeof(ESaveGroup)))
            {
                _groupDatas.Add(group, new SaveGroupData());
            }
        }

        public void SaveData(ESaveGroup group, string identifier, SaveData data)
        {
            _groupDatas[group].Save(identifier, data);
        }

        public void LoadAllData()
        {
            foreach (ESaveGroup group in Enum.GetValues(typeof(ESaveGroup)))
            {
                LoadGroupData(group);
            }
        }

        public void LoadGroupData(ESaveGroup group, string suffix = "")
        {
            var path = SaveUtility.GenerateSavePath($"{group}{suffix}Group.json");
            var fileInfo = FileTool.GetFileInfo(path);
            if (!fileInfo.Exists)
            {
                JsonTool.SaveAsJson(SaveGroupData.CreateDefault(group), path, JsonTool.PolyMorphicSettings);
            }

            _groupDatas[group] = JsonTool.LoadFromJson<SaveGroupData>(path, JsonTool.PolyMorphicSettings);
            //如果加载出来是空的就用默认的
            if (_groupDatas[group] is null)
            {
                _groupDatas[group] = SaveGroupData.CreateDefault(group);
            }
        }

        public void SaveGroupToDisk(ESaveGroup group, string suffix = "")
        {
            var path = SaveUtility.GenerateSavePath($"{group}{suffix}Group.json");
            JsonTool.SaveAsJson(_groupDatas[group], path, JsonTool.PolyMorphicSettings);
        }

        public T GetData<T>(ESaveGroup group, string identifier) where T: SaveData
        {
            return (T)_groupDatas[group].Get(identifier);
        }
    }
}