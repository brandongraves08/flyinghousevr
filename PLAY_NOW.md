# FlyingHouseVR - PLAY IN 60 SECONDS

## Option 1: Quick Start (Fastest)

### Step 1: Clone
```bash
git clone https://github.com/brandongraves08/flyinghousevr.git
cd flyinghousevr
```

### Step 2: Open Unity
1. Open Unity Hub
2. Add project from cloned folder
3. Open project
4. Open scene: `Assets/Scenes/QuickStart.unity`
5. Press **Play** ▶️

### Step 3: Fly
Wait 3 seconds (or press any key) → **You're flying over NYC!**

---

## Option 2: Full Game (Title Screen)

1. Open `Assets/Scenes/TitleScreen.unity`
2. Press Play
3. Click **Start Game**
4. Click **Quick Flight** (bypass setup)
5. **Fly**

---

## What Works RIGHT NOW

✅ Fly anywhere on Earth (real Cesium terrain)
✅ 4 cities: NYC (1), Paris (2), Tokyo (3), SF (4)
✅ 4 weather modes: Clear (F1), Cloudy (F2), Rain (F3), Storm (F4)
✅ Steam from buildings (Night mode)
✅ 7 unlockable rooms (Rooms menu: R)
✅ Earn credits by flying
✅ Desktop + VR support
✅ Save progress

---

## Desktop Controls

| Key | Action |
|-----|--------|
| **WASD** | Steer & Throttle |
| **Space** | Brake / Hover |
| **1-4** | Jump to cities |
| **F1-F4** | Change weather |
| **R** | Room Menu (if unlocked) |
| **Esc** | Pause |
| **Q/E** | Roll left/right |

---

## VR Controls (Quest)

- **Grip** — Grab wheel / lever
- **Trigger** — Interact / select
- **Thumbstick** — Menu navigation
- **Menu** — Pause

---

## Build for Quest (Shareable APK)

```bash
# Build Settings → Android → Switch Platform
# Player Settings: Texture Compression = ASTC
# Texture Compression: ASTC
# Build → APK
# Install via SideQuest or ADB
```

---

## Troubleshooting

| Issue | Fix |
|-------|-----|
| Pink terrain | Set Cesium token (Project Settings > Cesium) |
| Can't steer | Check DesktopInputManager is enabled |
| No audio | Adjust volume in Settings (accessible from Title Screen) |
| Stuck loading | Wait 10s for Cesium initial load, check internet |

---

## Quick Setup if Issues

**Cesium Token (required once):**
1. Go to https://cesium.com/ion
2. Sign up free → Tokens → Create Token
3. In Unity: Project Settings → Cesium → Paste token

---

**GitHub:** [github.com/brandongraves08/flyinghousevr](https://github.com/brandongraves08/flyinghousevr)

**Latest:** `d22622e` (Compilation bug fixed)

**Status:** ✅ PLAYABLE NOW
