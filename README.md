# NoHapticsPickup
U# Componets for pickup without haptics.

## Differences from regular VRC_Pickup
- `GetPickupInHand` will not return this object.
- Some properties and methods are not supported.

## NHP_HandTracker

Used to pick up and hold object.
objects must contain NHP_NoHapticsPickup.
**Only one NHP_HandTracker must be placed in a world.**

|Parameter|Description|
|-|-|
|Layer Mask|Only objects on the selected layers will be picked up.|
|Pickup Radius|How far the object can be pick up.|
|Desktop Pickup Radius|How far the object can be pick up in Desktop.|

## NHP_NoHapticsPickup

Used to allow object to be picked up and held.
This component does not vibrate and highlight when the object can pick up.

|Parameter|Description|
|-|-|
|Exact Grip|The position object will be held if set to Exact Grip.|
|Auto Hold|Should the pickup remain in the users hand after they let go of the grab button.|