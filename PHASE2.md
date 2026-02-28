# Phase 2: Multi-Room House

## Overview
Expand from single-room cockpit to a multi-room flying house with unlockable rooms, AR room scanning, and seamless room transitions.

## New Features

### 8 Unlockable Rooms

| Room | Cost | Unlock Requirement | Description |
|------|------|-------------------|-------------|
| **Captain's Cockpit** | Free | Starting room | Flight deck with steering |
| **Navigation Hub** | $500 | Complete 1 flight | Maps & route planning |
| **Sky Pantry** | $300 | Fly 30 min total | Food & supplies |
| **Engine Bay** | $1000 | Turbo Propellers upgrade | Engine access & upgrades |
| **Cargo Hold** | $800 | Visit 5 cities | Storage for collectibles |
| **Medical Bay** | $750 | Survive severe turbulence | Emergency supplies |
| **Star Observatory** | $1500 | Altitude 2000m | 360° glass dome |
| **Captain's Study** | $2000 | Complete all scenarios | Flight logs & relax |

### Room System
- **HomeRoomSystem**: Manages all rooms, unlocked status, current room
- **RoomConnector**: Doorways between rooms with visual feedback
- **ScreenFader**: Smooth transitions between rooms
- **RoomSelectionUI**: Menu to view rooms, unlock, teleport

### AR Room Scanning (Meta Quest)
- **ARRoomScanner**: Detect physical room dimensions
- Uses Depth API on Quest, falls back to simulation on desktop
- Saves room size for precise placement
- Minimum 2m x 2m area required

## Controls

### Room Selection
- `R` - Open room menu
- Click room to view details
- "Teleport" - Go to unlocked room
- "Unlock" - Purchase room (if enough credits)

### Room Connector (VR)
- Grab door handle
- Pull to teleport to connected room

### Room Connector (Desktop)
- Walk to door
- Press `E` to activate

## Setup

1. AR Room Scanning (Quest only):
2. In-game: Options > Scan Room
3. Walk around your space
4. Once complete, rooms are placed automatically

## Room Layout

Rooms connect like a branching tree:

```
     [Engine Bay]
         |
[Cockpit]—[Navigation]—[Observatory]
     |        |              |
[Pantry] [Cargo Hold]  [Medical Bay]
     |              |
[Study]-----------+--------+
```

## Credits System

- Earn credits by completing flights and scenarios
- Credits persist via SaveSystem
- Spend credits in HouseUpgradeSystem for ship upgrades
- Spend credits in RoomSelectionUI for new rooms

## Technical Notes

- Rooms are instantiated at runtime from prefabs
- Only current room is active (others disabled)
- Room positions based on scanned room size or defaults
- Screen fades prevent jarring transitions
- Save file stores unlocked rooms and positions

## Future (Phase 3+)

- Custom room decorations
- Furniture placement
- Photo-realistic room models
- Room-specific mini-games
- Multi-room simultaneous display

## Status

Phase 2 in Progress \- see commit history for latest
