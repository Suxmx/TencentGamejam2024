using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Framework
{
    public class QuaternionConverter : JsonConverter
    {
        // 序列化 Quaternion 为 JSON
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Quaternion quaternion = (Quaternion)value;
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(quaternion.x);
            writer.WritePropertyName("y");
            writer.WriteValue(quaternion.y);
            writer.WritePropertyName("z");
            writer.WriteValue(quaternion.z);
            writer.WritePropertyName("w");
            writer.WriteValue(quaternion.w);
            writer.WriteEndObject();
        }

        // 从 JSON 反序列化为 Quaternion 对象
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            float x = (float)obj["x"];
            float y = (float)obj["y"];
            float z = (float)obj["z"];
            float w = (float)obj["w"];
            return new Quaternion(x, y, z, w);
        }

        // 告诉 Json.NET 这个转换器适用于 Quaternion 类型
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Quaternion);
        }
    }

}