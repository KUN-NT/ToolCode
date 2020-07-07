using System;

namespace PrintDemo.Common
{
    public class GuidHelper
    {
        public static string NewPureGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

    }
}
