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
using System.Configuration;

namespace OnlineStorage.Net
{
    public sealed class StoreFactory
    {
        #region Constructors
        private StoreFactory()
        {
        }

        static StoreFactory()
        {
        }
        #endregion

        #region Methods
        public static IStore CreateStore(string name, params object[] parameters)
        {
            try
            {
                Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                string fullTypeName = name;

                if (config != null)
                {
                    foreach (string key in config.AppSettings.Settings.AllKeys)
                    {
                        if (string.Compare(key, name) == 0)
                        {
                            fullTypeName = config.AppSettings.Settings[key].Value;
                            break;
                        }
                    }
                }

                Type t = Type.GetType(fullTypeName);

                if (t == null)
                {
                    throw new StoreNotFoundException(name);
                }

                object instance =
                    t.Assembly.CreateInstance(t.FullName,
                        false, System.Reflection.BindingFlags.CreateInstance,
                        null, parameters, null, null);

                if (instance is IStore)
                {
                    return instance as IStore;
                }
                else if (instance != null)
                {
                    throw new Exception("Need IStore type, but found:" +
                        instance.GetType().AssemblyQualifiedName);
                }
                else
                {
                    throw new StoreNotFoundException(name);
                }
            }
            catch (StoreNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new StoreNotFoundException(name, ex);
            }
        }
        #endregion
    }
}
