using UnityEditor;
using UnityEngine;

using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace VRUnited
{
    
    public class VRUAssetPostprocessor : AssetPostprocessor
    {

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            string extension = ".asset";
            foreach (string str in importedAssets)
            {
                if (str.EndsWith(extension))
                {
                    //Debug.Log("Reimported Asset: " + str);
                    VRUSceneSettings sceneSettings = AssetDatabase.LoadAssetAtPath<VRUSceneSettings>(str);
                    if (sceneSettings && !IsAssetAddressable(sceneSettings))
                    {
                        AddressableAssetSettings settings = VRUnitedEditor._settings;
                        AddressableAssetGroup g = settings.DefaultGroup;
                        string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(sceneSettings));

                        //This is the function that actually makes the object addressable
                        AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, g);
                        entry.labels.Add(VRUnitedEditor.LABEL_SCENE_SETTINGS);

                        //entry.address = custom_address;
                        //You'll need these to run to save the changes!
                        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }

        #region GET AND SET

        private static bool IsAssetAddressable(UnityEngine.Object obj)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetEntry entry = settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj)));
            return entry != null;
        }

        #endregion

    }

}

