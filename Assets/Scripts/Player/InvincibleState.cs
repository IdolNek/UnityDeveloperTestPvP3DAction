using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InvincibleState : NetworkBehaviour
{
    [SerializeField] private float _isDamaged—ountdownTime;
    [SerializeField] private UIInvincibleState _uIInvincibleState;
    [SerializeField] private Animator _animatorController;
    [SyncVar]
    private bool _isDamaged = false;
    public bool IsDamaged => _isDamaged;
    //public event UnityAction<float> OnDamaged;
    [Server]
    public void ApplyDamage()
    {
        if (_isDamaged) return;
        _isDamaged = true;
        RpcSetUIInvincibleState(_isDamaged—ountdownTime);
        _animatorController.SetTrigger("GetHit");
        StartCoroutine(Damaged—ountdown());
    }
    private IEnumerator Damaged—ountdown()
    {
        yield return new WaitForSeconds(_isDamaged—ountdownTime);
        _isDamaged = false;
    }

    //[Server]
    //private void SetIsDamaged(bool isDamaged)
    //{
    //    _isDamaged = isDamaged;
    //}
    //[Command]
    //private void CmdSetDamaged7(bool isDamaged)
    //{
    //    SetIsDamaged(isDamaged);
    //}
    [ClientRpc]
    private void RpcSetUIInvincibleState(float isDamaged—ountdownTime)
    {
        _uIInvincibleState.OnDamaged(isDamaged—ountdownTime);
    }

}
