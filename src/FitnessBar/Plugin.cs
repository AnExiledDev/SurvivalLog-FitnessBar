using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using ExiledToolkit;
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
    private const string Version = "0.1.6";
    private const string VitalSignsGuid = "com.dev.survivallog.vitalsigns";

    public override void Load()
    {
        var isEnabled = Config.Bind("FitnessBar", "Enabled", true, new ConfigDescription(
            "Shows a thin Fitness bar under the Life bar. Hover it to see what Fitness does and what your value means. Turn this off and the mod does nothing. While Vital Signs shows its Fitness bar, this mod stays off so only one bar shows.\n" +
            "在「生命」条下方显示一条细的健康条。鼠标悬停可以查看健康的作用和当前数值的含义。关闭后本模组不做任何改动。Vital Signs 显示它自己的健康条时，本模组保持关闭，只显示一条。"));
        var log = new ModLog(Log, "fitness-bar.log");
        var page = new BarPage(log.Write);
        var vitalSigns = VitalSignsSettings();

        log.Write($"===== Fitness Bar {Version} started");

        string? lastReason = "";

        LiveSettings.Watch(() =>
        {
            var isDrawing = Startup.IsVitalSignsDrawing(vitalSigns.IsInstalled, vitalSigns.On?.Value, vitalSigns.FitnessBar?.Value);
            var reason = Startup.OffReason(isEnabled.Value, isDrawing);

            if (reason != lastReason) log.Write(reason == null ? "drawing the Fitness bar" : "off: " + reason);

            lastReason = reason;
            page.IsOn = reason == null;
        }, vitalSigns.Watched(isEnabled));

        BarTicker.Log = log.Write;
        BarTicker.Page = page;

        AddComponent<BarTicker>();
    }

    private sealed record VitalSignsEntries(bool IsInstalled, ConfigEntry<bool>? On, ConfigEntry<bool>? FitnessBar)
    {
        public ConfigEntryBase[] Watched(ConfigEntryBase own) =>
            new[] { own, On, FitnessBar }.Where(entry => entry != null).Select(entry => entry!).ToArray();
    }

    /// <summary>Vital Signs loads first (see the soft dependency above), so its settings are bound by now.</summary>
    private static VitalSignsEntries VitalSignsSettings()
    {
        if (!IL2CPPChainloader.Instance.Plugins.TryGetValue(VitalSignsGuid, out var info)) return new VitalSignsEntries(false, null, null);

        var config = (info.Instance as BasePlugin)?.Config;

        return new VitalSignsEntries(true, Entry(config, "VitalSigns", "Enabled"), Entry(config, "Display", "FitnessBar"));
    }

    private static ConfigEntry<bool>? Entry(ConfigFile? config, string section, string key) =>
        config != null && config.TryGetEntry<bool>(section, key, out var entry) ? entry : null;
}
