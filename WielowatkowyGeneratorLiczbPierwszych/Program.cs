using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace WielowatkowyGeneratorLiczbPierwszych
{
    class Program
    {
        static void Main(string[] args)
        {
            bool _bSprMenu;
            int _iMenuWybor = 0;
            int _iLiczbaDoWygen = 0;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            LiczbyDoPliku LDP = new LiczbyDoPliku();
            LiczbyZPliku LZP = new LiczbyZPliku();

            Console.WriteLine("Aplikacja generująca losowe liczby całkowite oraz wyszukująca liczby pierwsze.");
            Console.WriteLine("Menu aplikacji:");
            Console.WriteLine("1. Wygeneruj losowe liczby całkowite do pliku.");
            Console.WriteLine("2. Wyszukaj liczby pierwsze z pliku.");
            Console.WriteLine("0. Wyjdż.");
            _bSprMenu = int.TryParse(Console.ReadLine(), out _iMenuWybor);
            if (_bSprMenu == false)
            {
                Console.WriteLine("Zły wybór!");
            }
            else
            {
                switch (_iMenuWybor)
                {
                    case 1:
                        List<string> _sPrzeproc = new List<string>();
                        Stopwatch parallelloop = new Stopwatch();
                        Stopwatch foreachloop = new Stopwatch();

                        Console.WriteLine("Ile liczb chcesz wygenerować? Zakres od 1 - 100 000 000");
                        _bSprMenu = int.TryParse(Console.ReadLine(), out _iLiczbaDoWygen);
                        if (_bSprMenu == false || _iLiczbaDoWygen < 1 || _iLiczbaDoWygen > 100000000)
                        {
                            Console.WriteLine("Zły wybór!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Proszę czekać...");
                            parallelloop.Start();
                            Parallel.For(0, _iLiczbaDoWygen, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount}, i =>
                            {
                                _sPrzeproc.Add(rnd.Next(1, _iLiczbaDoWygen).ToString() + Environment.NewLine);
                            });
                            parallelloop.Stop();
                            foreachloop.Start();
                            foreach (string s in _sPrzeproc)
                                LDP.Zapis(s, @"F:\WygenLiczby.txt");
                            foreachloop.Stop();
                            _sPrzeproc.Clear();
                            Console.WriteLine("Zakończono!");
                            Console.WriteLine("Czas parallel: " + parallelloop.ElapsedMilliseconds.ToString() + ", czas foreach: " + foreachloop.ElapsedMilliseconds.ToString());
                            Console.ReadKey();
                        }
                            break;
                    case 2:
                        Console.WriteLine("Trwa budowanie pliku wynikowego...");
                        LZP.Odczyt(@"F:\GenLiczb.txt");
                        Console.WriteLine("Gotowe!");
                        Console.ReadKey();
                        break;
                    case 0:
                        Environment.Exit(1);
                        break;

                    default:
                        Console.WriteLine("Zły wybór!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}