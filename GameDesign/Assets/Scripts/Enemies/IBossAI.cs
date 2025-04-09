using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public interface IBossAI
{
    Coroutine StartCoroutine(IEnumerator coroutine);
}

