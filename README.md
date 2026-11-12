# Fitness Bar

Shows your Fitness as a thin bar under the Life bar on the HUD. Hover it to see what Fitness does, your exact value and how close you are to being Weakened.

In Survival Log your character has a Fitness stat that the HUD never shows. High Fitness lowers your chance of getting sick, and below 30 you become Weakened. Staying Hungry or Fatigued for too long wears it down. Fitness Bar puts it on screen so you can see it drop before it hurts.

## Features

- **Fitness under the Life bar**: a thin bar that looks like the game's other bars, with the value on it.
- **Warning colors**: the bar is green, turns amber below 40 and turns red below 30, where Weakened starts.
- **Hover box**: it looks like the game's other tooltips and shows your Fitness and its maximum, what Fitness does, how far you are from Weakened, and what raises it.
- **Clicks work as before**: the bar only catches the mouse when you point right at it.
- **English or Chinese**: the bar and hover box are in English, or in Simplified Chinese when the game is set to Chinese.
- **Survives reloads**: the HUD can reload after a scene change. The mod notices within a couple of seconds and draws the bar again.
- **Log file**: the mod writes what it did at startup, and whether it found the Life bar, to `BepInEx\fitness-bar.log`.

Fitness Bar only reads your Fitness and draws it. It never changes anything in the game, adds nothing to your save and doesn't edit game files. You can remove it at any time.

## Known issues and limits

- The value is read every couple of seconds, so a change can take that long to show.
- Weakened at 30 is the number from the game's tip text. The mod doesn't read it from the game, so if an update moves it, the colors will be off until the mod is updated.
- With [Vital Signs](https://github.com/AnExiledDev/SurvivalLog-VitalSigns) installed, Fitness Bar stays off: Vital Signs draws its own Fitness bar in the same spot, and one bar is enough.

## Requirements

- Survival Log 1.1.20719. Built for this version. A game update can break it until the mod is updated.
- The [BepInEx Pack for Survival Log](https://www.nexusmods.com/survivallog/mods/12), the BepInEx 6 (IL2CPP) build for the game.
- [Exiled's Toolkit](https://github.com/AnExiledDev/SurvivalLog-ExiledToolkit) 0.20.0 or newer. Fitness Bar does not load without it.
- Optional: [Mod Manager](https://www.nexusmods.com/survivallog/mods/73). With it, you can change every setting in game on its Mods screen and see what each one does, without opening the config file.

## Install

1. Install the [BepInEx Pack for Survival Log](https://www.nexusmods.com/survivallog/mods/12) (if no other mods were installed before, start the game once so BepInEx finishes setup, then quit).
2. Install Exiled's Toolkit: download `ExiledToolkit-x.y.z.zip` from [its latest release](https://github.com/AnExiledDev/SurvivalLog-ExiledToolkit/releases/latest) or from [Nexus Mods](https://www.nexusmods.com/survivallog/mods/61), and extract it into the game folder (the folder with `SurvivalLog.exe`).
3. Download `FitnessBar-x.y.z.zip` from the [latest release](https://github.com/AnExiledDev/SurvivalLog-FitnessBar/releases/latest) or from [Nexus Mods](https://www.nexusmods.com/survivallog/mods/97).
4. Extract the zip into the game folder (the folder with `SurvivalLog.exe`). It holds a `BepInEx` folder, so the DLL lands in the right place. When asked, merge the folders. The result should be:
   `C:\Program Files (x86)\Steam\steamapps\common\Survival Log\BepInEx\plugins\FitnessBar.dll`

## Uninstall

Delete `FitnessBar.dll` from the `BepInEx\plugins` folder. For a clean slate, also delete `BepInEx\config\com.dev.survivallog.fitnessbar.cfg` and `BepInEx\fitness-bar.log`.

## Configuration

Settings live in `BepInEx\config\com.dev.survivallog.fitnessbar.cfg`, created the first time the game starts with the mod. Change them in game on the Mod Manager's Mods screen, or edit the file while the game is closed. Changes take effect at the next game start. Each setting in the cfg file is explained in English and Simplified Chinese. See [CONFIG.md](CONFIG.md) for every setting.

## Troubleshooting

Built for Survival Log 1.1.20719 with BepInEx `6.0.0-be.788`.

Check `BepInEx\LogOutput.log` for `===== Fitness Bar 0.1.5 started`. With `Enabled` set to `false` the line ends `off, FitnessBar/Enabled is false`. With Vital Signs installed it ends `off, Vital Signs is installed and draws the Fitness bar`.

Once a game has loaded, look for `page script sent to CoreUI1 (install 1): the Fitness strip is under the Life bar`. A line ending `no Life bar on the HUD right now` means the mod could not find the Life bar to put the Fitness bar under.

The same lines are in `BepInEx\fitness-bar.log`, with a timestamp each, and that file is kept across restarts.

## Feedback and bug reports

Found a bug, have an idea, or something feels off? Come say so on the Core Directive Discord: https://discord.gg/trTsyxUFqg . When reporting a bug, attach `BepInEx\LogOutput.log` and `BepInEx\fitness-bar.log`.

To reach me directly, message `exiled.dev` on Discord. You can find me in the [official Survival Log Discord](https://discord.gg/MZGg4gQAUe). I'm also on X as [@AnExiledDev](https://x.com/AnExiledDev).

## Changelog

Every version is in [CHANGELOG.md](CHANGELOG.md).

## Build

This is a BepInEx 6 IL2CPP plugin. It compiles against BepInEx, the game's IL2CPP interop assemblies and Exiled's Toolkit, so a game install with BepInEx set up and started once is required. Those assemblies are not part of this repo. The .NET 8 SDK is required.

```
dotnet build src/FitnessBar -c Release -p:GameDir="C:\path\to\Survival Log" -p:ToolkitDll="C:\path\to\ExiledToolkit.dll"
```

`GameDir` is the game folder and `ToolkitDll` is a built `ExiledToolkit.dll` (0.20.0 or newer). The defaults in the project file are local paths you will almost certainly need to override. The output DLL is at `src\FitnessBar\bin\Release\FitnessBar.dll`.

The unit tests cover the pure logic, including the page script, which they run against a small stand-in for the HUD page. They need nothing from the game, but they compile one file from the Exiled's Toolkit source, which they find from `ToolkitDll` (its DLL sits under `bin\Release` in that source):

```
dotnet test tests/FitnessBar.Tests -p:ToolkitDll="C:\path\to\ExiledToolkit.dll"
```

## License

MIT. Copyright (c) 2026 AnExiledDev. See [LICENSE](LICENSE).
