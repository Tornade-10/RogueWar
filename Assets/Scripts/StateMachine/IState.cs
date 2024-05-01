using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnUpdate();
    void OnEnter();
    void OnExit();
}
