# How to Play FlyingHouseVR

## Quick Start (2 minutes)

### 1. Clone
```bash
git clone https://github.com/brandongraves08/flyinghousevr.git
cd flyinghousevr
```

### 2. Open Unity
- Unity Hub → Add project → Select this folder
- Unity 2022.3+ required

### 3. Add Cesium (Required for terrain)
```
Window → Package Manager
+ → Add package from git URL
Paste: https://github.com/CesiumGS/cesium-unity.git?path=/Packages/com.cesium.unity
```

### 4. Get Token
1. Go to https://cesium.com/ion
2. Sign up (free)
3. Tokens → Create token → Copy
4. Unity: Cesium → Cesium → Paste token

### 5. Open Scene
```
Assets → Scenes → FlyingHouse.unity
Double-click
```

### 6. Press PLAY

---

## Controls (Desktop)

| Key | Action |
|-----|--------|
| **WASD** | Steer |
| **Space** | Brake/Hover |
| **Right Click + Drag** | Look around |
| **1-4** | Jump to cities (NYC, Paris, Tokyo, SF) |
| **F1-F4** | Weather (Clear, Cloudy, Rain, Storm) |
| **R** | Open Room Menu |
| **E** | Use doorways/teleport |
| **Esc** | Pause/Menu |

---

## What Works

✅ Fly anywhere on Earth (real Cesium terrain)
✅ 4 cities with scenarios
✅ Weather system
✅ Multi-room house (unlock with credits)
✅ Achievements
✅ Save/load progress
✅ Desktop + VR

---

## Build for Quest (Optional)

```
File → Build Settings
Platform: Android
Switch Platform
Build and Run
```

Requires Quest connected via USB.

---

## Common Issues

**"Pink terrain"** = Cesium token not set. Get token from cesium.com

**"No VR"** = Desktop mode works automatically. For VR, enable OpenXR in Project Settings.

**Pink boxes everywhere** = That's placeholder art. We haven't made 3D models yet.

---

That's it. Clone, token, PLAY. Flying in 5 minutes or less.

**Repo:** github.com/brandongraves08/flyinghousevr
