#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RouletteGame.Scripts.Editor
{
    [CustomEditor(typeof(ServiceManager))]
    public class ServiceManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            if (GUILayout.Button("Print Registered Services"))
            {
                ServiceLocator.DebugPrintAllServices();
            }
        }
    }
}
#endif