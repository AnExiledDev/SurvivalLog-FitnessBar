using ExiledToolkit.Web;
using FitnessBar.Core;
using Xunit;

namespace FitnessBar.Tests;

public class PageCallsTests
{
    [Fact]
    public void The_install_call_carries_the_whole_script()
    {
        const string script = "(function(){window.__slfitness={apply:function(){return \"ok\";}};})();\n// last line";

        Assert.Contains(JsText.Literal(script), PageCalls.Install(script, new Fitness(72, 100), false));
    }

    [Fact]
    public void Every_call_hands_the_page_the_reading()
    {
        var fitness = new Fitness(72, 100);
        const string expected = "{value:72,max:100,zh:true}";

        Assert.Equal(expected, PageCalls.Reading(fitness, true));
        Assert.Contains(expected, PageCalls.Check(fitness, true));
        Assert.Contains(expected, PageCalls.Install("", fitness, true));
    }

    /// <summary>No player, or a maximum the bar can not be drawn against: the page is told there is nothing to show.</summary>
    [Fact]
    public void No_reading_reaches_the_page_as_nothing_to_show()
    {
        Assert.Equal("{value:-1,max:0,zh:false}", PageCalls.Reading(null, false));
        Assert.Equal("{value:-1,max:0,zh:false}", PageCalls.Reading(new Fitness(50, 0), false));
    }
}
