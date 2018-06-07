using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF2.Info
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
        None,
        SpawnWithFullCharge,
        AlwaysCrit,
        AlwaysFireWeapon,
        MiniBoss,
        UseBossHealthBar,
        HoldFireUntilFullReload,
        IgnoreFlag,
        TeleportToHint,
        AutoJump,
        AirChargeOnly,
        Parachute,
        VaccinatorBullets,
        VaccinatorBlast,
        VaccinatorFire,
        ProjectileShield
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
