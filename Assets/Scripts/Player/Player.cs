using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _nickName;
    public int Index;
    [SyncVar]
    public string NickName;
    //public override void OnStartLocalPlayer()
    //{
    //    if (RoundEvent.GetInstance().LocalPlayer == null) RoundEvent.GetInstance().LocalPlayer = this;
    //}
    private void Start()
    {
        _nickName.text = NickName;
    }

}
