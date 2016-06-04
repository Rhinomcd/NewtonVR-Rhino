using UnityEngine;
using System.Collections.Generic;

namespace NewtonVR_Rhino
{
    public class NVRPlayer : MonoBehaviour
    {
        public static NVRPlayer Instance;
        public bool PhysicalHands = false;

        public NVRHead Head;
        public NVRHand LeftHand;
        public NVRHand RightHand;

        [HideInInspector]
        public NVRHand[] Hands;

        private Dictionary<Collider, NVRHand> _colliderToHandMapping;

        private void Awake()
        {
            Instance = this;
            NVRInteractables.Initialize();

            if (Head == null)
            {
                Head = GetComponentInChildren<NVRHead>();
            }

            if (LeftHand == null || RightHand == null)
            {
                Debug.LogError("[FATAL ERROR] Please set the left and right hand to a nvrhands.");
            }

            if (Hands == null || Hands.Length == 0)
            {
                Hands = new[] { LeftHand, RightHand };
            }

            _colliderToHandMapping = new Dictionary<Collider, NVRHand>();
        }

        public void RegisterHand(NVRHand hand)
        {
            var colliders = hand.GetComponentsInChildren<Collider>();

            foreach (Collider coll in colliders)
            {
                if (_colliderToHandMapping.ContainsKey(coll) == false)
                {
                    _colliderToHandMapping.Add(coll, hand);
                }
            }
        }

        public NVRHand GetHand(Collider collider)
        {
            return _colliderToHandMapping[collider];
        }

        public static void DeregisterInteractable(NVRInteractable interactable)
        {
            foreach (var hand in Instance.Hands)
            {
                hand.DeregisterInteractable(interactable);
            }
        }
    }
}