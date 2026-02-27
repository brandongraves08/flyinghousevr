# Flying House VR 🏠✈️

A VR experience where your actual house becomes a flying ship. Look out your windows to see Cesium terrain streaming below as you fly anywhere on Earth.

**Status:** Phase 1 (Single Room Cockpit MVP) — In Development

[![Unity CI](https://github.com/brandongraves08/flyinghousevr/actions/workflows/ci.yml/badge.svg)](https://github.com/brandongraves08/flyinghousevr/actions)

---

## Features

### Current (Phase 1)
- ✅ AR Passthrough — See your real room while flying
- ✅ Grabbable steering wheel (persistent calibration)
- ✅ Altitude/speed lever control
- ✅ Cesium terrain streaming over real Earth
- ✅ Window portal masking

### Planned
- 🚧 Weather system (clear, cloudy, rain, storm)
- 🚧 Flight scenarios (NYC flyover, Rocky Mountains storm, etc.)
- 🚧 Virtual balcony (lean out windows)
- 🚧 Multi-room house (Phase 2)

---

## Quick Start

### Prerequisites
- Unity 2022.3 LTS or newer
- Android Build Support (for Quest)
- Meta Quest 2/3/Pro or compatible VR headset

### Setup

1. **Clone the repo:**
   ```bash
   git clone https://github.com/brandongraves08/flyinghousevr.git
   cd flyinghousevr
   ```

2. **Open in Unity Hub:**
   - Open Unity Hub
   - Click "Open"
   - Select the `flyinghousevr` folder

3. **Install Required Packages:**
   - Open `Window > Package Manager`
   - Click `+ > Add package from Git URL`
   - Add: `https://github.com/CesiumGS/cesium-unity.git?path=/Packages/com.cesium.unity`
   - Verify XR Interaction Toolkit is installed (should be via manifest)

4. **Set Up Cesium:**
   - Go to https://cesium.com/ion/
   - Create free account → Tokens → "Create Token"
   - Copy token
   - In Unity: `Cesium > Cesium` → paste token

5. **Build for Quest:**
   - `File > Build Settings`
   - Switch Platform to Android
   - Texture Compression: ASTC
   - `Build`

---

## Controls

| Action | Input |
|--------|-------|
| Steer | Grab wheel → Turn left/right |
| Altitude | Push lever forward (down/slower) / Pull back (up/faster) |
| Lean out window | Grab rail + pull trigger |
| Pause | Menu button |
| Grab objects | Grip button |

---

## Project Structure

```
Assets/
├── Scenes/
│   └── FlyingHouse.unity          # Main scene
├── Scripts/
│   ├── FlightController.cs        # Flight physics & movement
│   ├── SteeringWheel.cs           # Grabbable wheel logic
│   ├── AltitudeLever.cs           # Speed/altitude control
│   ├── CalibrationManager.cs      # Persistent positioning
│   ├── WindowPortal.cs            # Stencil window masking
│   ├── WeatherManager.cs          # Weather system
│   ├── ScenarioManager.cs         # Flight missions
│   └── BalconyManager.cs          # Lean out mechanic
├── Shaders/
│   ├── StencilMask.shader         # Window hole shader
│   └── TerrainVisible.shader      # Terrain visibility shader
├── Materials/
│   ├── WindowGlass.mat
│   ├── Wall.mat
│   └── Floor.mat
└── Editor/
    └── SceneBuilder.cs            # Editor helper for scene setup
```

---

## Development

See [CONTRIBUTING.md](CONTRIBUTING.md) for details.

---

## License

MIT License — see [LICENSE](LICENSE)

---

## Credits

- Cesium — 3D geospatial data streaming
- XR Interaction Toolkit — VR interactions
