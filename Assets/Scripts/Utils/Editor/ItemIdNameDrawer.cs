
using Data;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ItemIdNameAttribute))]
public class ItemIdNameDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        if (property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUI.BeginChangeCheck(); // Start of check for changed values

            // Draw item code
            var newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height / 2), label, property.intValue);

            // Draw item description
            EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Item Name", GetItemName(property.intValue));



            // If item code value has changed, then set value to new value
            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = newValue;
            }
        }

        EditorGUI.EndProperty();
    }

    private string GetItemName(int itemId)
    {
        SO_ItemList so_itemList;

        so_itemList = AssetDatabase.LoadAssetAtPath("Assets/Resources/Scriptable Objects/SO_ItemList.asset", typeof(SO_ItemList)) as SO_ItemList;

        List<ItemData>[] itemLists = new List<ItemData>[]
        {
            so_itemList.weaponDatas.Select(x=> (ItemData)x).ToList(),
            so_itemList.charmDatas.Select(x=> (ItemData)x).ToList(),
            so_itemList.miniMapDatas.Select(x=> (ItemData)x).ToList(),
            so_itemList.itemDatas.Select(x=> (ItemData)x).ToList(),
        };

        foreach(List<ItemData> list in itemLists)
        {
            ItemData data =list.Find(x=> x.itemId == itemId);
            if (data != null)
                return data.itemName;
        }

        return "";
    }
}
