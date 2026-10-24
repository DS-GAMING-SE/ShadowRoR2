using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowMod.Characters.Survivors.Shadow.Paths
{
    public static class Items
    {
        internal static void Init()
        {
            IL.RoR2.GenericPickupController.HandlePickupMessage += PathPickupNotification;
            IL.RoR2.UI.NotificationUIController.SetUpNotification += PathItemNotificationPrefab;
            On.RoR2.UI.GenericNotification.SetItem += ExtraTextForPathItems;
        }

        private static void PathPickupNotification(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            int masterIndex = -1;
            int pickupIndex = -1;
            Instruction normalPickupNotification = null;
            ILLabel afterPickupNotification = null;
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdloc(out masterIndex),
                x => x.MatchLdloc(out pickupIndex),
                x => x.MatchCallOrCallvirt(AccessTools.Method(typeof(CharacterMasterNotificationQueue), 
                nameof(CharacterMasterNotificationQueue.PushPickupNotification), [typeof(CharacterMaster), typeof(UniquePickup)]))))
            {
                normalPickupNotification = c.Prev;

                if (c.TryGotoPrev(MoveType.AfterLabel, x => x.MatchBgt(out afterPickupNotification)))
                {
                    c.Goto(normalPickupNotification);
                    c.EmitDelegate<Func<CharacterMaster, UniquePickup, bool>>((master, pickup) =>
                    {
                        if (master.GetBody() && master.GetBody().bodyIndex == ShadowPaths.shadowBodyIndex)
                        {
                            PickupDef pickupDef = PickupCatalog.GetPickupDef(pickup.pickupIndex);
                            if (pickupDef != null && pickupDef.itemIndex != ItemIndex.None)
                            {
                                ItemDef itemDef = ItemCatalog.GetItemDef(pickupDef.itemIndex);
                                if (GetItemPath(itemDef) != ShadowPaths.Path.Neutral)
                                {
                                    PushPathItemNotification(new CharacterMasterNotificationQueue.PushItemNotificationArgs
                                    {
                                        characterMaster = master,
                                        itemIndex = itemDef.itemIndex,
                                        isTemporary = pickup.isTempItem,
                                        isRental = pickup.isRental,
                                        upgradeCount = new int?(pickup.upgradeValue)
                                    });
                                    return true;
                                }
                            }
                        }
                        return false;
                    });
                    c.Emit(OpCodes.Brtrue, afterPickupNotification);
                    // Add stuff back to the stack in case my check fails because my delegate eated it all
                    c.Emit(OpCodes.Ldloc, masterIndex);
                    c.Emit(OpCodes.Ldloc, pickupIndex);
                }
                else
                {
                    Log.Error("PathPickupNotification IL 2 FAILED");
                }
            }
            else
            {
                Log.Error("PathPickupNotification IL 1 FAILED");
            }
        }
        private static void PathItemNotificationPrefab(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ILLabel itemDefPrefabSelection = null;
            ILLabel afterPrefabSelection = null;
            if (c.TryGotoNext(
                x => x.MatchIsinst<TrialVariantEventRelay.TrialVariantNotificationInfo>(),
                x => x.MatchBrfalse(out itemDefPrefabSelection) &&
                c.TryGotoNext(MoveType.After,
                x => x.MatchStfld<NotificationUIController>(nameof(NotificationUIController.currentNotification)),
                x => x.MatchBr(out afterPrefabSelection))))
            {
                c.Goto(itemDefPrefabSelection.Target, MoveType.AfterLabel);
                
                c.Emit(OpCodes.Ldarg_0); // NotificationUIController
                c.Emit(OpCodes.Ldarg_1); // notificationInfo
                c.Emit(OpCodes.Ldarg_0);
                c.Emit<NotificationUIController>(OpCodes.Ldfld, nameof(NotificationUIController.genericNotificationPrefab));
                c.EmitDelegate<Func<NotificationUIController, CharacterMasterNotificationQueue.NotificationInfo, GameObject, bool>>((ui, notificationInfo, genericPrefab) => 
                {
                    if (notificationInfo.data is PathItem pathItem)
                    {
                        GameObject prefab;
                        switch (pathItem.path)
                        {
                            case ShadowPaths.Path.Hero:
                                prefab = ShadowAssets.pathItemHeroNotificationPrefab;
                                break;
                            case ShadowPaths.Path.Dark:
                                prefab = ShadowAssets.pathItemDarkNotificationPrefab;
                                break;
                            default:
                                prefab = genericPrefab;
                                break;
                        }
                        ui.currentNotification = GameObject.Instantiate(prefab).GetComponent<GenericNotification>();
                        return true;
                    }
                    return false;
                });
                c.Emit(OpCodes.Brtrue, afterPrefabSelection);
                if (c.TryGotoNext(MoveType.After,
                x => x.MatchIsinst<ItemDef>()))
                {
                    c.Emit(OpCodes.Ldarg_1);
                    c.EmitDelegate<Func<ItemDef, CharacterMasterNotificationQueue.NotificationInfo, ItemDef>>((itemDef, notificationInfo) =>
                    {
                        if (notificationInfo.data is PathItem pathItem)
                        {
                            return pathItem.item;
                        }
                        return itemDef;
                    });
                }
                else
                {
                    Log.Error($"{il.Method.Name} IL 2 FAILED");
                }
            }
            else
            {
                Log.Error($"{il.Method.Name} IL 1 FAILED");
            }
        }
        public static void PushPathItemNotification(CharacterMasterNotificationQueue.PushItemNotificationArgs args, CharacterMasterNotificationQueue.CustomOverrideInfo overrides = null)
        {
            if (!args.characterMaster.hasAuthority) return;

            CharacterMasterNotificationQueue notificationQueueForMaster = CharacterMasterNotificationQueue.GetNotificationQueueForMaster(args.characterMaster);
            if (notificationQueueForMaster && args.itemIndex != ItemIndex.None)
            {
                ItemDef itemDef = ItemCatalog.GetItemDef(args.itemIndex);
                if (itemDef == null || itemDef.hidden)
                {
                    return;
                }
                float duration = 9f;
                /*if (args.characterMaster.inventory.GetItemCountEffective(args.itemIndex) > 1)
                {
                    duration = 6f;
                }*/
                notificationQueueForMaster.PushNotification(new CharacterMasterNotificationQueue.NotificationInfo(
                    new PathItem(ItemCatalog.GetItemDef(args.itemIndex), GetItemPath(itemDef)), null, overrides, true, args.isTemporary, args.upgradeCount.GetValueOrDefault(), args.isRental), duration);
            }
        }
        private static void ExtraTextForPathItems(On.RoR2.UI.GenericNotification.orig_SetItem orig, GenericNotification self, ItemDef item)
        {
            orig(self, item);
            if (self.extraDescriptionText)
            {
                self.extraDescriptionText.token = GetItemPath(item) == ShadowPaths.Path.Hero ? ShadowSurvivor.SHADOW_PREFIX + "HERO_PICKUP_SUFFIX" : ShadowSurvivor.SHADOW_PREFIX + "DARK_PICKUP_SUFFIX";
            }
        }
        public static ShadowPaths.Path GetItemPath(ItemDef item)
        {
            if (ItemTierCatalog.GetItemTierDef(item.tier) == HedgehogUtils.Forms.SuperForm.Items.emeraldTier)
            {
                return ShadowPaths.Path.Hero;
            }
            /*if (ItemTierCatalog.GetItemTierDef(item.tier) == ItemTierCatalog.GetItemTierDef(ItemTier.Tier1))
            {
                return ShadowPaths.Path.Dark;
            }*/
            return ShadowPaths.Path.Neutral;
        }
        public struct PathItem // This class just exists so I can include the fact that the itemDef is a path item when I send the object data over
        {
            public ItemDef item;
            public ShadowPaths.Path path;
            public PathItem(ItemDef item, ShadowPaths.Path path)
            {
                this.item = item;
                this.path = path;
            }
        }
    }
}
