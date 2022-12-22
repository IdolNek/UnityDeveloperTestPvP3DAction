using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkGamePlayer : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _nickName;
    public int Index;
    [SyncVar]
    public string NickName;
    private void Start()
    {
        _nickName.text = NickName;
    }

}
