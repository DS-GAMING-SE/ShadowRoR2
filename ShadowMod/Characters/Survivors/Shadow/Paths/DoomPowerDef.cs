using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowMod.Characters.Survivors.Shadow.Paths
{
    public class DoomPowerDef : IComparable<DoomPowerDef>
    {
        public SkillSlot skillSlot;
        public SkillDef skillToOverride;
        public SkillDef skillDef;
        public string pickupToken;
        public int requiredDoomPower = 1;

        public int CompareTo(DoomPowerDef doomPowerDef)
        {
            return requiredDoomPower.CompareTo(doomPowerDef.requiredDoomPower);
        }

        public static DoomPowerDef[] doomPowerDefs = [];
        public static void AddDoomPowerDefs(DoomPowerDef[] newDoomPowerDefs)
        {
            int oldLength = doomPowerDefs.Length;
            Array.Resize(ref doomPowerDefs, oldLength + newDoomPowerDefs.Length);
            for (int i = 0; i < newDoomPowerDefs.Length; i++)
            {
                doomPowerDefs[oldLength + i] = newDoomPowerDefs[i];
            }
            doomPowerDefs = doomPowerDefs.OrderBy(x => x).ToArray();
        }
        public static void AddDoomPowerDef(DoomPowerDef doomPowerDef)
        {
            Array.Resize(ref doomPowerDefs, doomPowerDefs.Length + 1);
            doomPowerDefs[doomPowerDefs.Length - 1] = doomPowerDef;
            doomPowerDefs = doomPowerDefs.OrderBy(x => x).ToArray();
        }
    }
}
