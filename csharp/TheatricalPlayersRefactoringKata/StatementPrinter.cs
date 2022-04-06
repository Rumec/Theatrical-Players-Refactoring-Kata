using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public static string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = $"Statement for {invoice.Customer}\n";
            var cultureInfo = new CultureInfo("en-US");

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                var thisAmount = CalculateThisAmount(play, perf);

                volumeCredits += CalculateVolumeCredits(perf, play);

                // print line for this order
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name,
                    Convert.ToDecimal(thisAmount / 100), perf.Audience);
                totalAmount += thisAmount;
            }

            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private static int CalculateVolumeCredits(Performance perf, Play play)
        {
            var volumeCreditsIncrement = Math.Max(perf.Audience - 30, 0);
            // add extra credit for every ten comedy attendees
            if ("comedy" == play.Type) volumeCreditsIncrement += (int) Math.Floor((decimal) perf.Audience / 5);
            return volumeCreditsIncrement;
        }


        private static int CalculateThisAmount(Play play, Performance perf)
        {
            int thisAmount;
            if (play.Type == "tragedy")
            {
                thisAmount = 40000;
                if (perf.Audience > 30)
                {
                    thisAmount += 1000 * (perf.Audience - 30);
                }

                return thisAmount;
            }

            if (play.Type == "comedy")
            {
                thisAmount = 30000;
                if (perf.Audience > 20)
                {
                    thisAmount += 10000 + 500 * (perf.Audience - 20);
                }

                thisAmount += 300 * perf.Audience;
                return thisAmount;
            }

            throw new Exception("unknown type: " + play.Type);
        }
    }
}