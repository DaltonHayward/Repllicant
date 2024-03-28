EXTERNAL npcAddItem(itemToAdd)

-> main

=== main ===
 Well, look who's awake! Hate to be the bearer of bad news, but your ship crashed and your crew has gone off. 
You look confused...
I'm Dalton! #speaker:Dalton
Do you need me to explain a few things? 

+ [I don't need help!]
    Alright! Alright! Get going then if you're in such a rush!
    -> tools
+ [Yes please]
    -> tutorial
    
=== tutorial ===
I figured! Let's start simple and get you moving.
If you haven't caught on yet, you can move around with the WASD keys! 
If you don't like any of your keybindings? 
You can always reset them in the settings menu. 
You can look in your bag by hitting -tab-!
You can equip some items so by right-clicking and selecting equip.
Same thing for if you need to unequip something. 
Need to get something out of your bag? 
Just drag and drop it outside of your inventory grid!
You can dodge out of harm's way by hitting the -space- key!
Once you have something equipped, you can swing it by left-clicking.
You can cycle your equipped items by using the -scroll wheel- on your mouse.
Does that make sense?
+ [Ya I guess]
    Great! Now... could you do me a favour?
    And clear those trees and rocks for me so I can set up my tent?
    -> tools
+ [Umm not really]
    -> tutorialRecap
    
===tutorialRecap===
-WASD- to move.
Can change keybindings in settings menu.
Inventory using -tab-.
Equip by -right click- and selecting -equip-, same for unequip. 
Dodge with -space-.
Attack with something equipped with -left click-.
Cycle items by using the -scroll wheel-
-> tools

=== tools ===
Before you go, here are some things that should help.
~ npcAddItem("Sword")
~ npcAddItem("Axe")
~ npcAddItem("Pickaxe")
Make sure to equip those!
Oh and once you chop down those trees, you should probably craft yourself a torch!
Good luck out there...
-> END