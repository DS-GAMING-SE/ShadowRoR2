using HedgehogUtils.Forms;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowMod.Characters.Survivors.Shadow.Paths
{
    public static class ShadowPaths
    {
        public static BodyIndex shadowBodyIndex;
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
        internal static void Init()
        {
            HedgehogUtils.Forms.FormHandler.onCheckCanTransform += SuperFormForHeroOnly;
        }
        private static void SuperFormForHeroOnly(FormDef form, FormComponent formComponent, bool hasItems, bool teamSuper, bool notTransformedMaxTimes, ref bool canTransform)
        {
            if (form == HedgehogUtils.Forms.SuperForm.SuperFormDef.superFormDef && formComponent.body.bodyIndex == shadowBodyIndex)
            {
                Path path = GetPath(formComponent.body.inventory);
                if (path != Path.Hero && path != Path.Neutral)
                {
                    canTransform = false;
                }
            }
        }
        [SystemInitializer(typeof(BodyCatalog))]
        private static void CacheShadowIndex()
        {
            shadowBodyIndex = BodyCatalog.FindBodyIndex(ShadowSurvivor.instance.bodyName);
        }
    }
}
