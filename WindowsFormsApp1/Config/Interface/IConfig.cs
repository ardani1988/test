using System;
using System.Collections.Generic;

namespace MC_Sicherung.Config.Interface
{
    interface IConfig
    {
        string getPath();
        Dictionary<string, VariableReference> getFieldMapping();
        List<Dictionary<string, Delegate>> getListMethods();
    }
}
