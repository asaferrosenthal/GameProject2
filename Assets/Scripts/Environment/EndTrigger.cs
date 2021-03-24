using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Environment
{

    public class EndTrigger : MonoBehaviour
    {

        public GameObject completeLevelUI;

        void OnCollisionEnter()  //IEnumerator
        {
            //yield return new WaitForSeconds(0.5f);
            //completeLevelUI.SetActive(true);
            //yield return new WaitForSeconds(1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
