
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common;

namespace Atodeyaru.Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class NHP_HandTracker : UdonSharpBehaviour
    {
        [SerializeField] private LayerMask _layerMask = int.MaxValue;
        [SerializeField] private float _pickupRadius = 0.05f;
        [SerializeField] private float _desktopPickupRadius = 1.0f;

        private bool _isUserInVR;

        void Start()
        {
            _isUserInVR = Networking.LocalPlayer.IsUserInVR();
        }

        public override void InputGrab(bool value, UdonInputEventArgs args)
        {
            if (value)
            {
                if (_isUserInVR)
                {
                    var left = args.handType == HandType.LEFT;
                    var hand = Networking.LocalPlayer.GetTrackingData(left ? VRCPlayerApi.TrackingDataType.LeftHand : VRCPlayerApi.TrackingDataType.RightHand);
                    var colliders = Physics.OverlapSphere(hand.position, _pickupRadius, _layerMask, QueryTriggerInteraction.Collide);

                    NHP_NoHapticsPickup pickup;
                    foreach (var collider in colliders)
                    {
                        if (collider == null) continue;

                        pickup = collider.GetComponent<NHP_NoHapticsPickup>();
                        if (pickup != null)
                        {
                            pickup.Grab(left);
                            return;
                        }
                    }
                }
                else
                {
                    var head = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                    
                    var hits = Physics.RaycastAll(head.position, head.rotation * Vector3.forward, _desktopPickupRadius, _layerMask, QueryTriggerInteraction.Collide);

                    NHP_NoHapticsPickup pickup;
                    foreach (var hit in hits)
                    {
                        if (hit.collider == null) continue;

                        pickup = hit.collider.gameObject.GetComponent<NHP_NoHapticsPickup>();
                        if (pickup != null)
                        {
                            pickup.Grab(false);
                            return;
                        }
                    }
                }
            }
        }
    }
}