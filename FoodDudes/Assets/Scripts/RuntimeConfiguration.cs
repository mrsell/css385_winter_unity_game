using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script contains public fields used as configuration parameters during runtime.
 * For example, the Main Menu might set certain values here that are used by other scenes
 * to allow for "global" configuration setting made from the main menu.
 * 
 * Yes - this uses static, public-accessible fields. This is an absolutely horrible
 * design practice. It'll work for a three-person team, though.
 * 
*/
public class RuntimeConfiguration : MonoBehaviour {

	// should tutorial dialogs be displayed during play?
	static public bool displayTutorialDialogs = false;

}
