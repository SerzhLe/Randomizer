using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using Randomizer.Application.DTOs.FileSystem;
using static Randomizer.Infrastructure.FileSystem.Pdf.PdfCommon;

namespace Randomizer.Infrastructure.FileSystem.Pdf.Content;

public class GameResultsContent : BaseContent<GameResultsDocumentDto>
{
    public override void DefineContent(Document document, GameResultsDocumentDto data)
    {
        document.Info.Title = $"Game{data.DisplayId} Results";

        var section = document.LastSection;

        section.AddParagraph($"Game #{data.DisplayId} - Final Results", H2FontStyle);

        section.AddParagraph($"Rounds: {data.CountOfRounds}");

        var contentTable = new Table
        {
            Borders = { Visible = false },
            Columns = new Columns(UnitMm(70), UnitMm(130)),
            Format = new ParagraphFormat { SpaceAfter = UnitMm(3) }
        };

        var row = contentTable.AddRow();

        var participantsCell = row.Cells[0];
        var participantsParagraph = participantsCell.AddParagraph("Participants:".ToUpper());
        participantsParagraph.Style = H3FontStyle;

        foreach (var participant in data.Participants.OrderBy(x => x.Position))
        {
            participantsParagraph.AddLineBreak();
            participantsParagraph.AddFormattedText($"- {participant.NickName}", BodyFontStyle);
        }

        var messagesCell = row.Cells[1];
        var messagesParagraph = messagesCell.AddParagraph("Messages:".ToUpper());
        messagesParagraph.Style = H3FontStyle;

        foreach (var message in data.Messages.OrderBy(x => x.Position))
        {
            messagesParagraph.AddLineBreak();
            messagesParagraph.AddFormattedText($"- {message.Content}", BodyFontStyle);
        }

        section.Add(contentTable);

        section.AddParagraph();

        section.AddParagraph("Round Results:", H2FontStyle);

        foreach (var round in data.Rounds.OrderBy(x => x.SequenceNumber))
        {
            var roundParagraph = section.AddParagraph($"Round #{round.SequenceNumber}".ToUpper(), H3FontStyle);
            roundParagraph.Style = BodyFontStyle;

            var roundTable = new Table
            {
                Borders = { Visible = false },
                Columns = new Columns(Enumerable.Repeat(UnitMm(40), 5).ToArray()),
            };

            var roundRow = roundTable.AddRow();
            roundRow.Style = BodySFontStyle;

            roundRow.Cells[0].AddParagraph("Who Perform Action");
            roundRow.Cells[1].AddParagraph("Who Perform Feedback");
            roundRow.Cells[2].AddParagraph("Question");
            roundRow.Cells[3].AddParagraph("Score");
            roundRow.Cells[4].AddParagraph("Comment");

            foreach (var roundResult in round.RoundResults.OrderBy(x => x.SequenceNumber))
            {
                roundRow = roundTable.AddRow();
                roundRow.Style = BodyFontStyle;

                roundRow.Cells[0].AddParagraph(roundResult.WhoPerformAction!.NickName);
                roundRow.Cells[1].AddParagraph(roundResult.WhoPerformFeedback!.NickName);
                roundRow.Cells[2].AddParagraph(roundResult.Message!.Content);
                roundRow.Cells[3].AddParagraph(ValueOrDefault(roundResult.Score));
                roundRow.Cells[4].AddParagraph(ValueOrDefault(roundResult.Comment!));
            }

            roundTable.SetEdge(0, 0, roundTable.Columns.Count, roundTable.Rows.Count, Edge.Top, BorderStyle.Single, Unit(0.1));
            roundTable.SetEdge(0, 0, roundTable.Columns.Count, roundTable.Rows.Count, Edge.Right, BorderStyle.Single, Unit(0.1));
            roundTable.SetEdge(0, 0, roundTable.Columns.Count, roundTable.Rows.Count, Edge.Bottom, BorderStyle.Single, Unit(0.1));
            roundTable.SetEdge(0, 0, roundTable.Columns.Count, roundTable.Rows.Count, Edge.Left, BorderStyle.Single, Unit(0.1));
            roundTable.Format.SpaceAfter = Unit(1);

            section.Add(roundTable);
        }

        section.AddParagraph();

        var winnersParagraph = section.AddParagraph("Winners:".ToUpper(), H3FontStyle);
        winnersParagraph.AddLineBreak();

        if (!data.Winners.Any())
        {
            winnersParagraph.AddFormattedText("Everyone is a winner in this game. Well done!", BodyFontStyle);
        }

        foreach (var winner in data.Winners)
        {
            winnersParagraph.Style = BodyFontStyle;
            winnersParagraph.AddText($"- {winner.NickName}, Total Score: {winner.TotalScore}");
            winnersParagraph.AddLineBreak();
        }
    }
}
