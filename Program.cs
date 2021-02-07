using System;
using System.Security.Cryptography;

namespace RockNPaper
{
    
    class Program
    { 
        //int halfStateSize;
        static void Main(string[] args)
        {
            int statesSize = args.Length;
            if(statesSize < 3 || statesSize % 2 == 0 || TestRepeat(in args))
            {
                Console.Write("Invalid input. You need to enter at least 3 (number should be odd) " +
                    "different game moves.\nExample of valid input: Rock Paper Scissors\n");
            }
            else
            {
                Gameplay(in args);
            }
        }
        static void Gameplay(in string[] gamemoves)
        {
            SHA384 hashCoder = SHA384.Create();
            var rng = RandomNumberGenerator.Create();
            int playerMove = 0;
            int computerMove;
            byte[] key = new byte[16];

            rng.GetBytes(key);
            byte[] hmac = hashCoder.ComputeHash(key);
            computerMove = key[15] % gamemoves.Length;

            Console.WriteLine("HMAC");
            foreach (var hmacByte in hmac)
            {
                Console.Write("{0:X}", hmacByte);
            }

            Console.WriteLine();

            bool exitFlag = false;
            while (!exitFlag) 
            {
                Console.WriteLine("Availible moves:");
                for (int i = 0; i < gamemoves.Length; i++)
                {
                    Console.WriteLine(i + 1 + " - " + gamemoves[i]);
                }
                Console.WriteLine("0 - exit");
                Console.Write("Enter your move: ");                
                try
                {
                    playerMove = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    continue;                    
                }
                if (playerMove == 0) Environment.Exit(0);
                else if (playerMove > 0 && playerMove <= gamemoves.Length) exitFlag = true;
            }
            playerMove--;
            Console.WriteLine("Your move: " + gamemoves[playerMove]);
            Console.WriteLine("Computer move: " + gamemoves[computerMove]);
            if (playerMove == computerMove)
            {
                Console.WriteLine("It's a draw");
            }
            else
            {
                if (PlayerWin(gamemoves.Length, in playerMove, in computerMove)) Console.WriteLine("You win");
                else Console.WriteLine("You lose");
            }
            Console.WriteLine("HMAC key:");
            foreach (var keyByte in key)
            {
                Console.Write("{0:X}", keyByte);
            }
        }
        static bool PlayerWin(in int arrLength, in int playerMove, in int computerMove)
        {
            int currArrPos = playerMove;
            for (int i = 0; i < (arrLength - 1) / 2; i++)
            {
                currArrPos++;
                if (currArrPos >= arrLength) currArrPos = 0;
                if (currArrPos == computerMove) return false;
            }
            return true;
        }
        static bool TestRepeat(in string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = i + 1; j < args.Length; j++)
                {
                    if (args[i] == args[j]) return true;
                }
            }
            return false;
        }
    }    
    
}
