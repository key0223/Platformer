using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BlueSlimeMovement : MonsterMovement
{
    #region Parameters
    const string BUFF_NAME = "BlueSlimeDebuff";
    const string BUFF_STAT_TYPE = "Attack";
    float _buffPercentage = 10f;
    float _duration = 5f;
    #endregion

    [Header("FX Settings")]
    [SerializeField] GameObject _fxPoint;

    protected override void Attack()
    {
        CreateFX();

        Collider2D hit = Physics2D.OverlapBox(_hitBoxPoint.position, _hitBoxSize, 0, _playerLayer);
        if (hit != null)
        {
            PlayerMovement player = hit.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.OnDamaged(_stat.CurrentAttack);
                Buff newBuff = new Buff();
                newBuff.Init(BuffType.Debuff, BUFF_NAME, BUFF_STAT_TYPE, _buffPercentage, _duration);

                BuffManager.Instance.AddBuff(newBuff);
            }
        }
    }

    void CreateFX()
    {
        GameObject fxGO = ResourceManager.Instance.Instantiate("FX/MonsterFX/Blue SlimeFX");
        fxGO.transform.SetParent(_fxPoint.transform, false);
        fxGO.SetActive(true);
    }
}
