using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EpicTortoiseStudios
{
    public class ModularUI_Instance : Editor
    {
        [MenuItem("GameObject/Modular UI/Button", priority = 0)]
        public static void AddButton()
        {
            Create("TEST_Button");
        }

        static GameObject clickedObject;

        private static GameObject Create(string objectName)
        {
            GameObject instance = Instantiate(Resources.Load<GameObject>(objectName));
            instance.name = objectName;
            clickedObject = UnityEditor.Selection.activeGameObject as GameObject;
            if (clickedObject != null)
            {
                instance.transform.SetParent(clickedObject.transform, false);
            }
            return instance;
        }
    }
}
