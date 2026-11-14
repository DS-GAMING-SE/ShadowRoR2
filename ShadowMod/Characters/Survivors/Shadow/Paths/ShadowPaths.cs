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
        public static void SetPath(Inventory inventory, Path path)
        {
            if (inventory) inventory.GiveItemPermanent(Items.pathIdentifierItemDef, ((int)path) - inventory.GetItemCountPermanent(Items.pathIdentifierItemDef));
        }
        public static Path GetPath(Inventory inventory)
        {
            return !inventory ? Path.NeutralConsumed : (Path)inventory.GetItemCountPermanent(Items.pathIdentifierItemDef);
        }
        public enum Path
        {
            NeutralConsumed,
            Neutral,
            Hero,
            Dark
        }
    }
}
