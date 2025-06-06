//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Input
    {
        
        public static SteamVR_Action_Boolean @__actions_default_in_InteractUI;
        
        public static SteamVR_Action_Boolean @__actions_default_in_Teleport;
        
        public static SteamVR_Action_Boolean @__actions_default_in_GrabPinch;
        
        public static SteamVR_Action_Boolean @__actions_default_in_GrabGrip;
        
        public static SteamVR_Action_Pose @__actions_default_in_Pose;
        
        public static SteamVR_Action_Skeleton @__actions_default_in_SkeletonLeftHand;
        
        public static SteamVR_Action_Skeleton @__actions_default_in_SkeletonRightHand;
        
        public static SteamVR_Action_Single @__actions_default_in_Squeeze;
        
        public static SteamVR_Action_Boolean @__actions_default_in_HeadsetOnHead;
        
        public static SteamVR_Action_Vibration @__actions_default_out_Haptic;
        
        public static SteamVR_Action_Vector2 @__actions_platformer_in_Move;
        
        public static SteamVR_Action_Boolean @__actions_platformer_in_Jump;
        
        public static SteamVR_Action_Vector2 @__actions_buggy_in_Steering;
        
        public static SteamVR_Action_Single @__actions_buggy_in_Throttle;
        
        public static SteamVR_Action_Boolean @__actions_buggy_in_Brake;
        
        public static SteamVR_Action_Boolean @__actions_buggy_in_Reset;
        
        public static SteamVR_Action_Pose @__actions_mixedreality_in_ExternalCamera;
        
        public static void Dynamic_InitializeActions()
        {
            SteamVR_Input.@__actions_default_in_InteractUI.Initialize();
            SteamVR_Input.@__actions_default_in_Teleport.Initialize();
            SteamVR_Input.@__actions_default_in_GrabPinch.Initialize();
            SteamVR_Input.@__actions_default_in_GrabGrip.Initialize();
            SteamVR_Input.@__actions_default_in_Pose.Initialize();
            SteamVR_Input.@__actions_default_in_SkeletonLeftHand.Initialize();
            SteamVR_Input.@__actions_default_in_SkeletonRightHand.Initialize();
            SteamVR_Input.@__actions_default_in_Squeeze.Initialize();
            SteamVR_Input.@__actions_default_in_HeadsetOnHead.Initialize();
            SteamVR_Input.@__actions_default_out_Haptic.Initialize();
            SteamVR_Input.@__actions_platformer_in_Move.Initialize();
            SteamVR_Input.@__actions_platformer_in_Jump.Initialize();
            SteamVR_Input.@__actions_buggy_in_Steering.Initialize();
            SteamVR_Input.@__actions_buggy_in_Throttle.Initialize();
            SteamVR_Input.@__actions_buggy_in_Brake.Initialize();
            SteamVR_Input.@__actions_buggy_in_Reset.Initialize();
            SteamVR_Input.@__actions_mixedreality_in_ExternalCamera.Initialize();
        }
        
        public static void Dynamic_InitializeInstanceActions()
        {
            Valve.VR.SteamVR_Input.@__actions_default_in_InteractUI = ((SteamVR_Action_Boolean)(SteamVR_Input_References.GetAction("__actions_default_in_InteractUI")));
            Valve.VR.SteamVR_Input.@__actions_default_in_Teleport = ((SteamVR_Action_Boolean)(SteamVR_Input_References.GetAction("__actions_default_in_Teleport")));
            Valve.VR.SteamVR_Input.@__actions_default_in_GrabPinch = ((SteamVR_Action_Boolean)(SteamVR_Input_References.GetAction("__actions_default_in_GrabPinch")));
            Valve.VR.SteamVR_Input.@__actions_default_in_GrabGrip = ((SteamVR_Action_Boolean)(SteamVR_Input_References.GetAction("__actions_default_in_GrabGrip")));
            Valve.VR.SteamVR_Input.@__actions_default_in_Pose = ((SteamVR_Action_Pose)(SteamVR_Input_References.GetAction("__actions_default_in_Pose")));
            Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonLeftHand = ((SteamVR_Action_Skeleton)(SteamVR_Input_References.GetAction("__actions_default_in_SkeletonLeftHand")));
            Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonRightHand = ((SteamVR_Action_Skeleton)(SteamVR_Input_References.GetAction("__actions_default_in_SkeletonRightHand")));
            Valve.VR.SteamVR_Input.@__actions_default_in_Squeeze = ((SteamVR_Action_Single)(SteamVR_Input_References.GetAction("__actions_default_in_Squeeze")));
            Valve.VR.SteamVR_Input.@__actions_default_in_HeadsetOnHead = ((SteamVR_Action_Boolean)(SteamVR_Input_References.GetAction("__actions_default_in_HeadsetOnHead")));
            Valve.VR.SteamVR_Input.@__actions_default_out_Haptic = ((SteamVR_Action_Vibration)(SteamVR_Input_References.GetAction("__actions_default_out_Haptic")));
            Valve.VR.SteamVR_Input.@__actions_platformer_in_Move = ((SteamVR_Action_Vector2)(SteamVR_Input_References.GetAction("__actions_platformer_in_Move")));
            Valve.VR.SteamVR_Input.@__actions_platformer_in_Jump = ((SteamVR_Action_Boolean)(SteamVR_Input_References.GetAction("__actions_platformer_in_Jump")));
            Valve.VR.SteamVR_Input.@__actions_buggy_in_Steering = ((SteamVR_Action_Vector2)(SteamVR_Input_References.GetAction("__actions_buggy_in_Steering")));
            Valve.VR.SteamVR_Input.@__actions_buggy_in_Throttle = ((SteamVR_Action_Single)(SteamVR_Input_References.GetAction("__actions_buggy_in_Throttle")));
            Valve.VR.SteamVR_Input.@__actions_buggy_in_Brake = ((SteamVR_Action_Boolean)(SteamVR_Input_References.GetAction("__actions_buggy_in_Brake")));
            Valve.VR.SteamVR_Input.@__actions_buggy_in_Reset = ((SteamVR_Action_Boolean)(SteamVR_Input_References.GetAction("__actions_buggy_in_Reset")));
            Valve.VR.SteamVR_Input.@__actions_mixedreality_in_ExternalCamera = ((SteamVR_Action_Pose)(SteamVR_Input_References.GetAction("__actions_mixedreality_in_ExternalCamera")));
            Valve.VR.SteamVR_Input.actions = new Valve.VR.SteamVR_Action[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_InteractUI,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Teleport,
                    Valve.VR.SteamVR_Input.@__actions_default_in_GrabPinch,
                    Valve.VR.SteamVR_Input.@__actions_default_in_GrabGrip,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Pose,
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonLeftHand,
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonRightHand,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Squeeze,
                    Valve.VR.SteamVR_Input.@__actions_default_in_HeadsetOnHead,
                    Valve.VR.SteamVR_Input.@__actions_default_out_Haptic,
                    Valve.VR.SteamVR_Input.@__actions_platformer_in_Move,
                    Valve.VR.SteamVR_Input.@__actions_platformer_in_Jump,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Steering,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Throttle,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Brake,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Reset,
                    Valve.VR.SteamVR_Input.@__actions_mixedreality_in_ExternalCamera};
            Valve.VR.SteamVR_Input.actionsIn = new Valve.VR.SteamVR_Action_In[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_InteractUI,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Teleport,
                    Valve.VR.SteamVR_Input.@__actions_default_in_GrabPinch,
                    Valve.VR.SteamVR_Input.@__actions_default_in_GrabGrip,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Pose,
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonLeftHand,
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonRightHand,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Squeeze,
                    Valve.VR.SteamVR_Input.@__actions_default_in_HeadsetOnHead,
                    Valve.VR.SteamVR_Input.@__actions_platformer_in_Move,
                    Valve.VR.SteamVR_Input.@__actions_platformer_in_Jump,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Steering,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Throttle,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Brake,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Reset,
                    Valve.VR.SteamVR_Input.@__actions_mixedreality_in_ExternalCamera};
            Valve.VR.SteamVR_Input.actionsOut = new Valve.VR.SteamVR_Action_Out[] {
                    Valve.VR.SteamVR_Input.@__actions_default_out_Haptic};
            Valve.VR.SteamVR_Input.actionsVibration = new Valve.VR.SteamVR_Action_Vibration[] {
                    Valve.VR.SteamVR_Input.@__actions_default_out_Haptic};
            Valve.VR.SteamVR_Input.actionsPose = new Valve.VR.SteamVR_Action_Pose[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_Pose,
                    Valve.VR.SteamVR_Input.@__actions_mixedreality_in_ExternalCamera};
            Valve.VR.SteamVR_Input.actionsBoolean = new Valve.VR.SteamVR_Action_Boolean[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_InteractUI,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Teleport,
                    Valve.VR.SteamVR_Input.@__actions_default_in_GrabPinch,
                    Valve.VR.SteamVR_Input.@__actions_default_in_GrabGrip,
                    Valve.VR.SteamVR_Input.@__actions_default_in_HeadsetOnHead,
                    Valve.VR.SteamVR_Input.@__actions_platformer_in_Jump,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Brake,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Reset};
            Valve.VR.SteamVR_Input.actionsSingle = new Valve.VR.SteamVR_Action_Single[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_Squeeze,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Throttle};
            Valve.VR.SteamVR_Input.actionsVector2 = new Valve.VR.SteamVR_Action_Vector2[] {
                    Valve.VR.SteamVR_Input.@__actions_platformer_in_Move,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Steering};
            Valve.VR.SteamVR_Input.actionsVector3 = new Valve.VR.SteamVR_Action_Vector3[0];
            Valve.VR.SteamVR_Input.actionsSkeleton = new Valve.VR.SteamVR_Action_Skeleton[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonLeftHand,
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonRightHand};
            Valve.VR.SteamVR_Input.actionsNonPoseNonSkeletonIn = new Valve.VR.SteamVR_Action_In[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_InteractUI,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Teleport,
                    Valve.VR.SteamVR_Input.@__actions_default_in_GrabPinch,
                    Valve.VR.SteamVR_Input.@__actions_default_in_GrabGrip,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Squeeze,
                    Valve.VR.SteamVR_Input.@__actions_default_in_HeadsetOnHead,
                    Valve.VR.SteamVR_Input.@__actions_platformer_in_Move,
                    Valve.VR.SteamVR_Input.@__actions_platformer_in_Jump,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Steering,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Throttle,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Brake,
                    Valve.VR.SteamVR_Input.@__actions_buggy_in_Reset};
        }
    }
}
