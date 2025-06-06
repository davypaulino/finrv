using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finrv.Shared;

public static class SanitizeExtension
{

    public static string Sanitize(this string s)
    {
        return s.Replace(Environment.NewLine, "").Replace("\r", "");
    }
}

