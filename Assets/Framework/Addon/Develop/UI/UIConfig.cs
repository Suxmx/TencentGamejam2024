using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public class UIUnit
    {
        public UIFormId Id;
        public string Path;
    }
    [CreateAssetMenu(menuName = "My/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        public string BasePath;
        public List<UIUnit> UIunits;

        public UIUnit GetUIUnit(UIFormId id)
        {
            foreach (var unit in UIunits)
            {
                if (unit.Id == id) return unit;
            }

            return null;
        }
        
        public string GetUIPath(UIUnit u)
        {
            return $"{BasePath}/{u.Path}.prefab";
        }
    }
}