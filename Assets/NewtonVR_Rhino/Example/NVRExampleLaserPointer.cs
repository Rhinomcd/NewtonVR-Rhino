﻿using UnityEngine;
using System.Collections;

namespace NewtonVR_Rhino.Example
{
    public class NVRExampleLaserPointer : MonoBehaviour
    {
        public Color LineColor;
        public float LineWidth = 0.02f;
        public bool ForceLineVisible = false;

        public bool OnlyVisibleOnTrigger = true;

        private LineRenderer Line;

        private NVRHand Hand;

        private void Awake()
        {
            Line = this.GetComponent<LineRenderer>();
            Hand = this.GetComponent<NVRHand>();

            if (Line == null)
            {
                Line = this.gameObject.AddComponent<LineRenderer>();
            }

            if (Line.sharedMaterial == null)
            {
                Line.material = new Material(Shader.Find("Unlit/Color"));
                Line.material.SetColor("_Color", LineColor);
                Line.SetColors(LineColor, LineColor);
            }

            Line.useWorldSpace = true;
        }

        private void LateUpdate()
        {
            Line.enabled = OnlyVisibleOnTrigger && Hand != null && Hand.Inputs[Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger].IsPressed;
            Debug.Log(Line.enabled);
            Debug.Log("Hand: " + Hand);
            if (Hand != null)
            {
                Debug.Log("Button Pressed:" + Hand.Inputs[Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger].IsPressed);
            
            }
            if (Line.enabled)
            {
                Line.material.SetColor("_Color", LineColor);
                Line.SetColors(LineColor, LineColor);
                Line.SetWidth(LineWidth, LineWidth);

                RaycastHit hitInfo;
                var hit = Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, 1000);
                Vector3 endPoint;

                if (hit == true)
                {
                    endPoint = hitInfo.point;
                }
                else
                {
                    endPoint = this.transform.position + (this.transform.forward * 1000f);
                }

                Line.SetPositions(new Vector3[] { this.transform.position, endPoint });
            }
        }
    }
}