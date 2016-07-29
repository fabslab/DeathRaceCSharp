using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Pedestrian.Pipeline.BitmapFonts
{
    [ContentImporter(".fnt", DefaultProcessor = "BitmapFontProcessor", DisplayName = "BMFont Importer")]
    public class BitmapFontImporter : ContentImporter<BitmapFontFile>
    {
        public override BitmapFontFile Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage("Importing XML file: {0}", filename);

            using (var streamReader = new StreamReader(filename))
            {
                var deserializer = new XmlSerializer(typeof(BitmapFontFile));
                return (BitmapFontFile)deserializer.Deserialize(streamReader);
            }
        }
    }
}