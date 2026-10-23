using FitnessBar.Core;
using Xunit;

namespace FitnessBar.Tests;

public class PageScriptTests
{
    private const string LifeRow = "page.rows[3]";

    private static string Reading(int value, int max = 100, bool isChinese = false) => PageCalls.Reading(new Fitness(value, max), isChinese);

    [Fact]
    public void The_strip_sits_on_the_Life_row_and_shows_the_value()
    {
        var page = PageScript.On();

        Assert.Equal(PageAnswer.Ok, page.Apply(Reading(72)));
        Assert.True(page.Is($"stripOn({LifeRow})"));
        Assert.Equal(1, page.Number("document.querySelectorAll('.slfb').length"));
        Assert.Equal("72", page.Text($"stripOn({LifeRow}).querySelector('.slfb-value').textContent"));
        Assert.Equal("72/100", page.Text($"stripOn({LifeRow}).querySelector('.st-value').textContent"));
        Assert.Equal("Fitness", page.Text($"stripOn({LifeRow}).querySelector('.st-title').textContent"));
    }

    /// <summary>The game marks the Life bar with a heart in attrList; the row is found by that mark, not by its place.</summary>
    [Fact]
    public void The_Life_row_is_found_by_its_heart_wherever_it_is()
    {
        var page = PageScript.On("{icons:['\\u2764','\\uD83C\\uDF56','\\uD83E\\uDDE0','\\u26A1']}");

        page.Apply(Reading(72));

        Assert.True(page.Is("stripOn(page.rows[0])"));
        Assert.False(page.Is("stripOn(page.rows[3])"));
    }

    [Fact]
    public void Without_the_Vue_state_the_fourth_row_is_taken_as_Life()
    {
        var page = PageScript.On("{hasNoVue:true}");

        Assert.Equal(PageAnswer.Ok, page.Apply(Reading(72)));
        Assert.True(page.Is($"stripOn({LifeRow})"));
    }

    [Theory]
    [InlineData("{icons:[]}")]
    [InlineData("{icons:['a','b','c']}")]
    public void With_no_Life_bar_on_the_page_nothing_is_drawn(string spec)
    {
        var page = PageScript.On(spec);

        Assert.Equal(PageAnswer.NoBar, page.Apply(Reading(72)));
        Assert.Equal(0, page.Number("document.querySelectorAll('.slfb').length"));
    }

    [Fact]
    public void With_no_player_the_strip_is_taken_away()
    {
        var page = PageScript.On();

        page.Apply(Reading(72));

        Assert.Equal(PageAnswer.NoPlayer, page.Apply(PageCalls.Reading(null, false)));
        Assert.False(page.Is($"stripOn({LifeRow})"));
        Assert.False(page.Is($"{LifeRow}.classList.contains('slfb-host')"));
    }

    /// <summary>The game's line is "below 30 you become Weakened"; amber warns from 39 down, red from 29 down.</summary>
    [Theory]
    [InlineData(100, "slfb-ok")]
    [InlineData(40, "slfb-ok")]
    [InlineData(39, "slfb-low")]
    [InlineData(30, "slfb-low")]
    [InlineData(29, "slfb-weak")]
    [InlineData(0, "slfb-weak")]
    public void The_strip_colour_follows_the_Weakened_line(int value, string level)
    {
        var page = PageScript.On();

        page.Apply(Reading(value));

        foreach (var each in new[] { "slfb-ok", "slfb-low", "slfb-weak" })
        {
            Assert.Equal(each == level, page.Is($"stripOn({LifeRow}).classList.contains('{each}')"));
        }
    }

    [Theory]
    [InlineData(72, "42 above 30, where Weakened starts.")]
    [InlineData(40, "10 above 30, where Weakened starts.")]
    [InlineData(35, "Close to Weakened: 6 more lost and it drops below 30.")]
    [InlineData(30, "Close to Weakened: 1 more lost and it drops below 30.")]
    [InlineData(29, "Below 30, so you are Weakened.")]
    public void The_hover_box_says_what_the_value_means(int value, string line)
    {
        var page = PageScript.On();

        page.Apply(Reading(value));

        Assert.Equal(line, page.Text($"stripOn({LifeRow}).querySelector('.slfb-state').textContent"));
    }

    [Fact]
    public void The_hover_box_explains_Fitness_in_the_game_s_own_terms()
    {
        var page = PageScript.On();

        page.Apply(Reading(72));

        var about = page.Text($"stripOn({LifeRow}).querySelector('.slfb-about').textContent");

        Assert.Contains("sick", about);
        Assert.Contains("Below 30 you become Weakened", about);
        Assert.Contains("Hungry or Fatigued", about);
        Assert.Equal("Food that gives Fitness raises it.", page.Text($"stripOn({LifeRow}).querySelector('.slfb-raise').textContent"));
    }

    [Fact]
    public void A_Chinese_game_gets_the_Chinese_texts()
    {
        var page = PageScript.On();

        page.Apply(Reading(35, isChinese: true));

        Assert.Equal("健康", page.Text($"stripOn({LifeRow}).querySelector('.st-title').textContent"));
        Assert.Equal("接近「亚健康」：再降低 6 点就会低于30。", page.Text($"stripOn({LifeRow}).querySelector('.slfb-state').textContent"));
    }

    [Theory]
    [InlineData(50, 100, "scaleX(0.5)")]
    [InlineData(100, 100, "scaleX(1)")]
    [InlineData(130, 100, "scaleX(1)")]
    [InlineData(0, 100, "scaleX(0)")]
    public void The_fill_is_the_share_of_the_maximum(int value, int max, string transform)
    {
        var page = PageScript.On();

        page.Apply(Reading(value, max));

        Assert.Equal(transform, page.Text($"stripOn({LifeRow}).querySelector('.slfb-fill').style.transform"));
    }

    /// <summary>The plugin calls every two seconds; the strip is made once and only its text changes after that.</summary>
    [Fact]
    public void Each_call_updates_the_one_strip()
    {
        var page = PageScript.On();

        page.Apply(Reading(72));
        page.Run("var first = stripOn(page.rows[3]);");
        page.Apply(Reading(71));
        page.Reinstall();
        page.Apply(Reading(70));

        Assert.True(page.Is($"stripOn({LifeRow}) === first"));
        Assert.Equal(1, page.Number("document.querySelectorAll('.slfb').length"));
        Assert.Equal("70", page.Text("first.querySelector('.slfb-value').textContent"));
        Assert.Equal(1, page.Number("document.head.children.length"));
        Assert.Equal(1, page.Number("core.clickThrough.syncs"));
    }

    /// <summary>The page hands the game only the areas marked this way; unmarked, hovering the strip would click the world behind it.</summary>
    [Fact]
    public void The_strip_takes_the_pointer_from_the_world_behind_it()
    {
        var page = PageScript.On();

        page.Apply(Reading(72));

        Assert.True(page.Is($"stripOn({LifeRow}).hasAttribute('data-interactive')"));
        Assert.True(page.Is($"{LifeRow}.classList.contains('slfb-host')"));
    }

    [Fact]
    public void A_strip_left_on_a_row_that_is_no_longer_Life_goes()
    {
        var page = PageScript.On();

        page.Apply(Reading(72));
        page.Run("page.attrList[3].attrIcon = 'x'; page.attrList[0].attrIcon = '\\u2764';");
        page.Apply(Reading(72));

        Assert.True(page.Is("stripOn(page.rows[0])"));
        Assert.False(page.Is($"stripOn({LifeRow})"));
        Assert.False(page.Is($"{LifeRow}.classList.contains('slfb-host')"));
    }
}
