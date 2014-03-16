KeyBV
=====

3DS X/Y Battle Video Data Viewer

KeyBV is a tool I wrote to decrypt portions of the battle videos stored in the extdata of your SD card. 
With this tool you can view IVs/Nature/ESV/TSV of Hatched Pokémon.

SUPPORTS BOTH RETAIL CARTRIDGES AND DIGITAL COPIES

Thread:
http://projectpokemon.org/forums/showthread.php?37568

Extra:

Put it together pretty quick; copied code from other projects so it is kinda dirty. 
The trick is similar to the one KeySAV uses; by arranging data we can reverse the static one time pad.
There are different one time pads per video slot, but if we force the game to save to the same slot... :)
