using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace VRUnited
{

    public static class VRUnitedEditor
    {

        #region CONSTANTS

        public const string VRUNITED_PROFILE_NAME = "VRUnited";
        public const string LABEL_AVATAR = "VRUAvatar";
        public const string LABEL_SCENE_SETTINGS = "VRUSceneSettings";

        private const string REMOTE_LOAD_PATH = "{QuickVR.QuickAddressablesManager.URL}";

        private const string GROUP_BUILD_IN_DATA = "Built In Data";

        #endregion

        #region PRIVATE ATTRIBUTES

        public static AddressableAssetSettings _settings
        {
            get
            {
                if (!m_Settings)
                {
                    if (AddressableAssetSettingsDefaultObject.Settings == null)
                    {
                        AddressableAssetSettingsDefaultObject.Settings = AddressableAssetSettings.Create(AddressableAssetSettingsDefaultObject.kDefaultConfigFolder, AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName, true, true);
                    }
                    m_Settings = AddressableAssetSettingsDefaultObject.Settings;
                }
                return m_Settings;
            }
        }
        private static AddressableAssetSettings m_Settings = null;

        #endregion

        #region CREATION AND DESTRUCTION

        [MenuItem("VRUnited/Init Addressable Settings")]
        public static void InitAddressableSettings()
        {
            InitProfile();
            InitLabels(new string[] { LABEL_AVATAR, LABEL_SCENE_SETTINGS });
            InitGroups();

            _settings.BuildRemoteCatalog = true;
        }

        [MenuItem("VRUnited/Convert Asset Bundles to Addressables")]
        public static void ConvertAssetBundlesToAddressables()
        {
            var bundleList = AssetDatabase.GetAllAssetBundleNames();
            if (bundleList.Length > 0)
            {
                AssetDatabase.RemoveUnusedAssetBundleNames();

                float fullCount = bundleList.Length;
                int currCount = 0;

                foreach (var bundle in bundleList)
                {
                    if (EditorUtility.DisplayCancelableProgressBar("Converting Legacy Asset Bundles", bundle, currCount / fullCount))
                    {
                        break;
                    }

                    currCount++;
                    var group = _settings.CreateGroup(bundle, false, false, false, null);
                    var schema = group.AddSchema<BundledAssetGroupSchema>();
                    schema.BuildPath.SetVariableByName(_settings, AddressableAssetSettings.kLocalBuildPath);
                    schema.LoadPath.SetVariableByName(_settings, AddressableAssetSettings.kLocalLoadPath);
                    schema.BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackTogether;
                    group.AddSchema<ContentUpdateGroupSchema>().StaticContent = true;

                    var assetList = AssetDatabase.GetAssetPathsFromAssetBundle(bundle);

                    foreach (var asset in assetList)
                    {
                        var guid = AssetDatabase.AssetPathToGUID(asset);
                        _settings.CreateOrMoveEntry(guid, group, false, false);
                        var imp = AssetImporter.GetAtPath(asset);
                        if (imp != null)
                        {
                            imp.SetAssetBundleNameAndVariant(string.Empty, string.Empty);
                        }
                    }
                }

                if (fullCount > 0)
                {
                    _settings.SetDirty(AddressableAssetSettings.ModificationEvent.BatchModification, null, true, true);
                }

                EditorUtility.ClearProgressBar();
                AssetDatabase.RemoveUnusedAssetBundleNames();
            }
            else
            {
                Debug.Log("No Asset Bundles found!!!");
            }
        }

        private static void InitProfile()
        {
            List<string> profileNames = _settings.profileSettings.GetAllProfileNames();
            if (!profileNames.Contains(VRUNITED_PROFILE_NAME))
            {
                _settings.profileSettings.AddProfile(VRUNITED_PROFILE_NAME, _settings.profileSettings.GetProfileId("Default"));
            }
            string profileID = _settings.profileSettings.GetProfileId(VRUNITED_PROFILE_NAME);
            _settings.activeProfileId = profileID;
            _settings.profileSettings.SetValue(profileID, "RemoteLoadPath", REMOTE_LOAD_PATH);
        }

        private static void InitLabels(string[] labels)
        {
            List<string> definedLabels = _settings.GetLabels();
            foreach (string l in labels)
            {
                if (!definedLabels.Contains(l))
                {
                    _settings.AddLabel(l);
                }
            }
        }

        public static void InitGroups()
        {
            foreach (AddressableAssetGroup group in _settings.groups)
            {
                InitGroup(group);
            }
        }

        public static void InitGroup(string groupName)
        {
            InitGroup(_settings.FindGroup(groupName));
        }

        private static void InitGroup(AddressableAssetGroup group)
        {
            if (group)
            {
                if (group.name != GROUP_BUILD_IN_DATA)
                {
                    BundledAssetGroupSchema schema = group.GetSchema<BundledAssetGroupSchema>();
                    schema.BuildPath.SetVariableByName(_settings, "RemoteBuildPath");
                    schema.LoadPath.SetVariableByName(_settings, "RemoteLoadPath");
                }
            }
        }

        #endregion

    }

}
