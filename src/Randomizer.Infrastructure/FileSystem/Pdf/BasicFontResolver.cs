using PdfSharp.Fonts;
using System.IO;
using System.Linq;
using System.Reflection;
using static LynxMarvelTor.BL.Services.FileSystem.Pdf.PdfCommon;

namespace LynxMarvelTor.BL.Services.FileSystem.Pdf
{
    public class BasicFontResolver : IFontResolver
    {
        private readonly string[] _fontNames = new string[]
        {
            DefaultFont,
            BoldFont,
            MediumFont,
            "Gibson-Book",
            "Gibson-Heavy",
            "Gibson-Thin"
        };

        public byte[] GetFont(string faceName)
        {
            if (_fontNames.Contains(faceName))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var stream = assembly.GetManifestResourceStream($"LynxMarvelTor.BL.Fonts.Gibson.{faceName}.otf");

                using (var reader = new StreamReader(stream))
                {
                    var bytes = default(byte[]);

                    using (var ms = new MemoryStream())
                    {
                        reader.BaseStream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }

                    return bytes;
                }
            }
            else
            {
                return GetFont(DefaultFont);
            }
        }

        // isItalic ignored cause we don't have corresponding font
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
            => _fontNames.Contains(familyName) ? new FontResolverInfo(isBold ? BoldFont : familyName) : null;
    }
}
