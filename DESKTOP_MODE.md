# Desktop Mode (Non-VR)

FlyingHouseVR can be played without a VR headset using keyboard and mouse controls.

## Setup

1. Open the project in Unity
2. Open `Assets/Scenes/FlyingHouse.unity`
3. In the Hierarchy, find `FlyingHouseManager`
4. Add the `DesktopInputManager` component if not present
5. Configure the component:
   - Check `Use Desktop Input`
   - Set `Player Camera` to your Main Camera
   - Reference the FlightController, SteeringWheel, and AltitudeLever objects

## Controls

| Action | Input |
|--------|-------|
| Steer Left/Right | A / D or Left / Right Arrows |
| Throttle Up/Down | W / S or Up / Down Arrows |
| Brake (Hover) | Spacebar |
| Mouse Look | Hold Right Click + Move Mouse |
| Lock/Unlock Cursor | Right Click or L key |
| Pause | Escape |
| Jump to NYC | Key 1 |
| Jump to Paris | Key 2 |
| Jump to Tokyo | Key 3 |
| Jump to SF | Key 4 |
| Weather: Clear | F1 |
| Weather: Cloudy | F2 |
| Weather: Rain | F3 |
| Weather: Storm | F4 |

## Building for Desktop

1. `File > Build Settings`
2. Select `PC, Mac & Linux Standalone`
3. `Switch Platform`
4. Set Target Platform to your OS
5. `Build and Run`

The desktop mode auto-detects if VR is available. If no VR headset is detected, it automatically switches to desktop controls.

## Troubleshooting

**Mouse not looking around:**
- Press Right Click or L to lock cursor
- Cursor must be locked for mouse look to work

**Controls not responding:**
- Check that DesktopInputManager is enabled
- Verify FlightController and other references are set
- Open Input Manager (`Edit > Project Settings > Input`) and ensure axes are configured

**Want to force VR mode:**
- Disable DesktopInputManager component
- Or check `Force Desktop In Editor` to false
