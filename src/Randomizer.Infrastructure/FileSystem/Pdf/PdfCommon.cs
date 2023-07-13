using MigraDoc.DocumentObjectModel;

namespace Randomizer.Infrastructure.FileSystem.Pdf;

public static class PdfCommon
{
    public const double A4WidthMm = 210.0;
    public const double A4HeightMm = 297.0;
    public const double MarginSideMm = 5.0;
    public const double A4WidthWithoutMargin = A4WidthMm - MarginSideMm * 2.0;

    // assuming that 1rem equals 16px
    public const double MmInRem = 4.2333333328;

    // Fonts
    public const string DefaultFont = "Gibson-Regular";
    public const string MediumFont = "Gibson-Medium";
    public const string BoldFont = "Gibson-Semibold";

    // Font Styles
    public const string BodyFontStyle = "Normal";
    public const string H2FontStyle = "PDF-h2";
    public const string H2BoldFontStyle = "PDF-h2-bold";
    public const string H3FontStyle = "PDF-h3";
    public const string BodyBoldFontStyle = "PDF-body-bold";
    public const string BodySFontStyle = "PDF-body-s";

    // Colors
    public static readonly Color Neutral600 = new Color(24, 24, 24);
    public static readonly Color Neutral500 = new Color(51, 51, 51);
    public static readonly Color Neutral400 = new Color(82, 82, 82);
    public static readonly Color Neutral300 = new Color(153, 153, 153);

    public static void DefineBasicStyles(Document document)
    {
        // normal equals PDF-body in style guide
        var style = document.Styles[BodyFontStyle];

        style.Font.Size = Unit(0.75);
        style.Font.Name = DefaultFont;
        style.ParagraphFormat.LineSpacing = Unit(1);
        style.Font.Color = Neutral600;
        style.ParagraphFormat.LineSpacingRule = LineSpacingRule.Exactly;
        style.ParagraphFormat.SpaceAfter = Unit(0.5);

        style = document.AddStyle(H2FontStyle, BodyFontStyle);
        style.Font.Size = Unit(1);
        style.ParagraphFormat.LineSpacing = Unit(1.5);
        style.ParagraphFormat.SpaceAfter = Unit(0.75);

        style = document.AddStyle(H2BoldFontStyle, H2FontStyle);
        style.Font.Bold = true;
        style.Font.Color = Neutral400;

        // this font is uppercase, MigraDoc does not support it, use this style with ToUpper()
        style = document.AddStyle(H3FontStyle, BodyFontStyle);
        style.Font.Size = Unit(0.63);
        style.ParagraphFormat.LineSpacing = Unit(1);
        style.ParagraphFormat.SpaceAfter = Unit(0.5);
        style.Font.Bold = true;
        style.Font.Color = Neutral500;

        style = document.AddStyle(BodyBoldFontStyle, BodyFontStyle);
        style.Font.Bold = true;

        style = document.AddStyle(BodySFontStyle, BodyFontStyle);
        style.Font.Size = Unit(0.63);
        style.ParagraphFormat.SpaceAfter = Unit(0.5);
        style.Font.Color = Neutral400;
    }

    public static void DefineBasicLayout(Document document)
    {
        var section = document.AddSection();

        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.BottomMargin = UnitMm(MarginSideMm);
        section.PageSetup.LeftMargin = UnitMm(MarginSideMm);
        section.PageSetup.RightMargin = UnitMm(MarginSideMm);
        section.PageSetup.TopMargin = UnitMm(MarginSideMm);
    }

    public static Unit Unit(double rem) => new (rem * MmInRem, UnitType.Millimeter);

    public static Unit UnitMm(double mm) => new (mm, UnitType.Millimeter);
}
