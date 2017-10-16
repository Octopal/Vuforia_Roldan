using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{
    
    public class TeleType : MonoBehaviour
    {
        private TMP_Text m_textMeshPro;
        private bool finished = false;

        void Awake()
        {
            // Get Reference to TextMeshPro Component
            m_textMeshPro = GetComponent<TMP_Text>();
            m_textMeshPro.enableWordWrapping = true;
            m_textMeshPro.alignment = TextAlignmentOptions.Top;
        }

        void OnEnable()
        {
            StartCoroutine(Start());
        }

        IEnumerator Start()
        {

            // Force and update of the mesh to get valid information.
            m_textMeshPro.ForceMeshUpdate();


            int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
            int counter = 0;
            int visibleCount = 0;

            while (counter < 12)
            {
                visibleCount = counter % (totalVisibleCharacters + 1);

                m_textMeshPro.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

                counter += 1;

                yield return new WaitForSeconds(0.1f);
            }
            
            //Debug.Log("Done revealing the text.");
        }

    }
}