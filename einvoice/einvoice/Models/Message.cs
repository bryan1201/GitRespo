using einvoice.Models.eInvoiceMessage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using einvoice.Models;
using System.Net;
using System.Text;

namespace einvoice.Models
{
    public class Message: Models.IRawData
    {
        void IRawData.SaveRawData(A0101 Invoice, string filename)
        {
            throw new NotImplementedException();
        }

        public string FtpRawData(string filename)
        {
            /*
                Id: einvoiceftpuser
                pwd: Iec+123
            */
            string userName = Constant.S_eInvoiceFTPUser;
            string password = Constant.S_eInvoiceFTPPWD;
            string rslt = string.Empty;
            try
            {
                string shortfilename = Path.GetFileName(filename);
                string uploadUrl = string.Format("{0}{1}{2}", Constant.S_eInvoiceFTPServer, Constant.S_eInoviceFTPA0101, shortfilename);

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filename);

                string xml = xmlDocument.OuterXml;
                byte[] data = Encoding.UTF8.GetBytes(xml);

                WebClient wc = new WebClient();
                wc.Credentials = new NetworkCredential(userName, password);
                wc.UploadData(uploadUrl, data);
            }
            catch(Exception ex)
            {
                rslt = ex.Message;
            }
            return rslt;
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }
            
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }

            return objectOut;
        }
    }
}