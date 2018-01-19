using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace WielowatkowyGeneratorLiczbPierwszych
{
    class LiczbyDoPliku
    {
        static private object _lock = new Object();

        public void Zapis(string bufor, string sciezka)
        {
            lock(_lock)
            {
                using (var fs = new FileStream(sciezka, FileMode.Append, FileAccess.Write))
                {
                    byte[] dataAsByteArray = new UTF8Encoding(true).GetBytes(bufor);
                    fs.Write(dataAsByteArray, 0, bufor.Length);
                    Thread.Sleep(1);
                }
            }
        }
    }
}