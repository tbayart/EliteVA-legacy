<div text-align="center">
<img src="https://github.com/EliteAPI/Icons/blob/main/logo_gradient_shine.jpg?raw=true" align="right"
     title="EliteVA by Somfic" width="280" height="280">
<h1 align="center">EliteVA</h1>
     
<p align="center"><i>A VoiceAttack plugin for Elite: Dangerous, powered by <a href="https://www.github.com/EliteAPI/EliteAPI">EliteAPI</a></i></p>
     
<p align="center">
     <a href="https://www.discord.gg/jwpFUPZ">
          <img alt="Discord" src="https://img.shields.io/discord/498422961297031168?color=%23f2a529&label=DISCORD&style=for-the-badge">
     </a>
     <a href="https://github.com/EliteAPI/EliteVA/releases">
        <img alt="GitHub release" src="https://img.shields.io/github/v/release/EliteAPI/EliteVA?color=%23f2a529&label=VERSION&style=for-the-badge">
     </a>
     <a href="https://github.com/EliteAPI/EliteVA/blob/master/LICENSE">
         <img alt="GitHub" src="https://img.shields.io/github/license/EliteAPI/EliteVA?color=%23f2a529&label=LICENSE&style=for-the-badge">
     </a>
</p>
<p>EliteVA is a plugin for VoiceAttack that can full connect your macros to your Elite: Dangerous game. With events, variables and keybindings support. With EliteVA it is now possible to make a truly intelligent voice assistant.</p>
</div>

## Why EliteVA?
Let's consider the scenario of wanting to retracting our landing gear through a VoiceAttack macro.

### Without EliteVA
Tradionally, the macro would be quite simple:

```
Press the `G` key
```

However, this isn't a very smart way to go about it. 
What if the landing gear is already retracted, what if the commander is currently in supercruise? 
Or what if the landing gear key is not actually the `G` key?

There are a lot of scenarios where this macro would fail. EliteVA can help you turn your profiles into intelligent voice assistantances that actually know what is going on in-game.

### With EliteVA
When using EliteVA we can make the macro smarter in a lot of ways.

We can first check if the landing gear is not already retracted:

``` 
Boolean compare: EliteAPI.Gear equals True 
```

Then we can check if the commander is in normal non-supercruise space

```
Boolean compare: EliteAPI.Supercruise equals False
``` 

Finally, we can have our macro press the corresponding landing gear key

```
Press variable key: [EliteAPI.LandingGearToggle]
```

Implementing all these checks into your profile will certainly boost your profile's intelligence significantly.

## Getting started
Let's get you up and running, commander.

## Installation
EliteVA is distributed through GitHub; the recommended way to install this plugin. Alternatively, the plugin could also be compiled to retrieve the plugin file.

Download the [EliteVA zip](https://github.com/EliteAPI/EliteVA/releases) and extract it in a new folder called `EliteVA` in your `VoiceAttack\Apps` directory. 
Make sure **Plugin Support** is enabled in VoiceAttack. After restarting VoiceAttack the EliteVA plugin will be ready to go.

## Events
```
((EliteAPI.Ship.Gear))
```

EliteVA converts a ton of in-game events to macro commands. For example, retracting your gear will trigger the `((EliteAPI.Ship.Gear))` command, while cracking an ateriod will trigger the `((EliteAPI.AsteroidCracked))` event.

## Variables
```
{BOOl:EliteAPI.Gear}
```
A number of variables are made available through EliteVA, these variables are synced with the game. For example, `{BOOl:EliteAPI.Gear}` holds the the value of the ship's landing gear, and `{BOOL:MassLocked}` contains information on whether or not you're currently mass-locked.

A list of all active variables are written to the `ActiveVariables.txt` file in your installation directory.

## Bindings
```
Variable keypress: [EliteAPI.LandingGearToggle]
```

All in-game keybindings are made available through EliteVA and are updated whenever a change is made to the keybindings preset in-game. 
Instead of traditionally having VoiceAttack press `G` when you want to retract the landing gear, your macro can press the actual key assigned to the gear using the `{TXT:EliteAPI.LandingGearToggle}` variable.

A list of all supported keybindings can be found in the `ActiveVariables.txt` file in your installation directory.

**Disclaimer**: VoiceAttack does not support external Joystick or Hotas triggers, so the plugin will only expose keyboard keybindings, either primary or secondary. 
