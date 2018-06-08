using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valve.TF2.Info
{
    //TF
    public enum TFClass
    {
        None = 0,
        Scout = 1,
        Soldier = 2,
        Pyro = 3,
        Heavy = 4,
        Engineer = 5,
        Demoman = 6,
        Sniper = 7,
        Spy = 8,
        Medic = 9
    }

    [Flags]
    public enum TFBotAttribute //BOT ATTRIBUTE
    {
        None = 0,
        SpawnWithFullCharge = 1 << 0,
        AlwaysCrit = 1 << 1,
        AlwaysFireWeapon = 1 << 2,
        MiniBoss = 1 << 3,
        UseBossHealthBar = 1 << 4,
        HoldFireUntilFullReload = 1 << 5,
        IgnoreFlag = 1 << 6,
        TeleportToHint = 1 << 7,
        AutoJump = 1 << 8,
        AirChargeOnly = 1 << 9,
        Parachute = 1 << 10,
        VaccinatorBullets = 1 << 11,
        VaccinatorBlast = 1 << 12,
        VaccinatorFire = 1 << 13,
        ProjectileShield = 1 << 14,
        All = int.MaxValue
    }

    public enum TFBotSkill //BOT SKILL LEVEL
    {
        Easy, Normal, Hard, Expert
    }

    public enum TFBotWeaponRestrictions //BOT WEAPON RESTRICTION
    {
        None, PrimaryOnly, SecondaryOnly, MeleeOnly
    }

    //ITEMS
    public enum TFItemSlot
    {
        None, Primary, Secondary, Melee,
        PDA1, PDA2, Building, Misc, Action
    }

    public enum TFAttrEffectType
    {
        Positive,
        Negative,
        Neutral
    }

    public enum TFAttrDescriptionFormat
    {
        Percentage,
        Inverted_Percentage,

        Additive,
        Additive_Percentage,

        From_Lookup_Table,
        Date,
        Or,

        Particle_Index,
        KillstreakEffect_Index,
        Killstreak_IdleEffect_Index
    }
}
