using System;
using System.Collections.Generic;

namespace MC_Sicherung.Config.Interface
{
    public interface IConfig
    {
        string getPath();
        int getHashCode();
        Dictionary<string, VariableReference> getFieldMapping();
        List<Dictionary<string, Delegate>> getListMethods();
    }
}
