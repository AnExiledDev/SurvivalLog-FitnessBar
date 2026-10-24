# Fitness Bar

Shows your Fitness on the HUD as a thin bar under the Life bar, with a hover box that says what Fitness does, what it is now and how close you are to Weakened.

In Survival Log your character has a Fitness stat that the HUD never shows. High Fitness lowers your chance of getting sick, and below 30 you become Weakened. Staying Hungry or Fatigued for too long wears it down. Fitness Bar puts it on screen so you can see it drop before it hurts.

## Features

- **Fitness under the Life bar**: a thin bar in the game's own bar style, with the value on it.
- **Warning colors**: green, amber below 40, red below 30, where Weakened starts.
- **Hover box**: in the game's own tooltip style. It shows Fitness as value and maximum, what Fitness does, how far you are from Weakened, and what raises it.
- **Clicks work as before**: the bar takes the pointer only over itself, like the game's own bars.
- **Any language**: the bar and hover box are in English, or in Simplified Chinese when the game is set to Chinese.
- **Survives reloads**: the HUD can reload after a scene change; the mod notices within a couple of seconds and draws the bar again.
- **Log file**: the mod writes what it did at startup and what the HUD answered to `BepInEx\fitness-bar.log`.

Fitness Bar only reads your Fitness and draws it. It never writes the game's state, adds nothing to your save and does not edit game files. You can remove it at any time.

## Known issues and limits

- The value is read every couple of seconds, so a change can take that long to show.
- The Weakened line at 30 is the one the game's own tip text gives.

## Requirements

- Survival Log 1.1.19611. Built and tested on this version; a game update can break it until the mod is updated.
- The [BepInEx Pack for Survival Log](https://www.nexusmods.com/survivallog/mods/12), the BepInEx 6 (IL2CPP) build for the game.

## Install

1. Install the [BepInEx Pack for Survival Log](https://www.nexusmods.com/survivallog/mods/12) (if no other mods were installed before, start the game once so BepInEx finishes setup, then quit).
2. Download `FitnessBar-x.y.z.zip` from the [latest release](https://github.com/AnExiledDev/SurvivalLog-FitnessBar/releases/latest) or from [Nexus Mods](https://www.nexusmods.com/survivallog/mods/97).
3. Extract the zip into the game folder (the folder with `SurvivalLog.exe`). It holds a `BepInEx` folder, so the DLL lands in the right place. When asked, merge the folders. The result should be:
   `C:\Program Files (x86)\Steam\steamapps\common\Survival Log\BepInEx\plugins\FitnessBar.dll`

## Uninstall

Delete `FitnessBar.dll` from the `BepInEx\plugins` folder. For a clean slate, also delete `BepInEx\config\com.dev.survivallog.fitnessbar.cfg` and `BepInEx\fitness-bar.log`.

## Configuration

Settings live in `BepInEx\config\com.dev.survivallog.fitnessbar.cfg`, created the first time the game starts with the mod. Change them in game on the Mod Manager's Mods screen, or edit the file while the game is closed. Changes take effect at the next game start. Each setting in the cfg file is explained in English and Simplified Chinese. See [CONFIG.md](CONFIG.md) for every setting.

## Troubleshooting

Tested on Survival Log 1.1.19611 with BepInEx `6.0.0-be.788`.

Check `BepInEx\LogOutput.log` for `===== Fitness Bar 0.1.1 started`. With `Enabled` set to `false` the line ends `off, FitnessBar/Enabled is false`.

Once a game has loaded, look for `page script sent to CoreUI1 (install 1): the Fitness strip is under the Life bar`. A line ending `no Life bar on the HUD right now` means the mod could not find the Life bar to put the Fitness bar under.

The same lines are in `BepInEx\fitness-bar.log`, with a timestamp each, and that file is kept across restarts.

## Feedback and bug reports

Found a bug, have an idea, or something feels off? Come say so on the Core Directive Discord: https://discord.gg/trTsyxUFqg . When reporting a bug, attach `BepInEx\LogOutput.log` and `BepInEx\fitness-bar.log`.

To reach me directly, message `exiled.dev` on Discord; you can find me in the [official Survival Log Discord](https://discord.gg/MZGg4gQAUe). I'm also on X as [@AnExiledDev](https://x.com/AnExiledDev).

## Changelog

- **0.1.1**: The hover box now shows with Stats UI installed.
- **0.1.0**: First version.

## Build

This is a BepInEx 6 IL2CPP plugin. It compiles against BepInEx and the game's IL2CPP interop assemblies, so a game install with BepInEx set up and started once is required. Those assemblies come from the game and are not part of this repo. The .NET 8 SDK is required.

```
dotnet build src/FitnessBar -c Release -p:GameDir="C:\path\to\Survival Log"
```

`GameDir` is the game folder. The default in the project file is a local path you will almost certainly need to override. The output DLL is at `src\FitnessBar\bin\Release\FitnessBar.dll`.

The unit tests cover the pure logic, including the page script, which they run against a small stand-in for the HUD page. They need nothing from the game:

```
dotnet test tests/FitnessBar.Tests
```

## License

MIT. Copyright (c) 2026 AnExiledDev. See [LICENSE](LICENSE).
