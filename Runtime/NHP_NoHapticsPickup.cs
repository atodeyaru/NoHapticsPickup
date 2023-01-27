
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common;

namespace Atodeyaru.Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
    public class NHP_NoHapticsPickup : UdonSharpBehaviour
    {
        [SerializeField] private Transform _exactGrip;
        [SerializeField] private bool _AutoHold;

        private bool _picking;
        private bool _left;
        private Vector3 _positionOffset;
        private Quaternion _rotationOffset;

        void Start()
        {
        }

        private void Update()
        {
            if (_picking)
            {
                var hand = Networking.LocalPlayer.GetTrackingData(_left ? VRCPlayerApi.TrackingDataType.LeftHand : VRCPlayerApi.TrackingDataType.RightHand);
                this.transform.position = hand.position + hand.rotation * _positionOffset;
                this.transform.rotation = hand.rotation * _rotationOffset;
            }
        }

        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            Drop();
        }

        public override void InputDrop(bool value, UdonInputEventArgs args)
        {
            if (args.handType == (_left ? HandType.LEFT : HandType.RIGHT) && value)
            {
                Drop();
            }
        }

        public override void InputGrab(bool value, UdonInputEventArgs args)
        {
            if (args.handType == (_left ? HandType.LEFT : HandType.RIGHT) && !value && !_AutoHold)
            {
                Drop();
            }
        }

        public void Grab(bool left)
        {
            TakeOwnership();
            _picking = true;
            _left = left;
            var hand = Networking.LocalPlayer.GetTrackingData(left ? VRCPlayerApi.TrackingDataType.LeftHand : VRCPlayerApi.TrackingDataType.RightHand);
            if (_exactGrip != null)
            {
                _positionOffset = Quaternion.Inverse(_exactGrip.rotation) * (this.transform.position - _exactGrip.position);
                _rotationOffset = Quaternion.Inverse(_exactGrip.rotation) * this.transform.rotation;
            }
            else
            {
                _positionOffset = Quaternion.Inverse(hand.rotation) * (this.transform.position - hand.position);
                _rotationOffset = Quaternion.Inverse(hand.rotation) * this.transform.rotation;
            }
        }

        private void Drop()
        {
            _picking = false;
        }

        private void TakeOwnership()
        {
            if (VRCPlayerApi.GetPlayerCount() > 1 && !Networking.IsOwner(this.gameObject))
            {
                Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
            }
        }
    }
}
