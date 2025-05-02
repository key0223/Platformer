using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(ItemTest))]
public class Custom_ItemTest : Editor
{

    // 인스펙터를 그리는 함수
    public override void OnInspectorGUI()
    {
        ItemTest itemTest = (ItemTest)target;
      
        itemTest._itemId = EditorGUILayout.IntField("Item Id",itemTest._itemId);

        if (itemTest._itemId == 0)
            return;

        EditorGUILayout.Space();
        if (GUILayout.Button("ADD ITEM"))
            itemTest.AddItem();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif