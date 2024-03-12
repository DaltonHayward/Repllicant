-> main

=== main ===
Hello! I am a simple test NPC. 
This is a test of my dialogue function.
Here is a test of my dialogue choice function!
Is this working?
    + [Yes]
        -> chosen("Yes")
    + [No]
        ->chosen("No")
    + [Maybe]
        -> chosen("Maybe")
        
=== chosen(answer) ===
Wow! Thank you for your helpful input.
Glad to hear it's a {answer}...
-> END