using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEvents : EventArgs
{
    public GameObject PickUpEvent { get; set; }

}

public class SetUpEvents : EventArgs 
{ 
    public GameObject SetPathEvents { get; set;}
}
