using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using ExiledToolkit.Kit;
using FitnessBar.Adapters;
using FitnessBar.Core;

namespace FitnessBar;

// Soft, so Vital Signs loads first and is in the plugin list when Load() asks for it.
[BepInDependency(VitalSignsGuid, BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(ExiledToolkit.Plugin.Guid, ">=0.20.0")]
[BepInPlugin(Guid, "Fitness Bar", Version)]
public sealed class Plugin : BasePlugin
{
    private const string Guid = "com.dev.survivallog.fitnessbar";
    private const string Version = "0.1.5";
    private const string VitalSignsGuid = "com.dev.survivallog.vitalsigns";

    public override void Load()
    {
        var isEnabled = Config.Bind("FitnessBar", "Enabled", true, new ConfigDescription(
            "Shows a thin Fitness bar under the Life bar. Hover it to see what Fitness does and what your value means. Turn this off and the mod does nothing. With Vital Signs installed, this mod stays off because Vital Signs draws the same bar. Takes effect at the next game start.\n" +
            "在「生命」条下方显示一条细的健康条。鼠标悬停可以查看健康的作用和当前数值的含义。关闭后本模组不做任何改动。安装了 Vital Signs 时本模组保持关闭，因为 Vital Signs 会显示同样的健康条。下次启动游戏时生效。", null, "ModManager.Restart")).Value;
        var log = new ModLog(Log, "fitness-bar.log");

        var offReason = Startup.OffReason(isEnabled, IL2CPPChainloader.Instance.Plugins.ContainsKey(VitalSignsGuid));

        if (offReason != null)
        {
            log.Write($"===== Fitness Bar {Version} started: off, {offReason}");

            return;
        }

        log.Write($"===== Fitness Bar {Version} started");

        BarTicker.Log = log.Write;
        BarTicker.Page = new BarPage(log.Write);

        AddComponent<BarTicker>();
    }
}
