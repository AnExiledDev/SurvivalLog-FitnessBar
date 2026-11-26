namespace FitnessBar.Core;

/// <summary>Whether the bar is drawn right now, and the reason when it is not.</summary>
public static class Startup
{
    /// <summary>
    /// Vital Signs draws its own Fitness bar under the Life bar, so while it does, this mod stays off: two bars in the
    /// same spot read as a bug. Null means the bar is drawn.
    /// </summary>
    public static string? OffReason(bool isEnabled, bool isVitalSignsDrawing)
    {
        if (isVitalSignsDrawing) return "Vital Signs draws the Fitness bar";
        if (!isEnabled) return "FitnessBar/Enabled is false";

        return null;
    }

    /// <summary>
    /// Vital Signs draws its Fitness bar only while it is on and its Display/FitnessBar setting is on. A setting this mod
    /// could not find (null) is taken as on, so an older Vital Signs still wins and only one bar shows.
    /// </summary>
    public static bool IsVitalSignsDrawing(bool isInstalled, bool? isOn, bool? isFitnessBarOn) =>
        isInstalled && (isOn ?? true) && (isFitnessBarOn ?? true);
}
