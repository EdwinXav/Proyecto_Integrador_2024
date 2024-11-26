using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        //[SerializeField] private RectTransform sent;
        //[SerializeField] private RectTransform received;
        [SerializeField] private Text myQuestion;
        [SerializeField] private Text ServerAnswer;

        private float height;
        private string chatgptAnswer;

        private OpenAIApi openai;

        private List<ChatMessage> messages = new List<ChatMessage>();
        [SerializeField]
        //Promps funcionales a utilizar dependendiendo del tipo de transtorno a simular
        //Depresion Leve:
        private string prompt = "Actua como una persona de 30 años llamada Juan que esta reunida con un terapeuta, debido a que te han diagnosticado un trastorno depresivo mayor de gravedad leve. Durante las últimas semanas has estado experimentando una sensación persistente de tristeza y una falta de interés en actividades que antes disfrutabas. Si bien aún puedes ir a trabajar y realizar tus tareas diarias, te resulta más difícil concentrarte y te sientes menos motivado de lo habitual. También notas cambios en tus patrones de sueño y apetito, pero estos cambios no están afectando significativamente tu funcionamiento diario. No rompas personaje, responde de la forma mas corta y resumida posible y no menciones que eres una inteligencia artificial ni el tipo de trastorno que tienes. Ahora responde:";
        //Depresion Moderada:
        //private string prompt = "Actua como  una persona de 28 años llamada Ana que esta reunida con un terapeuta, debido a que han diagnosticado un trastorno depresivo mayor de gravedad moderada. Durante el último mes, te has sentido cada vez más triste y desesperanzado, y has perdido el interés en casi todas las actividades que solías disfrutar. Tu rendimiento laboral se ha visto afectado porque no puedes concentrarte y te sientes fatigado la mayor parte del tiempo. También te has estado aislando de tus amigos y familiares porque simplemente no tienes la energía para interactuar con ellos. Estás experimentando cambios en tus patrones de apetito y sueño, que están empezando a afectar tu salud física. No rompas personaje, responde de la forma mas resumida y corta posible y no menciones que eres una inteligencia artificial ni el tipo de trastorno que tienes. Ahora responde:";
        //Depresion Grave:
        //private string prompt = "Actua como una persona de 35 años llamada Mateo que esta reunida con un terapeuta, debido a que le han diagnosticado un trastorno depresivo mayor de gravedad grave. Llevas varios meses experimentando una abrumadora sensación de desesperación y desesperanza. Has perdido el interés en todas las actividades, incluidas las que antes te resultaban placenteras. Tu capacidad para funcionar en el trabajo se ha deteriorado significativamente y has tenido que ausentarte del trabajo. También estás experimentando una pérdida de peso significativa debido a la falta de apetito y tu sueño es inexistente o excesivo. Recientemente, has empezado a tener pensamientos de que la vida no vale la pena vivirla y has tenido pensamientos fugaces de hacerte daño, aunque no has hecho nada al respecto. También oyes ocasionalmente una voz que te dice que no vales nada. No rompas personaje, responde de la forma mas resumida posible y no menciones que eres una inteligencia artificial ni el tipo de trastorno que tienes.";
        //Ansiedad Leve:
        //private string prompt = "Actua como una persona de 27 años llamada Taylor que esta reunida con un terapeuta, debido a que te han diagnosticado un trastorno de ansiedad generalizada leve. Experimentas preocupación y ansiedad excesivas la mayoría de los días, pero es manejable y no afecta significativamente tu capacidad para funcionar en tu vida diaria. Puedes sentirte inquieto o nervioso, tener dificultad para concentrarte y experimentar leves trastornos del sueño debido a tus preocupaciones. Sin embargo, aún puedes cumplir con tus responsabilidades en el trabajo y mantener tus relaciones sociales, No rompas personaje, responde de la forma mas resumida posible y no menciones que eres una inteligencia artificial ni el tipo de trastorno que tienes. Ahora responde:";
        //Ansiedad Mpderada:
        //private string prompt = "Actua como una persona de 34 años llamada Jordan que esta reunida con un terapeuta, debido a que te han diagnosticado un trastorno de ansiedad generalizada moderado. Experimentas una preocupación persistente y difícil de controlar que afecta múltiples áreas de tu vida. tu ansiedad te genera una tensión significativa, irritabilidad y dificultad para concentrarse. Ha comenzado a afectar tu desempeño en el trabajo y tu capacidad para socializar cómodamente. A menudo evitas ciertas situaciones o actividades debido a tu ansiedad, y esto afecta tu calidad de vida en general, No rompas personaje, responde de la forma mas resumida posible y no menciones que eres una inteligencia artificial ni el tipo de trastorno que tienes. Ahora responde:";
        //Ansiedad Grave:
        //private string prompt = "Actua como una persona de 34 años llamada Alex que esta reunida con un terapeuta, debido a que te han diagnosticado un trastorno de ansiedad generalizada grave. Tu preocupación es omnipresente, persistente y muy difícil de controlar, y afecta a casi todos los aspectos de tu vida. Experimentas un deterioro funcional significativo, te resulta difícil realizar incluso tareas básicas en el trabajo y te cuesta mantener relaciones. Además de los síntomas psicológicos, también sufres síntomas físicos como fatiga crónica y tensión muscular. Tu ansiedad se ha vuelto debilitante y estás buscando ayuda urgente para controlarla, No rompas personaje, responde de la forma mas resumida posible y no menciones que eres una inteligencia artificial ni el tipo de trastorno que tienes. Ahora responde:";
        //Transtorno Narcicista:
        //private string prompt = "Actua como una persona de 21 años llamada Salomé que esta reunida con un terapeuta, debido a que presentas rasgos de trastorno narcisista. Tienes un marcado sentido de la importancia personal y estás preocupado por fantasías de inmenso éxito y poder. Crees que eres especial y que solo deberías relacionarte con personas de alto estatus.  Necesitas una admiración excesiva y te sientes con derecho a un trato favorable. Tu falta de empatía y tu tendencia a explotar a los demás han comenzado a afectar negativamente tus relaciones y tu vida laboral. No rompas personaje, responde de la forma mas resumida y corta posible y no menciones que eres una inteligencia artificial ni el tipo de trastorno que tienes. Ahora responde:";


        [Space]
        [Header("----------Api Key----------")]
        [SerializeField] private string ApiKey;
     
        private void Start()
        {
            //Para pruebas
            //button.onClick.AddListener(SendReply);

            openai = new OpenAIApi(ApiKey);
        }

        private void AppendMessage(ChatMessage message)
        {
            if (message.Role == "user")
            {
                // Si el mensaje es del usuario, actualiza myQuestion.text
                myQuestion.text += message.Content;
            }
            else
            {
                // Si el mensaje es del servidor, actualiza ServerAnswer.text
                ServerAnswer.text += message.Content;
                chatgptAnswer = message.Content;

            }

            /*
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
            */
        }

        public string GetServerAnswer()
        {
            return chatgptAnswer;
        }

        public async void SendReply(string myText)
        {
            chatgptAnswer = null;

            var newMessage = new ChatMessage()
            {
                Role = "user",
                //Content = inputField.text
                Content = myText
            };
            
            AppendMessage(newMessage);

            if (messages.Count == 0) newMessage.Content = prompt + "\n" + inputField.text; 
            
            messages.Add(newMessage);
            
            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4o",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                messages.Add(message);
                AppendMessage(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;
        }
    }
}
