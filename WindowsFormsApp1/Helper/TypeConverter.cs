using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.IO.Compression;
using Ionic.Zip;

namespace MC_Sicherung.Helper
{
    class TypeConverter
    {
        public static bool toBool(string value)
        {
            switch (value.ToUpper())
            {
                case "TRUE":
                case "1":
                    return true;
                case "FALSE":
                case "0":
                default:
                    return false;
            }
        }

        public static Ionic.Zlib.CompressionLevel toCompressionLevel(string value)
        {
            switch(value.ToUpper())
            {
                case "BESTCOMPRESSION": return Ionic.Zlib.CompressionLevel.BestCompression;
                case "BESTSPEED": return Ionic.Zlib.CompressionLevel.BestSpeed;
                case "DEFAULT": return Ionic.Zlib.CompressionLevel.Default;
                case "LEVEL2": return Ionic.Zlib.CompressionLevel.Level2;
                case "LEVEL3": return Ionic.Zlib.CompressionLevel.Level3;
                case "LEVEL4": return Ionic.Zlib.CompressionLevel.Level4;
                case "LEVEL5": return Ionic.Zlib.CompressionLevel.Level5;
                case "LEVEL7": return Ionic.Zlib.CompressionLevel.Level7;
                case "LEVEL8": return Ionic.Zlib.CompressionLevel.Level8;
                case "NONE": return Ionic.Zlib.CompressionLevel.None;
                default: return Ionic.Zlib.CompressionLevel.Default;
            }
        }

        public static string CompressionLevelToString(Ionic.Zlib.CompressionLevel value)
        {
            switch(value)
            {
                case Ionic.Zlib.CompressionLevel.BestCompression: return "BestCompression";
                case Ionic.Zlib.CompressionLevel.BestSpeed: return "BestSpeed";
                case Ionic.Zlib.CompressionLevel.Default: return "Default";
                case Ionic.Zlib.CompressionLevel.Level2: return "Level2";
                case Ionic.Zlib.CompressionLevel.Level3: return "Level3";
                case Ionic.Zlib.CompressionLevel.Level4: return "Level4";
                case Ionic.Zlib.CompressionLevel.Level5: return "Level5";
                case Ionic.Zlib.CompressionLevel.Level7: return "Level7";
                case Ionic.Zlib.CompressionLevel.Level8: return "Level8";
                case Ionic.Zlib.CompressionLevel.None: return "None";
                default: return "Default";
            }
        }
    }
}