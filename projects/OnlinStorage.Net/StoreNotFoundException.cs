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

namespace OnlineStorage.Net
{
    public class StoreNotFoundException : ApplicationException
    {
        public StoreNotFoundException(string name) : base("Store with name:" + name + " is not found")
        {
        }

        public StoreNotFoundException(string name, Exception innerException) : base("Store with name:" + name + " is not found", innerException)
        {
        }
    }
}
