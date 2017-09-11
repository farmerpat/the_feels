The Co-dependant
================
**Or: The healer**
**Or: The Shaman**

The object of the game is to balance the feelings of every other character.
Each character will have a level dedicated to them.  The first portion of
the level will be the player navigating a course of objects from the life
of that person.  After completing each level, the player will ascend to
the ethereal plan, becoming the pair of colored circles and the orbital shooter.
They will then tackle the "boss" (the emotional representation of the target).
Empathy is the players gift/curse.

Level 0 could be a target that is far away from the player, a child, say.
A friend's kid maybe.  Or its supposed to be me as a child, and the whole
game is supposed to be me revisting periods of my past and each level will
contain artifacts that helped to color my perception of each one of those periods.
As the levels progress, the targets get closer and closer to the player (or
closer and closer to me in age) until the final level is the present moment.


The terrestrial levels will be in greyscale because color is a whole different
animal, but it might be interesting to keep them that way. It could be a statement
about the human day-to-day perception of reality.  The objects in the terrestrial
plane (including projectiles) will be either positive or negative, and will have a
certain number of + or - points associated with them.  Grey oejbects will be
neutral, e.g. have no effect.  If the player is standing on a red object, or
example, his negative feels meter will increase.  Same for positive feels.  If the
meters become too out of balance, the player is wasted.  It probably makes sense
to have colored platforms become grey after then have discharged their full value,
so that the player can't just stand in the same place to regain balance.  Similarly,
certain projectiles won't fire if it would be too helpful.
On the ground, the player's color could reflect their balance too. So, purple is 50/50.
Additionally, meters should be displayed in the HUD. May or may not use murderous items
like spikes.  The level will contain artifacts from the target's life, revealing some
information about how they became imbalanced in the way they are.

Thoughts
--------
* consider making the controls for the terrestrial shooting arm the same as those for
  the orbital
* consider a head-detatching, expanding, morphing animation for the ascention following
  terrestrial level completion
* consider making the player discharge feeling values when shooting. conditionally though, this mechanic is exploitable

=======
Terrestrial Plane
-----------------
* try and turn on grid snap
  - this can be leveraged with help from the 32 pixels per unit
  - should be able to start with single tiles with white in them
  - these tiles can be colored and combined into collections of game objects that will become the building-blocks of levels

* a platform segment game object can have the following:
  - a depleatable charge
  - a charge cycle
    * some time red
    * some time blue
    * some time white (no charge)

Ethereal Plane
--------------
* the player's goal is to navigate the psyche and balance the damaged parts
* the level itself will represent the psyche
* it should be mostly comprised of smooth, cool-colored graphics. like a steel cave
* the exceptions to this smoothness will be the jagged, damaged parts
* the damnaged portions will have colors, and need to be balanced
* as they become more balanced by the opposite color, the jagged sprite will be replaced by more and more full sprites, until the area is completely healed.
* the damaged area should shoot its own color at the player
* perhaps very damanged areas will shoot defense mechanisms that do not affect balance, but cause direct damage
* there should probably be random damaging projectiles coming from other level elements
* there should probably be static pricklies to navigate around also
