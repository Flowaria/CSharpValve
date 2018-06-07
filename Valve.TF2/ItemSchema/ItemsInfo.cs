using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using TF2.Info;
using Valve.Econ;

namespace TF2.Items
{
    public enum FetchSchemaResult
    {
        FILE_EXIST,
        FAIL_INVALID_APIKEY,
        FAIL_ACCESS,
        SUCCESS
    }
    public class ItemsInfo : ItemSchema
    {
        private const string VERIFY_APIKEY_URL = "https://api.steampowered.com/IEconItems_440/GetSchemaURL/v1/?key={0}&format=xml";
        private const string SCHEMA_ITEMS_URL = "https://api.steampowered.com/IEconItems_440/GetSchemaItems/v1/?key={0}&language={1}&format=xml";
        private const string SCHEMA_ATTRIBUTES_URL = "https://api.steampowered.com/IEconItems_440/GetSchemaOverview/v1/?key={0}&language={1}&format=xml";
        private static List<TFItem> items = new List<TFItem>();
        private static List<TFAttribute> attributes = new List<TFAttribute>();
        private static string schema_items_dir;
        private static string schema_attributes_dir;

        public static string Apikey { get { return apikey; } }
        public static string Language { get { return language; } }
        public static string Version { get { return version; } }

        public static Func<XmlNode, TFItem> ItemsHandler = null;
        public static Func<XmlNode, TFAttribute> AttributesHandler = null;

        public static TFItem[] Items
        {
            get
            {
                return items.ToArray();
            }
        }

        public static TFAttribute[] Attributes
        {
            get
            {
                return attributes.ToArray();
            }
        }

        public static bool Register(string api_key, string lang = "en")
        {
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                try
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    string content = wc.DownloadString(String.Format(VERIFY_APIKEY_URL, api_key));
                    if (!content.Contains("Forbidden"))
                    {
                        apikey = api_key;
                        language = lang;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (System.Net.WebException)
                {
                    return false;
                }
            }
        }

        //Fetch And Version Check
        public static async Task<FetchSchemaResult> FetchSchema(string directory)
        {
            if(apikey == null)
            {
                return FetchSchemaResult.FAIL_INVALID_APIKEY;
            }

            if(Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string schemaurl = String.Format(SCHEMA_ITEMS_URL, apikey, language);
            schema_items_dir = Path.Combine(directory, language + ".items.schama.xml");
            schema_attributes_dir = Path.Combine(directory, language + ".attributes.schama.xml");
                
            //File not Exist (full or one)
            if(!File.Exists(schema_items_dir) || !File.Exists(schema_attributes_dir))
            {
                File.Delete(schema_items_dir);
                File.Delete(schema_attributes_dir);
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    try
                    {
                        await wc.DownloadFileTaskAsync(String.Format(SCHEMA_ITEMS_URL, apikey, language), schema_items_dir);
                        await wc.DownloadFileTaskAsync(String.Format(SCHEMA_ATTRIBUTES_URL, apikey, language), schema_attributes_dir);
                        return FetchSchemaResult.SUCCESS;
                    }
                    catch(System.Net.WebException)
                    {
                        return FetchSchemaResult.FAIL_ACCESS;
                    }
                }
            }
            //All File Exist
            else
            {
                try
                {
                    //Get Current Version
                    XmlDocument doc = new XmlDocument();
                    doc.Load(schema_attributes_dir);
                    if(doc.DocumentElement["items_game_url"] != null)
                    {
                        version = UrlToVersionString(doc.DocumentElement["items_game_url"].InnerText);
                    }

                    //Check Version
                    if(version != null)
                    {
                        using (System.Net.WebClient wc = new System.Net.WebClient())
                        {
                            wc.Encoding = System.Text.Encoding.UTF8;
                            string content = wc.DownloadString(String.Format(VERIFY_APIKEY_URL, apikey));
                            if (!content.Contains("Forbidden"))
                            {
                                doc.Load(content);
                                if (doc.DocumentElement["items_game_url"] != null)
                                {
                                    string webversion = UrlToVersionString(doc.DocumentElement["items_game_url"].InnerText);
                                    if (!version.Equals(webversion))
                                    {
                                        version = webversion;
                                        File.Delete(schema_items_dir);
                                        File.Delete(schema_attributes_dir);
                                        await wc.DownloadFileTaskAsync(String.Format(SCHEMA_ITEMS_URL, apikey, language), schema_items_dir);
                                        await wc.DownloadFileTaskAsync(String.Format(SCHEMA_ATTRIBUTES_URL, apikey, language), schema_attributes_dir);
                                        return FetchSchemaResult.SUCCESS;
                                    }
                                    else
                                    {
                                        return FetchSchemaResult.FILE_EXIST;
                                    }
                                } 
                            }
                            else
                            {
                                return FetchSchemaResult.FAIL_INVALID_APIKEY;
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

        public static async Task ReadSchema()
        {
            await Task.Factory.StartNew(delegate
            {
                XmlDocument schema = new XmlDocument();
                schema.Load(schema_items_dir);
                foreach (XmlNode node in schema.DocumentElement["items"])
                {
                    if (node.Name.ToLower().Equals("item"))
                    {
                        var item = ItemsHandler(node);
                        if(item != null)
                        {
                            items.Add(item);
                        }
                    }
                }
                schema.Load(schema_attributes_dir);
                foreach (XmlNode node in schema.DocumentElement["attributes"])
                {
                    if (node.Name.ToLower().Equals("attribute"))
                    {
                        var attribute = AttributesHandler(node);
                        if (attribute != null)
                        {
                            attributes.Add(attribute);
                        }
                    }
                }
            });
        } 

        public static string UrlToVersionString(string url)
        {
            string[] splited = url.Split('/');
            return splited[splited.Length - 1].Split('.')[1];
            //http://media.steampowered.com/apps/440/scripts/items/items_game.9219e13469eb57d1a2513dc5124786b5df20cf6c.txt
        }

        private static string FormatImageURL(string fullurl)
        {
            string[] splited = fullurl.Split('/');
            return splited[splited.Length - 1];
        }
    }
}