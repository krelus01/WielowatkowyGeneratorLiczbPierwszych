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
            int _iLiczbDoWygen = 0;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

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
                        _bSprMenu = int.TryParse(Console.ReadLine(), out _iLiczbDoWygen);
                        if (_bSprMenu == false || _iLiczbDoWygen < 1 || _iLiczbDoWygen > 100000000)
                        {
                            Console.WriteLine("Zły wybór!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Proszę czekać...");
                            parallelloop.Start();
                            Parallel.For(0, _iLiczbDoWygen, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount}, i =>
                            {
                                _sPrzeproc.Add(rnd.Next(1, _iLiczbDoWygen).ToString());
                            });
                            parallelloop.Stop();
                            foreachloop.Start();
                            using (var output = File.AppendText(@"F:\WygenLiczby.txt"))
                            {
                                output.AutoFlush = true;
                                foreach (string s in _sPrzeproc)
                                    output.WriteLine(s);
                            }
                            foreachloop.Stop();
                            _sPrzeproc.Clear();
                            Console.WriteLine("Zakończono!");
                            Console.WriteLine("Czas parallel: " + parallelloop.ElapsedMilliseconds.ToString() + ", czas foreach: " + foreachloop.ElapsedMilliseconds.ToString());
                            foreachloop.Reset(); parallelloop.Reset();
                            Console.ReadKey();
                        }
                            break;
                    case 2:
                        int _iLiczba = 0, _iNPierwsz = 0;
                        var _sPrzeprocPierw = File.ReadLines(@"F:\WygenLiczby.txt");
                        List<string> _sPrzeprocPierwOK = new List<string>();
                        Console.WriteLine("Trwa budowanie pliku wynikowego...");
                        Parallel.For(0, _sPrzeprocPierw.Count(), new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
                        {
                            foreach (var s in _sPrzeprocPierw)
                            {
                                _iLiczba = int.Parse(s);
                                for (int j = 1; j <= _iLiczba; j++)
                                {
                                    if (_iLiczba % j == 0)
                                        _iNPierwsz++;
                                }
                                if (_iNPierwsz == 2)
                                {
                                    _sPrzeprocPierwOK.Add(s);
                                    _iNPierwsz = 0;
                                }
                                _iLiczba = 0;
                                _iNPierwsz = 0;
                            }
                            using (var output = File.AppendText(@"F:\WynikPierwsze.txt"))
                            {
                                output.AutoFlush = true;
                                foreach (string s in _sPrzeprocPierwOK)
                                    output.WriteLine(s);
                            }
                        });
                        _sPrzeprocPierwOK.Clear();
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