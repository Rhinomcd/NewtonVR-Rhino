﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NewtonVR_Rhino;

namespace NewtonVR_Rhino
{
    public class NVRAttachPoint : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody Rigidbody;

        [HideInInspector]
        public NVRInteractableItem Item;

        public float PositionMagic = 10f;

        public bool IsAttached;

        protected virtual void Awake()
        {
            IsAttached = false;

            Item = FindNVRItem(this.gameObject);
            if (Item == null)
            {
                Debug.LogError("No NVRInteractableItem found on this object. " + this.gameObject.name, this.gameObject);
            }

            AttachPointMapper.Register(this.GetComponent<Collider>(), this);
        }

        protected virtual void Start()
        {
            Rigidbody = Item.Rigidbody;
        }

        private NVRInteractableItem FindNVRItem(GameObject gameobject)
        {
            var item = gameobject.GetComponent<NVRInteractableItem>();

            if (item != null)
                return item;

            if (gameobject.transform.parent != null)
                return FindNVRItem(gameobject.transform.parent.gameObject);

            return null;
        }

        public virtual void Attached(NVRAttachJoint joint)
        {
            var TargetPosition = joint.transform.position + (Item.transform.position - this.transform.position);
            Rigidbody.MovePosition(TargetPosition);

            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero; 

            IsAttached = true;
            Rigidbody.useGravity = false;
        }
        public virtual void Detached(NVRAttachJoint joint)
        {
            IsAttached = false;
            Rigidbody.useGravity = true;
        }

        public virtual void PullTowards(Vector3 jointPosition)
        {
            var delta = jointPosition - this.transform.position;
            Rigidbody.AddForceAtPosition(delta * PositionMagic, this.transform.position, ForceMode.VelocityChange);
        }
    }
}