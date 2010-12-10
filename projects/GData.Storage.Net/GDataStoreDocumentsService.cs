#region File Header
/**
 * The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 * 
 * Code Author: jingnan.si@gmail.com
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Google.GData.Documents;
using System.IO;
using Google.GData.Client;
using System.Net;

namespace GData.Storage.Net
{
    class GDataStoreDocumentsService : DocumentsService
    {
        private List<string> fileExts_ = new List<string>();
        private string username_ = string.Empty;
        private string password_ = string.Empty;
        private string applicationName_ = null;

        public GDataStoreDocumentsService(string applicationName) : base(applicationName)
        {
            applicationName_ = applicationName;

            fileExts_.AddRange(new string[] {".oo", ".rtf", ".doc", ".pdf", ".txt", ".csv", ".xls", ".ods", ".ppt"});
        }

        public new void setUserCredentials(string username, string password)
        {
            username_ = username;
            password_ = password;

            base.setUserCredentials(username, password);
        }

        public Stream DownloadDocument(string documentName)
        {
            DocumentsListQuery query = new DocumentsListQuery();
            query.Title = documentName;
            query.TitleExact = true;

            DocumentsFeed feed = Query(query);

            if (feed.Entries.Count == 0)
                throw new ApplicationException("Document with name:" + documentName + " is not found");

            DocumentEntry entry = feed.Entries[0] as DocumentEntry;

            return DownloadDocument(entry);
        }

        public Stream DownloadDocument(DocumentEntry entry)
        {
            string alterUri = entry.AlternateUri.Content;

            int begin = alterUri.LastIndexOf("=");

            string docId = alterUri.Substring(begin + 1);

            string url = null;

            if (entry.IsSpreadsheet)
            {
                url = "https://spreadsheets.google.com/ccc?output=xls" +
                    "&key=" +
                    docId;

                return DownloadSpreadSheet(username_, password_, url);
            }

            string filename = entry.Title.Text;

            string ext = Path.GetExtension(filename);

            if (entry.IsPresentation)
                ext = ".ppt";
            else if (entry.IsPDF)
                ext = ".pdf";
            else if (!fileExts_.Contains(ext))
            {
                ext = ".doc";
            }

            url = "https://docs.google.com/MiscCommands?command=saveasdoc&exportformat=" +
                ext.Substring(1) +
                "&docID=" + 
                docId +
                "&hl=en";

            IGDataRequest request =
                RequestFactory.CreateRequest(Google.GData.Client.GDataRequestType.Query,
                new Uri(url));
            request.Credentials = Credentials;

            request.Execute();

            return request.GetResponseStream();
        }
        
        private Stream DownloadSpreadSheet(string user, string pwd, string url)
        {
            // Create a new request to the authentication URL.    
            Uri authHandler = new Uri("https://www.google.com/accounts/ServiceLoginAuth");

            WebRequest downloadRequest = WebRequest.Create(authHandler);
            HttpWebRequest web = downloadRequest as HttpWebRequest;
            web.KeepAlive = false;

            downloadRequest.ContentType = HttpFormPost.Encoding;
            downloadRequest.Method = HttpMethods.Post;
            ASCIIEncoding encoder = new ASCIIEncoding();

            web.UserAgent =
                "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.8.1.6) Gecko/20061201 Firefox/2.0.0.6 (Ubuntu-feisty)";
            web.CookieContainer = new CookieContainer();

            // now enter the data in the stream
            string postData = GoogleAuthentication.Email + "=" + Utilities.UriEncodeUnsafe(user) + "&";
            postData += GoogleAuthentication.Password + "=" + Utilities.UriEncodeUnsafe(pwd) + "&";
            postData += GoogleAuthentication.Source + "=" + Utilities.UriEncodeUnsafe(applicationName_) + "&";
            postData += GoogleAuthentication.Service + "=" + Utilities.UriEncodeUnsafe("wise") + "&";
            postData += "continue" + "=" + Utilities.UriEncodeUnsafe(url) + "&";
            postData += "followup" + "=" + Utilities.UriEncodeUnsafe(url) + "&";

            byte[] encodedData = encoder.GetBytes(postData);
            downloadRequest.ContentLength = encodedData.Length;

            Stream requestStream = downloadRequest.GetRequestStream();
            requestStream.Write(encodedData, 0, encodedData.Length);
            requestStream.Close();
            WebResponse downloadResponse = downloadRequest.GetResponse();

            return downloadResponse.GetResponseStream();
        }
    }

}
