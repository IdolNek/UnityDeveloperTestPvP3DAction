using Mirror;
using System.Collections;
using UnityEngine;

public class InvincibleState : NetworkBehaviour
{
    [SerializeField] private float _isDamaged—ountdownTime;
    [SerializeField] private UIInvincibleState _uIInvincibleState;
    [SerializeField] private Animator _animatorController;
    [SyncVar]
    private bool _isDamaged = false;
    public bool IsDamaged => _isDamaged;
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
    [ClientRpc]
    private void RpcSetUIInvincibleState(float isDamaged—ountdownTime)
    {
        _uIInvincibleState.OnDamaged(isDamaged—ountdownTime);
    }

}
