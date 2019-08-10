using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace URLShortner.Models
{
    public class bitly
    {

        private string loginAccount;
        private string apiKeyForAccount;
        private string submitPath = @"http://api.bit.ly/shorten?version=2.0.1&format=xml";
        private int errorStatus = 0;
        private string errorMessage = "";


        /// <summary>
        /// Default constructor which will login with demo credentials
        /// </summary>
        /// <returns>A bitly class object</returns>
        public bitly()
            : this("o_mofttooh8", "R_d59bfb41308d4819a77be9151b929ce3")
        {

        }


        /// <summary>
        /// Overloaded constructor that takes a bit.ly login and apikey (if applicable)
        /// </summary>
        /// <returns>A bitly class object</returns>
        public bitly(string login, string APIKey)
        {
            loginAccount = login;
            apiKeyForAccount = APIKey;

            submitPath += "&login=" + loginAccount + "&apiKey=" + apiKeyForAccount;
        }


        // Properties to retrieve error information.
        public int ErrorCode
        {
            get { return errorStatus; }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
        }


        /// <summary>
        /// Shortens a provided URL
        /// </summary>
        /// <param name="url">A URL</param>
        /// <returns>A shortened bit.ly URL String</returns>
        public string shorten(string url)
        {
            errorStatus = 0;
            errorMessage = "";

            XmlDocument doc = buildDocument(url);

            if (doc.DocumentElement != null)
            {
                XmlNode shortenedNode = doc.DocumentElement.SelectSingleNode("results/nodeKeyVal/shortUrl");

                if (shortenedNode != null)
                {
                    return shortenedNode.InnerText;
                }
                else
                {
                    errorCode(doc);
                }
            }
            else
            {
                this.errorStatus = -1;
                this.errorMessage = "Unable to connect to bit.ly for shortening of URL";
            }

            return "";

        }


        // Sets error code and message in the situation we receive a response, but there was
        // something wrong with our submission.
        private void errorCode(XmlDocument doc)
        {
            XmlNode errorNode = doc.DocumentElement.SelectSingleNode("errorCode");
            XmlNode errorMessage = doc.DocumentElement.SelectSingleNode("errorMessage");

            if (errorNode != null)
            {
                this.errorStatus = Convert.ToInt32(errorNode.InnerText);
                this.errorMessage = errorMessage.InnerText;
            }
        }


        // Builds an XmlDocument using the XML returned by bit.ly in response 
        // to our URL being submitted
        private XmlDocument buildDocument(string url)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                // Load the XML response into an XML Document and return it.
                doc.LoadXml(readSource(submitPath + "&longUrl=" + url));
                return doc;
            }
            catch (Exception e)
            {
                return new XmlDocument();
            }
        }


        // Fetches a result from bit.ly provided the URL submitted
        private string readSource(string url)
        {
            WebClient client = new WebClient();

            try
            {
                using (StreamReader reader = new StreamReader(client.OpenRead(url)))
                {
                    // Read all of the response
                    return reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}