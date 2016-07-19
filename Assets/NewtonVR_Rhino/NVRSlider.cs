using UnityEngine;
using System.Collections;

namespace NewtonVR_Rhino
{
    public class NVRSlider : NVRInteractable
    {
        [Tooltip("Set to zero when the slider is at StartPoint. Set to one when the slider is at EndPoint.")]
        public float CurrentValue = 0f;

        [Tooltip("A transform at the position of the zero point of the slider")]
        public Transform StartPoint;

        [Tooltip("A transform at the position of the one point of the slider")]
        public Transform EndPoint;
        
        protected float AttachedPositionMagic = 3000f;

        protected Transform PickupTransform;
        protected Vector3 SliderPath;

        protected override void Awake()
        {
            base.Awake();

            if (StartPoint == null)
            {
                Debug.LogError("This slider has no StartPoint.");
            }
            if (EndPoint == null)
            {
                Debug.LogError("This slider has no EndPoint.");
            }

            this.transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, CurrentValue);
            SliderPath = EndPoint.position - StartPoint.position;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsAttached == true)
            {
                var PositionDelta = (PickupTransform.position - this.transform.position);

                var velocity = PositionDelta * AttachedPositionMagic * Time.fixedDeltaTime;
                this.Rigidbody.velocity = ProjectVelocityOnPath(velocity, SliderPath);
            }

            if (this.transform.hasChanged == true)
            {
                var totalDistance = Vector3.Distance(StartPoint.position, EndPoint.position);
                var distance = Vector3.Distance(StartPoint.position, this.transform.position);
                CurrentValue = distance / totalDistance;

                this.transform.hasChanged = false;
            }
        }

        public override void BeginInteraction(NVRHand hand)
        {
            base.BeginInteraction(hand);

            var closestPoint = Vector3.zero;
            var shortestDistance = float.MaxValue;
            for (var index = 0; index < Colliders.Length; index++)
            {
                var closest = Colliders[index].bounds.ClosestPoint(AttachedHand.transform.position);
                var distance = Vector3.Distance(AttachedHand.transform.position, closest);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPoint = closest;
                }
            }

            PickupTransform = new GameObject(string.Format("[{0}] PickupTransform", this.gameObject.name)).transform;
            PickupTransform.parent = hand.transform;
            PickupTransform.position = this.transform.position;
            PickupTransform.rotation = this.transform.rotation;
        }

        public override void EndInteraction()
        {
            base.EndInteraction();

            if (PickupTransform != null)
                Destroy(PickupTransform.gameObject);
        }

        protected Vector3 ProjectVelocityOnPath(Vector3 velocity, Vector3 path)
        {
            return Vector3.Project(velocity, path);
        }
    }
}

