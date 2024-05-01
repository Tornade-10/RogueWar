using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State
{
    void OnUpdate();
    void OnEnter();
    void OnExit();
}
