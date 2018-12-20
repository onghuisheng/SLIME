using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable {

    /// <summary>
    /// Called when an arrow hit this object
    /// </summary>
    void OnShot(ArrowBase arrow);

}