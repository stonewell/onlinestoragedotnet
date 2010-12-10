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

namespace GData.Storage.Net
{
    public class GDataStoreFileInfo : IStoreFileInfo
    {
        #region Fields
        private DocumentEntry entry_ = null;
        #endregion

        #region Constructors
        internal GDataStoreFileInfo(DocumentEntry entry)
        {
            entry_ = entry;
        }
        #endregion

        #region IStoreFileInfo Members

        public string Name
        {
            get { return entry_.Title.Text; }
        }

        public DateTime LastUpdated
        {
            get { return entry_.Updated; }
        }

        #endregion
    }
}
