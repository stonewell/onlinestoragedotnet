using System;
using System.Collections.Generic;
using System.Text;
using OnlineStorage.Net;
using System.Net;
using System.IO;

namespace TestOnlineStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IStore store = StoreFactory.CreateStore("gdatastore");

                store.Login("jingnan.si@gmail.com", "");

                //WebRequest request =
                //    HttpWebRequest.Create(@"https://docs.google.com/MiscCommands?command=saveasdoc&exportformat=txt&docID=dd6p5pmn_2hgnwsdf2&hl=en");

                //request.Credentials = new NetworkCredential("jingnan.si@gmail.com", "");

                //WebResponse res = request.GetResponse();

                //Stream s = res.GetResponseStream();
                Stream s = store.DownloadFile("Test Presentation");

                byte[] buf = new byte[4096];

                int count = s.Read(buf, 0, buf.Length);

                FileStream fs = new FileStream("d:\\Test Presentation.ppt", FileMode.Create, FileAccess.Write);

                while (count > 0)
                {
                    fs.Write(buf, 0, count);

                    count = s.Read(buf, 0, buf.Length);
                }

                fs.Flush();
                fs.Close();

                store.GetStoredFiles();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }
    }
}
