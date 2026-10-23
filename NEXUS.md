Name: Fitness Bar
Summary: Shows your hidden Fitness stat on the HUD as a thin bar under the Life bar, turning amber below 40 and red below 30, where you become Weakened. Hover it for a box that says what Fitness does, its exact value, how far you are from Weakened and what raises it. Reads only, in the game's own bar style, English or Chinese.

Description (BBCode):
Your character has a Fitness stat the HUD never shows. High Fitness lowers your chance of getting sick, and below 30 you become Weakened. Staying Hungry or Fatigued for too long wears it down. Fitness Bar puts it on screen, right under the Life bar, so you can see it drop before it hurts.

[size=5]Main features[/size]
[list]
[*][b]Fitness under the Life bar[/b]: a thin bar in the game's own bar style, with the value on it.[/*]
[*][b]Warning colors[/b]: green, amber below 40, red below 30, where Weakened starts.[/*]
[*][b]Hover box[/b]: shows Fitness as value and maximum, what Fitness does, how far you are from Weakened, and what raises it.[/*]
[*][b]Clicks work as before[/b]: the bar takes the pointer only over itself.[/*]
[*][b]English or Chinese[/b]: follows the game's language.[/*]
[/list]
Fitness Bar only reads your Fitness and draws it. It never changes the game's data, adds nothing to your save and does not edit game files. You can remove it at any time.

[size=5]Known issues[/size]
The value is read every couple of seconds, so a change can take that long to show.

[size=5]Requirements[/size]
[list]
[*]Survival Log 1.1.19131. A game update can break it until the mod is updated.[/*]
[*][url=https://www.nexusmods.com/survivallog/mods/12]BepInEx for Survival Log[/url]. Install it first.[/*]
[/list]

[size=5]Installation[/size]
[list=1]
[*]Install [url=https://www.nexusmods.com/survivallog/mods/12]BepInEx[/url] [i](if no other mods were installed before, start the game once so BepInEx finishes setup, then quit)[/i].[/*]
[*]Download the zip from the Files tab.[/*]
[*]Extract it into the game folder (the folder with [font=Courier New]SurvivalLog.exe[/font]). The zip holds a BepInEx folder, so the DLL lands in the right place. When asked, merge the folders. The result should be:
[code]C:\Program Files (x86)\Steam\steamapps\common\Survival Log\BepInEx\plugins\FitnessBar.dll[/code][/*]
[/list]

[size=5]Uninstall[/size]
Delete [font=Courier New]FitnessBar.dll[/font] from the [font=Courier New]BepInEx\plugins[/font] folder.

[size=5]Configuration[/size]
One setting, [font=Courier New]Enabled[/font], in [font=Courier New]BepInEx\config\com.dev.survivallog.fitnessbar.cfg[/font] or on the Mod Manager's Mods screen. It takes effect at the next game start.

[size=5]Feedback[/size]
Found a bug or have an idea? Come say hi on the [url=https://discord.gg/trTsyxUFqg]Core Directive Discord[/url], or post in this page's Posts tab. For bugs, please include [font=Courier New]BepInEx\LogOutput.log[/font] and [font=Courier New]BepInEx\fitness-bar.log[/font].
To reach me directly, message [b]exiled.dev[/b] on Discord; you can find me in the [url=https://discord.gg/MZGg4gQAUe]official Survival Log Discord[/url]. I'm also on X as [url=https://x.com/AnExiledDev]@AnExiledDev[/url].
Check out my [url=https://sldb.core-directive.com/en/]Survival Log wiki[/url] for items, crafting, maps and more.

[size=5]Source and license[/size]
Open source under MIT: [url=https://github.com/AnExiledDev/SurvivalLog-FitnessBar]github.com/AnExiledDev/SurvivalLog-FitnessBar[/url]

[line]
[size=5]简体中文[/size]

你的角色有一项 HUD 从不显示的「健康」属性。良好的健康可以降低生病的概率，低于30会陷入「亚健康」。长期「饥饿」或「疲劳」会降低健康。Fitness Bar 把它显示在生命条正下方，让你在它造成影响之前就看到它在下降。

[size=5]主要功能[/size]
[list]
[*][b]生命条下的健康条[/b]：采用游戏自带样式的细条，上面显示数值。[/*]
[*][b]警示颜色[/b]：正常为绿色，低于40变成琥珀色，低于30（「亚健康」）变成红色。[/*]
[*][b]悬停提示[/b]：显示健康的当前值和上限、健康的作用、离「亚健康」还有多远，以及如何提高它。[/*]
[*][b]点击照常[/b]：只有鼠标在健康条上时才会被它接收。[/*]
[*][b]中英文[/b]：跟随游戏语言。[/*]
[/list]
Fitness Bar 只读取并显示你的健康，从不改动游戏数据，不会往存档里写入任何内容，也不会修改游戏文件。你可以随时移除它。

[size=5]已知问题[/size]
数值每隔几秒读取一次，所以变化可能要等这么久才会显示。

[size=5]前置要求[/size]
[list]
[*]Survival Log 1.1.19131。游戏更新后模组可能暂时失效，直到模组更新为止。[/*]
[*][url=https://www.nexusmods.com/survivallog/mods/12]BepInEx for Survival Log[/url]，请先安装。[/*]
[/list]

[size=5]安装[/size]
[list=1]
[*]安装 [url=https://www.nexusmods.com/survivallog/mods/12]BepInEx[/url] [i]（如果之前没有装过其他模组，先启动一次游戏，让 BepInEx 完成初始化，然后退出）[/i]。[/*]
[*]在「文件」页下载压缩包。[/*]
[*]把压缩包解压到游戏目录（[font=Courier New]SurvivalLog.exe[/font] 所在的文件夹）。压缩包里带有 BepInEx 文件夹，DLL 会自动放到正确位置。如果提示合并文件夹，选择合并。解压后应为：
[code]C:\Program Files (x86)\Steam\steamapps\common\Survival Log\BepInEx\plugins\FitnessBar.dll[/code][/*]
[/list]

[size=5]卸载[/size]
从 [font=Courier New]BepInEx\plugins[/font] 文件夹中删除 [font=Courier New]FitnessBar.dll[/font]。

[size=5]配置[/size]
只有一项设置 [font=Courier New]Enabled[/font]，位于 [font=Courier New]BepInEx\config\com.dev.survivallog.fitnessbar.cfg[/font]，也可以在 Mod Manager 的「模组」界面修改，在下次启动游戏时生效。

[size=5]反馈[/size]
发现 bug 或有好点子？欢迎来 [url=https://discord.gg/trTsyxUFqg]Core Directive Discord[/url] 打个招呼，或在本页的「Posts」标签里发帖。报告 bug 时，请附上 [font=Courier New]BepInEx\LogOutput.log[/font] 和 [font=Courier New]BepInEx\fitness-bar.log[/font]。
想直接联系我，可以在 Discord 上给 [b]exiled.dev[/b] 发消息，我在 [url=https://discord.gg/MZGg4gQAUe]Survival Log 官方 Discord[/url] 里。也可以在 X 上找我：[url=https://x.com/AnExiledDev]@AnExiledDev[/url]。
也欢迎看看我做的 [url=https://sldb.core-directive.com/zh/]Survival Log 维基[/url]，收录物品、制作、地图等资料。

[size=5]源代码与许可[/size]
基于 MIT 许可开源：[url=https://github.com/AnExiledDev/SurvivalLog-FitnessBar]github.com/AnExiledDev/SurvivalLog-FitnessBar[/url]
