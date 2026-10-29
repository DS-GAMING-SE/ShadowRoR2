using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowMod.Characters.Survivors.Shadow.Paths
{
    public static class ShadowPaths
    {
        public static BodyIndex shadowBodyIndex;

        [SystemInitializer(typeof(BodyCatalog))]
        private static void CacheShadowIndex()
        {
            shadowBodyIndex = BodyCatalog.FindBodyIndex(ShadowSurvivor.instance.bodyName);
        }
        public enum Path
        {
            Neutral,
            Hero,
            Dark
        }
    }
}
