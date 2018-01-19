using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace WielowatkowyGeneratorLiczbPierwszych
{
    class LiczbyZPliku
    {
        public void Odczyt (string sciezka)
        {
            LiczbyDoPliku LDP = new LiczbyDoPliku();
            var fs = new System.IO.FileStream(sciezka, FileMode.Open, FileAccess.Read);
            var file = new System.IO.StreamReader(fs, Encoding.UTF8, true, 128);
            string lineOfText;
            int _iLiczba = 0, _iNPierwsz =0;

            while ((lineOfText = file.ReadLine()) != null)
            {
                _iLiczba = int.Parse(lineOfText);
                for (int i=1; i<=_iLiczba; i++)
                {
                    if (_iLiczba % i == 0)
                        _iNPierwsz++;
                }
                if (_iNPierwsz == 2)
                {
                    LDP.Zapis(lineOfText + Environment.NewLine, @"F:\WynikPierwsze.txt");
                    _iNPierwsz = 0;
                }
                _iLiczba = 0;
                _iNPierwsz = 0;
            }
            fs.Close();
        }
    }
}