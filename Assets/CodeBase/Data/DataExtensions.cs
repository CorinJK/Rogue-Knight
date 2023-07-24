using UnityEngine;

namespace CodeBase.Data
{
    public static class DataExtensions
    {
        // Преобразовать Vector3 в сохраняемый скрипт Vector3Data
        public static Vector3Data AsVectorData(this Vector3 vector) => 
            new Vector3Data(vector.x, vector.y, vector.z);

        // Получить из скрипта Vector3Data - Vector3
        public static Vector3 AsUnityVector(this Vector3Data vector3Data) =>
            new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);

        // Добавить высоту по оси Y
        public static Vector3 AddY(this Vector3 vector, float y)
        {
            vector.y += y;
            return vector;
        }

        // Записать в формат Json
        public static string ToJson(this object obj) => 
            JsonUtility.ToJson(obj);

        // Получить из Json
        public static T ToDeserialized<T>(this string json) => 
            JsonUtility.FromJson<T>(json);
    }
}