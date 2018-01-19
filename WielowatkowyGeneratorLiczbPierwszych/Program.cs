using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
                        Console.WriteLine("Ile liczb chcesz wygenerować? Zakres od 1 - 100 000 000");
                        _bSprMenu = int.TryParse(Console.ReadLine(), out _iLiczbaDoWygen);
                        if (_bSprMenu == false)
                        {
                            Console.WriteLine("Zły wybór!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Proszę czekać...");
                            Parallel.For(0, _iLiczbaDoWygen, new ParallelOptions { MaxDegreeOfParallelism = 5 }, i =>
                            {
                                new LiczbyDoPliku().Zapis(rnd.Next(1, _iLiczbaDoWygen).ToString() + Environment.NewLine, @"F:\GenLiczb.txt");
                            });
                            Console.WriteLine("Zakończono!");
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
                        break;
                }
            }
        }
    }
}