using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valve.TF2.Info
{
    public class TFEnumConvert
    {
        public static TFClass StringToTFClass(string str)
        {
            switch (str.ToLower())
            {
                case "scout":
                    return TFClass.Scout;
                case "soldier":
                    return TFClass.Soldier;
                case "pyro":
                    return TFClass.Pyro;
                case "engineer":
                case "engi":
                    return TFClass.Engineer;
                case "heavyweapons":
                case "heavy":
                    return TFClass.Heavy;
                case "demoman":
                case "demo":
                    return TFClass.Demoman;
                case "medic":
                    return TFClass.Medic;
                case "sniper":
                    return TFClass.Sniper;
                case "spy":
                    return TFClass.Spy;
                default:
                    return TFClass.None;
            }
        }
        public static TFItemSlot StringToSlot(string str)
        {
            switch (str.ToLower())
            {
                case "primary":
                    return TFItemSlot.Primary;
                case "secondary":
                    return TFItemSlot.Secondary;
                case "melee":
                    return TFItemSlot.Melee;
                case "pda":
                    return TFItemSlot.PDA1;
                case "pda2":
                    return TFItemSlot.PDA2;
                case "building":
                    return TFItemSlot.Building;
                case "misc":
                    return TFItemSlot.Misc;
                default:
                    return TFItemSlot.None;
            }
        }
        public static TFAttrDescriptionFormat StringToDescriptionFormat(string str)
        {
            switch (str.ToLower())
            {
                case "value_is_percentage":
                    return TFAttrDescriptionFormat.Percentage;
                case "value_is_inverted_percentage":
                    return TFAttrDescriptionFormat.Inverted_Percentage;
                case "value_is_additive":
                    return TFAttrDescriptionFormat.Additive;
                case "value_is_additive_percentage":
                    return TFAttrDescriptionFormat.Additive_Percentage;
                case "value_is_from_lookup_table":
                    return TFAttrDescriptionFormat.From_Lookup_Table;
                case "value_is_date":
                    return TFAttrDescriptionFormat.Date;
                case "value_is_or":
                    return TFAttrDescriptionFormat.Or;
                case "value_is_particle_index":
                    return TFAttrDescriptionFormat.Particle_Index;
                case "value_is_killstreakeffect_index":
                    return TFAttrDescriptionFormat.KillstreakEffect_Index;
                case "value_is_killstreak_idleeffect_index":
                    return TFAttrDescriptionFormat.Killstreak_IdleEffect_Index;
                default:
                    return TFAttrDescriptionFormat.Additive;
            }
        }

        public static TFAttrEffectType StringToEffectType(string str)
        {
            switch (str.ToLower())
            {
                case "positive":
                    return TFAttrEffectType.Positive;
                case "negative":
                    return TFAttrEffectType.Negative;
                case "neutral":
                default:
                    return TFAttrEffectType.Neutral;
            }
        }
    }
    
}
