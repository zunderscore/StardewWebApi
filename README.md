# Stardew Valley Web API

A mod for Stardew Valley that provides access to some game basic information, actions, and events via an HTTP interface.

It's my first game mod, so be gentle.

## Info

### General Info

- `/info/player`: Information about the current player
- `/info/world`: Information about the world, including in-game date and farm weather

### NPC Info

- `/npcs`: Gets a basic list of all named NPCs
- `/npc`: Gets a specific NPC by name
- `/npc/birthday`: Gets a list of all NPCs with a birthday on the specified day of the season

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
- `/action/giveMoney`: Gives money to/takes money away from the player
- `/action/giveItem`: Attempts to add the specified item to the player's inventory
- `/action/warpPlayer`: Warps the player to the specified location
- `/action/petFarmAnimal`: Pets the named farm animal (i.e. chickens, cows, etc., but not pets like dogs, cats, or turtles)

## Events

Opening a WebSocket connection to `/events` will allow another application to listen for some basic in-game events.