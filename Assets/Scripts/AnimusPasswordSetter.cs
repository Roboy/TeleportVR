using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimusPasswordSetter : MonoBehaviour
{
    [SerializeField] private ClientLogic clientLogic;
    // [SerializeField] private string AccountEmail;
    // [SerializeField] private string AccountPassword;
    
    void Awake()
    {
        clientLogic.AccountEmail = "";
        clientLogic.AccountPassword = "";
    }

}
