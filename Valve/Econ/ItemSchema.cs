﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Valve.Econ
{
    public enum FetchSchemaResult
    {
        FILE_EXIST,
        FAIL_INVALID_APIKEY,
        FAIL_ACCESS,
        SUCCESS
    }

    public class ItemSchema
    {
        private const string VERIFY_APIKEY_URL = "https://api.steampowered.com/IEconItems_{0}/GetSchemaURL/v1/?key={1}";

        public string Key { get; set; }
        public string Language { get; set; } = "en";
        public int AppID { get; set;  }
        public string SchemaURL { get; set; } = "https://api.steampowered.com/IEconItems_{0}/GetSchema/v1/?key={1}&language={2}&format=xml";
        public string SchemaFile { get; private set; }

        public string GetSchemaURL()
        {
            return String.Format(SchemaURL, AppID, Key, Language);
        }

        public string GetValidateURL()
        {
            return String.Format(VERIFY_APIKEY_URL, AppID, Key);
        }

        public bool Register(string api_key, string lang = "en")
        {
            using (var wc = new WebClient())
            {
                try
                {
                    wc.Encoding = Encoding.UTF8;
                    string content = wc.DownloadString(GetValidateURL());
                    if (!content.Contains("Forbidden"))
                    {
                        Key = api_key;
                        Language = lang;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (WebException)
                {
                    return false;
                }
            }
        }

        //Fetch And Version Check
        public async Task<FetchSchemaResult> FetchSchema(string directory, string file = "{LANGUAGE}.item.schama.xml")
        {
            if (Key == null)
            {
                return FetchSchemaResult.FAIL_INVALID_APIKEY;
            }

            if (Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            SchemaFile = Path.GetFullPath(Path.Combine(directory, file.Replace("{LANGUAGE}",Language)));

            //File not Exist (full or one)
            if (!File.Exists(SchemaFile))
            {
                using (var wc = new WebClient())
                {
                    try
                    {
                        await wc.DownloadFileTaskAsync(GetSchemaURL(), SchemaFile);
                        return FetchSchemaResult.SUCCESS;
                    }
                    catch (System.Net.WebException)
                    {
                        return FetchSchemaResult.FAIL_ACCESS;
                    }
                }
            }
            else
            {
                try
                {
                    //Get Current Version
                    string version = null;

                    XmlDocument doc = new XmlDocument();
                    doc.Load(SchemaFile);
                    if (doc.DocumentElement["items_game_url"] != null)
                    {
                        version = doc.DocumentElement["items_game_url"].InnerText;
                    }

                    //Check Version
                    if (version != null)
                    {
                        using (var wc = new WebClient())
                        {
                            wc.Encoding = Encoding.UTF8;
                            string content = wc.DownloadString(String.Format(VERIFY_APIKEY_URL, Key));
                            if (content.Contains("Forbidden"))
                            {
                                return FetchSchemaResult.FAIL_INVALID_APIKEY;
                            }

                            doc.Load(content);
                            if (doc.DocumentElement["items_game_url"] != null)
                            {
                                string webversion = doc.DocumentElement["items_game_url"].InnerText;
                                if (!version.Equals(webversion))
                                {
                                    version = webversion;
                                    File.Delete(SchemaFile);
                                    await wc.DownloadFileTaskAsync(SchemaURL, SchemaFile);
                                    return FetchSchemaResult.SUCCESS;
                                }
                                else
                                {
                                    return FetchSchemaResult.FILE_EXIST;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    return FetchSchemaResult.FAIL_ACCESS;
                }
            }
            return FetchSchemaResult.FILE_EXIST;
        }

        public async Task ReadSchema()
        {
            await Task.Factory.StartNew(delegate
            {
                XmlDocument schema = new XmlDocument();
                //schema.Load(schema_items_dir);
            });
        }
    }
}
