using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace SetSettingToBlank
{
    class Program
    {
        static void Main(string[] args)
        {
            string locationsettingsFile = args[0];
            string appconfigFile = args[1];
            XmlDocument xmlDoc = new XmlDocument();
            XmlDocument xmlDocSource = new XmlDocument();

            if (File.Exists(locationsettingsFile))
            {
                // Collect properties in development node
                // These are the ones that we will set to blank values
                xmlDocSource.Load(locationsettingsFile);
                XmlNodeList colNodes = xmlDocSource.SelectNodes(String.Format("//Settings/Location[@Name='{0}']/Property/@Name", "development"));
                if (File.Exists(appconfigFile))
                {
                    xmlDoc.Load(@appconfigFile);
                    foreach (XmlNode property in colNodes)
                    {
                        try
                        {
                            var path = String.Format("//userSettings//Nuix_X.Properties.Settings//setting[@name='{0}']//value", property.Value);
                            XmlNode node = xmlDoc.SelectSingleNode(path);
                            Console.WriteLine("Old value of {0} = {1}", property.Value, node.InnerText);
                            node.InnerText = String.Empty;
                            Console.WriteLine("Setting {0} succesfully set to empty.", property.Value);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception caught trying to set {0} to blank. {1}", property.Value, ex.Message);
                        }
                    }
                    try
                    {
                        XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                        XmlWriter writer = XmlWriter.Create(@appconfigFile, settings);
                        xmlDoc.Save(writer);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception caught trying to save {0}. {1}", appconfigFile, ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine(String.Format("Specified location file, {0}, not found. Exiting.", appconfigFile));
                }
            }
            else
            {
                Console.WriteLine(String.Format("Specified location file, {0}, not found. Exiting.", locationsettingsFile));
            }
            
        }

    }
}
