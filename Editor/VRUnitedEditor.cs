using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.AddressableAssets.Initialization;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

public static class VRUnitedEditor
{

    #region CONSTANTS

    private const string VRUNITED_PROFILE_NAME = "VRUnited";
    private const string LABEL_AVATAR = "VRUAvatar";
    private const string LABEL_SCENE = "VRUScene";

    private const string REMOTE_LOAD_PATH = "{QuickVR.QuickAddressablesManager.URL}";

    private const string GROUP_BUILD_IN_DATA = "Built In Data";

    #endregion

    #region PRIVATE ATTRIBUTES

    private static AddressableAssetSettings _settings
    {
        get
        {
            if (!m_Settings)
            {
                m_Settings = AddressableAssetSettingsDefaultObject.Settings;
            }
            return m_Settings;
        }
    }
    private static AddressableAssetSettings m_Settings = null;

    #endregion

    #region CREATION AND DESTRUCTION

    [MenuItem("VRUnited/InitAddressableSettings")]
    public static void InitAddressableSettings()
    {
        InitProfile();
        InitLabels(new string[] { LABEL_AVATAR, LABEL_SCENE });
        InitGroups();

        _settings.BuildRemoteCatalog = true;
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
