using FitnessBar.Core;
using Xunit;

namespace FitnessBar.Tests;

public class StartupTests
{
    [Fact]
    public void The_bar_is_drawn_when_enabled_and_Vital_Signs_is_not_installed()
    {
        Assert.Null(Startup.OffReason(isEnabled: true, hasVitalSigns: false));
    }

    /// <summary>Vital Signs draws the same bar in the same spot, so it wins and only one bar shows.</summary>
    [Fact]
    public void Vital_Signs_installed_turns_the_bar_off_even_when_enabled()
    {
        Assert.Equal("Vital Signs is installed and draws the Fitness bar", Startup.OffReason(isEnabled: true, hasVitalSigns: true));
    }

    [Fact]
    public void The_setting_turned_off_turns_the_bar_off()
    {
        Assert.Equal("FitnessBar/Enabled is false", Startup.OffReason(isEnabled: false, hasVitalSigns: false));
    }
}
