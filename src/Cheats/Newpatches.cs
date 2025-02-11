using AmongUs.Data;
using HarmonyLib;
using MalumMenu;
using UnityEngine;



[HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.Update))]
public static class PlayerTabIsSelectedItemEquippedPatch
{
    public static void Postfix(PlayerTab __instance)
    {
        if (CheatToggles.Any_colors)
        {
            __instance.currentColorIsEquipped = false;
        }
        
    }


}
[HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.UpdateAvailableColors))]
public static class PlayerTabUpdateAvailableColorsPatch
{
    public static bool Prefix(PlayerTab __instance)
    {
        if (CheatToggles.Any_colors)
        {
            __instance.AvailableColors.Clear();
            for (var i = 0; i < Palette.PlayerColors.Count; i++)
            {
                __instance.AvailableColors.Add(i);
            }
            return false;
        }
        return true;
    }
}


[HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
public static class PingTracker_Update
{
    // Postfix patch of PingTracker.Update to ping
    public static void Postfix(PingTracker __instance)
    {
        if (!CheatToggles.Ping_Colors) return;

        __instance.text.text = $"{Utils.getColoredPingText(AmongUsClient.Instance.Ping)}";
    }
}


[HarmonyPatch(typeof(StatsManager), nameof(StatsManager.BanMinutesLeft), MethodType.Getter)]
public static class StatsManager_BanMinutesLeft_Getter
{
    // Prefix patch of Getter method for StatsManager.BanMinutesLeft to remove disconnect penalty
    public static void Postfix(StatsManager __instance, ref int __result)
    {
        if (CheatToggles.avoidBans)
        {
            __instance.BanPoints = 0f; // Removes all BanPoints
            __result = 0; // Removes all BanMinutes
        }
    }
}



[HarmonyPatch(typeof(HatManager), nameof(HatManager.Initialize))]
public static class HatManager_Initialize
{
    public static void Postfix(HatManager __instance)
    {

        CosmeticsUnlocker.unlockCosmetics(__instance);

    }
}

[HarmonyPatch(typeof(FullAccount), nameof(FullAccount.CanSetCustomName))]
public static class FullAccount_CanSetCustomName
{
    // Prefix patch of FullAccount.CanSetCustomName to allow the usage of custom names
    public static void Prefix(ref bool canSetName)
    {
        if (CheatToggles.unlockFeatures)
        {
            canSetName = true;
        }
    }
}
[HarmonyPatch(typeof(AccountManager), nameof(AccountManager.CanPlayOnline))]
public static class AccountManager_CanPlayOnline
{
    // Prefix patch of AccountManager.CanPlayOnline to allow online games
    public static void Postfix(ref bool __result)
    {
        if (CheatToggles.unlockFeatures)
        {
            __result = true;
        }
    }
}

[HarmonyPatch(typeof(InnerNet.InnerNetClient), nameof(InnerNet.InnerNetClient.JoinGame))]
public static class InnerNet_InnerNetClient_JoinGame
{
    // Prefix patch of InnerNet.InnerNetClient.JoinGame to allow online games
    public static void Prefix()
    {
        if (CheatToggles.unlockFeatures)
        {
            DataManager.Player.Account.LoginStatus = EOSManager.AccountLoginStatus.LoggedIn;
        }
    }
}


