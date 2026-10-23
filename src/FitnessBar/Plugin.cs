using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using FitnessBar.Adapters;

namespace FitnessBar;

[BepInPlugin(Guid, "Fitness Bar", Version)]
public sealed class Plugin : BasePlugin
{
    private const string Guid = "com.dev.survivallog.fitnessbar";
    private const string Version = "0.1.1";

    public override void Load()
    {
        var isEnabled = Config.Bind("FitnessBar", "Enabled", true, new ConfigDescription(
            "Show a thin Fitness bar under the Life bar on the screen, with a hover box that explains Fitness and what the value means. Off: the mod does nothing. Takes effect at the next game start.\n" +
            "在屏幕上的「生命」条下方显示一条细的健康条，鼠标悬停时说明健康是什么以及当前数值的含义。关闭后本模组不做任何改动。下次启动游戏时生效。", null, "ModManager.Restart")).Value;
        var log = new BarLog(Log, Path.Combine(Paths.BepInExRootPath, "fitness-bar.log"));

        if (!isEnabled)
        {
            log.Write($"===== Fitness Bar {Version} started: off, FitnessBar/Enabled is false");

            return;
        }

        log.Write($"===== Fitness Bar {Version} started");

        BarTicker.Log = log.Write;
        BarTicker.Page = new BarPage(log.Write);

        AddComponent<BarTicker>();
    }
}
