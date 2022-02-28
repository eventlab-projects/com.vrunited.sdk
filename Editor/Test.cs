using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.AddressableAssets.Initialization;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

public class Test : MonoBehaviour
{

    #region CONSTANTS

    private const string VRUNITED_PROFILE_NAME = "VRUnited";
    private const string LABEL_AVATAR = "VRUAvatar";
    private const string LABEL_SCENE = "VRUScene";

    private const string REMOTE_LOAD_PATH = "{QuickVR.QuickAddressablesManager.URL}";

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
    public static void MyTest()
    {
        InitProfile();
        InitLabels(new string[] { LABEL_AVATAR, LABEL_SCENE });
        _settings.BuildRemoteCatalog = true;
        //_settings.DefaultGroup.GetSchema<BundledAssetGroupSchema>().BuildPath.SetVariableByName(_settings, ;

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

    #endregion

}
