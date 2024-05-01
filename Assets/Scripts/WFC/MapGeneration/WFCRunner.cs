using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCRunner : MonoBehaviour
{
    private WFCGenerator _generator;
    private WFCAnalyzer _wfcAnalyzer;
    
    // Start is called before the first frame update
    void Start()
    {
        _generator = GetComponent<WFCGenerator>();
        
        if (_generator is not null)
        {
            _generator.Initiate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_generator is not null)
        {
            _generator.Step(new List<WFCSlot>());
        }
    }
}
