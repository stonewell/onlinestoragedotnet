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
using OnlineStorage.Net;
using Google.GData.Documents;
using Google.GData.Client;
using System.Diagnostics;

namespace GData.Storage.Net
{
    public class GDataStore : IStore
    {
        #region Fields
        //Keeps track of our logged in state.
        private bool loggedIn_ = false;
        //A connection with the DocList API.
        private GDataStoreDocumentsService service_ = null;
        #endregion

        #region Constructors
        #endregion

        #region IStore Members

        public bool Login(string username, string password)
        {
            if (loggedIn_)
            {
                return loggedIn_;
            }

            try
            {
                service_ = new GDataStoreDocumentsService("GDataStore.Storage.Net");
                ((GDataRequestFactory)service_.RequestFactory).KeepAlive = false;
                service_.setUserCredentials(username, password);
                //force the service to authenticate
                DocumentsListQuery query = new DocumentsListQuery();
                query.NumberToRetrieve = 1;
                service_.Query(query);
                loggedIn_ = true;

                return loggedIn_;
            }
            catch (AuthenticationException)
            {
                loggedIn_ = false;
                service_ = null;
                throw;
            }
        }

        public void Logout()
        {
            if (loggedIn_)
            {
                service_ = null;
            }

            loggedIn_ = false;
        }

        public IStoreFileInfo[] GetStoredFiles()
        {
            if (!loggedIn_)
            {
                throw new ApplicationException("call login first");
            }

            DocumentsListQuery query = new DocumentsListQuery();

            DocumentsFeed feed = service_.Query(query);

            foreach (DocumentEntry entry in feed.Entries)
            {
            }

            return null;
        }

        public IStoreFileInfo UploadFile(string filepath, string onlineFileName)
        {
            if (!loggedIn_)
            {
                throw new ApplicationException("call login first");
            }

            return new GDataStoreFileInfo(service_.UploadDocument(filepath, onlineFileName));
        }

        public System.IO.Stream DownloadFile(string onlineFileName)
        {
            if (!loggedIn_)
            {
                throw new ApplicationException("call login first");
            }

            return service_.DownloadDocument(onlineFileName);
        }

        #endregion
    }
}
