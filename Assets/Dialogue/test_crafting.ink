EXTERNAL npcAddItem(itemToAdd)
-> main

=== main ===
Hello! I am a simple test NPC. #speaker:Mr. Testing
This is a test of my ability to add items to your inventory!
Do you want a sword? You might need one!
+ [Yes]
    Let's craft!
    ~ npcAddItem("Sword")
    -> END
+ [No]
    Well... I'll be here if you change your mind!
-> END