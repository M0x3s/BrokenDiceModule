using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class Template : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   public KMSelectable upArrow;
   public KMSelectable rightArrow;
   public KMSelectable downArrow;
   public KMSelectable leftArrow;
   public GameObject die;
   public List<GameObject> XArms;
   public List<GameObject> XClaws;
   public List<GameObject> YArms;
   public List<GameObject> YClaws;
   static int ModuleIdCounter = 1;
   int ModuleId;
   private Quaternion currentRotation = Quaternion.identity;
   private float rotationAnimationTime = 0.5f;
   private bool rotating = false;
   private bool armsMoving = false;
   private bool ModuleSolved;

   void Awake () {
      ModuleId = ModuleIdCounter++;
      upArrow.OnInteract += delegate () {StartCoroutine("turnCube", "up"); return false;};
      rightArrow.OnInteract += delegate () {StartCoroutine("turnCube", "right"); return false;};
      leftArrow.OnInteract += delegate () {StartCoroutine("turnCube", "left"); return false;};
      downArrow.OnInteract += delegate () {StartCoroutine("turnCube", "down"); return false;};
      /*
      foreach (KMSelectable object in keypad) {
          object.OnInteract += delegate () { keypadPress(object); return false; };
      }
      */

      //button.OnInteract += delegate () { buttonPress(); return false; };
      
   }

   void Start () {
      ;
   }
   IEnumerator turnCube(string direction) {
      if (rotating || armsMoving) {
         yield break;
      }
      rotating = true;
      StartCoroutine("openArms", direction);
      List<GameObject> currClaws;
      if (direction == "right" || direction == "left") {
         currClaws = YClaws;
      }
      else {
         currClaws = XClaws;
      }

        Vector3 axis;
        switch (direction)
        {
            case "up":
                axis = Vector3.right;
                break;
            case "down":
                axis = -Vector3.right;
                break;
            case "right":
                axis = -Vector3.forward;
                break;
            default: //Left
                axis = Vector3.forward;
                break;
        }

        axis *= 90;

        Quaternion fromAngle = die.transform.localRotation;
        currentRotation = Quaternion.Euler(axis) * currentRotation;

        for (var t = 0f; t < 1; t += Time.deltaTime / rotationAnimationTime)
        {
            die.transform.localRotation = Quaternion.Lerp(fromAngle, currentRotation, t);
            foreach (GameObject i in currClaws) {
               i.transform.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(axis), t);
            }
            yield return null;
        }

        die.transform.localRotation = currentRotation;
        foreach (GameObject i in currClaws) {
               i.transform.localRotation = Quaternion.identity;
            }
        rotating = false;
      
   }
   IEnumerator openArms(string direction) {
      armsMoving = true;
      List<GameObject> currArms;
      if (direction == "right" || direction == "left") {
         currArms = XArms;
      }
      else {
         currArms = YArms;
      }
		
      GameObject arm1 = currArms[0];
      GameObject arm2 = currArms[1];
      Vector3 arm1Start = arm1.transform.localPosition;
      Vector3 arm1End = Vector3.Scale(arm1.transform.localPosition, new Vector3(1.5f, 1f, 1.5f));
      Vector3 arm2Start = arm2.transform.localPosition;
      Vector3 arm2End = Vector3.Scale(arm2.transform.localPosition, new Vector3(1.5f, 1f, 1.5f));
      
      for (var t = 0f; t < 1; t += Time.deltaTime / (rotationAnimationTime*0.5f))
        {
            arm1.transform.localPosition = Vector3.Lerp(arm1Start, arm1End, t);
            arm2.transform.localPosition = Vector3.Lerp(arm2Start, arm2End, t);
            yield return null;
        }
      for (var t = 0f; t < 1; t += Time.deltaTime / (rotationAnimationTime*0.5f))
        {
            arm1.transform.localPosition = Vector3.Lerp(arm1End, arm1Start, t);
            arm2.transform.localPosition = Vector3.Lerp(arm2End, arm2Start, t);
            yield return null;
        }
        arm1.transform.localPosition = arm1Start;
        arm2.transform.localPosition = arm2Start;
      armsMoving = false;
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
   }

   IEnumerator TwitchHandleForcedSolve () {
      yield return null;
   }
}
