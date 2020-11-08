using System;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SP_Tema4_Ej6
{
    class Program
    {
        /*player 1,2 1-10 sleep (100, 100*(rand?)
         * display array char sleep(200)
         if(player1 5-7)
                         +1
            display.wait +5
            win 20
          if(player 2 5-7)
                            -1
             display.start  -5
             win -20*/
        private static int cont = 0;
        private static bool paused = false;
        private static bool finished = false;
        private static Random rand = new Random();
        static object l = new object();
        static object m = new object();
        public static void initialize()
        {
            Console.SetCursorPosition(1, 1);
            Console.WriteLine(String.Format("Player One: {0,3}", 0));
            Console.SetCursorPosition(30, 1);
            Console.WriteLine(String.Format("Match: {0,3}", 0));
            Console.SetCursorPosition(1, 10);
            Console.WriteLine(String.Format("Player Two: {0,3}", 0));
            Console.SetCursorPosition(30, 1);
            Console.WriteLine(String.Format("Match: {0,3}", 0));
            Console.SetCursorPosition(30, 10);
            Console.WriteLine('\\');
        }

        static void Main(string[] args)
        {
            Thread player1 = new Thread(playerOne);
            Thread player2 = new Thread(playerTwo);
            Thread display = new Thread(displayQueer);

            initialize();

            display.IsBackground = true;
            display.Start();
            player1.Start();
            player2.Start();

            lock (m)
            {
                Monitor.Wait(m);
            }

            
            Console.SetCursorPosition(1,20);
                                                                        
            Console.WriteLine("Enter to continue...");
            
            Console.ReadLine();
            Console.Clear();
            Console.SetCursorPosition(0, 1);
            string winner = cont > 0 ? "Player 1 wins" : "Player 2 wins"; 
            Console.WriteLine(winner);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FINISH HIM!");

            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }




        public static void playerOne(/*object isOne*/)
        {
            // ||| Mucho coñazo pero la intencion estuvo ahi
            // vvv 
            //bool isO = (bool)isOne;
            int sum = 0;
            int theThrow = 0;
            while (!finished)
            {
                lock (l)
                {
                    if (!finished)
                    {
                        sum = paused ? 5 : 1;
                        theThrow = rand.Next(1, 11);
                        //sum = isO ? sum : sum * -1;
                       if(theThrow == 5 || theThrow == 7)
                        {
                            paused = true;
                        }


                        cont += sum;
                        finished = cont >= 20;

                        Console.SetCursorPosition(1, 1);
                        Console.WriteLine(String.Format("Player One: {0,3}", theThrow));

                        Console.SetCursorPosition(30, 1);
                        Console.WriteLine(String.Format("Match: {0,3}", cont));
                    }
                }
                        Thread.Sleep(100 * theThrow);
            }
            lock (m)
            {
                Monitor.Pulse(m);
            }
        }

        public static void playerTwo()
        {

            int sum = 0;
            int theThrow = 0;
            while (!finished)
            {
                lock (l)
                {
                    if (!finished)
                    {

                        sum = paused ? -1 : -5;
                        theThrow = rand.Next(1, 11);

                        if (theThrow == 5 || theThrow == 7)
                        {
                            paused = false;
                            Monitor.Pulse(l);
                        }
                        


                        cont += sum;
                        finished = cont <= -20;

                        Console.SetCursorPosition(1, 10);
                        Console.WriteLine(String.Format("Player Two: {0,3}", theThrow));

                        Console.SetCursorPosition(30, 1);
                        Console.WriteLine(String.Format("Match: {0,3}", cont));
                    }
                }
                        Thread.Sleep(100 * theThrow);
            }
            lock (m)
            {
                Monitor.Pulse(m);
            }
        }

        public static void displayQueer()
        {
            int index = 0;
            char[] queer = { '|', '/', '-', '\\' };
            while (!finished)
            {
                lock (l)
                {
                    while (paused)
                    {
                        Monitor.Wait(l);
                    }
                    index++;
                    if (index == queer.Length)
                    {
                        index = 0;
                    }
                    {

                        Console.SetCursorPosition(30, 10);
                        Console.WriteLine(queer[index]);
                    }
                }
                        Thread.Sleep(200);

            }
        }
    }
}
