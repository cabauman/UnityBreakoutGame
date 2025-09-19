using System.Collections;
using UnityEngine;

namespace BreakoutGame
{
    public class TestMono : MonoBehaviour
    {
        public void DoSomething()
        {
            StartCoroutine(DoSomething_());
        }

        private IEnumerator DoSomething_()
        {
            //yield return new WaitForSeconds(0.5f);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            Debug.Log("Did something after 0.5 seconds");
        }
    }
}