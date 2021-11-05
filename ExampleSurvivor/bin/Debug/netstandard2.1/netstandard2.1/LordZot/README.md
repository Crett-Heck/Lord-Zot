# Lord Zot
Lord Zot is a mod for Risk Of Rain 2.


## Discord
https://discord.gg/Mbs7Y4Sm88 (I hope this link doesn't randomly expire despite me telling it not to)

## Lord Zot

He is an original character designed as a vessel for me to implement fun ideas. He is largely composed of references to popular villains, particularly from video games.

His implementation in Risk Of Rain 2 is designed as a sort of role reversal. You are playing as a character who, in any other context, would be the antagonist.

You've all heard of DnD characters performing rituals or collecting magical artifacts to achieve ascension, in a maniacal pursuit of power. It then takes a whole party to defeat them.

That is what you will be playing as.
Zot is extremely WIP currently, and everything is subject to change. I am a junior programmer using this project as an opportunity to learn C#, and am motivated solely by the desire to craft a specific power fantasy.

Make sure you're prepared for the instability Zot can bring to this plane, and avert your eyes from the debug console.
He has no skin index currently, so he's rather incompatible with some mods. I'm working on this issue

![In game popup](https://cdn.discordapp.com/attachments/796789266973458452/796798696531689542/image0.png)

## About
Zot has a base MS of 3/MS. His sprint has no effect, and his jump is unwieldy, yet powerful, able to be charged for geater effect.

he pimp walks at an insultingly leisurely pace unless Titan's Stride is used, which causes him to either blink forward in a burst of speed; additionally, his jumps travel a great distance at high velocity based on gem power infused.

for Zot, movement is calculated and deliberate. he must choose when to move, but when he does, he really moves

which should compliment his overwhelming power. building power, conserving energy, managing and growing resources. most of the time he survives through sheer attrition, and when the time comes,

ending fights immediately in a burst of bibilically earthshattering motion. if he miscalculates, he can end up in a dangerous position for a long time, but to make up for it he is far and away the most durable survivor with a health pool that rivals bosses.



![description2](https://cdn.discordapp.com/attachments/796798631407517707/796799820109971456/image0.png)

Lord Zot

base stats
Health: 400
Health Regen: Zot regenerates a small percentage of his missing health every second.
Damage: 40
Speed: 3 m/s
Armor: 100 (+ 10 per level, again just a temporary thing)
Gem Power Per Level: +1 (temporary like everything else)


Passive 1 - unimplemented: Zot does not gain experience from defeating enemies. Instead, he gains power with each Gem obtained. 
He has a "Gem Power" resource, with a max quantity based on how many gems have been collected. Any of his skills can be held down to infuse it with gem power, granting it an additional effect based on how much power is used.
^
The gem system is not implemented yet, but prototype Gem Power is here!


Passive 2 - unimplemented: Special destructible nodes in the environment appear to Zot. These nodes are color-coded to the type of gem fragments they contain. Collected fragments heal Zot for 5% health. After collecting enough fragments of a certain type, Zot gains a gem that he sockets into his armor. Each type of gem grants different bonuses depending on where it is slotted.
Enemies rarely drop a gem fragment.



Passive 3: The body of Lord Zot is of such immense weight that his descent in the air is nearly impossible to stop. However, he cannot be moved or knocked back by attacks, his footsteps cause 40% damage and knockback to nearby enemies. When falling from heights, Zot takes no damage, and causes powerful quakes that deal 200-1800% damage based on velocity.

Passive 4 - Placeholder: Inevitable - Every 15 seconds, Zot gains 1 maximum gem power and his base health is multiplied by 1.05x (thus the bonus increases each time).

LMB - Eldritch Fury: Zot throws a punch with one of his shielded gauntlets, alternating arms with each swing. Deals 450% damage. His Red and Blue shields have different effects based on slotted gems. 
(Unimplemented)If charged for 1 second, Zot consumes all Gem Power and swings multiple times over the course of 1.5 seconds, swinging more times over the duration based on Gem Power infused.

RMB - Gem Bulwark: Zot swings with a backhand, deflecting projectiles and blocking frontal attacks for an instant. Nearby enemies are stunned.
If held, Zot then begins channeling 15% of his maximum Gem Power into his palm every second. Upon release, Zot fires a laser very similar to the ones fired by Golems, with a magnitude based on how much raw Gem Power was infused. 100% damage per 1 Gem Power.

Shift -  Titan's Stride: Zot blinks forward in an immense burst of speed.
(Can "pathfind", allowing it to cross gaps (even to a higher elevation!), and pass through thin walls. This has the unfortunate side effect of teleporting you to the nearest pathfind node,
meaning it teleports you out of secret areas, and plain doesn't work in the bazaar. Using it in the air circumvents this issue, simply moving in the direction of your aim.)

Costs 2 Gem Power.


-If the V key is pressed while in the air, Zot begins to float, consuming 1 Gem Power per second. He can attack freely while floating, and can fly around, albeit not that fast.

R - Eldritch Slam: Zot holds a fist in the air, and then hurls an immense blow with high knockback, dealing 900% damage. (If aimed towards the ground, half damage is dealt in an AoE, and enemies are knocked upwards.)
This ability can be charged, consuming 10% of Zot's Gem Power per second for a larger effect.
(Releasing this ability in the air sends Zot hurtling towards the ground. It can be charged without interrupting floating.)

All of the above is very subject to change, and Zot is very, very unfinished.


![description](https://cdn.discordapp.com/attachments/796794478736310333/796799455524683867/image0.png)

This isn't implemented yet, but I'll talk about it here.

Zot is fused with a sort of magic "gem engine" he uses, which will become quite bedazzled with enchanted minerals. there's no cap to how powerful and plentious your gem collection can become.

gems are not consumed with use but each one contributes to your maximum "gem power" resource and regeneration of said resource, which can be expended all at once on any ability.

Any part of his body that is golden is a potential placement point for gems.

The Gem System is currently not implemented.


## Credits

Geartor, GameBoy, and PentaKayle - Placeholder Icons

Geartor - ![punchi](https://i.imgur.com/OzvoxYJ.png) ![passive](https://i.imgur.com/MqfMvxU.png)


GameBoy - ![punch](https://i.imgur.com/G4vBmDs.png)

PentaKayle - ![stride](https://i.imgur.com/2huKfAs.png) 

Rob - I've referenced ExampleSurvivor and many of his mods, and it's probably the only reason I've gotten this far.

KomradeSpectre - Received lots of nice advice on Discord from this person!

Kinggrinyov - Insp.




## Known Bugs
- Zot falling through the ground, especially when using grounded utility
- Umbra Zot is a mess
- Animations occasionally bug out
- Everything about gem power
- Zot breaks UI for non-host players! He also used to mess up ability cooldowns! Everyone is just ever so thankful that I waited until now to include this in the known bugs list, 
after they've had to painstakingly scour their mod list only to find my disaster mod is the culprit! <--- UI ISSUES FIXED I THINK
- equipment wonkiness
- multiple charges of secondary causes wonkiness (might be fixed, haven't checked!)



## Changelogs

1.2.7
- Reopened the portal between worlds, allowing Zot's return.
- Gem Power scales over time similarly to Zot's maximum health (boost increases with game time, leading to very high values in the late-game) Very temporary adjustment
- Various stat adjustments, as well as animation tweaks. Zot will be harder to play in some ways, easier in others.
- Zot can walk up small obstacles.
- Jump physics made even worse. We'll see how long it takes for me to fix this
- "I miss the patches before Jot added all this weird stuff that makes everything clunkier" Indeed, and I do hope Lord Zot misses you after I tell him you said that

1.2.6
- Updated dependencies, needs ET 0.1.4.
- Enigma is a legend


1.2.5
- Superjump toggle is now LCTRL+L
- Cheat button is now LCTRL+U
- Flight is still V like normal. Just hope your friends don't type it in the chat at a bad time. Yes I will address this later.
- Titan's Stride requires 2 Gem Power, and travels somewhat further.
- Gem Bulwark SHOULD now destroy projectiles that are near its initial attack, but probably still doesn't.
- When approaching absurd jump speed, Zot quickly slows down to managable levels.
- Footstep knockback direction no longer sends enemies away from you.
- Hopefully, no more lingering effects at high charge levels. But what has hope done for me lately?
- Bunch of misc numbers tweaks
- Jump charges slightly faster.
- I think I actually somehow fixed his multiplayer UI issues?



1.2.4
- Works with anniversary update!

- lighter enemies don't make large wall impacts as easily. heavy enemies basically always do

-  Eldritch Slam's area has been massively decreased. HOWEVER!
aiming straight down while charging slam strikes the ground, dealing half damage in an even larger area than previously.
Using it in the air will send you hurtling to the ground (which will of course, cause a huge landing quake on top of the slam)

Essentially, you now have a single target option and an area effect option.

- eldritch slam can be charged during leap lmao

- eldritch fury and eldritch slam slow your vertical movement in the air somewhat.

- 250 base armor +10 per level -> 100 base armor + 10 per level
  when the gem system comes out, there will be ways for zot to accumulate armor over time, and i'll probably be nerfing his base values more as that comes into play.

-  No more debug logs (for now.)

-  No more random speedwalking.

-  hopefully, some fixes to make the UI more stable. though considering i did no multiplayer testing, probably just made it worse.

-  Eldritch Fury's radius decreased slightly (32m to 25m)
   (there will be gems for this kind of thing)

- Jump physics completely redone

- Many effects tweaked.

- More bugs!

- Couple new bugs this time.

- Activated 1.5x bug multiplier.

- More bugs.

- Increased bug capacity from 533 to [REDACTED]

- Added a few more bugs,

- More b-

1.2.3
- Should no longer double stock expenditure for all characters, effectively doubling cooldowns (oopsie on that one)
- Prototype knockback impact damage. When Zot's in-game, enemies take knockback damage based on their mass and velocity. So sending them flying into walls is quite destructive.
Probably not stable enough to include in a thunderstore release, but is ANYTHING in this mod really?
- Various number tweaks to damage scalings, blast attack falloff models, knockback, timers, whatever! I know reading what changed about these things is fun to read but my development process is such a blur.
- Pressing L now toggles percentage vs flat jump charge. You'll know it worked if Zot gets very... visually loud for a second. (This feature might be temporary!)
- Some bug fixes, and some bugs introduced!
- Did I forget anything? I'm sorry I've been awake for 17 hours


1.2.2
- Gem Power Prototype, with a basic UI. For now, Gem Power increases with level and time.
Instead of abilities becoming stronger based on time spent charging, they are boosted by how much gem power you spend while charging.
You use a % of your Gem Power per second when charging attack moves, and a flat amount when charging movement abilities like jump.
This is GOING to feel really clunky and awkward at first, please bear with me! So much still needs to be done.

- (Hotkey V) sustaining Flight costs gem power, but its movement speed has been increased to compensate.
P.S. Flight's hotkey can be really finicky, you may have to press it a few times before it will activate.


- Titan's Stride has no cooldown, but costs 1 Gem Power. Eldritch Fury's damage and range have been increased, but it now costs 1 Gem Power.
hopefully this can be considered a buff patch overall. I want Zot's values when he's playing with high gem power to be through the roof, but he has to choose when to use it.
He will scale crazily into late-game, of course.

- Zero Proc Coefficients (bands still work)

- New placeholder passive - Inevitable:
Every 15 Seconds, Zot gains 1 maximum gem power, and his base health is multiplied by 1.05x (thus, each max health boost is slightly larger than the previous).

- Brought back old footstep effect for now

- Zot's backhand knockback force is mildly increased by Zot's maximum gem power, and it's shockwave effect has a larger radius, but it does not cost anything for now.
The knockback direction is entirely influenced by your aim, but the hitbox only cares about where Zot is facing; Aim straight up and anything in front of you will be immediately
registered for the Z.S.P (Zot Space Program)

- Zot now alternates arms when swinging, no matter how much time has passed

- Zot is immune to the instant kills caused by Void Reavers, due to the physics of his body being inherently incompatible with its effect. 
However, this also causes an incredibly violent reaction, causing Zot to emit an intense, forceful blast that deals 1000 base damage to himself, allies and anything else in a huge radius.

- No more NREs every frame while Zot doesn't exist! Instead, NREs every frame that he DOES exist! Yay!

- Skindef added, so hopefully he won't destroy your character select screen anymore.

- Zot has an emissive glow on parts of his armor that activates when he is using gem power. (those weird colored lines are just me experimenting, they won't stay that way!)

- Remember, Zot is a mess, his code is a mess, and I live only for chaos and spectacle. You are downloading a mod created by a beginner, ADHD programmer. It may or may not glitch spectacularly in
unexpected or run-ending ways, and can even mess with your game when you're not playing as him!
For that reason, I recommend not having Zot enabled/installed unless you or someone in your group plans on playing him.


1.2.1
- V in the air to toggle float. Activation is very finicky and I don't know why yet, so you may have to spam it a bit to get it to work.
- Titan's Stride now has different, very powerful behavior when used on the ground. It utilises the game's pathfinding, allowing Zot to quickly navigate complex terrain, cross gaps, and even go through some walls,
and it will never cause Zot to leave the ground.
- Titan's Stride while floating will follow your aim direction, allowing for vertical movement. Any other mid-air striding will follow the usual behavior.
- Eldritch Slam prototype!
- Zot has even less attack speed. And even more damage. ALL Zot's numbers are placeholders right now, just roll with it lmaooo
- Zot regenerates a small percentage of his missing health every frame.
- Landing during float triggers a lighter landing that does not slow zot down, and is less destructive
- Increased base values for landing quake and Gem Bulwark. This is overall a huge buff patch for Zot, and I expect him to be utterly ridiculous for now.
- I either fixed multiplayer, or made it 5000x worse.
- Zot can no longer pseudo-sprint. Energy Drink is Dead.
- Explained Zot's mechanics slightly better in the in-game descriptions, but it's all still very rough.
- Infinite range Zot Laser.
- Incoming projectiles are destroyed by Gem Bulwark's initial attack. (May not be reliable)
- Fixed some bugs, created 5000 more.
- Zot will still clog your debug log with NREs! A great feature that many appreciate
- and he still has no skinIndex, so watch out, skinAPI users!
- Zot's charge rate for all chargeable abilities increases with his level! (A quick and shoddy emulation of the Gem Power system that will scale his abilities as he progresses)

1.1.9
- CTRL in the air to toggle float. Zot isn't any faster while doing this, of course, but can allow you to hold position in the air.
- Aerial Jump attacks mostly implemented. They have some wonkiness, and you can't move after landing with M2.
- M2's screenshake still needs adjustment, and the camera change is jarring, especially if only using the move for the initial knockback.
- This version will actually appear as the latest, unlike the last one.
- Aim animator made slightly worse thanks to a mistake on my part, will tune in the next version.
- Zot's animations can get slightly confused sometimes, I think I know why, but can't fix it quite yet.
- New M2 icon, courtesy of Geartor
- I am hesitantly removing the internal cooldowns on Zot's jump ability, and his landing effects. Now that hopoo feather has been, ahem, dealt with, and his landing values scale with fall velocity, it should hopefully not be such a simple task to spam.


1.1.8
- This is a fun one.
- Hopoo shader applied, elite materials work and Zot should look much better visually!
- Gem Bulwark prototype added. expect instability. walk animation is weird, aim is slightly weird, and i had to weirdify the camera somewhat to get the raycast to function properly. Animations are supremely unfinished.

1.1.7
- After a certain point in your jump dependent on charge power, Zot's fall speed will massively accelerate.
- Jump has a 1.5 second cooldown. This makes things more stable, and allows me to buff the shockwave effect to non-trivial levels.
- Jump Charge maximum duration is now truly infinite.
- Landing animation now fully functions out of jump.
- Jump Charge effects tweaked.
- Currently, when landing from jump, you cannot cancel out of the landing animation with any of your abilities.
- Now that jump has a cooldown, you can jump instantly if you choose, but it'll be the tiniest little hop.
- When Eldritch Fury is interrupted by landing, if it is at least 20% of the way towards unleashing its attack, it will do so immediately.
- Zot's fall speed generally accelerates over time even when not jumping.
- Zot's Hurtbox adjusted so enemies more appropriately aim at him.


1.1.61
- Credits!

1.1.6
- Can attack and turn while jumping. Eldritch Fury will be interrupted when landing.
- Attack frequency is now consistent for an enhanced muda experience 
- Hopefully fixed Zot speedwalking after jumping
- Blue and Pink colors desaturated somewhat
- Adjusted Villainous Skirt Physics
- I forgot to add additional placeholder icons again




1.1.5
-- GOD HOW DID I NOT NOTICE EVERYONE HAS ZOT JUMP
-- Titan's Stride tweaked.
-- Jump physics tweaked, you should have a bit more control over your jump now.


1.1.4

-- Zot's power jump implemented. If jump is held, Zot will begin storing energy for the jump. The longer jump is held, the higher Zot will jump. While jumping, Zot has increased speed, proportional to charge time. Additionally, enemies near Zot's initial jump will be carried straight into the air with him. Zot will have high air control at the start of the jump, but will soon lose the ability to change direction. Zot's charge rate increases slightly with character level.
-- Titan's Stride completes sooner. 
-- Fixed Zot's teamindex, he no longer damages allies.
-- Zot's quake is now sweetspotted, dealing far more damage to enemies closest to Zot, and far less to enemies further away.
-- Placeholder icons, courtesy of Geartor (and a lil later, PentaKayle and GameBoy)
-- Zot's footsteps and landing quake now send enemies slightly upwards.


1.1.3
- Titan's Stride now goes much further, to offset his lowered movement speed
- I made M1 look more ridiculous. I will probably change it later
- Fixed the uphill issue, hopefully.
- Tweaked acceleration a little bit. I don't want to bring his turnspeed down just yet.

1.1.2

- Aim Animator has been tweaked and should perform much better (though not perfectly)
- Zot no longer becomes airborne as easily when striding downhill (still doable)
- Zot is unable to move for a moment when landing, and the landing blast has greatly increased force (knockback)
- If I did it correctly, Zot is unable to be moved by large enemies even if they try to run into him.
- Zot has lowered movement and attack speed.
- Zot gains 10 armor per level, and 0.3 health regen per level. Like most of Zot's insane numbers, this is to compensate for his incomplete moveset..
- Zot's shields dematerialize after 6 seconds of inactivity, and are summoned when attacking
- Eldritch Fury has a subtle visual effect that helps indicate it's AoE somewhat. Keep in mind this is placeholder.
Although Eldritch Fury has an insanely large blast radius, it is coded to do far more damage within the very center.
- During Zot's immobile period after landing, Hopoo Feathers will not do the thing. Hopoo Feathers are still insane, but it's not as freely spammable, and you're slowed down while using it this way. Still bonkers. I need a hook to keep it from making Zot airborne if he is grounded.
- Zot gains iframes during Titan's Stride's dash (probably)
- Footstep blasts

1.1.1

Released Thunderstore Version



