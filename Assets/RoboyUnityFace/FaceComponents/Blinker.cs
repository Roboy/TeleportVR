using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRNG;

namespace Assets.FaceComponents
{
    // Blinking modelled after naturally occuring blinks.
    class Blinker
    {
        /*
        @article {MDS:MDS870120629,
        author = {Bentivoglio, Anna Rita and Bressman, Susan B. and Cassetta, Emanuele and Carretta, Donatella and Tonali, Pietro and Albanese, Alberto},
        title = {Analysis of blink rate patterns in normal subjects},
        journal = {Movement Disorders},
        volume = {12},
        number = {6},
        publisher = {Wiley Subscription Services, Inc., A Wiley Company},
        issn = {1531-8257},
        url = {http://dx.doi.org/10.1002/mds.870120629},
        doi = {10.1002/mds.870120629},
        pages = {1028--1034},
        keywords = {Blinking, Blepharospasm, Dystonia, Tics},
        year = {1997},
        }*/

        private struct BlinkStyle
        {
            float mu;
            float sigma;
        }

        Dictionary<String,BlinkStyle> blinkStyles;
        public Blinker()
        {
            
        }
    }
}
