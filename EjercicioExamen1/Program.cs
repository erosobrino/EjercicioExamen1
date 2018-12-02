using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EjercicioExamen1
{
    class Program
    {
        bool finish = false;
        int posXLiebre = 0;
        int posXTortuga = 0;
        int lineaInfo = 4;
        bool despertada = false;
        Random rand = new Random();
        static readonly private object l = new object();
        static readonly private object t = new object();
        static void Main(string[] args)
        {
            Process notepad;
            ProcessStartInfo startInfo = new ProcessStartInfo("NotePad", "D:\\Ciclo\\fol.txt");
            notepad = Process.Start(startInfo);
            Console.WriteLine(notepad.Id);
            notepad.WaitForExit();

            Program p = new Program();
            Thread tortuga = new Thread(p.correTortuga);
            Thread liebre = new Thread(p.correLiebre);

            Console.WriteLine("Pulsa una tecla para empezar");
            Console.ReadKey();
            Console.Clear();
            p.inicio();
            tortuga.Start();
            liebre.Start();
            tortuga.Join();
            liebre.Join();

            Console.SetCursorPosition(0, p.lineaInfo);
            if (p.posXLiebre >= 25)
            {
                Console.WriteLine("Ha ganado la liebre");
            }
            else
            {
                Console.WriteLine("Ha ganado la tortuga");
            }
            Console.ReadKey();
        }

        public void inicio()
        {
            Console.WriteLine("T");
            Console.WriteLine("L");
            Console.SetCursorPosition(25, 0);
            Console.WriteLine("*");
            Console.SetCursorPosition(25, 1);
            Console.WriteLine("*");
        }

        public void correTortuga()
        {
            while (!finish)
            {
                lock (t)
                {
                    if (!finish)
                    {
                        posXTortuga += 1;
                        Console.SetCursorPosition(posXTortuga, 0);
                        Console.WriteLine("T");
                        Console.SetCursorPosition(32, 0);
                        Console.WriteLine(posXTortuga);
                        despertada = false;
                        if (posXLiebre == posXTortuga)
                        {
                            int aleatorio = rand.Next(1, 3);
                            if (aleatorio == 1)
                            {
                                Console.SetCursorPosition(0, lineaInfo);
                                Console.WriteLine("La tortuga hace ruido");
                                Console.WriteLine("Liebre se despierta");
                                lineaInfo += 2;
                                despertada = true;
                                lock (l)
                                    Monitor.Pulse(l);
                            }
                        }
                        if (posXTortuga >= 25)
                        {
                            finish = true;
                        }
                    }
                }
                Thread.Sleep(300);
            }
        }
        public void correLiebre()
        {
            while (!finish)
            {
                lock (l)
                {
                    if (!finish)
                    {
                        posXLiebre += 6;
                        Console.SetCursorPosition(posXLiebre, 1);
                        Console.WriteLine("L");
                        Console.SetCursorPosition(32, 1);
                        Console.WriteLine(posXLiebre);
                        if (posXLiebre >= 25)
                        {
                            finish = true;
                        }
                        else
                        {
                            int aleatorio = rand.Next(1, 11);
                            if (aleatorio <= 6)
                            {
                                Thread threadDormir = new Thread(dormir);
                                threadDormir.Start();
                                lock (l)
                                {
                                    Monitor.Wait(l);
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(200);
            }
        }

        public void dormir()
        {
            lock (l)
            {
                Console.SetCursorPosition(0, lineaInfo);
                Console.WriteLine("Liebre durmiendo");
                lineaInfo++;
                Thread.Sleep(rand.Next(0, 2501));
                if (!despertada)
                {
                    Console.SetCursorPosition(0, lineaInfo);
                    Console.WriteLine("Liebre despierta");
                }
                lineaInfo++;
                Monitor.Pulse(l);
            }
        }
    }
}