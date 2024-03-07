using UnityEngine;
using System;

/// <summary>
/// OWNER: Spencer Martin
/// Creates a scriptable object combined with .JSON for its format to store user profile data (GFX settings, Audio Settings, etc.)in an array of
/// instances of a data class named ProfileData. 
/// NOTE: depending on how we implement our save system this script may become obsolete
/// </summary>
/// 

namespace ReplicantPackage
{
    [CreateAssetMenu(fileName = "ProfileData", menuName = "CSharpFramework/ProfileScriptableObject")]
    // fileName is the default name for a ScriptableObject when we use the asset menu to add it to our project
    // menuName defines the location of the item

    [Serializable]
    public class ProfileScriptableObject : ScriptableObject
    {
        public Profiles theProfileData;
    }

    [Serializable]
    public class Profiles // can contain multiple instances of ProfileData if we need to store more than one user profile, may not be needed
    {
        public ProfileData[] profiles;
    }

    [Serializable]
    public class ProfileData
    {
        [SerializeField]
        public int myID;
        [SerializeField]
        public bool inUse;

        [SerializeField]
        public string profileName = "EMPTY";
        [SerializeField]
        public string playerName = "Anonymous";

        //[SerializeField]
        //public float sfxVolume;
        
        //[SerializeField]
        //public float musicVolume;
    }
}

