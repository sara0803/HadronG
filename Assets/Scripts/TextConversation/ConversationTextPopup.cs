using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TextConversation
{
   public class ConversationTextPopup : MonoBehaviour, IPointerClickHandler
   {
      [SerializeField]
      private string[] arrayMessages;

      [SerializeField]
      private TMP_Text textMessages;

      [SerializeField]
      private float readSpeed = 0.25f;

      [Header("Event")]
      [SerializeField]
      private UnityEvent OnMessageShowFull;

      [SerializeField]
      private UnityEvent OnMessageReadFinish;

      private bool readNextMessage = true;

      private int currentIndexMessage;

      private void Start()
      {
         StartCoroutine(CouReadMessage());
      }

      private IEnumerator CouReadMessage()
      {
         while(currentIndexMessage < arrayMessages.Length)
         {
            readNextMessage = false;
            textMessages.SetText(arrayMessages[currentIndexMessage]);
            textMessages.maxVisibleWords = 0;
            var tmpWordCount = textMessages.text.Split(' ').Length;

            for(int i = 0; i <= tmpWordCount; i++)
            {
               textMessages.maxVisibleWords = i;
               yield return new WaitForSeconds(readSpeed);

               if(readNextMessage)
               {
                  textMessages.maxVisibleWords = tmpWordCount;
                  readNextMessage = false;
                  break;
               }
            }
            
            currentIndexMessage++;
            OnMessageShowFull.Invoke();

            if(currentIndexMessage < arrayMessages.Length)
            {
               while(!readNextMessage)
                  yield return null;
            }
            else
            {
               OnMessageReadFinish.Invoke();
               break;
            }
         }

         OnMessageReadFinish.Invoke();
      }

      public void OnPointerClick(PointerEventData eventData)
      {
         readNextMessage = true;
      }
   }
}