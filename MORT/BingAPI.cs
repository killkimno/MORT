using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Services.Client;
using System.Text;
using System.Net;

namespace MORT
{
    class BingAPI
    {
        public static BingAPI instance;
        static string bingAccountKey;
        static TranslatorContainer tc;
        private string transCode = "en";
        private string resultCode = "ko";


        #region:::::::::::::::::::::::::::::::::::::::::::계정키 클래스:::::::::::::::::::::::::::::::::::::::::::

        public void setBingAccountKey(string newKey)
        {
            bingAccountKey = newKey;
            tc = InitializeTranslatorContainer();
        }

        public void SetTransCode(string transCode, string resultCode)
        {
            this.transCode = transCode;
            this.resultCode = resultCode;
        }

        public partial class Translation
        {

            private String _Text;

            public String Text
            {
                get
                {
                    return this._Text;
                }
                set
                {
                    this._Text = value;
                }
            }
        }


        public partial class TranslatorContainer : System.Data.Services.Client.DataServiceContext
        {

            public TranslatorContainer(Uri serviceRoot) :
                base(serviceRoot)
            {
            }

            /// <summary>
            /// </summary>
            /// <param name="Text">the text to translate Sample Values : hello</param>
            /// <param name="To">the language code to translate the text into Sample Values : nl</param>
            /// <param name="From">the language code of the translation text Sample Values : en</param>
            public DataServiceQuery<Translation> Translate(String Text, String To, String From)
            {
                if ((Text == null))
                {
                    throw new System.ArgumentNullException("Text", "Text value cannot be null");
                }
                if ((To == null))
                {
                    throw new System.ArgumentNullException("To", "To value cannot be null");
                }
                DataServiceQuery<Translation> query;
                query = base.CreateQuery<Translation>("Translate");
                if ((Text != null))
                {
                    query = query.AddQueryOption("Text", string.Concat("\'", System.Uri.EscapeDataString(Text), "\'"));
                }
                if ((To != null))
                {
                    query = query.AddQueryOption("To", string.Concat("\'", System.Uri.EscapeDataString(To), "\'"));
                }
                if ((From != null))
                {
                    query = query.AddQueryOption("From", string.Concat("\'", System.Uri.EscapeDataString(From), "\'"));
                }
                return query;
            }

        }
        #endregion

        private static TranslatorContainer InitializeTranslatorContainer()
        {
            // this is the service root uri for the Microsoft Translator service 
            System.Uri serviceRootUri = new Uri("https://api.datamarket.azure.com/Bing/MicrosoftTranslator/");

            // this is the Account Key I generated for this app
            string accountKey = bingAccountKey;

            // throw new Exception("Invalid Account Key");

            TranslatorContainer newTc = new TranslatorContainer(serviceRootUri);
            newTc.Credentials = new NetworkCredential(accountKey, accountKey);
            return newTc;
        }
        //bing 번역기로부터 번역문 얻기
        private static Translation TranslateString(TranslatorContainer tc, string inputString, string transCode, string resultCode)
        {

            System.Data.Services.Client.DataServiceQuery<MORT.BingAPI.Translation> translationQuery = tc.Translate(inputString, resultCode, transCode);

            // Call the query and get the results as a List
            System.Collections.Generic.List<MORT.BingAPI.Translation> translationResults = translationQuery.Execute().ToList();

            // Verify there was a result
            if (translationResults.Count() <= 0)
            {
                return null;
            }

            // In case there were multiple results, pick the first one
            Translation translationResult = translationResults.First();

            return translationResult;
        }

    }
}
