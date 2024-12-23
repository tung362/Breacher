using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RotaryHeart.Lib.SerializableDictionary;

namespace Breacher
{
    [CreateAssetMenu(fileName = "ItemObject", menuName = "ScriptableObjects/Create Item Object", order = 1)]
    public class ItemObject : ScriptableObject, ISerializationCallbackReceiver
    {
        #region Format
        [System.Serializable]
        public class ItemObjectDictionary : SerializableDictionaryBase<string, ItemData> { }
        #endregion

        public ItemObjectDictionary _ItemObjects = new ItemObjectDictionary();

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }

        #if UNITY_EDITOR
        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void Clear(bool save = true)
        {
            _ItemObjects = new ItemObjectDictionary();

            //Save
            if (save) Save();
        }
        #endif
    }
}
