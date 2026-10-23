using System.Text.Json;
using FitnessBar.Core;
using Xunit;

namespace FitnessBar.Tests;

public class PageCallsTests
{
    /// <summary>A JSON string literal is a JavaScript one, so a text that survives a JSON read survives the page.</summary>
    [Theory]
    [InlineData("var a = 1;")]
    [InlineData("el.setAttribute(\"data-interactive\", '');")]
    [InlineData("var re = /CoreUI1\\.html/i;")]
    [InlineData("two\nlines\r\tand a tab")]
    [InlineData("ends with a backslash\\")]
    [InlineData("line\u2028separator\u2029")]
    public void The_page_script_reaches_the_page_exactly_as_written(string script)
    {
        var literal = PageCalls.Quote(script);

        Assert.Equal(script, JsonSerializer.Deserialize<string>(literal));
        Assert.DoesNotContain('\n', literal);
        Assert.DoesNotContain('\u2028', literal);
        Assert.DoesNotContain('\u2029', literal);
    }

    [Fact]
    public void The_install_call_carries_the_whole_script()
    {
        const string script = "(function(){window.__slfitness={apply:function(){return \"ok\";}};})();\n// last line";

        Assert.Contains(PageCalls.Quote(script), PageCalls.Install(script, new Fitness(72, 100), false));
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
