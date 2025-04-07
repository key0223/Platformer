using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public const string TAG_GROUND = "Ground";
    public const string TAG_PLAYER = "Player";
    public const string TAG_POOLROOT = "PoolRoot";
    public enum CreatureState
    {
        Idle,
        Moving,
        Skill,
        Damaged,
        Dead,
    }

    public enum BuffType
    {
        Buff,
        Debuff,
    }

    public enum Layer
    {
        Ground = 3,
        Monster = 7,
        Breakable = 9,
    }

    public enum ItemType
    {
        Coin,
        Weapon,
        Charm,
        Spell,
    }

    public enum CharmType
    {
        Attack,
        Hp,
        Soul,
        Orbs,
    }

    public enum CharmSlotType
    {
        EquippedSlot,
        SelectionSlot,
    }

    public enum DoorToSpawnAt
    {
        None,
        One,
        Two,
        Three,
        four,
    }
}
