using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.DataBase
{
    public static class BetsHandler
    {
        public static decimal Jackpot = 5000;

        public static List<Bet> bets = new List<Bet>();

        private static Dictionary<Winning, decimal> winnings;

        public static List<int> WinnerNumbers = new List<int>();

        public static int ExtractedJolly;

        public static bool IsBettingPermitted = true;
        public static bool IsCloseable = true;        
        
        public static bool ManageBet(Bet bet)
        {
            if (!IsBettingPermitted)
            {
                return false;
            }

            if (GetUserBetsCount(bet.username) == 5)
            {
                return false;
            }

            if(bet.betNumbers.GroupBy(x => x).Count() < 5)
            {
                return false;
            }

            if(!bet.betNumbers.All(x => x > 0 && x <= 30))
            {
                return false;
            }

            bets.Add(bet);
            Jackpot += 5;

            return true;
        }

        public static List<Winner> PayBets()
        {
            
            PopulateWinnings();

            List<Winner> result = new List<Winner>();
            Random extraction = new Random();
            WinnerNumbers = new List<int>();

            while(WinnerNumbers.Count < 5)
            {
                int newWinner = extraction.Next(1, 31);
                if (WinnerNumbers.Contains(newWinner))
                    continue;
                else
                    WinnerNumbers.Add(newWinner);
            }

            ExtractedJolly = extraction.Next(1, 31);

            while (WinnerNumbers.Contains(ExtractedJolly))
                ExtractedJolly = extraction.Next(1, 31);

            var grouped = bets.GroupBy(x => x.username).ToList();
            foreach(var group in grouped)
            {
                int betsCount = group.Count();
                decimal winning = 0;

                group.ToList().ForEach(x =>
                {
                    int numbersHit = x.betNumbers.Where(x => WinnerNumbers.Contains(x)).Count();

                    winning += CalculateWinning(betsCount, numbersHit);
                });

                if (ExistingUsers.RegisteredUsers.Single(x => x.username == group.Key).jolly == ExtractedJolly)
                    winning *= 100;

                result.Add(new Winner() { username = group.Key, winning = winning });
            }

            IsCloseable = false;
            return result;
        }

        public static int GetUserBetsCount(string username)
        {
            return bets.Where(x => x.username == username).Count();
        }

        public static void CloseBets()
        {
            IsBettingPermitted = false;
        }

        private static void PopulateWinnings()
        {
            winnings = new Dictionary<Winning, decimal>();

            winnings.Add(new Winning() { betsCount = 1, numbersHit = 2 }, 10);
            winnings.Add(new Winning() { betsCount = 1, numbersHit = 3 }, 25);
            winnings.Add(new Winning() { betsCount = 1, numbersHit = 4 }, 100);
            winnings.Add(new Winning() { betsCount = 1, numbersHit = 5 }, 1000);

            winnings.Add(new Winning() { betsCount = 2, numbersHit = 2 }, 5);
            winnings.Add(new Winning() { betsCount = 2, numbersHit = 3 }, 20);
            winnings.Add(new Winning() { betsCount = 2, numbersHit = 4 }, 75);
            winnings.Add(new Winning() { betsCount = 2, numbersHit = 5 }, 750);

            winnings.Add(new Winning() { betsCount = 3, numbersHit = 2 }, (decimal)2.50);
            winnings.Add(new Winning() { betsCount = 3, numbersHit = 3 }, 15);
            winnings.Add(new Winning() { betsCount = 3, numbersHit = 4 }, 50);
            winnings.Add(new Winning() { betsCount = 3, numbersHit = 5 }, 500);

            winnings.Add(new Winning() { betsCount = 4, numbersHit = 2 }, 2);
            winnings.Add(new Winning() { betsCount = 4, numbersHit = 3 }, 10);
            winnings.Add(new Winning() { betsCount = 4, numbersHit = 4 }, 25);
            winnings.Add(new Winning() { betsCount = 4, numbersHit = 5 }, 250);

            winnings.Add(new Winning() { betsCount = 5, numbersHit = 2 }, 1);
            winnings.Add(new Winning() { betsCount = 5, numbersHit = 3 }, 5);
            winnings.Add(new Winning() { betsCount = 5, numbersHit = 4 }, 20);
            winnings.Add(new Winning() { betsCount = 5, numbersHit = 5 }, 200);
        }

        private static decimal CalculateWinning(int betsCount, int numbersHit)
        {
            var result = winnings.Where(x => x.Key.betsCount == betsCount && x.Key.numbersHit == numbersHit).FirstOrDefault();
            return result.Value;
        }

        public static string ExtractedNumbersString()
        {
            return string.Join(", ", WinnerNumbers);
        }
    }    
}
