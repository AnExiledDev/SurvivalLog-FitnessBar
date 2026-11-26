using FitnessBar.Core;
using Xunit;

namespace FitnessBar.Tests;

public class StartupTests
{
    [Fact]
    public void The_bar_is_drawn_when_enabled_and_Vital_Signs_is_not_drawing()
    {
        Assert.Null(Startup.OffReason(isEnabled: true, isVitalSignsDrawing: false));
    }

    /// <summary>Vital Signs draws the same bar in the same spot, so it wins and only one bar shows.</summary>
    [Fact]
    public void Vital_Signs_drawing_turns_the_bar_off_even_when_enabled()
    {
        Assert.Equal("Vital Signs draws the Fitness bar", Startup.OffReason(isEnabled: true, isVitalSignsDrawing: true));
    }

    [Fact]
    public void The_setting_turned_off_turns_the_bar_off()
    {
        Assert.Equal("FitnessBar/Enabled is false", Startup.OffReason(isEnabled: false, isVitalSignsDrawing: false));
    }

    [Fact]
    public void Vital_Signs_draws_its_bar_only_while_it_and_its_Fitness_bar_are_on()
    {
        Assert.True(Startup.IsVitalSignsDrawing(isInstalled: true, isOn: true, isFitnessBarOn: true));
        Assert.False(Startup.IsVitalSignsDrawing(isInstalled: false, isOn: true, isFitnessBarOn: true));
    }

    /// <summary>A player who turned off Vital Signs' Fitness bar, or Vital Signs itself, would otherwise have no Fitness bar at all.</summary>
    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void With_Vital_Signs_or_its_Fitness_bar_off_this_bar_is_drawn(bool isOn, bool isFitnessBarOn)
    {
        var isDrawing = Startup.IsVitalSignsDrawing(isInstalled: true, isOn, isFitnessBarOn);

        Assert.Null(Startup.OffReason(isEnabled: true, isDrawing));
    }

    /// <summary>An older Vital Signs without these settings still draws its bar, so it still wins.</summary>
    [Fact]
    public void Settings_that_cannot_be_found_count_as_on()
    {
        Assert.True(Startup.IsVitalSignsDrawing(isInstalled: true, isOn: null, isFitnessBarOn: null));
    }
}
