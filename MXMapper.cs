using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmitMapper;

namespace MXUtilities
{
    /*
     First get the emit mapper from nuget.org and give reference to your project.
     */

    /// <summary>
    /// Object to object mapper
    /// </summary>
    public static class MXMapper
    {
        public static TTo Map<TFrom, TTo>(TFrom tFrom)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>().Map(tFrom);
        }

        public static IEnumerable<TTo> Map<TFrom, TTo>(IEnumerable<TFrom> lstTFrom)
        {
            return (from c in lstTFrom
                    select ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>().Map(c)).ToList(); ;
        }
    }//end of class "Mapper"
}
