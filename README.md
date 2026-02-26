# Flying House VR

A VR experience where your actual house becomes a flying ship. Look out your windows to see Cesium terrain streaming below.

## Phase 1: Single Room Cockpit

### Features
- AR Passthrough (see your real room)
- Steerable wheel (placeable and persistent)
- Altitude/speed lever
- Window portals showing Cesium terrain
- Free-roam flight over real Earth

## Setup

### 1. Unity Setup
- Unity 2022.3 LTS or newer
- Android Build Support (Quest)
- OpenXR Plugin

### 2. Package Manager Dependencies
Add to `Window > Package Manager > + > Add from Git URL`:
- Cesium for Unity: `https://github.com/CesiumGS/cesium-unity.git?path=/Packages/com.cesium.unity`
- XR Interaction Toolkit: Built-in, add via Package Manager UI

### 3. Cesium Ion Token
1. Go to https://cesium.com/ion/
2. Sign up for free account
3. Go to `Tokens` > `Create Token`
4. Name it "FlyingHouseVR"
5. Copy the token
6. In Unity, open `Cesium` menu > `Cesium`
7. Paste your token in the Project Settings

### 4. Build for Quest
1. Switch to Android platform
2. Set Texture Compression to ASTC
3. Player Settings > XR Plug-in Management > OpenXR > Quest/Quest 2
4. Build and run

## Controls
- **Steering Wheel**: Grab and turn to bank left/right
- **Lever**: Push forward to descend/slow, pull back to ascend/speed up
- **Calibration**: Place wheel where comfortable, position remembered

## Architecture

### Scripts
- `FlightController.cs` - Handles movement, steering input, altitude
- `SteeringWheel.cs` - Grabbable wheel interaction
- `AltitudeLever.cs` - Speed/height control lever
- `CalibrationManager.cs` - Save/load wheel position
- `WindowPortal.cs` - Stencil masking for window views

### Scene Hierarchy
```
XR Origin (AR Passthrough)
├── Camera
├── SteeringWheelAnchor (persistent position)
│   └── SteeringWheel (grabbable)
├── LeverAnchor
│   └── AltitudeLever
└── HouseInterior
    ├── Walls (passthrough stencil)
    ├── Floor
    └── WindowFrame

Cesium3DTileset (World Terrain)
CesiumGeoreference
CesiumGlobeAnchor (attached to XR Origin)
```