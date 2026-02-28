# FlyingHouseVR Setup Guide

## Prerequisites

- Unity 2022.3 LTS or newer
- Git LFS installed (for large files)
- Cesium ion account (free at cesium.com)
- Optional: Meta Quest Developer Account (for Quest builds)

## Quick Start

### 1. Clone and Open

```bash
git clone https://github.com/brandongraves08/flyinghousevr.git
cd flyinghousevr
```

Open Unity Hub → Add project from cloned folder

### 2. Install Required Packages

Open Package Manager (`Window > Package Manager`):

1. Click `+ > Add package from Git URL`
2. Paste: `https://github.com/CesiumGS/cesium-unity.git?path=/Packages/com.cesium.unity`
3. Wait for import

Verify these are installed:
- XR Interaction Toolkit (should auto-install)
- OpenXR Plugin (for VR)

### 3. Configure Cesium

1. Go to https://cesium.com/ion
2. Sign up / Log in
3. Go to `Tokens` → `Create Token`
4. Name: "FlyingHouse"
5. Copy the token string
6. In Unity: `Cesium > Cesium`
7. Paste token in Project Settings

### 4. Configure XR (VR builds)

1. `Edit > Project Settings > XR Plug-in Management`
2. Select PC (if desktop VR) or Android (Quest)
3. Check `OpenXR`
4. Add `Oculus Touch Controller Profile` (if Quest)
5. Set appropriate interaction profiles

### 5. Build and Run

**Desktop (no VR):**
1. Open `Assets/Scenes/FlyingHouse.unity`
2. Press Play in Editor
3. Or `File > Build Settings > PC/Mac/Linux`

**Meta Quest:**
1. `File > Build Settings`
2. Switch Platform to Android
3. Set Texture Compression to ASTC
4. `Build and Run` (Quest connected via USB)

**PC VR (Quest Link / Rift / Index):**
1. Ensure headset connected
2. Open `Assets/Scenes/FlyingHouse.unity`
3. Press Play

## Troubleshooting

### "Cesium token invalid" error
- Re-copy fresh token from cesium.com/ion
- Ensure token pasted in Project Settings, not Scene

### No VR detected / only desktop mode
- Check VR headset is powered on
- Check OpenXR loader is set in Project Settings
- Try restarting Unity after hardware changes

### Scene looks empty / pink materials
- Cesium terrain not loaded → Check internet connection
- Wait 5-10 seconds for initial tile load
- Verify Cesium Georeference component exists

### Controller not working in VR
- Ensure XR Interaction Manager is in scene
- Check for Interaction Manager and Player prefab
- Verify XR Rig has Action-based components

### Game too slow / frame drops
- Lower quality settings: `Edit > Project Settings > Quality`
- Reduce Cesium tile cache size
- Enable dynamic resolution in XR settings

## Project Structure

```
Assets/
├── Scenes/
│   └── FlyingHouse.unity          ← Main scene
├── Scripts/
│   ├── FlightController.cs        ← Core flight physics
│   ├── SteeringWheel.cs           ← VR steering
│   ├── AltitudeLever.cs           ← Altitude control
│   ├── DesktopInputManager.cs     ← Fallback controls
│   ├── WeatherManager.cs          ← Weather system
│   ├── ScenarioManager.cs         ← Mission system
│   ├── WaypointManager.cs         ← Navigation
│   ├── HUDManager.cs              ← UI
│   ├── AudioManager.cs            ← Sound manager
│   └── SaveSystem.cs              ← Save/load
├── Prefabs/                       ← Placeholder prefabs
├── Shaders/                       ← Window masking
├── Materials/                     ← Interior materials
└── Resources/                     ← Shared assets
```

## Default Controls

See [DESKTOP_MODE.md](DESKTOP_MODE.md) for full control reference.

**Keyboard (Desktop):**
- `WASD` / Arrows: Steer & Throttle
- `Space`: Brake/hover
- `Right Click`: Lock mouse for look
- `1-4`: Jump to cities
- `F1-F4`: Change weather

**VR Controllers:**
- `Grip`: Grab wheel or lever
- `Trigger`: Actions, lean out window
- `Menu`: Pause

## First Run

1. Tutorial should auto-start (disable in TutorialManager if unwanted)
2. Follow on-screen prompts
3. Calibrate wheel position (grab it, place where comfortable)
4. Take off by pushing the altitude lever forward!

## Need Help?

- Check [GitHub Issues](https://github.com/brandongraves08/flyinghousevr/issues)
- Cesium Docs: https://cesium.com/learn/unity/
- Unity XR Forum: https://forum.unity.com/forums/xr.html
