using System;
using System.Linq;

namespace MC_Sicherung.Helper
{
    static class Placeholder
    {
        public static string replace(string schema, string path)
        {
            path = path.Split('\\').Last();
            string result = schema;

            result = replaceO(result, path);
            result = replaceD(result);
            result = replaceDU(result);
            result = replaceT(result);
            return result;
        }

        private static string replaceO(string sRep, string path)
        {
            return sRep.Replace("{O}", path);
        }

        private static string replaceD(string sRep)
        {
            string sRepDate = DateTime.Now.Date.ToString("ddMMyyyy");
            return sRep.Replace("{D}", sRepDate);
        }

        private static string replaceDU(string sRep)
        {
            string sRepDate = DateTime.Now.Date.ToString("yyyyMMdd");
            return sRep.Replace("{DU}", sRepDate);
        }

        private static string replaceT(string sRep)
        {
            string sRepTime = DateTime.Now.TimeOfDay.ToString("hhmmss");
            return sRep.Replace("{T}", sRepTime);
        }


    }
}
