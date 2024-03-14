-> main

=== main ===
Hey bud! My name is "Dalton"! 
Welcome to "Replicant"! 
Why is it called that? Who knows!
Did you know that you can change your weapon or tool by using the scroll wheel?
That thing on your mouse? 
You can tell that you have changed your weapon or tool because they look VERY different.
Uhhhh... what else...
Oh yeah! There are some enemies out there that you gotta watch out for!
You can use your tools to break trees and rocks that you find! 
I'm sure you can find 'em!
Does that all make sense?
    + [Yes]
        -> chosen("Yes")
    + [No]
        ->chosen("No")
    + [Maybe]
        -> chosen("Maybe")
        
=== chosen(answer) ===
{answer}?????? That'll have to do I guess.
Catch ya later pal! I will still be here.
I'm just a simple capsule y'know...
-> END