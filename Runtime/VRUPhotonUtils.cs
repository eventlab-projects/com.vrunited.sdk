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

        //Keys for Player's Custom Properties
        public const string KEY_PLAYER_GUEST_VIEW_ID = "GVID"; //If I am a Guest, this property identifies the PhotonView component of the Guest
        public const string KEY_PLAYER_SPOT_ID = "SID";
        public const string KEY_PLAYER_CALIBRATION_HAND_HEIGHT = "CHH";

        #endregion

        #region EXTENSION METHODS

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

        public static string GetSceneName(this Room room)
        {
            return room.CustomProperties.GetPropertyValue<string>(KEY_ROOM_SCENE_NAME);
        }

        public static void SetSceneName(this Room room, string sceneName)
        {
            room.SetCustomProperty(KEY_ROOM_SCENE_NAME, sceneName);
            //ExitGames.Client.Photon.Hashtable roomProperties = room.CustomProperties;
            //roomProperties[KEY_ROOM_SCENE_NAME] = sceneName;
            //room.SetCustomProperties(roomProperties);
        }

        public static string GetPassword(this Room room)
        {
            return room.CustomProperties.GetPropertyValue<string>(KEY_ROOM_PASSWORD);
        }

        public static int GetSpotID(this Player player)
        {
            return player.CustomProperties.GetPropertyValue<int>(KEY_PLAYER_SPOT_ID, -1);
        }

        public static void SetSpotID(this Player player, int spotID)
        {
            player.SetCustomProperty(KEY_PLAYER_SPOT_ID, spotID);
            //ExitGames.Client.Photon.Hashtable playerProperties = player.CustomProperties;
            //playerProperties[KEY_PLAYER_SPOT_ID] = spotID;
            //player.SetCustomProperties(playerProperties);
        }

        public static float GetCalibrationHandHeight(this Player player)
        {
            return player.CustomProperties.GetPropertyValue<float>(KEY_PLAYER_CALIBRATION_HAND_HEIGHT, 0);
        }

        public static void SetCalibrationHandHeight(this Player player, float handHeight)
        {
            player.SetCustomProperty(KEY_PLAYER_CALIBRATION_HAND_HEIGHT, handHeight);
            //ExitGames.Client.Photon.Hashtable playerProperties = player.CustomProperties;
            //playerProperties[KEY_PLAYER_CALIBRATION_HAND_HEIGHT] = handHeight;
            //player.SetCustomProperties(playerProperties);
        }

        public static int GetGuestViewID(this Player player)
        {
            return player.CustomProperties.GetPropertyValue<int>(KEY_PLAYER_GUEST_VIEW_ID, -1);
        }

        public static void SetGuestViewID(this Player player, int guestViewID)
        {
            player.SetCustomProperty(KEY_PLAYER_GUEST_VIEW_ID, guestViewID);
            //ExitGames.Client.Photon.Hashtable playerProperties = player.CustomProperties;
            //playerProperties[KEY_PLAYER_GUEST_VIEW_ID] = guestViewID;
            //player.SetCustomProperties(playerProperties);
        }

        public static bool IsGuest(this Player player)
        {
            return player.GetGuestViewID() != -1;
        }

        public static long GetFreeSpots(this Room room)
        {
            return room.CustomProperties.GetPropertyValue<long>(KEY_ROOM_FREE_SPOTS, 0);
        }

        public static void SetFreeSpots(this Room room, long freeSpots)
        {
            room.SetCustomProperty(KEY_ROOM_FREE_SPOTS, freeSpots);
            //ExitGames.Client.Photon.Hashtable roomProperties = room.CustomProperties;
            //roomProperties[KEY_ROOM_FREE_SPOTS] = freeSpots;
            //room.SetCustomProperties(roomProperties);
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

        #endregion

    }

}


