using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script sets the credentials for animus in a way that the Password doesn't have to be pushed to git.
/// The AnimusPasswordStore can be installed with the UsedPlugins.unitypackage from GDrive
/// </summary>
public class AnimusPasswordSetter : MonoBehaviour
{
    [SerializeField] private ClientLogic clientLogic;
    
    /// <summary>
    /// This Function sets the credentials for the clientLogic. Should be in Awake,
    /// so that it gets called before the ClientLogic gets executed.
    /// </summary>
    void Awake()
    {
        clientLogic.AccountEmail = AnimusPasswordStore.AnimusEmail;
        clientLogic.AccountPassword = AnimusPasswordStore.AnimusPassword;
    }
}
