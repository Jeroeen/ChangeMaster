using UnityEngine;
using System.Collections;

namespace LitJson.Extensions
{

    public static class Extensions
    {

        public static void WriteProperty(this JsonWriter jsonWriter, string name, long value)
        {
            jsonWriter.WritePropertyName(name);
            jsonWriter.Write(value);
        }

        public static void WriteProperty(this JsonWriter jsonWriter, string name, string value)
        {
            jsonWriter.WritePropertyName(name);
            jsonWriter.Write(value);
        }

        public static void WriteProperty(this JsonWriter jsonWriter, string name, bool value)
        {
            jsonWriter.WritePropertyName(name);
            jsonWriter.Write(value);
        }

        public static void WriteProperty(this JsonWriter jsonWriter, string name, double value)
        {
            jsonWriter.WritePropertyName(name);
            jsonWriter.Write(value);
        }

    }
}
