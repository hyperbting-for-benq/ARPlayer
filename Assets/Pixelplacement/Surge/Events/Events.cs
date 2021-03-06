/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pixelplacement
{
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    #if SURGE_COLLIDER_BUTTON
    // [System.Serializable]
    // public class ColliderButtonEvent : UnityEvent<ColliderButton> { }
    #endif
    
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
}