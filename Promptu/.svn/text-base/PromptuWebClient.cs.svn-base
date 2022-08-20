using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace ZachJohnson.Promptu
{
    internal class PromptuWebClient : WebClient
    {
        private string userAgent;

        public PromptuWebClient(string userAgent)
        {
            this.userAgent = userAgent;
        }

        public String UserAgent
        {
            get 
            { 
                return this.userAgent; 
            }

            set
            {
                this.userAgent = value;
            }
        }

        private void UpdateUserAgent()
        {
            this.Headers.Remove("User-Agent");
            string userAgent = this.userAgent;

            if (userAgent != null)
            {
                this.Headers.Add("User-Agent", this.userAgent);
            }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            this.UpdateUserAgent();
            return base.GetWebRequest(address);
        }
    }
}
