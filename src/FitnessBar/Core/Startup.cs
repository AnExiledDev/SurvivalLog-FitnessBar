namespace FitnessBar.Core;

/// <summary>Whether the bar is drawn this run, and the reason when it is not.</summary>
public static class Startup
{
    /// <summary>
    /// Vital Signs draws its own Fitness bar under the Life bar, so with it installed this mod stays off: two bars in the
    /// same spot read as a bug. Null means the bar is drawn.
    /// </summary>
    public static string? OffReason(bool isEnabled, bool hasVitalSigns)
    {
        if (hasVitalSigns) return "Vital Signs is installed and draws the Fitness bar";
        if (!isEnabled) return "FitnessBar/Enabled is false";

        return null;
    }
}
