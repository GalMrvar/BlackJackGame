using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace UtilitiesLib
{
    public static class UtilitiesStaticHelper<T>
    {
        public static bool XmlSerialize(string filename, List<T> obj)
        {
            XmlSerializer s = new XmlSerializer(typeof(List<T>));
            TextWriter w = new StreamWriter(filename);
            try
            {
                s.Serialize(w, obj);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                if (w != null)
                    w.Close();
            }
        }
    }
}
