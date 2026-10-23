namespace FitnessBar.Core;

/// <summary>
/// The player's Fitness as the game would draw it on a bar: the value rounded to a whole number, and the maximum. In the
/// game's code this stat is Health (AttrName 4, its maximum AttrName 104); players see it as Fitness.
/// </summary>
public readonly record struct Fitness(int Value, int Max)
{
    /// <summary>A reading with a maximum of zero or less can not be drawn as a bar.</summary>
    public bool IsDrawable => Max > 0;
}
