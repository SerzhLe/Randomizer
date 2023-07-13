using PdfSharp.Fonts;
using Randomizer.Application.Abstractions.Infrastructure;
using System.Reflection;
using static Randomizer.Infrastructure.FileSystem.Pdf.PdfCommon;

namespace Randomizer.Infrastructure.FileSystem.Pdf
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
                var assembly = Assembly.GetAssembly(typeof(IDocumentGenerator));
                var stream = assembly.GetManifestResourceStream($"Randomizer.Application.Fonts.{faceName}.otf");

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
