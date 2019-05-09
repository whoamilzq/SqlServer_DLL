using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServer_dll
{
    class NameValuePair
    {
        private String Name;
        private Object o;

        public NameValuePair(String Name, Object o)
        {
            this.Name = Name;
            this.o = o;
        }

        /**
	 * @return Returns the name.
	 */
        public String getName()
        {
            return Name;
        }
        /**
         * @param name The name to set.
         */
        public void setName(String name)
        {
            Name = name;
        }
        /**
         * @return Returns the o.
         */
        public Object getO()
        {
            return o;
        }
        /**
         * @param o The o to set.
         */
        public void setO(Object o)
        {
            this.o = o;
        }

        /*
         * 
         * */
        public String toString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(Name + ":");
            if (o != null)
            {
                result.Append(o.ToString());
            }                                              
            else
            {
                result.Append("null");
            }
            return result.ToString();
        }
    }
}
