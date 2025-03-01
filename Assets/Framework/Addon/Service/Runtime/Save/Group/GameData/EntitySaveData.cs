﻿using UnityEngine;

namespace Framework
{
    public class EntitySaveData : SaveData
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
    
    public class SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}