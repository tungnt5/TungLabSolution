using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LabSolution.WEB.Common
{
    public static class LinqToModel
    {
        public static void CopyProperties<T, S>(ref T Target, S Source)
        {
            if (Source == null)
            {
                Target = default(T);
                return;
            }

            PropertyInfo[] pSource = ((Type)Source.GetType()).GetProperties();
            Target = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] pTarget = ((Type)Target.GetType()).GetProperties();

            foreach (PropertyInfo pi in pTarget)
            {
                var prop = pSource.FirstOrDefault(x => x.Name.ToLower() == pi.Name.ToLower());
                if (prop != null)
                {
                    if (prop.GetValue(Source, null) != DBNull.Value)
                        pi.SetValue(Target, prop.GetValue(Source, null), null);
                }
            }
        }

        public static void ConvertLinqToModel<T, U>(ref List<T> lstModel, List<U> lstLinq)
        {
            if (lstLinq == null)
            {
                lstModel = null;
                return;
            }
            else if (lstLinq.Count == 0)
            {
                lstModel = new List<T>();
                return;
            }

            PropertyInfo[] linqProps = ((Type)lstLinq[0].GetType()).GetProperties();
            T modelInfo = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] modelProps = ((Type)modelInfo.GetType()).GetProperties();

            foreach (U linqInfo in lstLinq)
            {
                modelInfo = (T)Activator.CreateInstance(typeof(T));
                foreach (PropertyInfo pi in linqProps)
                {
                    var prop = modelProps.FirstOrDefault(x => x.Name.ToLower() == pi.Name.ToLower());
                    if (prop != null)
                    {
                        if (pi.GetValue(linqInfo, null) != DBNull.Value)
                            prop.SetValue(modelInfo, pi.GetValue(linqInfo, null), null);
                    }
                }
                lstModel.Add(modelInfo);
            }
        }

        public static void SetValueToListItem<T>(ref T Target, string ItemName, object value)
        {
            Target = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] pTarget = ((Type)Target.GetType()).GetProperties();

            var p = pTarget.ToList().FirstOrDefault(x => x.Name == ItemName);

            if (p != null)
            {
                p.SetValue(Target, value, null);
            }
        }
    }
}