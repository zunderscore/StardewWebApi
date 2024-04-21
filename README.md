# Stardew Valley Web API

A mod for Stardew Valley that provides access to some game basic information, actions, and events via an HTTP interface.

It's my first game mod, so be gentle.

## Info

### General Info

- `/info/player`: Information about the current player
- `/info/world`: Information about the world, including in-game date and farm weather

### Item Info

- `/items`: A basic list of all items registered in the game
- `/items/id/{itemId}`: Get an item by its fully qualified ID (e.g. `(F)1365`)
- `/items/type/{itemType}`: A basic list of all items of the given type (e.g. `F` for furniture)
- `/items/type/{itemType}/id/{itemId}`: Get an item by its type and individual ID (e.g. `F` for furniture type and `1365` for individual ID)

### NPC Info

- `/npcs`: Gets a basic list of all named NPCs
- `/npcs/name/{name}`: Gets a specific NPC by name
- `/npcs/birthday/{season}/{day}`: Gets a list of all NPCs whose birthday is on the specified day of the specified season
- `/npcs/pets`: Gets a list of all pet NPCs

### Mod Info

- `/mods`: Gets a list of all installed mods
- `/mods/{modId}`: Get info on a specific mod by its unique mod ID (e.g. `zunderscore.StardewWebApi`)

## Actions

### UI Actions

- `/ui/showHudMessage`: Show a HUD message in the bottom left corner
- `/ui/showHudMesssage/item`: Show a HUD message with an item icon
- `/ui/showHudMessage/large`: Show a larger HUD message with text wrapping and no icon box
- `/ui/showChatMessage`: Show a chat message

### Player Actions

- `/action/refillEnergy`: Fully refills the player's energy/stamina
- `/action/passOut`: Fully drains the player's energy/stamina, causing them to pass out
- `/action/fullyHeal`: Fully refills the player's health
- `/action/knockOut`: Fully drains the player's health, causing them to be knocked out/die
- `/action/giveMoney/{amount}`: Gives money to/takes money away from the player
- `/action/giveItem/name/{itemName}`: Attempts to add the specified item to the player's inventory, using the item's display name
- `/action/giveItem/id/{itemId}`: Same as above, but using the item's fully qualified ID (e.g. `(F)1365`) instead
- `/action/warpPlayer/{location}`: Warps the player to the specified location
- `/action/petFarmAnimal/{name}`: Pets the named farm animal (i.e. chickens, cows, etc., but not pets like dogs, cats, or turtles)

## Events

Opening a WebSocket connection to `/events` will allow another application to listen for some basic in-game events.

When events occur, all connected WebSocket connections are sent a broadcast message containing an event object. This object has two possible properties:
- `event`: The name of the event, as listed below
- `data`: An optional property that includes any relevant data about the event (e.g. `TimeChanged` will send values including `oldTime` and `newTime`)

### WebSocket Events

- `Connected`: When an application connects to the `/events` endpoint. This event is NOT broadcast to all connections, but rather sent ONLY to the current connection.

### Game Loop Events

- `SaveLoaded`: The player loads a save
- `Saved`: The player saves the game
- `ReturnedToTitle`: The player returns to the title screen
- `DayStarted`: A new day has started
- `DayEnding`: The current day is ending
- `TimeChanged`: The time has changed (e.g. from 6:00 AM to 6:10 AM)
- `PlayerInventoryChanged`: The player's inventory has changed in any way
- `PlayerLevelChanged`: The level of one of the player's skills has changed 
- `PlayerWarped`: The player has warped to a different map location, including normal travel between areas (e.g. walking from the farm to the bus stop)

### Relationship Events

- `FriendshipIncreased`: The player's friendship with an NPC has increased
- `MultipleFriendshipsIncreased`: The player's friendship with multiple NPCs has increased
- `FriendshipDecreased`: The player's friendship with an NPC has decreased
- `MultipleFriendshipsDecreased`: The player's friendship with multiple NPCs has decreased
- `PlayerStartedDating`: The player has started dating an NPC
- `PlayerStoppedDating`: The player is no longer dating an NPC
- `PlayerEngaged`: The player has become engaged to an NPC
- `PlayerNoLongerEngaged`: The player is no longer engaged to an NPC
- `PlayerMarried`: The player has married an NPC
- `PlayerDivorced`: The player has divorced an NPC

### Festival Events

- `FestivalStarted`: A festival has started
- `FestivalEnded`: A festival has ended