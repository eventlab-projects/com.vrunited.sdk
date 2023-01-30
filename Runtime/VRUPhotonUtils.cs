using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace VRUnited
{

    public static class VRUPhotonUtils
    {

        #region CONSTANTS

        //Keys for Room's Custom Properties
        public const string KEY_GEOMETRY_SCENE = "GeometryScene";
        public const string KEY_ROOM_PASSWORD = "pass";
        public const string KEY_ROOM_SCENE_NAME = "SN";
        public const string KEY_ROOM_FREE_SPOTS = "FS";
        public const string KEY_ROOM_SESSION_PATH = "SP";

        //Keys for Player's Custom Properties
        public const string KEY_PLAYER_GUEST_VIEW_ID = "GVID"; //If I am a Guest, this property identifies the PhotonView component of the Guest
        public const string KEY_PLAYER_SPOT_ID = "SID";
        public const string KEY_PLAYER_CALIBRATION_HAND_HEIGHT = "CHH";

        #endregion

        #region EXTENSION METHODS COMMON

        public static T GetPropertyValue<T>(this ExitGames.Client.Photon.Hashtable properties, string key, T defaultValue = default(T))
        {
            T result = defaultValue;

            if (properties.TryGetValue(key, out object obj))
            {
                result = (T)obj;
            }

            return result;
        }

        private static void SetCustomProperty<T>(this Player player, string key, T value)
        {
            ExitGames.Client.Photon.Hashtable properties = player.CustomProperties;
            properties[key] = value;
            player.SetCustomProperties(properties);
        }

        private static void SetCustomProperty<T>(this Room room, string key, T value)
        {
            ExitGames.Client.Photon.Hashtable properties = room.CustomProperties;
            properties[key] = value;
            room.SetCustomProperties(properties);
        }

        #endregion

        #region EXTENSION METHODS ROOM

        public static bool HasProperty(this Room room, string key)
        {
            return room.CustomProperties.ContainsKey(key);
        }

        public static string GetSceneName(this Room room)
        {
            return room.CustomProperties.GetPropertyValue<string>(KEY_ROOM_SCENE_NAME);
        }

        public static void SetSceneName(this Room room, string sceneName)
        {
            room.SetCustomProperty(KEY_ROOM_SCENE_NAME, sceneName);
        }

        public static string GetPassword(this Room room)
        {
            return room.CustomProperties.GetPropertyValue<string>(KEY_ROOM_PASSWORD);
        }

        public static long GetFreeSpots(this Room room)
        {
            return room.CustomProperties.GetPropertyValue<long>(KEY_ROOM_FREE_SPOTS, 0);
        }

        public static void SetFreeSpots(this Room room, long freeSpots)
        {
            room.SetCustomProperty(KEY_ROOM_FREE_SPOTS, freeSpots);
        }

        public static bool IsFreeSpot(this Room room, int spotID)
        {
            return (((long)1 << spotID) & room.GetFreeSpots()) == 0;
        }

        public static void LockSpot(this Room room, int spotID)
        {
            room.SetFreeSpots(room.GetFreeSpots() | ((long)1 << spotID));
        }

        public static void UnlockSpot(this Room room, int spotID)
        {
            if (spotID != -1)
            {
                room.SetFreeSpots(room.GetFreeSpots() & ~((long)1 << spotID));
            }
        }

        public static int GetFirstFreeSpot(this Room room, bool autoLock = true)
        {
            //Returns the first free spot in the room. If autoLock is enabled, it also locks that spot. 
            int freeSpotID = -1;
            for (int i = 0; i < 64 && freeSpotID == -1; i++)
            {
                if (room.IsFreeSpot(i))
                {
                    freeSpotID = i;
                }
            }

            if (autoLock && freeSpotID != -1)
            {
                room.LockSpot(freeSpotID);
                Debug.Log("freeSpotID = " + freeSpotID);
                Debug.Log("afterUnlock = " + room.GetFreeSpots());
            }

            return freeSpotID;
        }

        public static string GetRecordedSessionPath(this Room room)
        {
            return room.CustomProperties.GetPropertyValue<string>(KEY_ROOM_SESSION_PATH);
        }

        public static void SetRecordedSessionPath(this Room room, string sessionPath)
        {
            room.SetCustomProperty(KEY_ROOM_SESSION_PATH, sessionPath);
        }

        #endregion

        #region EXTENSION METHODS PLAYER

        public static bool HasProperty(this Player player, string key)
        {
            return player.CustomProperties.ContainsKey(key);
        }

        public static int GetSpotID(this Player player)
        {
            return player.CustomProperties.GetPropertyValue<int>(KEY_PLAYER_SPOT_ID, -1);
        }

        public static void SetSpotID(this Player player, int spotID)
        {
            player.SetCustomProperty(KEY_PLAYER_SPOT_ID, spotID);
        }

        public static float GetCalibrationHandHeight(this Player player)
        {
            return player.CustomProperties.GetPropertyValue<float>(KEY_PLAYER_CALIBRATION_HAND_HEIGHT, 0);
        }

        public static void SetCalibrationHandHeight(this Player player, float handHeight)
        {
            player.SetCustomProperty(KEY_PLAYER_CALIBRATION_HAND_HEIGHT, handHeight);
        }

        public static int GetGuestViewID(this Player player)
        {
            return player.CustomProperties.GetPropertyValue<int>(KEY_PLAYER_GUEST_VIEW_ID, -1);
        }

        public static void SetGuestViewID(this Player player, int guestViewID)
        {
            player.SetCustomProperty(KEY_PLAYER_GUEST_VIEW_ID, guestViewID);
        }

        public static bool IsGuest(this Player player)
        {
            return player.GetGuestViewID() != -1;
        }

        #endregion

    }

}


