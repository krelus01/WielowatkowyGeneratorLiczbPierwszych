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
            //Sekcja zmiennych:
            bool _bSprMenu;
            int _iMenuWybor = 0;
            int _iLiczbDoWygen = 0;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            Task[] tasks = new Task[4];
            Object lok = new Object();

            //Sekcja z menu aplikacji:
            Console.WriteLine("Aplikacja generująca losowe liczby całkowite oraz wyszukująca liczby pierwsze.");
            Console.WriteLine("Menu aplikacji:");
            Console.WriteLine("1. Wygeneruj losowe liczby całkowite do pliku.");
            Console.WriteLine("2. Wyszukaj liczby pierwsze z pliku.");
            Console.WriteLine("0. Wyjdż.");
            _bSprMenu = int.TryParse(Console.ReadLine(), out _iMenuWybor);

            //Sekcja główny program + sprawdzanie wyboru dla menu
            if (_bSprMenu == false)
            {
                Console.WriteLine("Zły wybór!");
            }
            else
            {
                switch (_iMenuWybor)
                {
                    case 1:
                        //Sekcja generowania liczb losowych do pliku. Menu wyboru ilości liczb do wygenerowania.
                        Console.WriteLine("Ile liczb chcesz wygenerować? Zakres od 1 - 100 000 000");
                        _bSprMenu = int.TryParse(Console.ReadLine(), out _iLiczbDoWygen);

                        //Sekcja generowania liczb + sprawdzenie wyboru ilości liczb do wygenerowaniia.
                        if (_bSprMenu == false || _iLiczbDoWygen < 1 || _iLiczbDoWygen > 100000000)
                        {
                            Console.WriteLine("Zły wybór!");
                            Console.ReadKey();
                        }
                        else
                        {
                            //Sekcja generowania do pliku zadanej liczby cyfr w 4 osobnych zadaniach
                            Console.WriteLine("Proszę czekać...");
                            for (int i=0; i<=tasks.Length-1; i++)
                            {
                                tasks[i] = Task.Factory.StartNew( ()=> {
                                    int _itestowy = 0;
                                    List<string> _sPrzeproc = new List<string>();

                                    for (int j = 1; j <= _iLiczbDoWygen / 4; j++)
                                    {
                                        _sPrzeproc.Add(rnd.Next(1, _iLiczbDoWygen).ToString());
                                        _itestowy++;
                                    }
                                    Console.WriteLine("Task " + Task.CurrentId.ToString() + " wykonał " + _itestowy.ToString() + " iteracji.");
                                    lock(lok)
                                    {
                                        using (var output = File.AppendText(@"F:\WygenLiczby.txt"))
                                        {
                                            output.AutoFlush = true;
                                            int _iproc = 0;
                                            System.Threading.Thread.Sleep(100);
                                            foreach (string s in _sPrzeproc)
                                            {
                                                output.WriteLine(s);
                                                _iproc++;
                                                Console.SetCursorPosition(0, (int)Task.CurrentId+15);
                                                Console.Write("Task " + Task.CurrentId.ToString() + " ukończył: " + ((_iproc * 100 / _sPrzeproc.Count())).ToString() + "%");
                                            }
                                        }
                                    }
                                });
                            }

                            //Zakończenie działania sekcji generowania do pliku.
                            Task.WaitAll(tasks);
                            Console.SetCursorPosition(0, 20);
                            Console.WriteLine("Zakończono!");
                            Console.ReadKey();
                        }
                            break;
                    case 2:
                        //Sekcja wyszukiwania liczb pierwszych z pliku z wygenerowanymi liczbami /  Sekcja zmiennych
                        int _iLiczba = 0, _iNPierwsz = 0;
                        var _stemp = File.ReadLines(@"F:\WygenLiczby.txt");
                        List<string> _sPrzeprocPierw = new List<string>(); foreach (string s in _stemp) _sPrzeprocPierw.Add(s);
                        List<string> _sPrzeprocPierwOK = new List<string>();

                        //Sekcja wyszukiwania w czterech osobnych zadaniach
                        Console.WriteLine("Trwa budowanie pliku wynikowego...");
                        for (int i=0; i<=tasks.Length-1;i++)
                        {
                            tasks[i] = Task.Factory.StartNew(() =>
                           {
                               int _iIlosc = _sPrzeprocPierw.Count() / 4;
                               int _iPocz = _iIlosc * (int)Task.CurrentId - _iIlosc;

                               Console.WriteLine("Task " + Task.CurrentId.ToString() + " rozpoczyna od " + _iPocz.ToString() + " linii.");
                               for (int j=_iPocz; j<=_iPocz + _iIlosc - 1; j++)
                               {
                                   _iLiczba = int.Parse(_sPrzeprocPierw[j]);
                                   for (int k = 1; k <= _iLiczba; k++)
                                   {
                                       if (_iLiczba % k == 0)
                                           _iNPierwsz++;
                                   }
                                   if (_iNPierwsz == 2)
                                   {
                                       _sPrzeprocPierwOK.Add(_sPrzeprocPierw[j]);
                                       _iNPierwsz = 0;
                                   }
                                   _iLiczba = 0;
                                   _iNPierwsz = 0;
                               }
                               lock (lok)
                               {
                                   using (var output = File.AppendText(@"F:\WynikPierwsze.txt"))
                                   {
                                       output.AutoFlush = true;
                                       int _iproc = 0;
                                       System.Threading.Thread.Sleep(100);
                                       foreach (string s in _sPrzeprocPierwOK)
                                       {
                                           output.WriteLine(s);
                                           _iproc++;
                                           Console.SetCursorPosition(0, (int)Task.CurrentId + 15);
                                           Console.Write("Task " + Task.CurrentId.ToString() + " ukończył: " + ((_iproc * 100 / _sPrzeprocPierwOK.Count())).ToString() + "%");
                                       }
                                   }
                               }
                           });
                        }

                        //Zakończenie działania sekcji.
                        Task.WaitAll(tasks);
                        Console.SetCursorPosition(0, 20);
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