# vertigo2

## Roadmap

* Develop story
    * Characters
    * Overall setting & plot
    * In-game events * cutscenes

* Level Editor
    * Define level file format (probably YAML)
    * Grid snapping & object placement
    * Develop interface good for both desktop and mobile    
          * Something like Mario maker, click on a category then drag to elements of the category
    * Add basic objects
          * Gravity blocks
          * Screen wrap blocks with movable indicators for where the screen will snap and where its limits will be
          * Player
          * Basic ground blocks
          * Key & Lock blocks
          * Goal block
    * As soon as the basic version is done, open it up to supporters (they'll have to share the files directly until the sharing system is done)

* Art
    * Decide on new art style (leaning towards greyscale for the main game)
    * Find a pixel artist that I like & get a quote (fiverr probably)
    
* Level design
    * 20 levels in base game
    * May use levels of supporters in the main game if any good ones get made
    
* New game mechanics to be used in levels
    * Bubbles, can only be jumped on once
    * etc.

* Level sharing system
    * Log in via Oauth Steam for PC and Google Play for mobile
    * AWS database for level, creator, ratings, difficulty
    * Community approval system
            * When submitted, a level can only be played by community volunteers that screen profanity, rate difficulty etc.
            * After it's been approved by several volunteers it can be downloaded by anybody
    * Random level button
    * Search by any of the fields above (creator, ratings, etc.)
    * Local storage of which levels have already been played & completed, avoid these on random
