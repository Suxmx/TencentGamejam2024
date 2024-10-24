using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain
{
    [CreateAssetMenu(menuName = "配置/材质配置")]
    public class ChangeableConfigSO : SerializedScriptableObject
    {
        public Dictionary<EMaterial, ChangeableConfigUnit> MaterialDict = new();
    }

    [Serializable]
    public class ChangeableConfigUnit
    {
        [LabelText("物体材质")] public List<Material> ObjMaterials;

        [LabelText("子弹材质")] public Material BulletMaterial;
    }
}