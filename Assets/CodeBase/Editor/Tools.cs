using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    public class Tools
    {
        [MenuItem("Tool/ClearPrefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}