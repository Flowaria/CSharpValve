using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.KeyValue;

namespace TF2.Population
{
    public class Population
    {
        public int StartingCurrency { get; set; }
        public int RespawnWaveTime { get; set; }
        public int AddSentryBusterWhenDamageDealtExceeds { get; set; }
        public int AddSentryBusterWhenKillCountExceeds { get; set; }
        public bool FixedRespawnWaveTime { get; set; }
        public bool CanBotsAttackWhileInSpawnRoom { get; set; }
        public bool Advanced { get; set; }
        public bool EventPopfile { get; set; }
        public int WaveLength { get; set; }

        public Population(int wave_length)
        {
            WaveLength = wave_length;
        }

        public static Population FromKeyValue(KeyValues kv)
        {
            return null;
        }
    }
}
