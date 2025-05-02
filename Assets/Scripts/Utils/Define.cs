using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public const string TAG_GROUND = "Ground";
    public const string TAG_PLAYER = "Player";
    public const string TAG_POOLROOT = "PoolRoot";
    public const string TAG_BOUNDS_CONFINER = "BoundsConfiner";

    public enum Dir
    {
        None,
        Right,
        Left,
        Up,
        Down,
    }
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
        Map,
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

    public enum InteractionType
    {
        None,
        Shop,
        Listen,
        Examine,
        Door,
        Rest,
    }

    public enum SceneName
    {
        Greenpath,
        Dirtmouth,
    }
}
